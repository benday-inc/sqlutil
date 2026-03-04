using Benday.CommandsFramework;

namespace Benday.SqlUtils.ShovelCli.Commands;

[Command(Name = "testconnection",
    Description = "Test connectivity to a database",
    Category = "Connections")]
public class TestConnectionCommand : DatabaseCommandBase
{
    public TestConnectionCommand(CommandExecutionInfo info, ITextOutputProvider outputProvider)
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

        try
        {
            var result = util.RunQuery("SELECT 1 AS Connected");

            if (result != null && result.Rows.Count > 0)
            {
                WriteLine("Connection successful.");
            }
            else
            {
                WriteLine("Connection failed: no result returned.");
                Environment.ExitCode = 1;
            }
        }
        catch (Exception ex)
        {
            throw new KnownException($"Connection failed: {ex.Message}");
        }
    }
}
