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
            IQueryRunner queryRunner,
            IDatabaseConnectionStringRepository repository,
            ITelemetryService telemetry) :
            base(msgManager, queryRunner, repository, telemetry)
        {

        }

        protected override void OnInitialize()
        {
            _searchBlockedBy = new SearchFieldViewModel(nameof(DatabaseSessionQueryResultRow.BlockedBy));
            _searchDatabaseName = new SearchFieldViewModel(nameof(DatabaseSessionQueryResultRow.DatabaseName));
            _searchHostname = new SearchFieldViewModel(nameof(DatabaseSessionQueryResultRow.HostName));
            _searchLogin = new SearchFieldViewModel(nameof(DatabaseSessionQueryResultRow.Login));
            _searchStatus = new SearchFieldViewModel(nameof(DatabaseSessionQueryResultRow.Status));
            _searchCommandText = new SearchFieldViewModel(nameof(DatabaseSessionQueryResultRow.Command));

            _Result = null;

            ResetFilters();
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

        private const string SearchCommandTextPropertyName = "SearchCommandText";

        private SearchFieldViewModel _searchCommandText;
        public SearchFieldViewModel SearchCommandText
        {
            get
            {
                return _searchCommandText;
            }
            set
            {
                _searchCommandText = value;
                RaisePropertyChanged(SearchCommandTextPropertyName);
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

        private ICommand _ResetFiltersCommand;
        public ICommand ResetFiltersCommand
        {
            get
            {
                if (_ResetFiltersCommand == null)
                {
                    _ResetFiltersCommand = new ExceptionHandlingRelayCommand(
                        Messages,
                        ResetFilters);
                }

                return _ResetFiltersCommand;
            }
        }

        private void ResetFilters()
        {
            SearchBlockedBy.Value = string.Empty;
            SearchDatabaseName.Value = string.Empty;
            SearchHostname.Value = string.Empty;
            SearchLogin.Value = string.Empty;
            SearchStatus.Value = string.Empty;
            SearchCommandText.Value = string.Empty;
        }

        private void AddFilters(DatabaseSessionQueryViewModel query)
        {            
            query.SetFilter(_searchBlockedBy);
            query.SetFilter(_searchDatabaseName);
            query.SetFilter(_searchHostname);
            query.SetFilter(_searchLogin);
            query.SetFilter(_searchStatus);
            query.SetFilter(_searchCommandText);
        }

        private void Search()
        {
            var query = new DatabaseSessionQueryViewModel(_queryRunner);

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
