using Benday.CommandsFramework;

namespace Benday.SqlUtils.ShovelCli.Commands;

[Command(Name = "searchcolumns",
    Description = "Search for columns by name pattern, optionally filtered by data type",
    Category = "Search")]
public class SearchColumnsCommand : DatabaseCommandBase
{
    public SearchColumnsCommand(CommandExecutionInfo info, ITextOutputProvider outputProvider)
        : base(info, outputProvider) { }

    public override ArgumentCollection GetArguments()
    {
        var args = new ArgumentCollection();
        AddConnectionArguments(args);

        args.AddString("search")
            .AsNotRequired()
            .WithDescription("Column name search pattern (supports % wildcards)");
        args.AddString("datatype")
            .AsNotRequired()
            .WithDescription("Data type filter pattern (e.g. varchar, int)");
        AddMatchArgument(args);

        return args;
    }

    protected override void OnExecute()
    {
        var hasSearch = Arguments.HasValue("search");
        var hasDataType = Arguments.HasValue("datatype");

        if (!hasSearch && !hasDataType)
        {
            throw new KnownException("You must provide /search and/or /datatype.");
        }

        var util = CreateDatabaseUtility();
        var queryArgs = new Dictionary<string, string>();
        string query;

        if (hasSearch && hasDataType)
        {
            queryArgs["COLUMN_NAME"] = ApplyMatchMethod(Arguments.GetStringValue("search"));
            queryArgs["COLUMN_DATA_TYPE"] = $"%{Arguments.GetStringValue("datatype")}%";
            query = @"select table_schema, table_name, column_name, data_type, character_maximum_length
from information_schema.columns where
data_type like @COLUMN_DATA_TYPE and
column_name like @COLUMN_NAME
ORDER BY COLUMN_NAME, TABLE_NAME";
        }
        else if (hasSearch)
        {
            queryArgs["COLUMN_NAME"] = ApplyMatchMethod(Arguments.GetStringValue("search"));
            query = @"select table_schema, table_name, column_name, data_type, character_maximum_length
from information_schema.columns where
column_name like @COLUMN_NAME
ORDER BY COLUMN_NAME, TABLE_NAME";
        }
        else
        {
            queryArgs["COLUMN_DATA_TYPE"] = $"%{Arguments.GetStringValue("datatype")}%";
            query = @"select table_schema, table_name, column_name, data_type, character_maximum_length
from information_schema.columns where
data_type like @COLUMN_DATA_TYPE
ORDER BY COLUMN_NAME, TABLE_NAME";
        }

        var result = util.RunQuery(query, queryArgs);
        WriteDataTable(result);
    }
}
