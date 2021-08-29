using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Benday.SqlUtils.Presentation.ViewModels
{
    public class SearchByStoredProcedureSourceCodeQueryViewModel : DatabaseQueryViewModelBase
    {
        public SearchByStoredProcedureSourceCodeQueryViewModel(IQueryRunner runner) : base(runner)
        {

        }

        protected override string SqlQueryTemplate
        {
            get
            {
                return @"select distinct params.specific_catalog as [Database Name], params.specific_schema as [Schema], params.specific_name 
as [Name]
from INFORMATION_SCHEMA.PARAMETERS params
join
sysobjects so
on
so.name=params.specific_name
join
syscomments sc
on
sc.id=so.id
where sc.[text] like @STORED_PROCEDURE_CODE
order by specific_name";
            }
        }

        protected override List<string> GetRequiredArguments()
        {
            List<string> args = new List<string>();

            args.Add("STORED_PROCEDURE_NAME");

            return args;
        }

        public override void Execute()
        {
            IsVisible = false;

            using var command = GetSqlCommand();
            _runner.Initialize(ConnectionString);
            var results = _runner.Run(command);

            base.Results = ToModels(results.Tables[0]);

            IsVisible = true;
        }

        private ObservableCollection<object> ToModels(DataTable dataTable)
        {
            var returnValue = new ObservableCollection<object>();

            foreach (DataRow item in dataTable.Rows)
            {
                returnValue.Add(new SearchByStoredProcedureSourceCodeResultRow(item));
            }

            return returnValue;
        }
    }
}
