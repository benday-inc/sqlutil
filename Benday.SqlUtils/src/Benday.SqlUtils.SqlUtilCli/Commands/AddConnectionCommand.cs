using Benday.CommandsFramework;

namespace Benday.SqlUtils.ShovelCli.Commands;

[Command(Name = "addconnection",
    Description = "Add or update a saved database connection",
    Category = "Connections")]
public class AddConnectionCommand : SynchronousCommand
{
    public AddConnectionCommand(CommandExecutionInfo info, ITextOutputProvider outputProvider)
        : base(info, outputProvider) { }

    public override ArgumentCollection GetArguments()
    {
        var args = new ArgumentCollection();

        args.AddString("name")
            .AsRequired()
            .WithDescription("Name for the connection");
        args.AddString("connectionstring")
            .AsRequired()
            .WithDescription("SQL Server connection string");

        return args;
    }

    protected override void OnExecute()
    {
        var name = Arguments.GetStringValue("name");
        var connStr = Arguments.GetStringValue("connectionstring");
        var configKey = $"{DatabaseCommandBase.ConnectionConfigPrefix}{name}";

        ExecutionInfo.Configuration.SetValue(configKey, connStr);
        WriteLine($"Connection '{name}' saved.");
    }
}
