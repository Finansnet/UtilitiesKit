namespace UtilitiesKit.Utilities
{
	using System;

	public static class DateTimeHelpers
	{
		public static DateTime Min(DateTime first, DateTime second)
		{
			if (first > second)
				return second;
			return first;
		}

		public static DateTime Max(DateTime first, DateTime second)
		{
			if (first > second)
				return first;
			return second;
		}
	}
}
