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
    /// <summary>
    /// Interaction logic for DatabaseSearchUserControl.xaml
    /// </summary>
    public partial class DatabaseSearchUserControl : UserControl
    {
        public DatabaseSearchUserControl()
        {
            InitializeComponent();
        }

        private void TextboxField_OnEnterKey(object sender, EventArgs e)
        {
            var context = this.DataContext as SearchViewModel;

            if (context != null)
            {
                context.SearchCommand.Execute(null);
            }
        }

        private SearchViewModel _Context = null;

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _Context = this.DataContext as SearchViewModel;
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

                if (context is SearchByTableNameQueryViewModel)
                {
                    PopulateContextMenu(context as SearchByTableNameQueryViewModel);
                }
                else if (context is SearchByColumnNameQueryViewModel)
                {
                    PopulateContextMenu(context as SearchByColumnNameQueryViewModel);
                }
                else if (context is SearchByTextColumnContentQueryViewModel)
                {
                    PopulateContextMenu(context as SearchByTextColumnContentQueryViewModel);
                }
                else if (context is SearchByStoredProcedureNameQueryViewModel)
                {
                    PopulateContextMenu(context as SearchByStoredProcedureNameQueryViewModel);
                }
                else if (context is SearchByStoredProcedureParameterNameQueryViewModel)
                {
                    PopulateContextMenu(context as SearchByStoredProcedureParameterNameQueryViewModel);
                }
                else if (context is SearchByStoredProcedureSourceCodeQueryViewModel)
                {
                    PopulateContextMenu(context as SearchByStoredProcedureSourceCodeQueryViewModel);
                }
                else
                {
                    return;
                }
            }
        }

        private void PopulateContextMenu(SearchByColumnNameQueryViewModel search)
        {
            AddDescribeTableToContextMenu();
        }

        private void PopulateContextMenu(SearchByTableNameQueryViewModel search)
        {
            AddDescribeTableToContextMenu();
        }
        private void PopulateContextMenu(SearchByTextColumnContentQueryViewModel search)
        {
            AddDescribeTableToContextMenu();
            AddCopyValueToClipboardContextMenu("QUERY");
        }

        private void PopulateContextMenu(SearchByStoredProcedureNameQueryViewModel search)
        {
            AddDescribeStoredProcedureToContextMenu();
        }

        private void PopulateContextMenu(SearchByStoredProcedureParameterNameQueryViewModel search)
        {
            AddDescribeStoredProcedureToContextMenu();
        }

        private void PopulateContextMenu(SearchByStoredProcedureSourceCodeQueryViewModel search)
        {
            AddDescribeStoredProcedureToContextMenu();
        }

        private void AddDescribeTableToContextMenu()
        {
            var selectedItem = _ResultGrid.SelectedItem as ITableName;

            if (selectedItem == null)
            {
                return;
            }

            var tableName = selectedItem.TableName;

            var descTable = new MenuItem();

            descTable.Header = $"Describe table: '{tableName}'";
            descTable.Tag = tableName;
            descTable.Click += DescribeTable_Click;

            _ResultGrid.ContextMenu.Items.Add(descTable);
        }

        private void AddDescribeStoredProcedureToContextMenu()
        {
            var selectedItem = _ResultGrid.SelectedItem as IStoredProcedureName;

            if (selectedItem == null)
            {
                return;
            }

            var name = selectedItem.Name;

            var descTable = new MenuItem();

            descTable.Header = $"Describe stored procedure: '{name}'";
            descTable.Tag = name;
            descTable.Click += DescribeStoredProcedure_Click;

            _ResultGrid.ContextMenu.Items.Add(descTable);
        }

        private void DescribeTable_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;

            if (menuItem != null &&
                menuItem.Tag != null &&
                String.IsNullOrWhiteSpace(menuItem.Tag.ToString()) == false)
            {
                var dialog = new TableDescriptionWindow();

                var connectionString = _Context.DatabaseConnections.SelectedItem.ConnectionString;

                dialog.DescribeTable(connectionString, menuItem.Tag.ToString());

                dialog.ShowDialog();
            }
        }

        private void DescribeStoredProcedure_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;

            if (menuItem != null &&
                menuItem.Tag != null &&
                String.IsNullOrWhiteSpace(menuItem.Tag.ToString()) == false)
            {
                var dialog = new StoredProcedureDescriptionWindow();

                var connectionString = _Context.DatabaseConnections.SelectedItem.ConnectionString;

                dialog.DescribeStoredProcedure(connectionString, menuItem.Tag.ToString());

                dialog.ShowDialog();
            }
        }

        private void AddCopyValueToClipboardContextMenu(string valueColumnName)
        {
            var selectedItem = _ResultGrid.SelectedItem as TextQueryResultRow;

            if (selectedItem == null)
            {
                return;
            }

            var value = selectedItem.Query;

            var copyToClipboard = new MenuItem();

            copyToClipboard.Header = $"Copy query to clipboard";
            copyToClipboard.Tag = value;
            copyToClipboard.Click += (s, e) => { Clipboard.SetText(copyToClipboard.Tag as string); };

            _ResultGrid.ContextMenu.Items.Add(copyToClipboard);
        }

        private void _ResultGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var columnNameMap = new Dictionary<string, string>();

            columnNameMap.Add("ParameterName", "Parameter Name");
            columnNameMap.Add("DataType", "Data Type");
            columnNameMap.Add("ValueLength", "Value Length");
            columnNameMap.Add("ParameterMode", "Parameter Mode");
            columnNameMap.Add("FieldLength", "Field Length");
            columnNameMap.Add("Records", "Record Count");

            if (columnNameMap.ContainsKey(e.PropertyName) == true)
            {
                e.Column.Header = columnNameMap[e.PropertyName];
            }
        }
    }
}
