using System.Data;
using System.Text;
using Benday.CommandsFramework;
using Benday.SqlUtils.Api;

namespace Benday.SqlUtils.ShovelCli;

public abstract class DatabaseCommandBase : SynchronousCommand
{
    protected const string ArgConnectionName = "connectionname";
    protected const string ArgConnectionString = "connectionstring";
    public const string ConnectionConfigPrefix = "connection:";
    public const string DefaultConnectionName = "default";

    public DatabaseCommandBase(CommandExecutionInfo info, ITextOutputProvider outputProvider)
        : base(info, outputProvider) { }

    protected const string ArgMatch = "match";

    protected void AddConnectionArguments(ArgumentCollection args)
    {
        args.AddString(ArgConnectionName)
            .AsNotRequired()
            .WithDescription("Name of a saved connection");
        args.AddString(ArgConnectionString)
            .AsNotRequired()
            .WithDescription("SQL Server connection string");
    }

    protected void AddMatchArgument(ArgumentCollection args)
    {
        args.AddString(ArgMatch)
            .WithAllowedValues("contains", "exact", "startswith", "endswith")
            .AsNotRequired()
            .WithDescription("Match method: contains (default), exact, startswith, endswith")
            .WithDefaultValue("contains");
    }
    protected string ApplyMatchMethod(string value)
    {
        var method = Arguments.HasValue(ArgMatch)
            ? Arguments.GetStringValue(ArgMatch).ToLowerInvariant()
            : "contains";

        return method switch
        {
            "exact" => value,
            "startswith" => $"{value}%",
            "endswith" => $"%{value}",
            _ => $"%{value}%"
        };
    }

    protected void ValidateConnectionArguments()
    {
        if (!Arguments.HasValue(ArgConnectionName) && !Arguments.HasValue(ArgConnectionString))
        {
            var defaultKey = $"{ConnectionConfigPrefix}{DefaultConnectionName}";
            if (!ExecutionInfo.Configuration.HasValue(defaultKey))
            {
                throw new KnownException(
                    $"You must provide either /{ArgConnectionName} or /{ArgConnectionString}, " +
                    $"or set a default connection with 'sqlutil addconnection /default'.");
            }
        }
    }

    protected string GetConnectionString()
    {
        if (Arguments.HasValue(ArgConnectionString))
        {
            return Arguments.GetStringValue(ArgConnectionString);
        }

        var connectionName = Arguments.HasValue(ArgConnectionName)
            ? Arguments.GetStringValue(ArgConnectionName)
            : DefaultConnectionName;

        var configKey = $"{ConnectionConfigPrefix}{connectionName}";

        if (!ExecutionInfo.Configuration.HasValue(configKey))
        {
            throw new KnownException(
                $"No saved connection named '{connectionName}'. " +
                $"Use 'sqlutil addconnection' to create one or pass /{ArgConnectionString} directly.");
        }

        return ExecutionInfo.Configuration.GetValue(configKey);
    }

    protected SqlServerDatabaseUtility CreateDatabaseUtility()
    {
        ValidateConnectionArguments();
        var connString = GetConnectionString();
        var util = new SqlServerDatabaseUtility();
        util.Initialize(connString);
        return util;
    }

    protected void WriteDataTable(DataTable? dataTable)
    {
        if (dataTable == null || dataTable.Rows.Count == 0)
        {
            WriteLine("No results found.");
            return;
        }

        var widths = new int[dataTable.Columns.Count];
        for (int i = 0; i < dataTable.Columns.Count; i++)
        {
            widths[i] = dataTable.Columns[i].ColumnName.Length;
        }

        foreach (DataRow row in dataTable.Rows)
        {
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                var val = row[i]?.ToString() ?? string.Empty;
                if (val.Length > widths[i]) widths[i] = val.Length;
            }
        }

        // Header
        var sb = new StringBuilder();
        for (int i = 0; i < dataTable.Columns.Count; i++)
        {
            if (i > 0) sb.Append("  ");
            sb.Append(dataTable.Columns[i].ColumnName.PadRight(widths[i]));
        }
        WriteLine(sb.ToString());

        // Separator
        sb.Clear();
        for (int i = 0; i < dataTable.Columns.Count; i++)
        {
            if (i > 0) sb.Append("  ");
            sb.Append(new string('-', widths[i]));
        }
        WriteLine(sb.ToString());

        // Rows
        foreach (DataRow row in dataTable.Rows)
        {
            sb.Clear();
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                if (i > 0) sb.Append("  ");
                var val = row[i]?.ToString() ?? string.Empty;
                sb.Append(val.PadRight(widths[i]));
            }
            WriteLine(sb.ToString());
        }

        WriteLine();
        WriteLine($"{dataTable.Rows.Count} row(s).");
    }
}
