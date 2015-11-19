namespace UtilitiesKit.Database
{
	using System;
	using System.Data;
	using System.Data.Common;
	using System.Data.SqlClient;

	public static class SqlServerHelper
	{
		public static void WrapUpTransaction(string connectionString, Action<SqlTransaction> callback, IsolationLevel isolationLevel = IsolationLevel.Serializable)
		{
			using(SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				using (SqlTransaction transaction = connection.BeginTransaction(isolationLevel))
				{
					callback(transaction);
					transaction.Commit();
				}
			}
		}

		#region GetScalar

		public static object GetScalar(string connectionString, string query, params object[] arguments)
		{
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				using (SqlTransaction transaction = connection.BeginTransaction())
					return GetScalar(transaction, query, arguments);
			}
		}

		public static object GetScalar(DbTransaction transaction, string query, params object[] arguments)
		{
			using (DbCommand command = transaction.Connection.CreateCommand())
			{
				command.CommandText = query;
				command.Transaction = transaction;
				LoadParameters(command, arguments);
				object result = command.ExecuteScalar();
				return (result == DBNull.Value) ? null : result;
			}
		}

		#endregion

		#region GetTable overloads

		/// <summary>
		/// Gets the data table.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="query">The query.</param>
		/// <param name="arguments">The arguments.</param>
		/// <returns></returns>
		public static DataTable GetTable(string connectionString, string query, params object[] arguments)
		{
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				return GetTable(connection, query, arguments);
			}
		}

		public static DataTable GetTable(DbConnection connection, string query, params object[] arguments)
		{
			using (DbTransaction transaction = connection.BeginTransaction())
				return GetTable(transaction, query, arguments);
		}

		public static DataTable GetTable(DbTransaction transaction, string query, params object[] arguments)
		{
			using (DbCommand command = transaction.Connection.CreateCommand())
			{
				command.Transaction = transaction;
				command.CommandText = query;
				LoadParameters(command, arguments);
				using (DbDataReader reader = command.ExecuteReader())
				{
					// Set DataTable name to make it serializable.
					DataTable result = new DataTable("Dummy");
					result.Load(reader);
					return result;
				}
			}
		}

		#endregion

		#region ExecuteNonQuery overloads

		/// <summary>
		/// Executes the non query.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="query">The query to be executed. Numbers for naming parameters. Ex: parameter @0 in statement will be replaced with first argument passed in 'arguments' array.</param>
		/// <param name="transaction">The transaction. pass null if no transaction is needed.</param>
		/// <param name="arguments">The arguments.</param>
		public static void ExecuteNonQuery(string connectionString, string query, params object[] arguments)
		{
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				ExecuteNonQuery(connection, query, arguments);
			}
		}

		public static void ExecuteNonQuery(DbConnection connection, string query, params object[] arguments)
		{
			using (DbCommand command = connection.CreateCommand())
			{
				command.CommandText = query;
				LoadParameters(command, arguments);
				command.ExecuteNonQuery();
			}
		}

		public static void ExecuteNonQuery(DbTransaction transaction, string query, params object[] arguments)
		{
			using (DbCommand command = transaction.Connection.CreateCommand())
			{
				command.CommandText = query;
				command.Transaction = transaction;
				LoadParameters(command, arguments);
				command.ExecuteNonQuery();
			}
		}

		#endregion

		#region Private helpers

		private static void LoadParameters(DbCommand command, params object[] arguments)
		{
			for (int argumentIndex = 0; argumentIndex < arguments.Length; argumentIndex++)
			{
				DbParameter parameter = command.CreateParameter();
				parameter.ParameterName = argumentIndex.ToString();
				parameter.Value = (arguments[argumentIndex] == null) ? DBNull.Value : arguments[argumentIndex];
				command.Parameters.Add(parameter);
			}
		}

		#endregion

	}
}
