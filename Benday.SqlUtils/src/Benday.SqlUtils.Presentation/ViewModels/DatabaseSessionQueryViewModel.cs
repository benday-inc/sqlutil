using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Benday.SqlUtils.Presentation.ViewModels
{
    public class DatabaseSessionQueryViewModel : DatabaseQueryViewModelBase
    {
        protected override string SqlQueryTemplate
        {
            get
            {
                return @"exec sp_who2";
            }
        }

        protected override List<string> GetRequiredArguments()
        {
            return new List<string>();
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

            base.Results = ToModels(results.Tables[0]);

            IsVisible = true;
        }

        private ObservableCollection<object> ToModels(DataTable dataTable)
        {
            var returnValue = new ObservableCollection<object>();

            foreach (DataRow item in dataTable.Rows)
            {
                returnValue.Add(new DatabaseSessionQueryResultRow(item));
            }

            return returnValue;
        }
    }
}
