using System;
using System.Collections.Generic;
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
    /// Interaction logic for DatabaseConnectionsEditor.xaml
    /// </summary>
    public partial class DatabaseConnectionsEditor : UserControl
    {
        public DatabaseConnectionsEditor()
        {
            InitializeComponent();
        }

        private void _ButtonDebug_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine();
        }

        private void _ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine();
        }
    }
}
