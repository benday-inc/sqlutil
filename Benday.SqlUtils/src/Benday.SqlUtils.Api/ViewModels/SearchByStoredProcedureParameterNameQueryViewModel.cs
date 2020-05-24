using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Benday.SqlUtils.Core.ViewModels
{
    public class SearchByStoredProcedureParameterNameQueryViewModel : DatabaseQueryViewModelBase
    {
        protected override string SqlQueryTemplate
        {
            get
            {
                return @"SELECT SPECIFIC_SCHEMA, SPECIFIC_NAME, PARAMETER_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, PARAMETER_MODE
FROM INFORMATION_SCHEMA.PARAMETERs
WHERE PARAMETER_NAME LIKE @STORED_PROCEDURE_PARAMETER_NAME
ORDER BY SPECIFIC_NAME, ORDINAL_POSITION";
            }
        }

        protected override List<string> GetRequiredArguments()
        {
            List<string> args = new List<string>();

            args.Add("STORED_PROCEDURE_PARAMETER_NAME");

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

            base.Results = results.Tables[0];

            IsVisible = true;

        }
    }
}
