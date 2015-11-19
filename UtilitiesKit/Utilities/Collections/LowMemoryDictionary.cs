namespace UtilitiesKit.Utilities.Collections
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public class LowMemoryDictionary<TKey, TValue>
	{
		private int _Capacity;
		private Dictionary<TKey, TValue>[] _Dictionaries;

		public LowMemoryDictionary(int capacity = 100)
		{
			_Capacity = capacity;
			_Dictionaries = new Dictionary<TKey, TValue>[capacity];
			for (int index = 0; index < _Capacity; index++)
				_Dictionaries[index] = new Dictionary<TKey, TValue>();
		}

		public int Count { get { return _Dictionaries.Sum(item => item.Count); } }

		public void Add(TKey key, TValue value)
		{
			Dictionary<TKey, TValue> dictionary = _Dictionaries[GetDictionaryIndex(key)];
			dictionary.Add(key, value);
		}

		private int GetDictionaryIndex(TKey key)
		{
			return Math.Abs(key.GetHashCode()) % _Capacity;
		}

		public bool ContainsKey(TKey key)
		{
			int index = GetDictionaryIndex(key);
			Dictionary<TKey, TValue> dictionary = _Dictionaries[index];
			return dictionary.ContainsKey(key);
		}

		public List<TKey> Keys
		{
			get
			{
				List<TKey> keys = new List<TKey>();
				foreach (Dictionary<TKey, TValue> dictionary in _Dictionaries)
				{
					keys.AddRange(dictionary.Keys);
				}
				return keys;
			}
		}

		public List<TValue> Values
		{
			get
			{
				List<TValue> result = new List<TValue>();
				foreach (Dictionary<TKey, TValue> dictionary in _Dictionaries)
				{
					result.AddRange(dictionary.Values);
				}
				return result;
			}
		}

		public TValue this[TKey key]
		{
			get
			{
				int index = GetDictionaryIndex(key);
				Dictionary<TKey, TValue> dictionary = _Dictionaries[index];
				return dictionary[key];
			}
			set
			{
				int index = GetDictionaryIndex(key);
				Dictionary<TKey, TValue> dictionary = _Dictionaries[index];
				dictionary[key] = value;
			}
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			int index = GetDictionaryIndex(key);
			Dictionary<TKey, TValue> dictionary = _Dictionaries[index];
			return dictionary.TryGetValue(key, out value);
		}

		public void Clear()
		{
			_Dictionaries.ForEach(item => item.Clear());
		}
	}
}
