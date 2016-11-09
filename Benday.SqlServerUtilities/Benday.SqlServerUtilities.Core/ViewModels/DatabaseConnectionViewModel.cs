using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benday.SqlServerUtilities.Core.ViewModels
{
    public class DatabaseConnectionViewModel : ViewModelBase
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
            set
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

            PopulateFromConnection(connection);
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
    }
}
