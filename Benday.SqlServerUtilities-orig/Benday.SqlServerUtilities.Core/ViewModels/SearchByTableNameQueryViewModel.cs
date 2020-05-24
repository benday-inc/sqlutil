using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Benday.SqlServerUtilities.Core.ViewModels
{
    public class SearchByTableNameQueryViewModel : DatabaseQueryViewModelBase
    {
        protected override string SqlQueryTemplate
        {
            get
            {
                return @"select table_schema, table_name
                        from INFORMATION_SCHEMA.TABLES
                        where table_name like @TABLE_NAME
                        order by table_name";
            }
        }

        protected override List<string> GetRequiredArguments()
        {
            List<string> args = new List<string>();

            args.Add("TABLE_NAME");

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
