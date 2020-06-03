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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private Dictionary<string, UserControl> _Controls;
        private Dictionary<string, Button> _MenuButtons;

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
        
        private void _ButtonExportData_Click(object sender, RoutedEventArgs e)
        {
            SelectControl("DataExport");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _Controls = new Dictionary<string, UserControl>();
            _MenuButtons = new Dictionary<string, Button>();

            _Controls.Add("Connections", new DatabaseConnectionsEditor());
            _MenuButtons.Add("Connections", _ButtonEditConnections);
            
            _Controls.Add("Search", new DatabaseSearchUserControl());
            _MenuButtons.Add("Search", _ButtonSearchByName);

            _Controls.Add("DataExport", new DataExportUserControl());
            _MenuButtons.Add("DataExport", _ButtonExportData);

            _Controls.Add("About", new AboutUserControl());
            _MenuButtons.Add("About", _ButtonAbout);

            SelectControl("Search");
        }

        private void SelectControl(string controlName)
        {
            _CurrentControl.Content = _Controls[controlName];

            var selectedButton = _MenuButtons[controlName];

            if (selectedButton == _ButtonSearchByName)
            {
                SelectButton(_ButtonSearchByName);
                UnselectButton(_ButtonExportData);
                UnselectButton(_ButtonEditConnections);
                UnselectButton(_ButtonAbout);
            }
            else if (selectedButton == _ButtonExportData)
            {
                UnselectButton(_ButtonSearchByName);
                SelectButton(_ButtonExportData);
                UnselectButton(_ButtonEditConnections);
                UnselectButton(_ButtonAbout);
            }
            else if (selectedButton == _ButtonAbout)
            {
                UnselectButton(_ButtonSearchByName);
                UnselectButton(_ButtonExportData);
                UnselectButton(_ButtonEditConnections);
                SelectButton(_ButtonAbout);
            }
            else
            {
                UnselectButton(_ButtonSearchByName);
                UnselectButton(_ButtonExportData);
                SelectButton(_ButtonEditConnections);
                UnselectButton(_ButtonAbout);
            }
        }

        private void SelectButton(Button button)
        {
            var style = Application.Current.TryFindResource("MenuButtonSelected") as Style;

            if (style != null)
            {
                button.Style = style;
            }
        }

        private void UnselectButton(Button button)
        {
            var style = Application.Current.TryFindResource("MenuButtonUnselected") as Style;

            if (style != null)
            {
                button.Style = style;
            }
        }

        private void _ButtonAbout_Click(object sender, RoutedEventArgs e)
        {
            SelectControl("About");
        }
    }
}
