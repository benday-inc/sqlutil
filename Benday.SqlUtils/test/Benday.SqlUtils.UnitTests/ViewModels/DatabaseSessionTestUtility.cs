using Benday.SqlUtils.Api;
using System;
using System.Data;

namespace Benday.SqlUtils.UnitTests.ViewModels
{
    public static class DatabaseSessionTestUtility
    {
        public static DataTable GetDatabaseSessionQueryResultTable()
        {
            var table = new DataTable();

            AddColumn<int>(table, Constants.ColumnName_SessionId);
            AddColumn<string>(table, Constants.ColumnName_Status);
            AddColumn<string>(table, Constants.ColumnName_Login);
            AddColumn<string>(table, Constants.ColumnName_HostName);
            AddColumn<string>(table, Constants.ColumnName_BlockedBy);
            AddColumn<string>(table, Constants.ColumnName_DatabaseName);
            AddColumn<string>(table, Constants.ColumnName_Command);
            AddColumn<string>(table, Constants.ColumnName_ProgramName);

            var dataset = new DataSet();

            dataset.Tables.Add(table);

            return table;
        }

        public static void AddColumn<T>(DataTable table, string columnName)
        {
            DataColumn column = new DataColumn(columnName, typeof(T));

            column.AllowDBNull = true;

            table.Columns.Add(column);
        }

        public const int ExpectedSessionId = 1234;
        public const string ExpectedStatus = "Status Value";
        public const string ExpectedLogin = "Login Value";
        public const string ExpectedHostName = "HostName Value";
        public const string ExpectedBlockedBy = "BlockedBy Value";
        public const string ExpectedDatabaseName = "DatabaseName Value";
        public const string ExpectedCommand = "Command Value";
        public const string ExpectedProgramName = "ProgramName Value";

        public static DataTable InitalizeAllFieldsToNull()
        {
            var table = DatabaseSessionTestUtility.GetDatabaseSessionQueryResultTable();

            DataRow row = table.NewRow();

            foreach (DataColumn col in table.Columns)
            {
                row[col] = DBNull.Value;
            }

            table.Rows.Add(row);

            return table;
        }

        public static DataTable InitalizeAllStringFieldsToWhitespace()
        {
            var table = DatabaseSessionTestUtility.GetDatabaseSessionQueryResultTable();

            DataRow row = table.NewRow();

            var valueString = "       ";

            foreach (DataColumn col in table.Columns)
            {
                if (col.DataType == typeof(int))
                {
                    row[col] = DBNull.Value;
                }
                else
                {
                    row[col] = valueString;
                }
            }

            table.Rows.Add(row);

            return table;
        }

        public static DataTable InitalizeAllStringFieldsToDash()
        {
            var table = DatabaseSessionTestUtility.GetDatabaseSessionQueryResultTable();

            DataRow row = table.NewRow();

            var valueString = "  .";

            foreach (DataColumn col in table.Columns)
            {
                if (col.DataType == typeof(int))
                {
                    row[col] = DBNull.Value;
                }
                else
                {
                    row[col] = valueString;
                }
            }

            table.Rows.Add(row);

            return table;
        }

        public static DataTable InitalizeAllFieldsToValues()
        {
            var table = DatabaseSessionTestUtility.GetDatabaseSessionQueryResultTable();

            DataRow row = table.NewRow();

            row[Constants.ColumnName_SessionId] = ExpectedSessionId;
            row[Constants.ColumnName_Status] = ExpectedStatus;
            row[Constants.ColumnName_Login] = ExpectedLogin;
            row[Constants.ColumnName_HostName] = ExpectedHostName;
            row[Constants.ColumnName_BlockedBy] = ExpectedBlockedBy;
            row[Constants.ColumnName_DatabaseName] = ExpectedDatabaseName;
            row[Constants.ColumnName_Command] = ExpectedCommand;
            row[Constants.ColumnName_ProgramName] = ExpectedProgramName;

            table.Rows.Add(row);

            return table;
        }
    }
}
