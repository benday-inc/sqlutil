using Benday.SqlUtils.WpfUi.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
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
    /// Interaction logic for AboutUserControl.xaml
    /// </summary>
    public partial class AboutUserControl : UserControl
    {
        public AboutUserControl()
        {
            InitializeComponent();
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenProcess("https://www.benday.com");
        }

        private void OpenProcess(string value)
        {
            var info = new ProcessStartInfo()
            {
                FileName = value,
                UseShellExecute = true
            };

            var temp = TryFindResource("Locator") as ViewModelLocator;

            if (temp != null)
            {
                temp.Telemetry.TrackEvent($"About - Click - {value}");
            }

            System.Diagnostics.Process.Start(info);
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenProcess("https://www.benday.com");
        }

        private void Label_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            OpenProcess("mailto:info@benday.com");
        }
    }
}
