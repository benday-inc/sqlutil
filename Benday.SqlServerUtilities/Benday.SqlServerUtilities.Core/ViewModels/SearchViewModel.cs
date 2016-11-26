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
