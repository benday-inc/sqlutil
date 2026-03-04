using Benday.CommandsFramework;

namespace Benday.SqlUtils.ShovelCli.Commands;

[Command(Name = "listconnections",
    Description = "List all saved database connections",
    Category = "Connections")]
public class ListConnectionsCommand : SynchronousCommand
{
    public ListConnectionsCommand(CommandExecutionInfo info, ITextOutputProvider outputProvider)
        : base(info, outputProvider) { }

    public override ArgumentCollection GetArguments()
    {
        return new ArgumentCollection();
    }

    protected override void OnExecute()
    {
        var allValues = ExecutionInfo.Configuration.GetValues();
        var connections = allValues
            .Where(kvp => kvp.Key.StartsWith(DatabaseCommandBase.ConnectionConfigPrefix))
            .ToList();

        if (connections.Count == 0)
        {
            WriteLine("No saved connections. Use 'shovel add-connection' to add one.");
            return;
        }

        var maxNameLen = connections.Max(c =>
            c.Key.Substring(DatabaseCommandBase.ConnectionConfigPrefix.Length).Length);
        maxNameLen = Math.Max(maxNameLen, 4); // "Name" header

        WriteLine($"{"Name".PadRight(maxNameLen)}  Connection String");
        WriteLine($"{new string('-', maxNameLen)}  {new string('-', 40)}");

        foreach (var kvp in connections.OrderBy(c => c.Key))
        {
            var name = kvp.Key.Substring(DatabaseCommandBase.ConnectionConfigPrefix.Length);
            WriteLine($"{name.PadRight(maxNameLen)}  {kvp.Value}");
        }

        WriteLine();
        WriteLine($"{connections.Count} connection(s).");
    }
}
