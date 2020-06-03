using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Benday.SqlUtils.Api
{
    public class SqlDataExport
    {
        private string _ConnectionString;
        private IDatabaseUtility _DatabaseUtility;
        
        public SqlDataExport(IDatabaseUtility databaseUtility, string connectionString, string query)
        {
            _ConnectionString = connectionString;

            PopulateExportTableName(query);

            RunQuery(query);
        }

        public string ExportTableName { get; private set; }

        public TableDescription TableDescription
        {
            get;
            private set;
        }

        public DataTable QueryResults { get; private set; }

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

        private void PopulateExportTableName(string query)
        {
            var queryWithoutNewLines = query.Replace(Environment.NewLine, " ");

            const string queryTokenForFromStatement = " from ";

            var fromPosition = queryWithoutNewLines.IndexOf(queryTokenForFromStatement,
                StringComparison.InvariantCultureIgnoreCase);

            if (fromPosition == -1)
            {
                throw new InvalidOperationException("Could not locate table name in query.");
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

                this.ExportTableName = tableName.ToString();
            }
        }

        private void RunQuery(string query)
        {
            TableDescription = _DatabaseUtility.DescribeTable(ExportTableName);

            QueryResults = _DatabaseUtility.RunQuery(query);
        }

        public string GetInsertScript(bool generateIdentityInsert)
        {
            StringBuilder builder = new StringBuilder();

            string identityColumnName = TableDescription.GetIdentityColumnName();

            var tableName = ExportTableName;

            if (identityColumnName != null && generateIdentityInsert == true)
            {
                builder.Append("SET IDENTITY_INSERT ");
                builder.Append(tableName);
                builder.Append(" ON");
                builder.Append(Environment.NewLine);
                builder.Append(Environment.NewLine);
            }

            StringBuilder insertCommand = null;

            DataTable resultsToExportDataTable = QueryResults;

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
                        generateIdentityInsert == true))
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
                        generateIdentityInsert == true))
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

        public string GetMergeIntoScript()
        {
            StringBuilder builder = new StringBuilder();

            var tableName = ExportTableName;
            DataTable resultsToExportDataTable = QueryResults;

            var primaryKeyColumnName = TableDescription.PrimaryKeyColumnName;

            builder.AppendLine($"SET IDENTITY_INSERT {tableName} ON");
            builder.AppendLine("GO");
            builder.AppendLine();
            builder.AppendLine($"MERGE INTO {tableName} AS Target");
            builder.AppendLine($"USING(");
            builder.AppendLine($"    VALUES");

            var lastResultRow = resultsToExportDataTable.Rows[resultsToExportDataTable.Rows.Count - 1];

            bool needsComma = false;

            foreach (DataRow resultRow in resultsToExportDataTable.Rows)
            {
                var values = new CodeBuilder();

                values.IncreaseIndent();

                values.AppendLine("(");

                values.IncreaseIndent();

                needsComma = false;

                foreach (DataColumn resultColumn in resultsToExportDataTable.Columns)
                {
                    if (resultColumn.DataType == typeof(byte[]))
                    {
                        // skip byte arrays & concurrency columns
                        continue;
                    }

                    if (needsComma == true)
                    {
                        values.Append(", ");
                        values.NewLine();
                    }

                    needsComma = true;

                    if (resultRow[resultColumn.ColumnName] == DBNull.Value)
                    {
                        values.Append("NULL");
                    }
                    else if (resultColumn.DataType == typeof(bool))
                    {
                        bool valueAsBool = (bool)resultRow[resultColumn.ColumnName];

                        if (valueAsBool == true)
                        {
                            values.Append("1");
                        }
                        else
                        {
                            values.Append("0");
                        }
                    }
                    else if (resultColumn.DataType == typeof(string) ||
                        resultColumn.DataType == typeof(Guid) ||
                        resultColumn.DataType == typeof(DateTime))
                    {
                        values.Append("'");
                        values.Append(resultRow[resultColumn.ColumnName].ToString().Replace("'", "''"));
                        values.Append("'");
                    }
                    else
                    {
                        values.Append(resultRow[resultColumn.ColumnName].ToString());
                    }
                }

                values.NewLine();

                values.DecreaseIndent();

                if (resultRow != lastResultRow)
                {
                    values.AppendLine("),");
                }
                else
                {
                    values.AppendLine(")");
                }

                values.DecreaseIndent();

                builder.Append(values.ToString());
            }

            builder.AppendLine(")");

            builder.AppendLine("AS Source (");

            needsComma = false;

            builder.Append("\t");

            foreach (DataColumn resultColumn in resultsToExportDataTable.Columns)
            {
                if (resultColumn.DataType == typeof(byte[]))
                {
                    // skip byte arrays & concurrency columns
                    continue;
                }

                if (needsComma == true)
                {
                    builder.Append(", ");
                }

                needsComma = true;
                builder.Append(resultColumn.ColumnName);
            }

            builder.AppendLine();
            builder.AppendLine(")");

            builder.AppendLine($"ON Target.{primaryKeyColumnName} = Source.{primaryKeyColumnName}");
            builder.AppendLine("WHEN MATCHED THEN UPDATE SET");

            needsComma = false;

            foreach (DataColumn resultColumn in resultsToExportDataTable.Columns)
            {
                if (resultColumn.DataType == typeof(byte[]))
                {
                    // skip byte arrays & concurrency columns
                    continue;
                }
                else if (resultColumn.ColumnName == primaryKeyColumnName)
                {
                    // don't try to update the value for the primary key
                    continue;
                }

                if (needsComma == true)
                {
                    builder.AppendLine(", ");
                }

                needsComma = true;
                builder.Append("\t");
                builder.Append(resultColumn.ColumnName);
                builder.Append(" = Source.");
                builder.Append(resultColumn.ColumnName);
            }

            builder.AppendLine();

            builder.AppendLine("WHEN NOT MATCHED BY TARGET THEN");

            needsComma = false;
            builder.Append("\t");
            builder.Append("INSERT (");
            foreach (DataColumn resultColumn in resultsToExportDataTable.Columns)
            {
                if (resultColumn.DataType == typeof(byte[]))
                {
                    // skip byte arrays & concurrency columns
                    continue;
                }

                if (needsComma == true)
                {
                    builder.Append(", ");
                }

                needsComma = true;
                builder.Append(resultColumn.ColumnName);
            }

            builder.AppendLine(")");

            needsComma = false;
            builder.Append("\t");
            builder.Append("VALUES (");
            foreach (DataColumn resultColumn in resultsToExportDataTable.Columns)
            {
                if (resultColumn.DataType == typeof(byte[]))
                {
                    // skip byte arrays & concurrency columns
                    continue;
                }

                if (needsComma == true)
                {
                    builder.Append(", ");
                }

                needsComma = true;
                builder.Append(resultColumn.ColumnName);
            }

            builder.AppendLine(")");

            builder.AppendLine("WHEN NOT MATCHED BY SOURCE THEN DELETE;");
            builder.AppendLine("GO");

            return builder.ToString();
        }
    }
}
