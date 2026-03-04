using Benday.CommandsFramework;

namespace Benday.SqlUtils.ShovelCli.Commands;

[Command(Name = "comparetables",
    Description = "Compare table and view schemas between two databases",
    Category = "Compare")]
public class CompareTablesCommand : CompareDatabaseCommandBase
{
    public CompareTablesCommand(CommandExecutionInfo info, ITextOutputProvider outputProvider)
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

        WriteLine($"Comparing tables: {db1Name} vs {db2Name}...");
        var diffs = CompareTables(conn1, conn2, db1Name, db2Name);

        WriteLine($"Comparing views: {db1Name} vs {db2Name}...");
        diffs.AddRange(CompareViews(conn1, conn2, db1Name, db2Name));

        WriteLine();
        WriteDiffs(diffs);
    }
}
