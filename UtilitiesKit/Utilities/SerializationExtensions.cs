﻿namespace UtilitiesKit.Utilities
{
	using System.IO;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Xml.Serialization;

	public static class SerializationExtensions
	{
		/// <summary>
		/// An extension method that serializes object to XML.
		/// </summary>
		/// <typeparam name="T">Base type the serializer is created for. </typeparam>
		/// <param name="input">The input to be serialzied. </param>
		/// <returns>
		/// Serialzied XML.
		/// </returns>
		public static string XmlSerialize<T>(this T input)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				XmlSerializer serializer = new XmlSerializer(typeof(T));
				serializer.Serialize(stream, input);
				return new UTF8Encoding().GetString(stream.ToArray());
			}
		}

		/// <summary>
		/// An extension method that deserializes object from XML.
		/// </summary>
		/// <typeparam name="T">Generic type that is to be deserialized. </typeparam>
		/// <param name="input">The input to be deserialzed. </param>
		/// <returns>
		/// Deserialized object.
		/// </returns>
		public static T XmlDeserialize<T>(this string input)
		{
			using (MemoryStream memoryStream = new MemoryStream(new UTF8Encoding().GetBytes(input)))
			{
				XmlSerializer serializer = new XmlSerializer(typeof(T));
				return (T)serializer.Deserialize(memoryStream);
			}
		}

		public static string DataContractSerialize(this object input)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				DataContractSerializer serializer = new DataContractSerializer(input.GetType());
				serializer.WriteObject(stream, input);
				return new UTF8Encoding().GetString(stream.ToArray());
			}
		}

		public static T DataContractDeserialize<T>(this string input)
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
