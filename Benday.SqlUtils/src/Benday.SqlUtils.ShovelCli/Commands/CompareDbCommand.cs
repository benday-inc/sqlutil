using Benday.CommandsFramework;

namespace Benday.SqlUtils.ShovelCli.Commands;

[Command(Name = "comparedb",
    Description = "Full schema comparison between two databases (tables, views, procs, functions, foreign keys)",
    Category = "Compare")]
public class CompareDbCommand : CompareDatabaseCommandBase
{
    public CompareDbCommand(CommandExecutionInfo info, ITextOutputProvider outputProvider)
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

        var diffs = new List<DbDiff>();

        WriteLine($"Comparing tables: {db1Name} vs {db2Name}...");
        diffs.AddRange(CompareTables(conn1, conn2, db1Name, db2Name));

        WriteLine($"Comparing views: {db1Name} vs {db2Name}...");
        diffs.AddRange(CompareViews(conn1, conn2, db1Name, db2Name));

        WriteLine($"Comparing stored procedures: {db1Name} vs {db2Name}...");
        diffs.AddRange(CompareStoredProcs(conn1, conn2, db1Name, db2Name));

        WriteLine($"Comparing functions: {db1Name} vs {db2Name}...");
        diffs.AddRange(CompareFunctions(conn1, conn2, db1Name, db2Name));

        WriteLine($"Comparing foreign keys: {db1Name} vs {db2Name}...");
        diffs.AddRange(CompareForeignKeys(conn1, conn2, db1Name, db2Name));

        WriteLine();
        WriteDiffs(diffs);
    }
}
