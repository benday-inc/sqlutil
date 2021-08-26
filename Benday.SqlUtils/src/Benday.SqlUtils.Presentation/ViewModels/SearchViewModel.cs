using Benday.Presentation;
using Benday.SqlUtils.Api;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Benday.SqlUtils.Presentation.ViewModels
{
    public class SearchViewModel : DatabaseUtilityViewModelBase
    {
        public SearchViewModel(
            IMessageManager msgManager,
            IDatabaseConnectionStringRepository repository,
            ITelemetryService telemetry) : 
            base(msgManager, repository, telemetry)
        {
            
        }

        protected override void OnInitialize()
        {
            _SearchByTableName = new ViewModelField<string>();
            _SearchByColumnName = new ViewModelField<string>();
            _SearchByValue = new ViewModelField<string>();
            _SearchStringMethod = new SingleSelectListViewModel(GetSearchStringMethods());
            _Result = null;

            _SearchType = new SingleSelectListViewModel(GetSearchTypes());

            _SearchType.OnItemSelected += _SearchType_OnItemSelected;

            UpdateFieldVisibilityForSearchType();

            _SearchByColumnName.IsValid = true;
            _SearchByTableName.IsValid = true;
            _SearchByValue.IsValid = true;
            _SearchStringMethod.IsValid = true;
            _SearchType.IsValid = true;
        }        

        private void _SearchType_OnItemSelected(object sender, EventArgs e)
        {
            UpdateFieldVisibilityForSearchType();
        }

        private void UpdateFieldVisibilityForSearchType()
        {
            if (_SearchType.SelectedItem != null)
            {
                var searchType = _SearchType.SelectedItem.Text;

                if (searchType == Constants.SearchTypeTableName)
                {
                    InitializeForTableNameSearch();
                }
                else if (searchType == Constants.SearchTypeColumn)
                {
                    InitializeForColumnNameSearch();
                }
                else if (searchType == Constants.SearchTypeFindTextInTableColumn)
                {
                    InitializeForFindTextInTableColumnSearch();
                }
                else
                {
                    InitializeForStoredProcedureSearch();
                }
            }
        }

        private void InitializeForTableNameSearch()
        {
            SearchByTableName.IsVisible = true;
            SearchByColumnName.IsVisible = false;
            SearchStringMethod.IsVisible = true;
            SearchByValue.IsVisible = false;
        }

        private void InitializeForColumnNameSearch()
        {
            SearchByTableName.IsVisible = false;
            SearchByColumnName.IsVisible = true;
            SearchStringMethod.IsVisible = true;
            SearchByValue.IsVisible = false;
        }

        private void InitializeForStoredProcedureSearch()
        {
            SearchByTableName.IsVisible = false;
            SearchByColumnName.IsVisible = false;
            SearchStringMethod.IsVisible = true;
            SearchByValue.IsVisible = true;
        }

        private void InitializeForFindTextInTableColumnSearch()
        {
            SearchByTableName.IsVisible = true;
            SearchByColumnName.IsVisible = true;
            SearchStringMethod.IsVisible = false;
            SearchByValue.IsVisible = true;
        }

        private IEnumerable<ISelectableItem> GetSearchStringMethods()
        {
            var returnValues = new List<SelectableItem>();

            returnValues.Add(new SelectableItem(true,
                Constants.SearchStringMethodContains));
            returnValues.Add(new SelectableItem(false,
                Constants.SearchStringMethodExact));
            returnValues.Add(new SelectableItem(false,
                Constants.SearchStringMethodStartsWith));
            returnValues.Add(new SelectableItem(false,
                Constants.SearchStringMethodEndsWith));

            return returnValues;
        }


        private IEnumerable<ISelectableItem> GetSearchTypes()
        {
            var returnValues = new List<SelectableItem>();

            returnValues.Add(new SelectableItem(true, Constants.SearchTypeTableName, Constants.SearchTypeTableName, "Find tables by name"));
            returnValues.Add(new SelectableItem(false, Constants.SearchTypeColumn, Constants.SearchTypeColumn, "Find table columns by name"));
            returnValues.Add(new SelectableItem(false, Constants.SearchTypeStoredProcedureName, Constants.SearchTypeStoredProcedureName, "Find stored procedures by name"));
            returnValues.Add(new SelectableItem(false, Constants.SearchTypeStoredProcedureParameterName, Constants.SearchTypeStoredProcedureParameterName, "Find stored procedures by parameter name"));
            returnValues.Add(new SelectableItem(false, Constants.SearchTypeStoredProcedureSourceCode, Constants.SearchTypeStoredProcedureSourceCode, "Find stored procedures by source code text"));
            returnValues.Add(new SelectableItem(false, Constants.SearchTypeFindTextInTableColumn, Constants.SearchTypeFindTextInTableColumn, "Find text in a table column"));

            return returnValues;
        }



        

        private const string SearchByTableNamePropertyName = "SearchByTableName";

        private ViewModelField<string> _SearchByTableName;
        public ViewModelField<string> SearchByTableName
        {
            get
            {
                return _SearchByTableName;
            }
            set
            {
                _SearchByTableName = value;
                RaisePropertyChanged(SearchByTableNamePropertyName);
            }
        }

        private const string SearchByColumnNamePropertyName = "SearchByColumnName";

        private ViewModelField<string> _SearchByColumnName;
        public ViewModelField<string> SearchByColumnName
        {
            get
            {
                return _SearchByColumnName;
            }
            set
            {
                _SearchByColumnName = value;
                RaisePropertyChanged(SearchByColumnNamePropertyName);
            }
        }

        private const string SearchByValuePropertyName = "SearchByValue";

        private ViewModelField<string> _SearchByValue;
        public ViewModelField<string> SearchByValue
        {
            get
            {
                return _SearchByValue;
            }
            set
            {
                _SearchByValue = value;
                RaisePropertyChanged(SearchByValuePropertyName);
            }
        }

        private const string SearchTypePropertyName = "SearchType";

        private SingleSelectListViewModel _SearchType;
        public SingleSelectListViewModel SearchType
        {
            get
            {
                return _SearchType;
            }
            set
            {
                _SearchType = value;
                RaisePropertyChanged(SearchTypePropertyName);
            }
        }

        private const string SearchStringMethodPropertyName = "SearchStringMethod";

        private SingleSelectListViewModel _SearchStringMethod;
        public SingleSelectListViewModel SearchStringMethod
        {
            get
            {
                return _SearchStringMethod;
            }
            set
            {
                _SearchStringMethod = value;
                RaisePropertyChanged(SearchStringMethodPropertyName);
            }
        }

        private ICommand _SearchCommand;
        public ICommand SearchCommand
        {
            get
            {
                if (_SearchCommand == null)
                {
                    _SearchCommand = new ExceptionHandlingRelayCommand(Messages, Search);
                }

                return _SearchCommand;
            }
        }

        private ICommand _DebugCommand;
        public ICommand DebugCommand
        {
            get
            {
                if (_DebugCommand == null)
                {
                    _DebugCommand = new ExceptionHandlingRelayCommand(Messages, DoDebug);
                }

                return _DebugCommand;
            }
        }

        private void DoDebug()
        {
            Console.WriteLine();
        }

        private string HasValue(string value)
        {
            var returnValue = !String.IsNullOrWhiteSpace(value);

            return returnValue.ToString();
        }

        private void Search()
        {
            if (SearchType.SelectedItem == null)
            {
                
            }
            else if (SearchType.SelectedItem.Value == Constants.SearchTypeTableName)
            {
                var query = new SearchByTableNameQueryViewModel();

                query.ConnectionString = this.DatabaseConnections.SelectedItem.ConnectionString;

                query.SetArgumentValue("TABLE_NAME", SearchByTableName.Value);
                query.MatchMethod = SearchStringMethod.SelectedItem.Value;

                Result = query;

                Telemetry.TrackEvent(
                    $"Search - {SearchType.SelectedItem.Value}",
                    "SearchType", SearchType.SelectedItem.Value, 
                    "MatchMethod", SearchStringMethod.SelectedItem.Value);

                query.Execute();
            }
            else if (SearchType.SelectedItem.Value == Constants.SearchTypeColumn)
            {
                var query = new SearchByColumnNameQueryViewModel();

                query.ConnectionString = this.DatabaseConnections.SelectedItem.ConnectionString;

                query.SetArgumentValue("COLUMN_NAME", SearchByColumnName.Value);
                query.MatchMethod = SearchStringMethod.SelectedItem.Value;

                Result = query;

                Telemetry.TrackEvent(
                    $"Search - {SearchType.SelectedItem.Value}",
                    "SearchType", SearchType.SelectedItem.Value,
                    "MatchMethod", SearchStringMethod.SelectedItem.Value);

                query.Execute();
            }
            else if (SearchType.SelectedItem.Value == Constants.SearchTypeFindTextInTableColumn)
            {
                var query = new SearchByTextColumnContentQueryViewModel();

                query.ConnectionString = this.DatabaseConnections.SelectedItem.ConnectionString;

                query.SetArgumentValue("COLUMN_NAME", SearchByColumnName.Value);
                query.SetArgumentValue("TABLE_NAME", SearchByTableName.Value);
                query.SetArgumentValue("SEARCH_TEXT", SearchByValue.Value);

                query.MatchMethod = SearchStringMethod.SelectedItem.Value;

                Result = query;

                Telemetry.TrackEvent(
                    $"Search - {SearchType.SelectedItem.Value}",
                    "SearchType", SearchType.SelectedItem.Value,
                    "MatchMethod", SearchStringMethod.SelectedItem.Value, 
                    "ColumnNameHasValue", HasValue(SearchByColumnName.Value),
                    "TableNameHasValue", HasValue(SearchByTableName.Value),
                    "SearchTextHasValue", HasValue(SearchByValue.Value));

                query.Execute();
            }
            else if (SearchType.SelectedItem.Value == Constants.SearchTypeStoredProcedureName)
            {
                var query = new SearchByStoredProcedureNameQueryViewModel();

                query.ConnectionString = this.DatabaseConnections.SelectedItem.ConnectionString;

                query.SetArgumentValue("STORED_PROCEDURE_NAME", this.SearchByValue.Value);
                query.MatchMethod = SearchStringMethod.SelectedItem.Value;

                Result = query;

                Telemetry.TrackEvent(
                    $"Search - {SearchType.SelectedItem.Value}",
                    "SearchType", SearchType.SelectedItem.Value,
                    "MatchMethod", SearchStringMethod.SelectedItem.Value,
                    "StoredProcedureNameHasValue", HasValue(SearchByValue.Value));

                query.Execute();
            }
            else if (SearchType.SelectedItem.Value == Constants.SearchTypeStoredProcedureParameterName)
            {
                var query = new SearchByStoredProcedureParameterNameQueryViewModel();

                query.ConnectionString = this.DatabaseConnections.SelectedItem.ConnectionString;

                query.SetArgumentValue("STORED_PROCEDURE_PARAMETER_NAME", this.SearchByValue.Value);
                query.MatchMethod = SearchStringMethod.SelectedItem.Value;

                Result = query;

                Telemetry.TrackEvent(
                    $"Search - {SearchType.SelectedItem.Value}",
                    "SearchType", SearchType.SelectedItem.Value,
                    "MatchMethod", SearchStringMethod.SelectedItem.Value,
                    "StoredProcedureParameterNameHasValue", HasValue(SearchByValue.Value));

                query.Execute();
            }
            else if (SearchType.SelectedItem.Value == Constants.SearchTypeStoredProcedureSourceCode)
            {
                var query = new SearchByStoredProcedureSourceCodeQueryViewModel();

                query.ConnectionString = this.DatabaseConnections.SelectedItem.ConnectionString;

                query.SetArgumentValue("STORED_PROCEDURE_CODE", this.SearchByValue.Value);
                query.MatchMethod = SearchStringMethod.SelectedItem.Value;

                Result = query;

                Telemetry.TrackEvent(
                    $"Search - {SearchType.SelectedItem.Value}",
                    "SearchType", SearchType.SelectedItem.Value,
                    "MatchMethod", SearchStringMethod.SelectedItem.Value,
                    "SearchTextHasValue", HasValue(SearchByValue.Value));

                query.Execute();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private const string ResultPropertyName = "Result";

        private DatabaseQueryViewModelBase _Result;

        public DatabaseQueryViewModelBase Result
        {
            get
            {
                return _Result;
            }
            set
            {
                _Result = value;
                RaisePropertyChanged(ResultPropertyName);
            }
        }
    }
}
