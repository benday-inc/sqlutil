using System;
using System.Data;

namespace Benday.SqlUtils.Api
{
    public class ColumnDescription
    {
        public ColumnDescription(DataRow fromValue)
        {
            InitializeFromDataRow(fromValue);
        }

        public string Schema { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public bool IsNullable { get; set; }
        public bool IsIdentity { get; set; }

        private void InitializeFromDataRow(DataRow value)
        {
            Schema = GetStringValue(value, "TABLE_SCHEMA");
            TableName = GetStringValue(value, "TABLE_NAME");
            ColumnName = GetStringValue(value, "COLUMN_NAME");
            IsNullable = GetBooleanValue(value, "IsNullable");
            DataType = GetStringValue(value, "DATA_TYPE");
            IsIdentity = GetBooleanValue(value, "IsIdentity");
        }

        private bool GetBooleanValue(DataRow value, string columnName)
        {
            if (value.IsNull(columnName) == true)
            {
                return false;
            }
            else
            {
                return Boolean.Parse(value[columnName].ToString());
            }
        }
        
        private string GetStringValue(DataRow value, string columnName)
        {
            if (value.IsNull(columnName) == true)
            {
                return String.Empty;
            }
            else
            {
                return value[columnName].ToString();
            }
        }
    }
}
