using Benday.Presentation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Threading;

namespace Benday.SqlUtils.Presentation.ViewModels
{
    public class SearchByTextColumnContentQueryViewModel : DatabaseQueryViewModelBase
    {
        public SearchByTextColumnContentQueryViewModel()
        {
            _ProgressInfo.IsCancelable = true;
            _ProgressInfo.OnCancelRequested += _ProgressInfo_OnCancelRequested;
            _ProgressInfo.IsProgressBarVisible = false;
        }

        private void _ProgressInfo_OnCancelRequested(object sender, EventArgs e)
        {
            _StopSearchRequested = true;
        }        

        protected override string SqlQueryTemplate
        {
            get
            {
                return @"select table_schema, table_name
                        from INFORMATION_SCHEMA.TABLES
                        where table_name like @TABLE_NAME
                        order by table_name";
            }
        }

        protected override List<string> GetRequiredArguments()
        {
            List<string> args = new List<string>();

            args.Add("TABLE_NAME");
            args.Add("COLUMN_NAME");
            args.Add("SEARCH_TEXT");

            return args;
        }

        private object _SyncLock = new object();

        public override void Execute()
        {
            IsVisible = true;

            Results = new System.Collections.ObjectModel.ObservableCollection<object>();

            BindingOperations.EnableCollectionSynchronization(Results, _SyncLock);

            FindTextInColumn(true);
        }

        private void FindTextInColumn(bool runAsync)
        {
            ProgressInfo.ProgressBarMessage = "Starting search...";

            IsSearchRunning = true;

            if (runAsync == true)
            {
                // ThreadPool.QueueUserWorkItem(new WaitCallback(FindTextInColumn), results);

                Task.Run(() => FindTextInColumn());
            }
            else
            {
                FindTextInColumn();
            }
        }

        private void FindTextInColumn()
        {
            string columnName = GetArgumentValue("COLUMN_NAME");
            string tableName = GetArgumentValue("TABLE_NAME");

            if (columnName == null)
            {
                columnName = String.Empty;
            }

            if (tableName == null)
            {
                tableName = String.Empty;
            }

            ProgressInfo.IsProgressBarVisible = true;
            ProgressInfo.ProgressBarMessage = "Starting search...";
            _StopSearchRequested = false;

            try
            {
                string query = null;
                DataTable textColumnsDataTable;
                SqlCommand getTextColumnsCommand;

                if (columnName.Trim().Length > 0 &&
                    tableName.Trim().Length > 0)
                {
                    query = GetTextColumnQueryForTableAndColumn();

                    getTextColumnsCommand = GetSqlCommand(query);

                    AddParameterForLikeStatement(getTextColumnsCommand, "COLUMN_NAME");
                    AddParameterForLikeStatement(getTextColumnsCommand, "TABLE_NAME");
                }
                else if (tableName.Trim().Length > 0)
                {
                    query = GetTextColumnQueryForTable();

                    getTextColumnsCommand = GetSqlCommand(query);

                    AddParameterForLikeStatement(getTextColumnsCommand, "TABLE_NAME");
                }
                else if (columnName.Trim().Length > 0)
                {
                    query = GetTextColumnQueryForColumn();

                    getTextColumnsCommand = GetSqlCommand(query);

                    AddParameterForLikeStatement(getTextColumnsCommand, "COLUMN_NAME");
                }
                else
                {
                    query = GetTextColumnQueryForAllTextColumns();

                    getTextColumnsCommand = GetSqlCommand(query);
                }

                textColumnsDataTable = ExecuteCommand(getTextColumnsCommand);

                string template =
                    "select count(*) as recordCount from [{0}].[{1}] where [{2}] like '%{3}%'";

                string message = null;

                using (SqlConnection connection = GetSqlConnection())
                {
                    connection.Open();

                    int recordCount = 0;

                    foreach (DataRow row in textColumnsDataTable.Rows)
                    {
                        if (_StopSearchRequested == true)
                        {
                            // a stop has been requested
                            ProgressInfo.ProgressBarMessage = "Search canceled.";
                            break;
                        }

                        message = "Searching [" + row["table_schema"].ToString() + "]." +
                            "[" + row["table_name"].ToString() + "].[" +
                            row["column_name"].ToString() + "]...";

                        Console.WriteLine(message);
                        ProgressInfo.ProgressBarMessage = message;
                            
                        query = String.Format(
                            template, row["table_schema"], row["table_name"], row["column_name"],
                            GetArgumentValue("SEARCH_TEXT"));

                        recordCount = ExecuteScalarGetInt32(
                            connection, query);

                        if (recordCount > 0)
                        {
                            // AddRow(row, recordCount, query);

                            Console.WriteLine("** Adding row for {0}", query);
                            
                            Results.Add(new TextQueryResultRow(row, query, recordCount));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ProgressInfo.ProgressBarMessage = ex.ToString();
            }
            finally
            {
                ProgressInfo.ProgressBarMessage = "Search complete.";
                ProgressInfo.IsProgressBarVisible = false;
                IsSearchRunning = false;
                RaisePropertyChanged(ResultsPropertyName);
            }
        }

        private bool _StopSearchRequested;
        
        private const string IsSearchRunningPropertyName = "IsSearchRunning";

        private bool _IsSearchRunning;
        public bool IsSearchRunning
        {
            get
            {
                return _IsSearchRunning;
            }
            set
            {
                _IsSearchRunning = value;
                RaisePropertyChanged(IsSearchRunningPropertyName);
            }
        }

        private string GetTextColumnQueryForAllTextColumns()
        {
            string query = @"select 
c.table_schema,
c.table_name,
c.column_name,
c.column_default,
c.is_nullable,
c.data_type, 
c.character_maximum_length,
c.numeric_precision,
c.numeric_precision_radix,
c.numeric_scale

from 
INFORMATION_SCHEMA.COLUMNS c
join
INFORMATION_SCHEMA.TABLES t
on
t.table_name=c.table_name
where 
data_type in ('varchar' , 'nvarchar', 'uniqueidentifier')
and t.table_type!='VIEW'
order by c.table_name, c.column_name
";

            return query;
        }

        private string GetTextColumnQueryForTable()
        {
            string query = @"select 
c.table_schema,
c.table_name,
c.column_name,
c.column_default,
c.is_nullable,
c.data_type, 
c.character_maximum_length,
c.numeric_precision,
c.numeric_precision_radix,
c.numeric_scale

from 
INFORMATION_SCHEMA.COLUMNS c
join
INFORMATION_SCHEMA.TABLES t
on
t.table_name=c.table_name
where 
data_type in ('varchar' , 'nvarchar', 'uniqueidentifier')
and t.table_type!='VIEW'
AND t.TABLE_NAME LIKE @TABLE_NAME
order by c.table_name, c.column_name
";

            return query;
        }
        private string GetTextColumnQueryForColumn()
        {
            string query = @"select 
c.table_schema,
c.table_name,
c.column_name,
c.column_default,
c.is_nullable,
c.data_type, 
c.character_maximum_length,
c.numeric_precision,
c.numeric_precision_radix,
c.numeric_scale

from 
INFORMATION_SCHEMA.COLUMNS c
join
INFORMATION_SCHEMA.TABLES t
on
t.table_name=c.table_name
where 
data_type in ('varchar' , 'nvarchar', 'uniqueidentifier')
and t.table_type!='VIEW'
AND c.COLUMN_NAME LIKE @COLUMN_NAME
order by c.table_name, c.column_name
";

            return query;
        }
        private string GetTextColumnQueryForTableAndColumn()
        {
            string query = @"select 
c.table_schema,
c.table_name,
c.column_name,
c.column_default,
c.is_nullable,
c.data_type, 
c.character_maximum_length,
c.numeric_precision,
c.numeric_precision_radix,
c.numeric_scale

from 
INFORMATION_SCHEMA.COLUMNS c
join
INFORMATION_SCHEMA.TABLES t
on
t.table_name=c.table_name
where 
data_type in ('varchar' , 'nvarchar', 'uniqueidentifier')
and t.table_type!='VIEW'
AND c.TABLE_NAME LIKE @TABLE_NAME
AND c.COLUMN_NAME LIKE @COLUMN_NAME
order by c.table_name, c.column_name
";

            return query;
        }

        private SqlCommand GetSqlCommand(string query)
        {
            var command = new SqlCommand(query);

            return command;
        }

        private void AddParameterForLikeStatement(SqlCommand command, string key)
        {
            var parameter = new SqlParameter();

            parameter.ParameterName = String.Format("@{0}", key);
            parameter.DbType = DbType.String;
            parameter.Value = String.Format("%{0}%", GetArgumentValue(key));

            command.Parameters.Add(parameter);
        }

        private SqlConnection GetSqlConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        private int ExecuteScalarGetInt32(SqlConnection connection, string query)
        {
            using (var command = GetSqlCommand(query))
            {
                command.Connection = connection;

                return (int)command.ExecuteScalar();
            }
        }

        /*
        private void AddRow(DataRow row, int recordCount, string query)
        {
            try
            {
                Console.WriteLine("AddRow(): preparing row...");
                var tempRow = Results.NewRow();

                tempRow["table schema"] = row["table_schema"];
                tempRow["table name"] = row["table_name"];
                tempRow["column name"] = row["column_name"];
                tempRow["record count"] = recordCount;
                tempRow["query"] = query;


                Console.WriteLine("AddRow(): adding row...");
                Results.Rows.Add(tempRow);
                RaisePropertyChanged(ResultsPropertyName);

                Console.WriteLine("AddRow(): row added");
            }
            catch(Exception ex)
            {
                Console.WriteLine("*** AddRow() error: {0}", ex);
            }
        }
        */

        public DataTable ExecuteCommand(SqlCommand command)
        {
            DataSet results = new DataSet();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                command.Connection = connection;

                using (var adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(results);
                }
            }

            return results.Tables[0];
        }

        public void ExecuteCommandAndFillDataTable(SqlCommand command, DataTable table)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                command.Connection = connection;

                using (var adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(table);
                }
            }
        }
    }
}
