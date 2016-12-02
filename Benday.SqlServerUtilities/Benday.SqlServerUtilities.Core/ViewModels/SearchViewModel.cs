using Benday.Presentation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Benday.SqlServerUtilities.Core.ViewModels
{
    public class SearchViewModel : ViewModelBase
    {
        public SearchViewModel(IDatabaseConnectionStringRepository repository) 
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository), 
                    $"{nameof(repository)} is null.");

            _Repository = repository;

            InitializeProperties();
        }

        private IDatabaseConnectionStringRepository _Repository;

        private void InitializeProperties()
        {
            _DatabaseConnections = 
                new SelectableCollectionViewModel<DatabaseConnectionViewModel>();
            _SearchByTableName = new ViewModelField<string>();
            _SearchByColumnName = new ViewModelField<string>();
            _SearchByValue = new ViewModelField<string>();
            _SearchStringMethod = new SingleSelectListViewModel(GetSearchStringMethods());
            _Results = new ObservableCollection<object>();
            
            _SearchType = new SingleSelectListViewModel(GetSearchTypes());

            _SearchType.OnItemSelected += _SearchType_OnItemSelected;

            UpdateFieldVisibilityForSearchType();

            RefreshDatabaseConnections();

            if (_DatabaseConnections.Items.Count > 0)
            {
                _DatabaseConnections.Items[0].IsSelected = true;
            }

            _DatabaseConnections.IsValid = true;
            _SearchByColumnName.IsValid = true;
            _SearchByTableName.IsValid = true;
            _SearchByValue.IsValid = true;
            _SearchStringMethod.IsValid = true;
            _SearchType.IsValid = true;
        }

        private void RefreshDatabaseConnections()
        {
            _DatabaseConnections.Items.Clear();

            var connections = _Repository.GetAll();

            DatabaseConnectionViewModel item;
            DatabaseConnectionString connString;

            foreach (var connection in connections)
            {
                item = new DatabaseConnectionViewModel();
                connString = new DatabaseConnectionString();
                connString.Load(connection.ConnectionString);

                item.Initialize(connection.Id, connection.Name,
                    connString);

                _DatabaseConnections.Add(item);
            }            
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

                if (searchType == "Table Name")
                {
                    InitializeForTableNameSearch();
                }
                else if (searchType == "Column Name")
                {
                    InitializeForColumnNameSearch();
                }
                else if (searchType == "Find Text In Any Table Column")
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

            returnValues.Add(new SelectableItem(true, "Contains"));
            returnValues.Add(new SelectableItem(false, "Exact"));
            returnValues.Add(new SelectableItem(false, "Starts With"));
            returnValues.Add(new SelectableItem(false, "Ends With"));

            return returnValues;
        }

        private IEnumerable<ISelectableItem> GetSearchTypes()
        {
            var returnValues = new List<SelectableItem>();

            returnValues.Add(new SelectableItem(true, "Table Name"));
            returnValues.Add(new SelectableItem(false, "Column Name"));
            returnValues.Add(new SelectableItem(false, "Stored Procedure Name"));
            returnValues.Add(new SelectableItem(false, "Stored Procedure Parameter Name"));
            returnValues.Add(new SelectableItem(false, "Stored Procedure Source Code"));
            returnValues.Add(new SelectableItem(false, "Find Text In Any Table Column"));

            return returnValues;
        }

        private const string DatabaseConnectionsPropertyName = "DatabaseConnections";

        private SelectableCollectionViewModel<DatabaseConnectionViewModel> _DatabaseConnections;
        public SelectableCollectionViewModel<DatabaseConnectionViewModel> DatabaseConnections
        {
            get
            {
                return _DatabaseConnections;
            }
            set
            {
                _DatabaseConnections = value;
                RaisePropertyChanged(DatabaseConnectionsPropertyName);
            }
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
                    _SearchCommand = new RelayCommand(Search);
                }

                return _SearchCommand;
            }
        }

        private ICommand _RefreshConnectionsCommand;
        public ICommand RefreshConnectionsCommand
        {
            get
            {
                if (_RefreshConnectionsCommand == null)
                {
                    _RefreshConnectionsCommand = 
                        new RelayCommand(RefreshDatabaseConnections);
                }

                return _RefreshConnectionsCommand;
            }
        }

        private void Search()
        {
            throw new NotImplementedException();
        }

        private const string ResultsPropertyName = "Results";

        private ObservableCollection<object> _Results;
        
        public ObservableCollection<object> Results
        {
            get
            {
                return _Results;
            }
            set
            {
                _Results = value;
                RaisePropertyChanged(ResultsPropertyName);
            }
        }

    }
}
