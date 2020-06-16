using Benday.Presentation;
using Benday.SqlUtils.Api;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Benday.SqlUtils.Presentation.ViewModels
{
    public class DatabaseConnectionViewModel : ViewModelBase, ISelectableItem, IStoredDatabaseConnectionString
    {
        public DatabaseConnectionViewModel()
        {
            Id = Guid.NewGuid().ToString();
            Name = new ViewModelField<string>(String.Empty);
            Server = new ViewModelField<string>(String.Empty);
            Database = new ViewModelField<string>(String.Empty);
            UseIntegratedSecurity = new ViewModelField<bool>(true);
            Username = new ViewModelField<string>(String.Empty);
            Password = new ViewModelField<string>(String.Empty);
            
            SubscribeFieldsToOnValueChanged();
            RefreshControlsOnUseIntegratedSecurityChange();
        }

        private void SubscribeFieldsToOnValueChanged()
        {
            SubscribeFieldToOnValueChanged<string>(Name);
            SubscribeFieldToOnValueChanged<string>(Server);
            SubscribeFieldToOnValueChanged<string>(Database);
            SubscribeFieldToOnValueChanged<bool>(UseIntegratedSecurity);
            SubscribeFieldToOnValueChanged<string>(Username);
            SubscribeFieldToOnValueChanged<string>(Password);
        }

        private void SubscribeFieldToOnValueChanged<T>(ViewModelField<T> field)
        {
            field.OnValueChanged += Field_OnValueChanged;
        }

        private void Field_OnValueChanged(object sender, EventArgs e)
        {
            RaisePropertyChanged(ConnectionStringPropertyName);

            if (sender == UseIntegratedSecurity)
            {
                RefreshControlsOnUseIntegratedSecurityChange();
            }
        }

        private void RefreshControlsOnUseIntegratedSecurityChange()
        {
            Username.IsEnabled = !UseIntegratedSecurity.Value;
            Password.IsEnabled = !UseIntegratedSecurity.Value;
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

        private ViewModelField<string> _Name;
        public ViewModelField<string> Name
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

        private ViewModelField<string> _Server;
        public ViewModelField<string> Server
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

        private ViewModelField<string> _Database;
        public ViewModelField<string> Database
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

        private ViewModelField<bool> _UseIntegratedSecurity;
        public ViewModelField<bool> UseIntegratedSecurity
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

        private ViewModelField<string> _Username;
        public ViewModelField<string> Username
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

        private ViewModelField<string> _Password;
        public ViewModelField<string> Password
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
            Name.Value = originalName;
            _OriginalConnectionName = originalName;

            PopulateFromConnection(_OriginalConnectionString);
        }

        private void PopulateFromConnection(DatabaseConnectionString fromValue)
        {
            var toValue = this;

            toValue.Database.Value = fromValue.Database;
            toValue.Username.Value = fromValue.Username;
            toValue.UseIntegratedSecurity.Value = fromValue.UseIntegratedSecurity;
            toValue.Password.Value = fromValue.Password;
            toValue.Server.Value = fromValue.Server;

            RefreshControlsOnUseIntegratedSecurityChange();
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
            Name.Value = _OriginalConnectionName;
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
            RaisePropertyChanged(NamePropertyName);
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
                return Name.Value;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override string ToString()
        {
            return Name.Value;
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

        string IStoredDatabaseConnectionString.Id
        {
            get
            {
                return Id;
            }
        }

        string IStoredDatabaseConnectionString.Name
        {
            get
            {
                return Name.Value;
            }
        }

        string IStoredDatabaseConnectionString.ConnectionString
        {
            get
            {
                return GetConnectionString();
            }
        }

        private string GetConnectionString()
        {
            var temp = new DatabaseConnectionString();

            temp.UseIntegratedSecurity = UseIntegratedSecurity.Value;
            temp.Database = Database.Value;
            temp.Password = Password.Value;
            temp.Server = Server.Value;
            temp.Username = Username.Value;

            return temp.ConnectionString;
        }


        private const string TooltipTextPropertyName = "TooltipText";

        private string _TooltipText;
        public string TooltipText
        {
            get
            {
                return _TooltipText;
            }
            set
            {
                _TooltipText = value;
                RaisePropertyChanged(TooltipTextPropertyName);
            }
        }
        
    }
}
