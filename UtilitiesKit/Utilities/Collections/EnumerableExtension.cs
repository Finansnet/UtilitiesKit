namespace UtilitiesKit.Utilities.Collections
{
	using System;
	using System.Collections;
	using System.Collections.Generic;

	public static class EnumerableExtension
	{
		public static void ForEach<T>(this IEnumerable<T> input, Action<T> callback)
		{
			foreach (T item in input)
				callback(item);
		}
	}
}
