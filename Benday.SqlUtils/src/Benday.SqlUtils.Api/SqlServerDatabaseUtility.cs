using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Benday.SqlUtils.Api
{
    public class SqlServerDatabaseUtility : IDatabaseUtility
    {
        private string _ConnectionString = null;
        private bool _IsInitialized = false;

        private const string _DescribeTableQuery = @"select 
TABLE_SCHEMA, TABLE_NAME, COLUMN_NAME, CONVERT(bit, CASE IS_NULLABLE WHEN 'NO' THEN 0 WHEN 'YES' THEN 1 END) as IsNullable, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH,	ORDINAL_POSITION, COLUMN_DEFAULT,
COLUMNPROPERTY(object_id(TABLE_NAME), COLUMN_NAME, 'IsIdentity') as IsIdentity, 
IDENT_SEED(TABLE_NAME) as IdentitySeed, 
IDENT_INCR(TABLE_NAME) as IdentityIncrement 
from INFORMATION_SCHEMA.COLUMNS where table_name = @TABLE_NAME";

        private const string _PrimaryKeyQuery = @"select kcu.TABLE_SCHEMA, kcu.CONSTRAINT_NAME, kcu.TABLE_NAME, kcu.COLUMN_NAME, kcu.ORDINAL_POSITION
from INFORMATION_SCHEMA.key_column_usage kcu 
join
INFORMATION_SCHEMA.table_constraints tc 
on 
tc.constraint_name=kcu.constraint_name
where 
kcu.table_name=@TABLE_NAME
and tc.constraint_type='primary key'";

        public TableDescription DescribeTable(string tableName)
        {
            AssertIsInitialized();

            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException($"{nameof(tableName)} is null or empty.", nameof(tableName));
            }

            var args = new Dictionary<string, string>();

            args.Add("TABLE_NAME", tableName);

            var descResult = RunQuery(_DescribeTableQuery, args);

            string primaryKeyColumn = GetPrimaryKeyValue(
                RunQuery(_PrimaryKeyQuery, args));

            var returnValue = new TableDescription(descResult);

            returnValue.PrimaryKeyColumnName = primaryKeyColumn;

            return returnValue;
        }

        private string GetPrimaryKeyValue(DataTable table)
        {
            if (table == null || table.Rows.Count == 0 ||
                table.Columns.Contains("COLUMN_NAME") == false)
            {
                return null;
            }
            else
            {
                var value = table.Rows[0]["COLUMN_NAME"].ToString();

                return value;
            }
        }

        private void AssertIsInitialized()
        {
            if (_IsInitialized == false)
            {
                throw new InvalidOperationException("Initialize() was not called.");
            }
        }

        public void Initialize(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException($"{nameof(connectionString)} is null or empty.", nameof(connectionString));
            }

            _ConnectionString = connectionString;

            _IsInitialized = true;
        }

        public DataTable RunQuery(string query, Dictionary<string, string> args = null)
        {
            DataSet results = new DataSet();

            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {
                using (var command = GetSqlCommand(query, args))
                {
                    command.Connection = connection;

                    using (var adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(results);
                    }
                }
            }

            if (results.Tables.Count == 0)
            {
                return null;
            }
            else
            {
                return results.Tables[0];
            }            
        }

        protected SqlCommand GetSqlCommand(string query, 
            Dictionary<string, string> args)
        {
            var command = new SqlCommand(query);

            if (args != null)
            {
                foreach (var key in args.Keys)
                {
                    var parameter = new SqlParameter();

                    parameter.ParameterName = String.Format("@{0}", key);
                    parameter.SqlDbType = SqlDbType.NVarChar;
                    parameter.Value = args[key];

                    command.Parameters.Add(parameter);
                }
            }

            return command;
        }
    }
}