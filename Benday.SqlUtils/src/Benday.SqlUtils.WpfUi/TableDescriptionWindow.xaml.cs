using Benday.SqlUtils.Api;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Benday.SqlUtils.WpfUi
{
    /// <summary>
    /// Interaction logic for TableDescription.xaml
    /// </summary>
    public partial class TableDescriptionWindow : Window
    {
        public TableDescriptionWindow()
        {
            InitializeComponent();
        }

        public void DescribeTable(string connectionString, string tableName)
        {
            if (String.IsNullOrWhiteSpace(tableName) == true)
            {
                MessageBox.Show("Null or empty table name", "Well that's weird.");
            }
            else
            {
                var util = new SqlServerDatabaseUtility();

                util.Initialize(connectionString);

                var desc = util.DescribeTable(tableName);

                _Result.ItemsSource = desc.Columns;
                
                this.Title = $"Table Description: '{desc.TableName}'";
                _PrimaryKeyMessage.Content = $"Primary Key: '{desc.PrimaryKeyColumnName}'";
            }
        }
    }
}
