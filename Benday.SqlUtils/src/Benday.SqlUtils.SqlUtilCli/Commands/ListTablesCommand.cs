using Benday.CommandsFramework;

namespace Benday.SqlUtils.ShovelCli.Commands;

[Command(Name = "listtables",
    Description = "List all tables in a database",
    Category = "Schema")]
public class ListTablesCommand : DatabaseCommandBase
{
    public ListTablesCommand(CommandExecutionInfo info, ITextOutputProvider outputProvider)
        : base(info, outputProvider) { }

    public override ArgumentCollection GetArguments()
    {
        var args = new ArgumentCollection();
        AddConnectionArguments(args);

        args.AddString("schema")
            .AsNotRequired()
            .WithDescription("Filter by schema name (e.g. dbo)");

        return args;
    }

    protected override void OnExecute()
    {
        var util = CreateDatabaseUtility();

        string query;
        Dictionary<string, string>? queryArgs = null;

        if (Arguments.HasValue("schema"))
        {
            queryArgs = new Dictionary<string, string>
            {
                ["TABLE_SCHEMA"] = Arguments.GetStringValue("schema")
            };
            query = @"SELECT table_schema, table_name, table_type
FROM information_schema.tables
WHERE table_schema = @TABLE_SCHEMA
ORDER BY table_schema, table_name";
        }
        else
        {
            query = @"SELECT table_schema, table_name, table_type
FROM information_schema.tables
ORDER BY table_schema, table_name";
        }

        var result = util.RunQuery(query, queryArgs);
        WriteDataTable(result);
    }
}
