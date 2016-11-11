using Benday.Presentation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Benday.SqlServerUtilities.Core.ViewModels
{
    public class DatabaseConnectionViewModel : ViewModelBase, ISelectableItem, IStoredDatabaseConnectionString
    {
        public DatabaseConnectionViewModel()
        {
            Id = Guid.NewGuid().ToString();
            Name = String.Empty;
            Server = String.Empty;
            Database = String.Empty;
            UseIntegratedSecurity = true;
            Username = String.Empty;
            Password = String.Empty;
        }

        private const string IdPropertyName = "Id";

        private string _Id;
        public string Id
        {
            get
            {
                return _Id;
            }
            private set
            {
                _Id = value;
                RaisePropertyChanged(IdPropertyName);
            }
        }

        private const string NamePropertyName = "Name";

        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
                RaisePropertyChanged(NamePropertyName);
            }
        }

        private const string ServerPropertyName = "Server";

        private string _Server;
        public string Server
        {
            get
            {
                return _Server;
            }
            set
            {
                _Server = value;
                RaisePropertyChanged(ServerPropertyName);
                RaisePropertyChanged(ConnectionStringPropertyName);
            }
        }

        private const string DatabasePropertyName = "Database";

        private string _Database;
        public string Database
        {
            get
            {
                return _Database;
            }
            set
            {
                _Database = value;
                RaisePropertyChanged(DatabasePropertyName);
                RaisePropertyChanged(ConnectionStringPropertyName);
            }
        }

        private const string UseIntegratedSecurityPropertyName = "UseIntegratedSecurity";

        private bool _UseIntegratedSecurity;
        public bool UseIntegratedSecurity
        {
            get
            {
                return _UseIntegratedSecurity;
            }
            set
            {
                _UseIntegratedSecurity = value;
                RaisePropertyChanged(UseIntegratedSecurityPropertyName);
                RaisePropertyChanged(ConnectionStringPropertyName);
            }
        }

        private const string UsernamePropertyName = "Username";

        private string _Username;
        public string Username
        {
            get
            {
                return _Username;
            }
            set
            {
                _Username = value;
                RaisePropertyChanged(UsernamePropertyName);
                RaisePropertyChanged(ConnectionStringPropertyName);
            }
        }

        private const string PasswordPropertyName = "Password";

        private string _Password;
        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                _Password = value;
                RaisePropertyChanged(PasswordPropertyName);
                RaisePropertyChanged(ConnectionStringPropertyName);
            }
        }

        private DatabaseConnectionString _OriginalConnectionString;
        private string _OriginalConnectionName;

        public void Initialize(
            string databaseConnectionId, string originalName, 
            DatabaseConnectionString connection)
        {
            if (string.IsNullOrEmpty(databaseConnectionId))
                throw new ArgumentException($"{nameof(databaseConnectionId)} is null or empty.", nameof(databaseConnectionId));
            if (string.IsNullOrEmpty(originalName))
                throw new ArgumentException($"{nameof(originalName)} is null or empty.", nameof(originalName));
            if (connection == null)
                throw new ArgumentNullException(nameof(connection), $"{nameof(connection)} is null.");

            _OriginalConnectionString = connection;
            Id = databaseConnectionId;
            Name = originalName;
            _OriginalConnectionName = originalName;

            PopulateFromConnection(_OriginalConnectionString);
        }

        private void PopulateFromConnection(DatabaseConnectionString fromValue)
        {
            var toValue = this;

            toValue.Database = fromValue.Database;
            toValue.Username = fromValue.Username;
            toValue.UseIntegratedSecurity = fromValue.UseIntegratedSecurity;
            toValue.Password = fromValue.Password;
            toValue.Server = fromValue.Server;
        }

        private ICommand _CancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                if (_CancelCommand == null)
                {
                    _CancelCommand = new RelayCommand(Cancel);
                }

                return _CancelCommand;
            }
        }

        private void Cancel()
        {
            Name = _OriginalConnectionName;
            PopulateFromConnection(_OriginalConnectionString);
        }

        private ICommand _SaveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (_SaveCommand == null)
                {
                    _SaveCommand = new RelayCommand(Save);
                }

                return _SaveCommand;
            }
        }

        private void Save()
        {
            OnSaveRequested?.Invoke(this, new EventArgs());
        }

        public event EventHandler OnSaveRequested;

        public const string ConnectionStringPropertyName = "ConnectionString";

        public string ConnectionString
        {
            get
            {
                return GetConnectionString();
            }
            private set
            {
                RaisePropertyChanged(ConnectionStringPropertyName);
            }
        }

        public string Text
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string Value
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        int ISelectableItem.Id
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        private const string IsSelectedPropertyName = "IsSelected";

        private bool _IsSelected;
        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                _IsSelected = value;
                RaisePropertyChanged(IsSelectedPropertyName);
            }
        }

        private string GetConnectionString()
        {
            var temp = new DatabaseConnectionString();

            temp.UseIntegratedSecurity = UseIntegratedSecurity;
            temp.Database = Database;
            temp.Password = Password;
            temp.Server = Server;
            temp.Username = Username;

            return temp.ConnectionString;
        }
    }
}
