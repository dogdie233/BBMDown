using System.Text.RegularExpressions;

namespace BBMDown
{
    public static class Utils
	{
		private readonly static Regex rangeNumberRegex = new(@"^(-?\d+)-(-?\d+)$", RegexOptions.Compiled | RegexOptions.Singleline);
		private readonly static Regex comicIdRegex = new(@"mc(\d+).*", RegexOptions.Compiled | RegexOptions.Singleline);

		public static int[] ParseIntRangeString(string text)
		{
			var result = new List<int>();
			var blocks = text.Split(',');
			foreach (var block in blocks)
			{
				var block1 = block.Trim();
				if (int.TryParse(block1, out var number))
				{
					result.Add(number);
					continue;
				}
				var match = rangeNumberRegex.Match(block1);
				if (!match.Success) continue;
				if (!int.TryParse(match.Groups[1].Value, out var numberLeft)) continue;
				if (!int.TryParse(match.Groups[2].Value, out var numberRight)) continue;
				if (numberLeft > numberRight) { (numberLeft, numberRight) = (numberRight, numberLeft); }
				result.AddRange(Enumerable.Range(numberLeft, numberRight - numberLeft + 1));
			}
			return result.Distinct().ToArray();
		}

		public static bool TryGetComicIdByLink(string link, out int id)
        {
			if (link.StartsWith("https://")) link = link.Substring(8, link.Length - 8);
			if (link.StartsWith("http://")) link = link.Substring(7, link.Length - 7);

			if (link.StartsWith("manga.bilibili.com/detail/"))
            {
				var match = comicIdRegex.Match(link.Substring(26, link.Length - 26));
				if (match.Success && int.TryParse(match.Groups[1].Value, out id)) return true;
            }

			if (link.StartsWith("manga.bilibili.com/"))
            {
				var match = comicIdRegex.Match(link.Substring(19, link.Length - 19));
				if (match.Success && int.TryParse(match.Groups[1].Value, out id)) return true;
            }

			if (int.TryParse(link, out id)) return true;

			var match2 = comicIdRegex.Match(link);
            if (match2.Success && int.TryParse(match2.Groups[1].Value, out id)) return true;

            id = 0;
            return false;
        }
	}
}
