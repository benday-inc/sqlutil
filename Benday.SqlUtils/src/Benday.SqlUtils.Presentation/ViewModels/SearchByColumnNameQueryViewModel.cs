using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Benday.SqlUtils.Presentation.ViewModels
{
    public class SearchByColumnNameQueryViewModel : DatabaseQueryViewModelBase
    {
        protected override string SqlQueryTemplate
        {
            get
            {
                return @"select table_schema, table_name, column_name, data_type, character_maximum_length
from information_schema.columns where column_name like @COLUMN_NAME ORDER BY COLUMN_NAME, TABLE_NAME";
            }
        }

        protected override List<string> GetRequiredArguments()
        {
            List<string> args = new List<string>();

            args.Add("COLUMN_NAME");

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
