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
    public class DatabaseConnectionsViewModel : ViewModelBase
    {
        public DatabaseConnectionsViewModel()
        {
            _Connections = new SelectableCollectionViewModel<DatabaseConnectionViewModel>();
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

        private void AddConnection()
        {
            var temp = new DatabaseConnectionViewModel();

            Connections.Add(temp);

            temp.IsSelected = true;
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
                Connections.Items.Remove(removeThis);
            }
        }
    }
}
