namespace UtilitiesKit.Utilities.Collections
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public static class DictionaryExtension
	{
		public static T GetValueOrDefault<K, T>(this IDictionary<K, T> input, K key)
		{
			T result = default(T);
			input.TryGetValue(key, out result);
			return result;
		}
	}
}
