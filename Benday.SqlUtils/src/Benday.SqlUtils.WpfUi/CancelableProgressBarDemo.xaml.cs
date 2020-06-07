using Benday.SqlUtils.Presentation.ViewModels;
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
    /// Interaction logic for CancelableProgressBarDemo.xaml
    /// </summary>
    public partial class CancelableProgressBarDemo : Window
    {
        public CancelableProgressBarDemo()
        {
            InitializeComponent();
        }

        private CancellableProgressBarDemoViewModel _ViewModel = new CancellableProgressBarDemoViewModel();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = _ViewModel;
        }
    }
}
