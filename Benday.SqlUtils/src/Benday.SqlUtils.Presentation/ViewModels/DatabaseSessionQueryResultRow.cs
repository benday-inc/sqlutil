using Benday.SqlUtils.Api;
using System;
using System.Data;

namespace Benday.SqlUtils.Presentation.ViewModels
{
    public class DatabaseSessionQueryResultRow 
    {
        private const string SpWho2EmptyValue = "  .";
        public DatabaseSessionQueryResultRow(DataRow fromValue)
        {
            PopulatePropertiesFromRow(fromValue);
        }

        private void PopulatePropertiesFromRow(DataRow value)
        {
            SessionId = GetIntFromRow(value, Constants.ColumnName_SessionId);
            Status = GetStringFromRow(value, Constants.ColumnName_Status);
            Login = GetStringFromRow(value, Constants.ColumnName_Login);
            HostName = GetStringFromRow(value, Constants.ColumnName_HostName);
            BlockedBy = GetStringFromRow(value, Constants.ColumnName_BlockedBy);
            DatabaseName = GetStringFromRow(value, Constants.ColumnName_DatabaseName);
            Command = GetStringFromRow(value, Constants.ColumnName_Command);
            ProgramName = GetStringFromRow(value, Constants.ColumnName_ProgramName);
        }

        private string GetStringFromRow(DataRow row, string columnName)
        {
            if (row == null || row.Table == null)
            {
                return string.Empty;
            }
            else
            {
                if (row.Table.Columns.Contains(columnName) == false)
                {
                    return string.Empty;
                }
                else if (row.Table.Columns[columnName].DataType != typeof(string))
                {
                    return "(error: unexpected column data type)";
                }
                else
                {
                    if (row[columnName] == DBNull.Value)
                    {
                        return string.Empty;
                    }
                    else
                    {
                        var temp = (string)row[columnName];

                        if (string.IsNullOrEmpty(temp) == true)
                        {
                            return string.Empty;
                        }
                        else if (temp == SpWho2EmptyValue)
                        {
                            return string.Empty;
                        }
                        else
                        {
                            return temp.Trim();
                        }
                    }
                }
            }
        }

        private int GetIntFromRow(DataRow row, string columnName)
        {
            if (row == null || row.Table == null)
            {
                return -1;
            }
            else
            {
                if (row.Table.Columns.Contains(columnName) == false)
                {
                    return -1;
                }
                else if (row.Table.Columns[columnName].DataType != typeof(int))
                {
                    return -2;
                }
                else
                {
                    if (row[columnName] == DBNull.Value)
                    {
                        return -1;
                    }
                    else
                    {
                        return (int)row[columnName];
                    }
                }
            }
        }

        public int SessionId { get; set; }
        public string Status { get; set; }
        public string Login { get; set; }
        public string HostName { get; set; }
        public string BlockedBy { get; set; }
        public string DatabaseName { get; set; }
        public string Command { get; set; }
        public string ProgramName { get; set; }
    }
}
