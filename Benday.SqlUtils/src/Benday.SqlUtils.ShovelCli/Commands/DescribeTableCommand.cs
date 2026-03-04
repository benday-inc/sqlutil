using Benday.CommandsFramework;

namespace Benday.SqlUtils.ShovelCli.Commands;

[Command(Name = "describetable",
    Description = "Show columns and types for a specific table",
    Category = "Schema")]
public class DescribeTableCommand : DatabaseCommandBase
{
    public DescribeTableCommand(CommandExecutionInfo info, ITextOutputProvider outputProvider)
        : base(info, outputProvider) { }

    public override ArgumentCollection GetArguments()
    {
        var args = new ArgumentCollection();
        AddConnectionArguments(args);

        args.AddString("table")
            .AsRequired()
            .WithDescription("Name of the table to describe");

        return args;
    }

    protected override void OnExecute()
    {
        var util = CreateDatabaseUtility();
        var tableName = Arguments.GetStringValue("table");

        var description = util.DescribeTable(tableName);

        if (description.Columns.Count == 0)
        {
            WriteLine($"Table '{tableName}' not found or has no columns.");
            return;
        }

        if (!string.IsNullOrEmpty(description.PrimaryKeyColumnName))
        {
            WriteLine($"Primary Key: {description.PrimaryKeyColumnName}");
            WriteLine();
        }

        var maxCol = Math.Max(6, description.Columns.Max(c => c.ColumnName.Length));
        var maxType = Math.Max(9, description.Columns.Max(c => c.DataType.Length));

        WriteLine($"{"Column".PadRight(maxCol)}  {"Data Type".PadRight(maxType)}  {"Nullable"}  {"Identity"}");
        WriteLine($"{new string('-', maxCol)}  {new string('-', maxType)}  {new string('-', 8)}  {new string('-', 8)}");

        foreach (var col in description.Columns)
        {
            var nullable = col.IsNullable ? "Yes" : "No";
            var identity = col.IsIdentity ? "Yes" : "No";
            WriteLine($"{col.ColumnName.PadRight(maxCol)}  {col.DataType.PadRight(maxType)}  {nullable.PadRight(8)}  {identity}");
        }

        WriteLine();
        WriteLine($"{description.Columns.Count} column(s).");
    }
}
