namespace UtilitiesKit.Utilities
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using System.Xml.Serialization;

	public static class SerializationExtensions
	{
		public static string XmlSerialize(this object input)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				DataContractSerializer serializer = new DataContractSerializer(input.GetType());
				serializer.WriteObject(stream, input);
				return new UTF8Encoding().GetString(stream.ToArray());
			}
		}

		public static T XmlDeserialize<T>(this string input)
		{
			using (MemoryStream memoryStream = new MemoryStream(new UTF8Encoding().GetBytes(input)))
			{
				DataContractSerializer serializer = new DataContractSerializer(typeof(T));
				return (T)serializer.ReadObject(memoryStream);
			}
		}

		/// <summary>
		/// Clones the object using XML serialization/deserialization mechanisms.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="toBeClonned"></param>
		/// <returns></returns>
		public static T Clone<T>(this T toBeClonned)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				DataContractSerializer serializer = new DataContractSerializer(toBeClonned.GetType());
				serializer.WriteObject(stream, toBeClonned);
				stream.Seek(0, SeekOrigin.Begin);
				return (T)serializer.ReadObject(stream);
			}
		}
	}
}
