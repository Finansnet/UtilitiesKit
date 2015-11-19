using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UtilitiesKit.Utilities;

namespace Utilities.Test
{
	[TestClass]
	public class SerializationExtensionsTest
	{
		public class SampleClass
		{
			public string String { get; set; }
			public DateTimeOffset DateTimeOffset { get; set; }
			public int Int { get; set; }

			public SampleClass Sample { get; set; }
		}

		[TestMethod]
		public void TestSerialization()
		{
			SampleClass instance = new SampleClass();
			instance.String = "XYZ";
			instance.DateTimeOffset = new DateTimeOffset(2015, 1, 3, 0, 0, 0, new TimeSpan(1, 0, 0));
			instance.Int = 5;
			instance.Sample = new SampleClass() { String = "Test", DateTimeOffset = new DateTimeOffset(2015, 1, 4, 0, 0, 0, new TimeSpan(1, 0, 0)) };

			string serialized = SerializationExtensions.XmlSerialize(instance);
			SampleClass deserialized = SerializationExtensions.XmlDeserialize<SampleClass>(serialized);

			Assert.AreEqual(instance.String, deserialized.String);
			Assert.AreEqual(instance.DateTimeOffset, deserialized.DateTimeOffset);
			Assert.AreEqual(instance.Int, deserialized.Int);
			Assert.AreEqual(instance.Sample.String, deserialized.Sample.String);
			Assert.AreEqual(instance.Sample.DateTimeOffset, deserialized.Sample.DateTimeOffset);
		}

		[TestMethod]
		public void TestClone()
		{
			SampleClass instance = new SampleClass();
			instance.String = "XYZ";
			instance.DateTimeOffset = new DateTimeOffset(2015, 1, 3, 0, 0, 0, new TimeSpan(1, 0, 0));
			instance.Int = 5;
			instance.Sample = new SampleClass() { String = "Test", DateTimeOffset = new DateTimeOffset(2015, 1, 4, 0, 0, 0, new TimeSpan(1, 0, 0)) };

			SampleClass deserialized = SerializationExtensions.Clone(instance);

			Assert.AreEqual(instance.String, deserialized.String);
			Assert.AreEqual(instance.DateTimeOffset, deserialized.DateTimeOffset);
			Assert.AreEqual(instance.Int, deserialized.Int);
			Assert.AreEqual(instance.Sample.String, deserialized.Sample.String);
			Assert.AreEqual(instance.Sample.DateTimeOffset, deserialized.Sample.DateTimeOffset);
		}
	}
}
