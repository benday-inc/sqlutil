using Benday.CommandsFramework;

namespace Benday.SqlUtils.ShovelCli.Commands;

[Command(Name = "compareprocs",
    Description = "Compare stored procedure and function definitions between two databases",
    Category = "Compare")]
public class CompareProcsCommand : CompareDatabaseCommandBase
{
    public CompareProcsCommand(CommandExecutionInfo info, ITextOutputProvider outputProvider)
        : base(info, outputProvider) { }

    public override ArgumentCollection GetArguments()
    {
        var args = new ArgumentCollection();
        AddTwoConnectionArguments(args);
        return args;
    }

    protected override void OnExecute()
    {
        ValidateTwoConnectionArguments();

        var conn1 = GetConnectionString1();
        var conn2 = GetConnectionString2();
        var db1Name = GetDb1Name();
        var db2Name = GetDb2Name();

        WriteLine($"Comparing stored procedures: {db1Name} vs {db2Name}...");
        var diffs = CompareStoredProcs(conn1, conn2, db1Name, db2Name);

        WriteLine($"Comparing functions: {db1Name} vs {db2Name}...");
        diffs.AddRange(CompareFunctions(conn1, conn2, db1Name, db2Name));

        WriteLine();
        WriteDiffs(diffs);
    }
}
