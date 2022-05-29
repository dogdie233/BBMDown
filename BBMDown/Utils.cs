using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BBMDown
{
    public static class Utils
	{
		private readonly static Regex rangeNumberRegex = new(@"^(-?\d+)-(-?\d+)$", RegexOptions.Compiled | RegexOptions.Singleline);

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
				if (match == null || !match.Success) { continue; }
				if (!int.TryParse(match.Groups[1].Value, out var numberLeft)) { continue; }
				if (!int.TryParse(match.Groups[2].Value, out var numberRight)) { continue; }
				if (numberLeft > numberRight) { (numberLeft, numberRight) = (numberRight, numberLeft); }
				result.AddRange(Enumerable.Range(numberLeft, numberRight - numberLeft + 1));
			}
			return result.Distinct().ToArray();
		}
	}
}
