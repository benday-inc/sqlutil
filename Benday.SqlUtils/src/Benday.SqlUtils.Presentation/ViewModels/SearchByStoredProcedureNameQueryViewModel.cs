using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Benday.SqlUtils.Presentation.ViewModels
{
    public class SearchByStoredProcedureNameQueryViewModel : DatabaseQueryViewModelBase
    {
        public SearchByStoredProcedureNameQueryViewModel(IQueryRunner runner) : base(runner)
        {

        }

        protected override string SqlQueryTemplate
        {
            get
            {
                return @"SELECT 
SPECIFIC_SCHEMA as [Schema], SPECIFIC_NAME as [Name]
FROM INFORMATION_SCHEMA.ROUTINES
WHERE 
SPECIFIC_NAME LIKE @STORED_PROCEDURE_NAME";
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
                returnValue.Add(new SearchByStoredProcedureNameResultRow(item));
            }

            return returnValue;
        }
    }
}
