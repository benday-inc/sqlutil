using Benday.CommandsFramework;

namespace Benday.SqlUtils.ShovelCli.Commands;

[Command(Name = "searchtables",
    Description = "Search for tables by name pattern",
    Category = "Search")]
public class SearchTablesCommand : DatabaseCommandBase
{
    public SearchTablesCommand(CommandExecutionInfo info, ITextOutputProvider outputProvider)
        : base(info, outputProvider) { }

    public override ArgumentCollection GetArguments()
    {
        var args = new ArgumentCollection();
        AddConnectionArguments(args);

        args.AddString("search")
            .AsRequired()
            .WithDescription("Table name search pattern");
        AddMatchArgument(args);

        return args;
    }

    protected override void OnExecute()
    {
        var util = CreateDatabaseUtility();
        var search = Arguments.GetStringValue("search");

        var queryArgs = new Dictionary<string, string>
        {
            { "TABLE_NAME", ApplyMatchMethod(search) }
        };

        var query = @"select table_schema, table_name
from INFORMATION_SCHEMA.TABLES
where table_name like @TABLE_NAME
order by table_name";

        var result = util.RunQuery(query, queryArgs);
        WriteDataTable(result);
    }
}
