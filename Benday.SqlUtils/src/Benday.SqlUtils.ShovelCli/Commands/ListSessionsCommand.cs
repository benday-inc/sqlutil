using Benday.CommandsFramework;

namespace Benday.SqlUtils.ShovelCli.Commands;

[Command(Name = "listsessions",
    Description = "List active database sessions",
    Category = "Sessions")]
public class ListSessionsCommand : DatabaseCommandBase
{
    public ListSessionsCommand(CommandExecutionInfo info, ITextOutputProvider outputProvider)
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
        var result = util.RunQuery("exec sp_who2");
        WriteDataTable(result);
    }
}
