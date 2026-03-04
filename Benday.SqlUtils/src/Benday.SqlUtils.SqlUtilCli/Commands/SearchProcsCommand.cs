using Benday.CommandsFramework;

namespace Benday.SqlUtils.ShovelCli.Commands;

[Command(Name = "searchprocs",
    Description = "Search for stored procedures by name pattern",
    Category = "Search")]
public class SearchProcsCommand : DatabaseCommandBase
{
    public SearchProcsCommand(CommandExecutionInfo info, ITextOutputProvider outputProvider)
        : base(info, outputProvider) { }

    public override ArgumentCollection GetArguments()
    {
        var args = new ArgumentCollection();
        AddConnectionArguments(args);

        args.AddString("search")
            .AsRequired()
            .WithDescription("Stored procedure name search pattern");
        AddMatchArgument(args);

        return args;
    }

    protected override void OnExecute()
    {
        var util = CreateDatabaseUtility();
        var search = Arguments.GetStringValue("search");

        var queryArgs = new Dictionary<string, string>
        {
            { "STORED_PROCEDURE_NAME", ApplyMatchMethod(search) }
        };

        var query = @"SELECT SPECIFIC_SCHEMA as [Schema], SPECIFIC_NAME as [Name]
FROM INFORMATION_SCHEMA.ROUTINES
WHERE SPECIFIC_NAME LIKE @STORED_PROCEDURE_NAME
ORDER BY SPECIFIC_NAME";

        var result = util.RunQuery(query, queryArgs);
        WriteDataTable(result);
    }
}
