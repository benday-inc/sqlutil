using Benday.Presentation;
using Benday.SqlUtils.Api;
using GalaSoft.MvvmLight.Command;
using System;
using System.Linq;
using System.Windows.Input;

namespace Benday.SqlUtils.Presentation.ViewModels
{
    public abstract class DatabaseUtilityViewModelBase : ViewModelBase
    {
        public DatabaseUtilityViewModelBase(IDatabaseConnectionStringRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository),
                    $"{nameof(repository)} is null.");

            _Repository = repository;

            InitializeProperties();

            OnInitialize();
        }

        protected virtual void OnInitialize()
        {

        }

        private IDatabaseConnectionStringRepository _Repository;

        protected virtual void InitializeProperties()
        {
            _DatabaseConnections =
                new SelectableCollectionViewModel<DatabaseConnectionViewModel>();

            RefreshDatabaseConnections();

            if (_DatabaseConnections.Items.Count > 0)
            {
                _DatabaseConnections.Items[0].IsSelected = true;
            }

            _DatabaseConnections.IsValid = true;
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
    }
}
