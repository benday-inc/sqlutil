using Benday.Presentation;
using Benday.SqlUtils.Api;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Benday.SqlUtils.Presentation.ViewModels
{
    public class DatabaseConnectionsViewModel : ViewModelBase
    {
        private ITelemetryService _TelemetryService;

        private DatabaseConnectionsViewModel(ITelemetryService telemetryService)
        {
            if (telemetryService == null)
            {
                throw new ArgumentNullException("telemetryService", "Argument cannot be null.");
            }

            _TelemetryService = telemetryService;

            _Connections = new SelectableCollectionViewModel<DatabaseConnectionViewModel>();

            _Connections.OnItemSelected += _Connections_OnItemSelected;
        }

        private void _Connections_OnItemSelected(object sender, EventArgs e)
        {
            IsConnectionEditorEnabled = true;
        }

        private IDatabaseConnectionStringRepository _Repository;

        public DatabaseConnectionsViewModel(
            IDatabaseConnectionStringRepository repository, ITelemetryService telemetryService) : 
            this(telemetryService)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository), $"{nameof(repository)} is null.");

            _Repository = repository;

            var storedConnections = _Repository.GetAll();

            Initialize(storedConnections);
        }

        private const string ConnectionsPropertyName = "Connections";

        private SelectableCollectionViewModel<DatabaseConnectionViewModel> _Connections;
        public SelectableCollectionViewModel<DatabaseConnectionViewModel> Connections
        {
            get
            {
                return _Connections;
            }
            private set
            {
                _Connections = value;
                RaisePropertyChanged(ConnectionsPropertyName);
            }
        }

        private const string IsConnectionEditorEnabledPropertyName = "IsConnectionEditorEnabled";

        private bool _IsConnectionEditorEnabled;
        public bool IsConnectionEditorEnabled
        {
            get
            {
                return _IsConnectionEditorEnabled;
            }
            set
            {
                _IsConnectionEditorEnabled = value;
                RaisePropertyChanged(IsConnectionEditorEnabledPropertyName);
            }
        }

        private ICommand _AddConnectionCommand;
        public ICommand AddConnectionCommand
        {
            get
            {
                if (_AddConnectionCommand == null)
                {
                    _AddConnectionCommand = new RelayCommand(AddConnection);
                }

                return _AddConnectionCommand;
            }
        }

        private void SubscribeToEvents(DatabaseConnectionViewModel item)
        {
            item.OnSaveRequested += Item_OnSaveRequested;
        }

        private void Item_OnSaveRequested(object sender, EventArgs e)
        {
            if (sender != null && sender is IStoredDatabaseConnectionString)
            {
                var saveThis = sender as IStoredDatabaseConnectionString;

                _TelemetryService.TrackEvent("Database Connections - Save");

                _Repository.Save(saveThis);
            }
        }

        private void AddConnection()
        {
            var temp = new DatabaseConnectionViewModel();

            temp.Name.Value = "(new connection)";

            SubscribeToEvents(temp);

            Connections.Add(temp);

            temp.IsSelected = true;

            _TelemetryService.TrackEvent("Database Connections - Add");
        }

        private ICommand _DeleteConnectionCommand;

        public ICommand DeleteConnectionCommand
        {
            get
            {
                if (_DeleteConnectionCommand == null)
                {
                    _DeleteConnectionCommand = new RelayCommand(DeleteConnection);
                }

                return _DeleteConnectionCommand;
            }
        }

        private void DeleteConnection()
        {
            var removeThis = Connections.SelectedItem;

            if (removeThis != null)
            {
                _Repository.Delete(removeThis);
                Connections.Items.Remove(removeThis);
                IsConnectionEditorEnabled = false;

                _TelemetryService.TrackEvent("Database Connections - Delete");
            }
        }

        private void Initialize(
            IList<IStoredDatabaseConnectionString> storedConnections)
        {
            foreach (var item in storedConnections)
            {
                var temp = new DatabaseConnectionViewModel();

                var connString = new DatabaseConnectionString();

                connString.Load(item.ConnectionString);

                temp.Initialize(item.Id, item.Name, connString);

                SubscribeToEvents(temp);

                Connections.Add(temp);
            }
        }
    }
}
