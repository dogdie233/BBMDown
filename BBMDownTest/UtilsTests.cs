using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Linq;
using System;
using BBMDown;

namespace BBMDownTest
{
    [TestClass]
    public class UtilsTests
    {
		[TestMethod]
		public void ParseIntRangeString_ValidResult()
		{
			// arrange
			var input = "1 , -2 , 0-5,-3";
			var expect = new int[] { -3, -2, 0, 1, 2, 3, 4, 5 };

			// act
			var result = Utils.ParseIntRangeString(input).ToArray();

			Array.Sort(expect);
			Array.Sort(result);

			// assert
			Assert.IsTrue(Enumerable.SequenceEqual(expect, result));
		}

		[TestMethod]
		public void ParseIntRangeString_InValidResult()
		{
			// arrange
			var input = "1 , -2 ,a, ,-,, 8a , a--54, 0-5,-3";
			var expect = new int[] { -3, -2, 0, 1, 2, 3, 4, 5 };

			// act
			var result = Utils.ParseIntRangeString(input).ToArray();

			Array.Sort(expect);
			Array.Sort(result);

			// assert
			Assert.IsTrue(Enumerable.SequenceEqual(expect, result));
		}

		[TestMethod]
		public void ParseIntRangeString_EmptyInput()
		{
			// arrange
			var input = "";
			var expect = new int[0];

			// act
			var result = Utils.ParseIntRangeString(input).ToArray();

			// assert
			Assert.IsTrue(Enumerable.SequenceEqual(expect, result));
		}
	}
}