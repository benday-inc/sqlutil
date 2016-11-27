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

namespace Benday.SqlServerUtilities.WpfUi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private Dictionary<string, UserControl> _Controls;

        private void _CurrentControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void _ButtonSearchByName_Click(object sender, RoutedEventArgs e)
        {
            SelectControl("Search");
        }

        private void _ButtonEditConnections_Click(object sender, RoutedEventArgs e)
        {
            SelectControl("Connections");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _Controls = new Dictionary<string, UserControl>();

            _Controls.Add("Connections", new DatabaseConnectionsEditor());
            _Controls.Add("Search", new DatabaseSearchUserControl());

            SelectControl("Search");
        }

        private void SelectControl(string controlName)
        {
            _CurrentControl.Content = _Controls[controlName];
        }
    }
}
