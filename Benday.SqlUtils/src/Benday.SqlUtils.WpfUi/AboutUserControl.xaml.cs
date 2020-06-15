using Benday.SqlUtils.WpfUi.ViewModel;
using GalaSoft.MvvmLight.Command;
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
            OpenProcess("https://www.benday.com", "image to home page");
        }

        private void OpenProcess(string value, string desc)
        {
            var info = new ProcessStartInfo()
            {
                FileName = value,
                UseShellExecute = true
            };

            var temp = TryFindResource("Locator") as ViewModelLocator;

            if (temp != null)
            {
                temp.Telemetry.TrackEvent($"About - Click - {desc}");
            }

            System.Diagnostics.Process.Start(info);
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenProcess("https://www.benday.com", "link to home page");
        }

        private void Label_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            OpenProcess("mailto:info@benday.com", "info@benday.com");
        }

        private void _LabelViewPrivacyPolicy_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenProcess("https://www.benday.com/privacy-policy/", "privacy policy");
        }

        private void _CheckboxSendTelemetry_Checked(object sender, RoutedEventArgs e)
        {
            SetTelemetry(true);
        }

        private void _CheckboxSendTelemetry_Unchecked(object sender, RoutedEventArgs e)
        {
            SetTelemetry(false);
        }
                
        private void SetTelemetry(bool value)
        {
            var temp = TryFindResource("Locator") as ViewModelLocator;

            if (temp == null)
            {
                MessageBox.Show("Could not change telemetry value because view model was null.");
            }
            else
            {
                temp.SetTelemetry(value);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var temp = TryFindResource("Locator") as ViewModelLocator;

            if (temp == null)
            {
                MessageBox.Show("Could not load telemetry value because view model was null.");
            }
            else
            {
                _CheckboxSendTelemetry.IsChecked = temp.IsTelemetryEnabled();
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(
                "Reset your settings and close the app?", 
                "Reset settings?", 
                MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel) == MessageBoxResult.OK)
            {
                SqlUtilSettings.Default.Reset();
                System.Windows.Application.Current.Shutdown();
            }
        }
    }
}
