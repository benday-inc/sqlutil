using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Benday.SqlUtils.Presentation.ViewModels
{
    public class SearchByStoredProcedureSourceCodeQueryViewModel : DatabaseQueryViewModelBase
    {
        protected override string SqlQueryTemplate
        {
            get
            {

//                return @"SELECT SPECIFIC_SCHEMA as [Schema], SPECIFIC_NAME as [Name],
//PARAMETER_NAME as [Parameter Name], DATA_TYPE as [Data Type], CHARACTER_MAXIMUM_LENGTH as [Length], PARAMETER_MODE as [Parameter Mode]

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

            //todo: fix this
            // base.Results = results.Tables[0];

            IsVisible = true;

        }
    }
}
