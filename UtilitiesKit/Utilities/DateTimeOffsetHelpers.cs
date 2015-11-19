namespace UtilitiesKit.Utilities
{
	using System;

	public static class DateTimeOffsetHelpers
	{
		public static DateTimeOffset Min(DateTimeOffset left, DateTimeOffset right)
		{
			if (left < right)
				return left;
			return right;
		}

		public static DateTimeOffset Max(DateTimeOffset left, DateTimeOffset right)
		{
			if (left > right)
				return left;
			return right;
		}
	}
}
