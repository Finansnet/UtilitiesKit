namespace Utilities.Test
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using System.Collections.Generic;
	using UtilitiesKit.Utilities.Collections;

	[TestClass]
	public class DictionrayExtensionTest
	{
		[TestMethod]
		public void TestGetValueOrDefaultExisting()
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			dictionary["Dummy"] = 1;
			int result = dictionary.GetValueOrDefault("Dummy");
			Assert.AreEqual(1, result);
		}

		[TestMethod]
		public void TestGetValueOrDefaultInexisting()
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			dictionary["Dummy"] = 1;
			int result = dictionary.GetValueOrDefault("Dummy2");
			Assert.AreEqual(default(int), result);
		}
	}
}
