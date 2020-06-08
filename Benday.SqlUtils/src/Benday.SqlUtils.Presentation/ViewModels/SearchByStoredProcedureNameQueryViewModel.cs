using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Benday.SqlUtils.Presentation.ViewModels
{
    public class SearchByStoredProcedureNameQueryViewModel : DatabaseQueryViewModelBase
    {
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

            DataSet results = new DataSet();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (var command = GetSqlCommand())
                {
                    command.Connection = connection;

                    using (var adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(results);
                    }
                }
            }

            // base.Results = results.Tables[0];

            //todo: fix this
            base.Results = new System.Collections.ObjectModel.ObservableCollection<object>();

            IsVisible = true;
        }
    }
}
