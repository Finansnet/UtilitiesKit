namespace UtilitiesKit.Utilities.Collections
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// Uses given hash to improve search speed. Additionally uses LowMemoryDictionary to decrease amount of memory usage.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class HashedCollection<T> : ICollection<T>
	{
		private Func<T, string> _HashingMethod;
		private LowMemoryDictionary<string, List<T>> _Dictionary = new LowMemoryDictionary<string, List<T>>();

		public HashedCollection(Func<T, string> hashingMethod)
		{
			_HashingMethod = hashingMethod;
		}

		public List<T> GetSimilar(T input)
		{
			string hash = _HashingMethod(input);
			List<T> output;
			if (_Dictionary.TryGetValue(hash, out output))
				return output;
			else
				return new List<T>();
		}

		public void Add(T item)
		{
			string hash = _HashingMethod(item);
			List<T> output;
			if (!_Dictionary.TryGetValue(hash, out output))
			{
				output = new List<T>();
				_Dictionary[hash] = output;
			}
			output.Add(item);
		}

		public void Clear()
		{
			_Dictionary.Clear();
		}

		public bool Contains(T item)
		{
			string hash = _HashingMethod(item);
			List<T> output;
			if (_Dictionary.TryGetValue(hash, out output))
				return output.Contains(item);
			else
				return false;
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		public int Count
		{
			get { return _Dictionary.Values.Sum(item => item.Count); }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(T item)
		{
			string hash = _HashingMethod(item);
			List<T> output;
			if (_Dictionary.TryGetValue(hash, out output))
				return output.Remove(item);
			return false;
		}

		public IEnumerator<T> GetEnumerator()
		{
			throw new NotImplementedException();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}
	}
}
