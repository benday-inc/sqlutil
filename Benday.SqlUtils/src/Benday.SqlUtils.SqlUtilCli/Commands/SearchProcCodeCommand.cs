using Benday.CommandsFramework;

namespace Benday.SqlUtils.ShovelCli.Commands;

[Command(Name = "searchproccode",
    Description = "Search stored procedure source code for text",
    Category = "Search")]
public class SearchProcCodeCommand : DatabaseCommandBase
{
    public SearchProcCodeCommand(CommandExecutionInfo info, ITextOutputProvider outputProvider)
        : base(info, outputProvider) { }

    public override ArgumentCollection GetArguments()
    {
        var args = new ArgumentCollection();
        AddConnectionArguments(args);

        args.AddString("search")
            .AsRequired()
            .WithDescription("Text to search for in stored procedure source code");
        AddMatchArgument(args);

        return args;
    }

    protected override void OnExecute()
    {
        var util = CreateDatabaseUtility();
        var search = Arguments.GetStringValue("search");

        var queryArgs = new Dictionary<string, string>
        {
            { "STORED_PROCEDURE_CODE", ApplyMatchMethod(search) }
        };

        var query = @"select distinct params.specific_catalog as [Database Name],
params.specific_schema as [Schema], params.specific_name as [Name]
from INFORMATION_SCHEMA.PARAMETERS params
join sysobjects so on so.name=params.specific_name
join syscomments sc on sc.id=so.id
where sc.[text] like @STORED_PROCEDURE_CODE
order by specific_name";

        var result = util.RunQuery(query, queryArgs);
        WriteDataTable(result);
    }
}
