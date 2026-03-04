using Benday.CommandsFramework;

namespace Benday.SqlUtils.ShovelCli.Commands;

[Command(Name = "removeconnection",
    Description = "Remove a saved database connection",
    Category = "Connections")]
public class RemoveConnectionCommand : SynchronousCommand
{
    public RemoveConnectionCommand(CommandExecutionInfo info, ITextOutputProvider outputProvider)
        : base(info, outputProvider) { }

    public override ArgumentCollection GetArguments()
    {
        var args = new ArgumentCollection();

        args.AddString("name")
            .AsRequired()
            .WithDescription("Name of the connection to remove");

        return args;
    }

    protected override void OnExecute()
    {
        var name = Arguments.GetStringValue("name");
        var configKey = $"{DatabaseCommandBase.ConnectionConfigPrefix}{name}";

        if (!ExecutionInfo.Configuration.HasValue(configKey))
        {
            throw new KnownException($"No saved connection named '{name}'.");
        }

        ExecutionInfo.Configuration.RemoveValue(configKey);
        WriteLine($"Connection '{name}' removed.");
    }
}
