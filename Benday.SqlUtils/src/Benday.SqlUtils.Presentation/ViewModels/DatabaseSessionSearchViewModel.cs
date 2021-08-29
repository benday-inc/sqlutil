using Benday.Presentation;
using Benday.SqlUtils.Api;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Benday.SqlUtils.Presentation.ViewModels
{
    public class DatabaseSessionSearchViewModel : DatabaseUtilityViewModelBase
    {
        public DatabaseSessionSearchViewModel(
            IMessageManager msgManager,
            IDatabaseConnectionStringRepository repository,
            ITelemetryService telemetry) :
            base(msgManager, repository, telemetry)
        {

        }

        protected override void OnInitialize()
        {
            _searchBlockedBy = new SearchFieldViewModel(nameof(DatabaseSessionQueryResultRow.BlockedBy));
            _searchDatabaseName = new SearchFieldViewModel(nameof(DatabaseSessionQueryResultRow.DatabaseName));
            _searchHostname = new SearchFieldViewModel(nameof(DatabaseSessionQueryResultRow.HostName));
            _searchLogin = new SearchFieldViewModel(nameof(DatabaseSessionQueryResultRow.Login));
            _searchStatus = new SearchFieldViewModel(nameof(DatabaseSessionQueryResultRow.Status));

            _Result = null;
        }

        private const string SearchStatusPropertyName = "SearchStatus";

        private SearchFieldViewModel _searchStatus;
        public SearchFieldViewModel SearchStatus
        {
            get
            {
                return _searchStatus;
            }
            set
            {
                _searchStatus = value;
                RaisePropertyChanged(SearchStatusPropertyName);
            }
        }

        private const string SearchLoginPropertyName = "SearchLogin";

        private SearchFieldViewModel _searchLogin;
        public SearchFieldViewModel SearchLogin
        {
            get
            {
                return _searchLogin;
            }
            set
            {
                _searchLogin = value;
                RaisePropertyChanged(SearchLoginPropertyName);
            }
        }

        private const string SearchHostnamePropertyName = "SearchHostname";

        private SearchFieldViewModel _searchHostname;
        public SearchFieldViewModel SearchHostname
        {
            get
            {
                return _searchHostname;
            }
            set
            {
                _searchHostname = value;
                RaisePropertyChanged(SearchHostnamePropertyName);
            }
        }

        private const string SearchBlockedByPropertyName = "SearchBlockedBy";

        private SearchFieldViewModel _searchBlockedBy;
        public SearchFieldViewModel SearchBlockedBy
        {
            get
            {
                return _searchBlockedBy;
            }
            set
            {
                _searchBlockedBy = value;
                RaisePropertyChanged(SearchBlockedByPropertyName);
            }
        }

        private const string SearchDatabaseNamePropertyName = "SearchDatabaseName";

        private SearchFieldViewModel _searchDatabaseName;
        public SearchFieldViewModel SearchDatabaseName
        {
            get
            {
                return _searchDatabaseName;
            }
            set
            {
                _searchDatabaseName = value;
                RaisePropertyChanged(SearchDatabaseNamePropertyName);
            }
        }

        private ICommand _SearchCommand;
        public ICommand SearchCommand
        {
            get
            {
                if (_SearchCommand == null)
                {
                    _SearchCommand = new ExceptionHandlingRelayCommand(
                        Messages, 
                        Search);
                }

                return _SearchCommand;
            }
        }

        private void AddFilters(DatabaseSessionQueryViewModel query)
        {
            if (_searchBlockedBy.HasSearchFilter == true)
            {
                query.SetFilter(_searchBlockedBy);
            }
            throw new NotImplementedException();
        }

        private void Search()
        {
            var query = new DatabaseSessionQueryViewModel();

            query.ConnectionString = this.DatabaseConnections.SelectedItem.ConnectionString;

            AddFilters(query);

            Result = query;

            Telemetry.TrackEvent(
                $"Database Session Search");

            query.Execute();
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
