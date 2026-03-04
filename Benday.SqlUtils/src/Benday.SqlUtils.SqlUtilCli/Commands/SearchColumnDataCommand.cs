using System.Data;
using System.Data.SqlClient;
using Benday.CommandsFramework;

namespace Benday.SqlUtils.ShovelCli.Commands;

[Command(Name = "searchcolumndata",
    Description = "Search for text within table column data",
    Category = "Search")]
public class SearchColumnDataCommand : DatabaseCommandBase
{
    public SearchColumnDataCommand(CommandExecutionInfo info, ITextOutputProvider outputProvider)
        : base(info, outputProvider) { }

    public override ArgumentCollection GetArguments()
    {
        var args = new ArgumentCollection();
        AddConnectionArguments(args);

        args.AddString("table")
            .AsNotRequired()
            .WithDescription("Table name filter (supports % wildcards)");
        args.AddString("column")
            .AsNotRequired()
            .WithDescription("Column name filter (supports % wildcards)");
        args.AddString("search")
            .AsRequired()
            .WithDescription("Text to search for in column data");

        return args;
    }

    protected override void OnExecute()
    {
        ValidateConnectionArguments();
        var connString = GetConnectionString();
        var searchText = Arguments.GetStringValue("search");

        var hasTable = Arguments.HasValue("table");
        var hasColumn = Arguments.HasValue("column");

        string columnsQuery;
        var queryArgs = new Dictionary<string, string>();

        if (hasTable && hasColumn)
        {
            columnsQuery = GetTextColumnQueryForTableAndColumn();
            queryArgs["TABLE_NAME"] = $"%{Arguments.GetStringValue("table")}%";
            queryArgs["COLUMN_NAME"] = $"%{Arguments.GetStringValue("column")}%";
        }
        else if (hasTable)
        {
            columnsQuery = GetTextColumnQueryForTable();
            queryArgs["TABLE_NAME"] = $"%{Arguments.GetStringValue("table")}%";
        }
        else if (hasColumn)
        {
            columnsQuery = GetTextColumnQueryForColumn();
            queryArgs["COLUMN_NAME"] = $"%{Arguments.GetStringValue("column")}%";
        }
        else
        {
            columnsQuery = GetTextColumnQueryForAllTextColumns();
        }

        var util = new Api.SqlServerDatabaseUtility();
        util.Initialize(connString);

        var textColumns = util.RunQuery(columnsQuery, queryArgs);

        if (textColumns == null || textColumns.Rows.Count == 0)
        {
            WriteLine("No matching text columns found.");
            return;
        }

        WriteLine($"Searching {textColumns.Rows.Count} column(s)...");
        WriteLine();

        var foundCount = 0;

        using var connection = new SqlConnection(connString);
        connection.Open();

        foreach (DataRow row in textColumns.Rows)
        {
            var schema = row["table_schema"].ToString();
            var table = row["table_name"].ToString();
            var column = row["column_name"].ToString();

            var countQuery = $"select count(*) as recordCount from [{schema}].[{table}] where [{column}] like @SEARCH_TEXT";

            using var cmd = new SqlCommand(countQuery, connection);
            cmd.Parameters.Add(new SqlParameter("@SEARCH_TEXT", SqlDbType.NVarChar) { Value = $"%{searchText}%" });

            try
            {
                var count = (int)cmd.ExecuteScalar();
                if (count > 0)
                {
                    WriteLine($"  [{schema}].[{table}].[{column}] - {count} match(es)");
                    foundCount++;
                }
            }
            catch
            {
                // Skip columns that cause errors (e.g., computed columns)
            }
        }

        WriteLine();
        if (foundCount == 0)
        {
            WriteLine("No matches found.");
        }
        else
        {
            WriteLine($"Found matches in {foundCount} column(s).");
        }
    }

    private static string GetTextColumnQueryForAllTextColumns() =>
        @"select c.table_schema, c.table_name, c.column_name
from INFORMATION_SCHEMA.COLUMNS c
join INFORMATION_SCHEMA.TABLES t on t.table_name=c.table_name
where data_type in ('varchar', 'nvarchar', 'uniqueidentifier')
and t.table_type!='VIEW'
order by c.table_name, c.column_name";

    private static string GetTextColumnQueryForTable() =>
        @"select c.table_schema, c.table_name, c.column_name
from INFORMATION_SCHEMA.COLUMNS c
join INFORMATION_SCHEMA.TABLES t on t.table_name=c.table_name
where data_type in ('varchar', 'nvarchar', 'uniqueidentifier')
and t.table_type!='VIEW'
AND t.TABLE_NAME LIKE @TABLE_NAME
order by c.table_name, c.column_name";

    private static string GetTextColumnQueryForColumn() =>
        @"select c.table_schema, c.table_name, c.column_name
from INFORMATION_SCHEMA.COLUMNS c
join INFORMATION_SCHEMA.TABLES t on t.table_name=c.table_name
where data_type in ('varchar', 'nvarchar', 'uniqueidentifier')
and t.table_type!='VIEW'
AND c.COLUMN_NAME LIKE @COLUMN_NAME
order by c.table_name, c.column_name";

    private static string GetTextColumnQueryForTableAndColumn() =>
        @"select c.table_schema, c.table_name, c.column_name
from INFORMATION_SCHEMA.COLUMNS c
join INFORMATION_SCHEMA.TABLES t on t.table_name=c.table_name
where data_type in ('varchar', 'nvarchar', 'uniqueidentifier')
and t.table_type!='VIEW'
AND c.TABLE_NAME LIKE @TABLE_NAME
AND c.COLUMN_NAME LIKE @COLUMN_NAME
order by c.table_name, c.column_name";
}
