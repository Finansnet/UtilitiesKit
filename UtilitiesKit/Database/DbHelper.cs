namespace UtilitiesKit.Database
{
	using System;

	public static class DbHelper
	{
		/// <summary>
		/// Replaces DBNull with the value given in argument
		/// </summary>
		/// <param name="input"></param>
		/// <param name="alternativeValue"></param>
		/// <returns></returns>
		public static object DbNull2Null(this object input, object alternativeValue = null)
		{
			return input == DBNull.Value ? alternativeValue : input;
		}

		/// <summary>
		/// Replaces DateTime value below the limit accepted by the SQL Server with the SQL Sever min value.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static DateTime NormalizeMinValue(DateTime input)
		{
			if (input == DateTime.MinValue)
				return new DateTime(1753, 1, 1);
			return input;
		}
	}
}
