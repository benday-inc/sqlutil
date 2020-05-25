using Benday.Presentation;
using Benday.SqlUtils.Core;
using Benday.SqlUtils.Core.ViewModels;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Input;

namespace Benday.SqlUtils.Api.ViewModels
{

    public class DataExportViewModel : DatabaseUtilityViewModelBase
    {
        private IDatabaseUtility _DatabaseUtility;

        public DataExportViewModel(
            IDatabaseConnectionStringRepository repository,
            IDatabaseUtility queryExecuter) :
                    base(repository)
        {
            if (queryExecuter == null)
            {
                throw new ArgumentNullException("queryExecuter", "Argument cannot be null.");
            }

            _DatabaseUtility = queryExecuter;
        }

        protected override void OnInitialize()
        {
            _Query = new ViewModelField<string>();
            _GeneratedQuery = new ViewModelField<string>();
            _GenerateIdentityInsert = new ViewModelField<bool>();
            _ExportTableName = new ViewModelField<string>();
            _Message = new ViewModelField<string>();
            _QueryResults = new ViewModelField<DataTable>();

            _GeneratedQuery.IsEnabled = false;
            _ExportTableName.IsEnabled = false;
            _Message.IsVisible = false;
            _QueryResults.IsEnabled = false;
        }

        private const string QueryPropertyName = "Query";

        private ViewModelField<string> _Query;
        public ViewModelField<string> Query
        {
            get
            {
                return _Query;
            }
            set
            {
                _Query = value;
                RaisePropertyChanged(QueryPropertyName);
            }
        }

        private const string ExportTableNamePropertyName = "ExportTableName";

        private ViewModelField<string> _ExportTableName;
        public ViewModelField<string> ExportTableName
        {
            get
            {
                return _ExportTableName;
            }
            set
            {
                _ExportTableName = value;
                RaisePropertyChanged(ExportTableNamePropertyName);
            }
        }

        private const string GenerateIdentityInsertPropertyName = "GenerateIdentityInsert";

        private ViewModelField<bool> _GenerateIdentityInsert;
        public ViewModelField<bool> GenerateIdentityInsert
        {
            get
            {
                return _GenerateIdentityInsert;
            }
            set
            {
                _GenerateIdentityInsert = value;
                RaisePropertyChanged(GenerateIdentityInsertPropertyName);
            }
        }

        private const string GeneratedQueryPropertyName = "GeneratedQuery";

        private ViewModelField<string> _GeneratedQuery;
        public ViewModelField<string> GeneratedQuery
        {
            get
            {
                return _GeneratedQuery;
            }
            set
            {
                _GeneratedQuery = value;
                RaisePropertyChanged(GeneratedQueryPropertyName);
            }
        }

        private ICommand _RunQueryCommand;
        public ICommand RunQueryCommand
        {
            get
            {
                if (_RunQueryCommand == null)
                {
                    _RunQueryCommand = new RelayCommand(RunQuery);
                }

                return _RunQueryCommand;
            }
        }

        private void RunQuery()
        {
            PopulateExportTableName(Query.Value);

            if (ExportTableName.IsValid == true)
            {
                _TableDescription = _DatabaseUtility.DescribeTable(ExportTableName.Value);

                _QueryResults.Value = _DatabaseUtility.RunQuery(Query.Value);
            }            
        }

        private const string QueryResultsPropertyName = "QueryResults";

        private ViewModelField<DataTable> _QueryResults;
        public ViewModelField<DataTable> QueryResults
        {
            get
            {
                return _QueryResults;
            }
            set
            {
                _QueryResults = value;
                RaisePropertyChanged(QueryResultsPropertyName);
            }
        }

        private const string TableDescriptionPropertyName = "TableDescription";

        private TableDescription _TableDescription;
        public TableDescription TableDescription
        {
            get
            {
                return _TableDescription;
            }
            set
            {
                _TableDescription = value;
                RaisePropertyChanged(TableDescriptionPropertyName);
            }
        }

        private const string MessagePropertyName = "Message";

        private ViewModelField<string> _Message;
        public ViewModelField<string> Message
        {
            get
            {
                return _Message;
            }
            set
            {
                _Message = value;
                RaisePropertyChanged(MessagePropertyName);
            }
        }

        private ICommand _CreateInsertScriptCommand;
        public ICommand CreateInsertScriptCommand
        {
            get
            {
                if (_CreateInsertScriptCommand == null)
                {
                    _CreateInsertScriptCommand = new RelayCommand(CreateInsertScript);
                }

                return _CreateInsertScriptCommand;
            }
        }
        private void CreateInsertScript()
        {
            if (ExportTableName.IsValid == true && 
                _TableDescription != null &&
                QueryResults.Value != null)
            {
                var script = GetInsertScript();

                GeneratedQuery.IsEnabled = true;
                GeneratedQuery.Value = script;                
            }
            else
            {
                GeneratedQuery.IsEnabled = false;
                GeneratedQuery.Value = String.Empty;
                Message.IsVisible = true;
                Message.Value = "Table name to export is not valid, query data has not been loaded, and/or table description has not been loaded.";
            }
        }

        private ICommand _CreateMergeIntoScriptCommand;
        public ICommand CreateMergeIntoScriptCommand
        {
            get
            {
                if (_CreateMergeIntoScriptCommand == null)
                {
                    _CreateMergeIntoScriptCommand = new RelayCommand(CreateMergeIntoScript);
                }

                return _CreateMergeIntoScriptCommand;
            }
        }
        private void CreateMergeIntoScript()
        {
            throw new NotImplementedException();
        }

        private void PopulateExportTableName(string query)
        {
            var queryWithoutNewLines = query.Replace(Environment.NewLine, " ");

            const string queryTokenForFromStatement = " from ";

            var fromPosition = queryWithoutNewLines.IndexOf(queryTokenForFromStatement,
                StringComparison.InvariantCultureIgnoreCase);

            if (fromPosition == -1)
            {
                this.ExportTableName.Value = "(table name not found)";
                this.ExportTableName.IsValid = false;
                this.ExportTableName.IsEnabled = false;
                this.ExportTableName.ValidationMessage = "Could not locate table name in query.";
            }
            else
            {
                StringBuilder builder = new StringBuilder();

                bool foundFirstChar = false;

                int fromStatementLength = queryTokenForFromStatement.Length;

                for (int index = fromPosition + fromStatementLength; index < queryWithoutNewLines.Length; index++)
                {
                    if (foundFirstChar == false)
                    {
                        while (queryWithoutNewLines[index] == ' ')
                        {
                            index++;
                        }

                        foundFirstChar = true;
                    }

                    if (queryWithoutNewLines[index] == ' ')
                    {
                        break;
                    }
                    else
                    {
                        builder.Append(queryWithoutNewLines[index]);
                    }
                }

                var tableName = builder.ToString().Trim();
                
                this.ExportTableName.Value = tableName.ToString();
                this.ExportTableName.IsValid = true;
                this.ExportTableName.IsEnabled = false;
            }
        }

        private static StringBuilder FormatTableName(string tableName)
        {
            StringBuilder tableNameFormatter = new StringBuilder();

            if (tableName.StartsWith("[") == false)
            {
                tableNameFormatter.Append("[");
            }

            tableNameFormatter.Append(tableName);

            if (tableName.EndsWith("]") == false)
            {
                tableNameFormatter.Append("]");
            }

            return tableNameFormatter;
        }

        private string GetInsertScript()
        {
            StringBuilder builder = new StringBuilder();

            string identityColumnName = _TableDescription.GetIdentityColumnName();

            var tableName = ExportTableName.Value;
            
            if (identityColumnName != null && GenerateIdentityInsert.Value == true)
            {
                builder.Append("SET IDENTITY_INSERT ");
                builder.Append(tableName);
                builder.Append(" ON");
                builder.Append(Environment.NewLine);
                builder.Append(Environment.NewLine);
            }

            StringBuilder insertCommand = null;

            DataTable resultsToExportDataTable = this.QueryResults.Value;

            foreach (DataRow resultRow in resultsToExportDataTable.Rows)
            {
                insertCommand = new StringBuilder();

                insertCommand.Append("INSERT INTO ");
                insertCommand.Append(tableName);
                insertCommand.Append(" ");
                insertCommand.Append(Environment.NewLine);
                insertCommand.Append("(");

                bool needsComma = false;

                foreach (DataColumn resultColumn in resultsToExportDataTable.Columns)
                {
                    if (resultColumn.DataType == typeof(byte[]))
                    {
                        // skip byte arrays & concurrency columns
                        continue;
                    }

                    if (resultColumn.ColumnName != identityColumnName ||
                        (resultColumn.ColumnName == identityColumnName && 
                        GenerateIdentityInsert.Value == true))
                    {
                        if (needsComma == true)
                        {
                            insertCommand.Append(", ");
                            insertCommand.Append(Environment.NewLine);
                        }

                        insertCommand.Append(resultColumn.ColumnName);
                        needsComma = true;
                    }
                }

                insertCommand.Append(")");
                insertCommand.Append(Environment.NewLine);

                insertCommand.Append("VALUES (");
                insertCommand.Append(Environment.NewLine);

                needsComma = false;

                foreach (DataColumn resultColumn in resultsToExportDataTable.Columns)
                {
                    if (resultColumn.DataType == typeof(byte[]))
                    {
                        // skip byte arrays & concurrency columns
                        continue;
                    }

                    if (resultColumn.ColumnName != identityColumnName ||
                        (resultColumn.ColumnName == identityColumnName && 
                        GenerateIdentityInsert.Value == true))
                    {
                        if (needsComma == true)
                        {
                            insertCommand.Append(", ");
                            insertCommand.Append(Environment.NewLine);
                        }

                        if (resultRow[resultColumn.ColumnName] == DBNull.Value)
                        {
                            insertCommand.Append("NULL");
                        }
                        else if (resultColumn.DataType == typeof(bool))
                        {
                            bool valueAsBool = (bool)resultRow[resultColumn.ColumnName];

                            if (valueAsBool == true)
                            {
                                insertCommand.Append("1");
                            }
                            else
                            {
                                insertCommand.Append("0");
                            }
                        }
                        else if (resultColumn.DataType == typeof(string) ||
                            resultColumn.DataType == typeof(Guid) ||
                            resultColumn.DataType == typeof(DateTime))
                        {
                            insertCommand.Append("'");
                            insertCommand.Append(resultRow[resultColumn.ColumnName].ToString().Replace("'", "''"));
                            insertCommand.Append("'");
                        }
                        else
                        {
                            insertCommand.Append(resultRow[resultColumn.ColumnName].ToString());
                        }

                        needsComma = true;
                    }
                }

                insertCommand.Append(")");
                insertCommand.Append(Environment.NewLine);

                insertCommand.Append(Environment.NewLine);

                builder.Append(insertCommand.ToString());
            }

            return builder.ToString();
        }
    }
}
