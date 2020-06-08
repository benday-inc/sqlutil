using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Benday.SqlUtils.Presentation.ViewModels
{
    public class SimpleDatabaseQueryViewModel : DatabaseQueryViewModelBase
    {

        public SimpleDatabaseQueryViewModel()
        {

        }

        private const string QueryTextPropertyName = "QueryText";

        private string _QueryText;
        public string QueryText
        {
            get
            {
                return _QueryText;
            }
            set
            {
                _QueryText = value;
                RaisePropertyChanged(QueryTextPropertyName);
            }
        }

        protected override string SqlQueryTemplate
        {
            get
            {
                return QueryText;
            }
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
            // todo: fix thiss

            IsVisible = true;
        }

        protected override List<string> GetRequiredArguments()
        {
            List<string> args = new List<string>();

            return args;
        }
    }
}
