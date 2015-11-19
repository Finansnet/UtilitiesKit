# UtilitiesKit
Set of small useful libraries for .NET

--------------------------------------------
UtilitiesKit.Database
--------------------------------------------

*** SqlServerHelper ***
Class simplyfying database access. Contains set of static methods such as:
  GetScalar
  GetTable
  ExecuteNonQuery

*** DbHelper ***
Static methods useful while working with databases.
  DbNull2Null - replaces DBNull with null
  NormalizeMinValue - replaces DateTime.MinValue with 1753.01.01 - the lowest date time value accepted by SqlServer in datetime data type.