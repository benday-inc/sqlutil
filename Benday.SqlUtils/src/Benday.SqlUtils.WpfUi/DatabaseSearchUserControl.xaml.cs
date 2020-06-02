using Benday.SqlUtils.Core.ViewModels;
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
            AddDescribeTableToContextMenu("TABLE_NAME");
        }
        private void PopulateContextMenu(SearchByTableNameQueryViewModel search)
        {
            AddDescribeTableToContextMenu("TABLE_NAME");
        }

        private void AddDescribeTableToContextMenu(string tableNameColumn)
        {
            var selectedItem = _ResultGrid.SelectedItem as DataRowView;

            if (selectedItem == null || selectedItem.Row.Table.Columns.Contains(tableNameColumn) == false)
            {
                return;
            }

            var tableName = selectedItem[tableNameColumn];

            var descTable = new MenuItem();

            descTable.Header = $"Describe table: '{tableName}'";

            _ResultGrid.ContextMenu.Items.Add(descTable);
        }

        private void AddCopyValueToClipboardContextMenu(string valueColumnName)
        {
            var selectedItem = _ResultGrid.SelectedItem as DataRowView;

            if (selectedItem == null || selectedItem.Row.Table.Columns.Contains(valueColumnName) == false)
            {
                return;
            }

            var value = selectedItem[valueColumnName];

            var copyToClipboard = new MenuItem();

            copyToClipboard.Header = $"Copy query to clipboard";
            copyToClipboard.Tag = value;
            copyToClipboard.Click += (s, e) => { Clipboard.SetText(copyToClipboard.Tag as string); };

            _ResultGrid.ContextMenu.Items.Add(copyToClipboard);
        }

        private void PopulateContextMenu(SearchByTextColumnContentQueryViewModel search)
        {
            AddDescribeTableToContextMenu("TABLE NAME");
            AddCopyValueToClipboardContextMenu("QUERY");
        }

        private void PopulateContextMenu(SearchByStoredProcedureNameQueryViewModel search)
        {

        }
        private void PopulateContextMenu(SearchByStoredProcedureParameterNameQueryViewModel search)
        {

        }
        private void PopulateContextMenu(SearchByStoredProcedureSourceCodeQueryViewModel search)
        {

        }

    }
}
