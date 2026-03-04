using Benday.CommandsFramework;

namespace Benday.SqlUtils.ShovelCli.Commands;

[Command(Name = "searchprocparams",
    Description = "Search for stored procedure parameters by name pattern",
    Category = "Search")]
public class SearchProcParamsCommand : DatabaseCommandBase
{
    public SearchProcParamsCommand(CommandExecutionInfo info, ITextOutputProvider outputProvider)
        : base(info, outputProvider) { }

    public override ArgumentCollection GetArguments()
    {
        var args = new ArgumentCollection();
        AddConnectionArguments(args);

        args.AddString("search")
            .AsRequired()
            .WithDescription("Parameter name search pattern");
        AddMatchArgument(args);

        return args;
    }

    protected override void OnExecute()
    {
        var util = CreateDatabaseUtility();
        var search = Arguments.GetStringValue("search");

        var queryArgs = new Dictionary<string, string>
        {
            { "STORED_PROCEDURE_PARAMETER_NAME", ApplyMatchMethod(search) }
        };

        var query = @"SELECT SPECIFIC_SCHEMA as [Schema], SPECIFIC_NAME as [Name],
PARAMETER_NAME as [Parameter Name], DATA_TYPE as [Data Type],
CHARACTER_MAXIMUM_LENGTH as [Length], PARAMETER_MODE as [Parameter Mode]
FROM INFORMATION_SCHEMA.PARAMETERS
WHERE PARAMETER_NAME LIKE @STORED_PROCEDURE_PARAMETER_NAME
ORDER BY SPECIFIC_NAME, ORDINAL_POSITION";

        var result = util.RunQuery(query, queryArgs);
        WriteDataTable(result);
    }
}
