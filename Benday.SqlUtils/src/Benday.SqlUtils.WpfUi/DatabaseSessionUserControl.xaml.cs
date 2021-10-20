using Benday.SqlUtils.Presentation.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Benday.SqlUtils.WpfUi
{
    public partial class DatabaseSessionUserControl : UserControl
    {
        public DatabaseSessionUserControl()
        {
            InitializeComponent();
        }

        private void TextboxField_OnEnterKey(object sender, EventArgs e)
        {
            var context = this.DataContext as DatabaseSessionSearchViewModel;

            if (context != null)
            {
                context.SearchCommand.Execute(null);
            }
        }

        private DatabaseSessionSearchViewModel _Context = null;

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _Context = this.DataContext as DatabaseSessionSearchViewModel;
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            var selectedItem = _ResultGrid.SelectedItem;

            if (selectedItem == null || _Context == null)
            {
                return;
            }
            else
            {
                _ResultGrid.ContextMenu.Items.Clear();

                var context = _ResultGrid.DataContext;

                if (context is DatabaseSessionQueryViewModel)
                {
                    PopulateContextMenu();
                }                
                else
                {
                    return;
                }
            }
        }

        private void PopulateContextMenu()
        {
            AddDescribeTableToContextMenu();
        }

        private void AddDescribeTableToContextMenu()
        {
            var selectedItem = _ResultGrid.SelectedItem as DatabaseSessionQueryResultRow;

            if (selectedItem == null)
            {
                return;
            }

            var sessionId = selectedItem.SessionId;

            var killSession = new MenuItem();

            killSession.Header = $"Kill session '{sessionId}'";
            killSession.Tag = sessionId;
            killSession.Click += KillSession_Click;

            _ResultGrid.ContextMenu.Items.Add(killSession);
        }



        private void KillSession_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            var context = _ResultGrid.DataContext as DatabaseSessionQueryViewModel;

            if (context != null && 
                menuItem != null &&
                menuItem.Tag != null &&
                String.IsNullOrWhiteSpace(menuItem.Tag.ToString()) == false)
            {
                context.KillSession((int)menuItem.Tag);
                context.Run();
            }
        }


        private void _ResultGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var columnNameMap = new Dictionary<string, string>();

            columnNameMap.Add(nameof(DatabaseSessionQueryResultRow.SessionId), "Session Id");
            columnNameMap.Add(nameof(DatabaseSessionQueryResultRow.Status), "Status");
            columnNameMap.Add(nameof(DatabaseSessionQueryResultRow.Login), "Login");
            columnNameMap.Add(nameof(DatabaseSessionQueryResultRow.HostName), "Host Name");
            columnNameMap.Add(nameof(DatabaseSessionQueryResultRow.BlockedBy), "Blocked By");
            columnNameMap.Add(nameof(DatabaseSessionQueryResultRow.DatabaseName), "Database Name");
            columnNameMap.Add(nameof(DatabaseSessionQueryResultRow.Command), "Command");
            columnNameMap.Add(nameof(DatabaseSessionQueryResultRow.ProgramName), "Program Name");

            if (columnNameMap.ContainsKey(e.PropertyName) == true)
            {
                e.Column.Header = columnNameMap[e.PropertyName];
            }
        }
    }
}
