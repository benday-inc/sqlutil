using Benday.CommandsFramework;

namespace Benday.SqlUtils.ShovelCli.Commands;

[Command(Name = "listdatabases",
    Description = "List all databases on a SQL Server instance",
    Category = "Schema")]
public class ListDatabasesCommand : DatabaseCommandBase
{
    public ListDatabasesCommand(CommandExecutionInfo info, ITextOutputProvider outputProvider)
        : base(info, outputProvider) { }

    public override ArgumentCollection GetArguments()
    {
        var args = new ArgumentCollection();
        AddConnectionArguments(args);
        return args;
    }

    protected override void OnExecute()
    {
        var util = CreateDatabaseUtility();

        var result = util.RunQuery(
            "SELECT name, database_id, create_date, state_desc FROM sys.databases ORDER BY name");

        WriteDataTable(result);
    }
}
