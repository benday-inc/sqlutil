using Benday.CommandsFramework;
using Benday.SqlUtils.Api;

namespace Benday.SqlUtils.ShovelCli.Commands;

[Command(Name = "exportdata",
    Description = "Export query results as INSERT or MERGE SQL statements",
    Category = "Export")]
public class ExportDataCommand : DatabaseCommandBase
{
    public ExportDataCommand(CommandExecutionInfo info, ITextOutputProvider outputProvider)
        : base(info, outputProvider) { }

    public override ArgumentCollection GetArguments()
    {
        var args = new ArgumentCollection();
        AddConnectionArguments(args);

        args.AddString("query")
            .AsRequired()
            .WithDescription("SELECT query to export data from");
        args.AddString("scripttype")
            .AsRequired()
            .WithDescription("Script type: insert, identityinsert, or mergeinto");
        args.AddString("filename")
            .AsNotRequired()
            .WithDescription("Output file path (default: stdout)");

        return args;
    }

    protected override void OnExecute()
    {
        var util = CreateDatabaseUtility();
        var query = Arguments.GetStringValue("query");
        var scriptType = Arguments.GetStringValue("scripttype").ToLowerInvariant();

        var exporter = new SqlDataExport(util, query);

        string result = scriptType switch
        {
            "insert" => exporter.GetInsertScript(false),
            "identityinsert" => exporter.GetInsertScript(true),
            "mergeinto" => exporter.GetMergeIntoScript(),
            _ => throw new KnownException(
                $"Invalid script type '{scriptType}'. Valid values: insert, identityinsert, mergeinto.")
        };

        if (string.IsNullOrWhiteSpace(result))
        {
            WriteLine("No data returned from query.");
            return;
        }

        if (Arguments.HasValue("filename"))
        {
            var filename = Arguments.GetStringValue("filename");
            var fullPath = Path.GetFullPath(filename);
            File.WriteAllText(fullPath, result);
            WriteLine($"Script written to '{fullPath}'.");
        }
        else
        {
            WriteLine(result);
        }
    }
}
