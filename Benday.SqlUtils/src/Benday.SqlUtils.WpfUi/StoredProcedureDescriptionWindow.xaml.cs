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
    /// Interaction logic for StoredProcedureDescriptionWindow.xaml
    /// </summary>
    public partial class StoredProcedureDescriptionWindow : Window
    {
        public StoredProcedureDescriptionWindow()
        {
            InitializeComponent();
        }

        public void DescribeStoredProcedure(string connectionString, string name)
        {
            if (String.IsNullOrWhiteSpace(name) == true)
            {
                MessageBox.Show("Null or empty table name", "Well that's weird.");
            }
            else
            {
                var util = new SqlServerDatabaseUtility();

                util.Initialize(connectionString);

                string query = "select so.name, sc.text from syscomments sc join sysobjects so on sc.id = so.id where so.name = @name";

                Dictionary<string, string> args = new Dictionary<string, string>();

                args.Add("name", name);

                var result = util.RunQuery(query, args);

                if (result == null || result.Rows.Count == 0 ||
                    result.Columns.Contains("text") == false ||
                    result.Columns.Contains("name") == false)
                {
                    _Result.Text = $"ERROR: could not locate stored procedure named '{name}'";
                    this.Title = $"Stored Procedure";
                }
                else
                {
                    var nameValue = result.Rows[0]["name"].ToString();
                    var text = result.Rows[0]["text"].ToString();

                    this.Title = $"Stored Procedure: '{nameValue}'";
                    _Result.Text = text;
                }
            }
        }
    }
}
