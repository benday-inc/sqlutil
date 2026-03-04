using Benday.CommandsFramework;

namespace Benday.SqlUtils.ShovelCli.Commands;

[Command(Name = "runquery",
    Description = "Run a SQL query and display the results",
    Category = "Query")]
public class RunQueryCommand : DatabaseCommandBase
{
    public RunQueryCommand(CommandExecutionInfo info, ITextOutputProvider outputProvider)
        : base(info, outputProvider) { }

    public override ArgumentCollection GetArguments()
    {
        var args = new ArgumentCollection();
        AddConnectionArguments(args);

        args.AddString("query")
            .AsRequired()
            .WithDescription("SQL query to execute");
        args.AddString("filename")
            .AsNotRequired()
            .WithDescription("Write results to a CSV file instead of stdout");

        return args;
    }

    protected override void OnExecute()
    {
        var util = CreateDatabaseUtility();
        var query = Arguments.GetStringValue("query");

        var result = util.RunQuery(query);

        if (Arguments.HasValue("filename"))
        {
            var fullPath = Path.GetFullPath(Arguments.GetStringValue("filename"));
            WriteCsv(result, fullPath);
            WriteLine($"Results written to '{fullPath}'.");
        }
        else
        {
            WriteDataTable(result);
        }
    }

    private void WriteCsv(System.Data.DataTable? dataTable, string path)
    {
        if (dataTable == null || dataTable.Rows.Count == 0)
        {
            File.WriteAllText(path, string.Empty);
            return;
        }

        var lines = new System.Text.StringBuilder();

        // Header
        lines.AppendLine(string.Join(",", dataTable.Columns
            .Cast<System.Data.DataColumn>()
            .Select(c => EscapeCsv(c.ColumnName))));

        // Rows
        foreach (System.Data.DataRow row in dataTable.Rows)
        {
            lines.AppendLine(string.Join(",", row.ItemArray
                .Select(v => EscapeCsv(v?.ToString() ?? string.Empty))));
        }

        File.WriteAllText(path, lines.ToString());
    }

    private static string EscapeCsv(string value)
    {
        if (value.Contains(',') || value.Contains('"') || value.Contains('\n'))
            return $"\"{value.Replace("\"", "\"\"")}\"";
        return value;
    }
}
