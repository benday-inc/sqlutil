using Benday.CommandsFramework;
using Benday.SqlUtils.Api;

namespace Benday.SqlUtils.ShovelCli.Commands;

[Command(Name = "addconnection",
    Description = "Add or update a saved database connection",
    Category = "Connections")]
public class AddConnectionCommand : SynchronousCommand
{
    public AddConnectionCommand(CommandExecutionInfo info, ITextOutputProvider outputProvider)
        : base(info, outputProvider) { }

    public override ArgumentCollection GetArguments()
    {
        var args = new ArgumentCollection();

        args.AddString("name")
            .AsRequired()
            .WithDescription("Name for the connection");

        args.AddString("connectionstring")
            .AsNotRequired()
            .WithDescription("Full SQL Server connection string (alternative to specifying individual fields)");

        args.AddString("server")
            .AsNotRequired()
            .WithDescription("SQL Server hostname or instance (e.g. localhost or server\\instance)");
        args.AddString("database")
            .AsNotRequired()
            .WithDescription("Database name");
        args.AddBoolean("integratedsecurity")
            .AsNotRequired()
            .AllowEmptyValue()
            .WithDescription("Use Windows integrated security instead of username/password");
        args.AddString("username")
            .AsNotRequired()
            .WithDescription("SQL Server login username");
        args.AddString("password")
            .AsNotRequired()
            .WithDescription("SQL Server login password");
        args.AddBoolean("trustservercertificate")
            .AsNotRequired()
            .AllowEmptyValue()
            .WithDescription("Trust the server certificate (default: true)")
            .WithDefaultValue(true);

        args.AddBoolean("default")
            .AsNotRequired()
            .AllowEmptyValue()
            .WithDescription("Also save this connection as the default");

        return args;
    }

    protected override void OnExecute()
    {
        var name = Arguments.GetStringValue("name");
        var connStr = BuildConnectionString();
        var configKey = $"{DatabaseCommandBase.ConnectionConfigPrefix}{name}";

        ExecutionInfo.Configuration.SetValue(configKey, connStr);
        WriteLine($"Connection '{name}' saved.");
        WriteLine($"Connection string: {connStr}");

        if (Arguments.GetBooleanValue("default"))
        {
            var defaultKey = $"{DatabaseCommandBase.ConnectionConfigPrefix}{DatabaseCommandBase.DefaultConnectionName}";
            ExecutionInfo.Configuration.SetValue(defaultKey, connStr);
            WriteLine($"Connection '{name}' set as default.");
        }
    }

    private string BuildConnectionString()
    {
        if (Arguments.HasValue("connectionstring"))
        {
            return Arguments.GetStringValue("connectionstring");
        }

        if (!Arguments.HasValue("server"))
            throw new KnownException("You must provide either /connectionstring or /server (plus /database and auth options).");
        if (!Arguments.HasValue("database"))
            throw new KnownException("You must provide /database when building a connection string from individual fields.");

        var useIntegrated = Arguments.HasValue("integratedsecurity") && Arguments.GetBooleanValue("integratedsecurity");

        if (!useIntegrated && (!Arguments.HasValue("username") || !Arguments.HasValue("password")))
            throw new KnownException("You must provide /username and /password, or use /integratedsecurity.");

        var conn = new DatabaseConnectionString
        {
            Server = Arguments.GetStringValue("server"),
            Database = Arguments.GetStringValue("database"),
            UseIntegratedSecurity = useIntegrated,
            TrustServerCertificate = Arguments.GetBooleanValue("trustservercertificate")
        };

        if (!useIntegrated)
        {
            conn.Username = Arguments.GetStringValue("username");
            conn.Password = Arguments.GetStringValue("password");
        }

        return conn.ConnectionString;
    }
}
