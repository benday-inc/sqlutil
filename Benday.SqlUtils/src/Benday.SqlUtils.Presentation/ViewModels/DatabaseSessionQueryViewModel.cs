using Benday.SqlUtils.Api;
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
        public DatabaseSessionQueryViewModel(IQueryRunner runner) : base(runner)
        {

        }

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

            using var command = GetSqlCommand();
            _runner.Initialize(ConnectionString);
            var results = _runner.Run(command);

            base.Results = ToModels(results.Tables[0]);

            IsVisible = true;
        }

        private ObservableCollection<object> ToModels(DataTable dataTable)
        {
            var returnValue = new ObservableCollection<object>();

            DatabaseSessionQueryResultRow row;

            foreach (DataRow item in dataTable.Rows)
            {
                row = new DatabaseSessionQueryResultRow(item);

                if (IsFiltered(row) == false)
                {
                    returnValue.Add(row);
                }
            }

            return returnValue;
        }

        private string GetValueFromRow(DatabaseSessionQueryResultRow row, SearchFilter filter)
        {
            var argName = filter.ArgName;

            if (argName == nameof(row.BlockedBy))
            {
                return row.BlockedBy;
            }
            else if (argName == nameof(row.Command))
            {
                return row.Command;
            }
            else if (argName == nameof(row.DatabaseName))
            {
                return row.DatabaseName;
            }
            else if (argName == nameof(row.HostName))
            {
                return row.HostName;
            }
            else if (argName == nameof(row.Login))
            {
                return row.Login;
            }
            else if (argName == nameof(row.ProgramName))
            {
                return row.ProgramName;
            }
            else if (argName == nameof(row.Status))
            {
                return row.Status;
            }
            else
            {
                throw new InvalidOperationException($"Unknown argument name '{argName}'.");
            }
        }

        private bool IsFiltered(DatabaseSessionQueryResultRow row)
        {
            if (_filters.Count == 0)
            {
                return false;
            }
            else
            {
                var isFiltered = false;

                foreach (var filter in _filters.Values)
                {
                    var value = GetValueFromRow(row, filter);

                    if (filter.SearchType == Constants.SearchTypeByValue)
                    {

                        if (value.Contains(filter.Value) == false)
                        {
                            isFiltered = true;
                        }

                    }
                    else if (filter.SearchType == Constants.SearchTypeBlankOrEmpty)
                    {
                        if (string.IsNullOrWhiteSpace(value) == false)
                        {
                            isFiltered = true;
                        }
                    }
                    else if (filter.SearchType == Constants.SearchTypeNotBlankOrEmpty)
                    {
                        if (string.IsNullOrWhiteSpace(value) == true)
                        {
                            isFiltered = true;
                        }
                    }

                    if (isFiltered == true)
                    {
                        break;
                    }
                }

                return isFiltered;
            }
        }

        private Dictionary<string, SearchFilter> _filters = new Dictionary<string, SearchFilter>();

        private Dictionary<string, SearchFilter> Filters
        {
            get
            {
                return _filters;
            }
        }

        public void SetFilter(ISearchFilterable filterBy)
        {
            if (filterBy == null)
            {
                throw new ArgumentNullException(nameof(filterBy), $"{nameof(filterBy)} is null.");
            }

            if (_filters.ContainsKey(filterBy.ArgName) == true)
            {
                Filters.Remove(filterBy.ArgName);
            }

            if (filterBy.HasSearchFilter == true)
            {
                _filters.Add(filterBy.ArgName, filterBy.GetSearchFilter());
            }
        }
    }
}
