using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Benday.SqlUtils.Presentation.ViewModels
{
    public class SimpleDatabaseQueryViewModel : DatabaseQueryViewModelBase
    {

        public SimpleDatabaseQueryViewModel(IQueryRunner runner) : base(runner)
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
                returnValue.Add(item);
            }

            return returnValue;
        }

        protected override List<string> GetRequiredArguments()
        {
            List<string> args = new List<string>();

            return args;
        }
    }
}
