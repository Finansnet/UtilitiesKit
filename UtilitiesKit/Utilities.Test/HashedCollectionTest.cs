using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UtilitiesKit.Utilities.Collections;

namespace Utilities.Test
{
	public class SampleClass
	{
		public string Hash { get; private set; }
		public SampleClass(string hash) { Hash = hash; }
	}

	public class SampleClassWithEquals
	{
		public string Hash { get; private set; }
		public SampleClassWithEquals(string hash) { Hash = hash; }

		public override bool Equals(object obj)
		{
			return ((SampleClassWithEquals)obj).Hash == Hash;
		}
	}

	[TestClass]
	public class HashedCollectionTest
	{
		[TestMethod]
		public void TestAdd()
		{
			HashedCollection<SampleClass> collection = new HashedCollection<SampleClass>((item) => item.Hash);

			collection.Add(new SampleClass("XXX"));
			collection.Add(new SampleClass("XXX"));
			collection.Add(new SampleClass("YYY"));

			Assert.AreEqual(3, collection.Count);
		}

		[TestMethod]
		public void TestContains()
		{
			HashedCollection<SampleClass> collection = new HashedCollection<SampleClass>((item) => item.Hash);

			var i1 = new SampleClass("XXX");
			var i2 = new SampleClass("YYY");
			var i3 = new SampleClass("ZZZ");

			collection.Add(i1);
			collection.Add(i2);

			Assert.IsFalse(collection.Contains(i3));
			Assert.IsTrue(collection.Contains(i1));
			Assert.IsTrue(collection.Contains(i2));
		}

		[TestMethod]
		public void TestContainsWithEquals()
		{
			HashedCollection<SampleClassWithEquals> collection = new HashedCollection<SampleClassWithEquals>((item) => item.Hash);

			var i1 = new SampleClassWithEquals("XXX");
			var i2 = new SampleClassWithEquals("YYY");
			var i3 = new SampleClassWithEquals("ZZZ");

			collection.Add(new SampleClassWithEquals("XXX"));
			collection.Add(new SampleClassWithEquals("YYY"));

			Assert.IsFalse(collection.Contains(i3));
			Assert.IsTrue(collection.Contains(i1));
			Assert.IsTrue(collection.Contains(i2));
		}

		[TestMethod]
		public void TestRemove()
		{
			HashedCollection<SampleClass> collection = new HashedCollection<SampleClass>((item) => item.Hash);

			var i1 = new SampleClass("XXX");
			var i2 = new SampleClass("YYY");

			collection.Add(i1);
			collection.Add(i2);

			Assert.AreEqual(2, collection.Count);

			collection.Remove(i1);
			collection.Remove(i2);

			Assert.AreEqual(0, collection.Count);
		}
	}
}
