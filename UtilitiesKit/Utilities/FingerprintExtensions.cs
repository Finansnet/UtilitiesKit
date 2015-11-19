namespace UtilitiesKit.Utilities
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Security.Cryptography;
	using System.Text;
	using System.Threading.Tasks;

	public static class FingerprintExtensions
	{
		/// <summary>
		/// Calculates object SHA1 fingerprint.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string GetSha1(this object input)
		{
			return GetSha1(input.XmlSerialize());
		}

		/// <summary>
		/// Calculates object MD5 fingerprint.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string GetMd5(this object input)
		{
			return GetMd5(input.XmlSerialize());
		}

		public static string GetSha1(string unencrypted)
		{
			SHA1 encoder = SHA1.Create();
			byte[] encodedBytes = encoder.ComputeHash(Encoding.UTF8.GetBytes(unencrypted));
			return string.Concat(encodedBytes.Select(item => item.ToString("x2")).ToArray());
		}

		public static string GetMd5(string unencrypted)
		{
			MD5 encoder = MD5.Create();
			byte[] encodedBytes = encoder.ComputeHash(Encoding.UTF8.GetBytes(unencrypted));
			return string.Concat(encodedBytes.Select(item => item.ToString("x2")).ToArray());
		}
	}
}
