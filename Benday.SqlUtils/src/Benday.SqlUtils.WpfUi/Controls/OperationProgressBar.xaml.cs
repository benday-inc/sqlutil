using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;

namespace Benday.SqlUtils.WpfUi.Controls
{
    public partial class OperationProgressBar : UserControl
    {
        public OperationProgressBar()
        {
            InitializeComponent();
        }

        public bool MessageOnly
        {
            get
            {
                return (bool)this.GetValue(MessageOnlyProperty);
            }
            set
            {
                this.SetValue(MessageOnlyProperty, value);

                if (value == true)
                {
                    _ProgressBar.SetBinding(ProgressBar.IsIndeterminateProperty, new Binding());
                    _ProgressBar.IsIndeterminate = false;
                    _ProgressBar.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        public static readonly DependencyProperty MessageOnlyProperty = DependencyProperty.Register(
          "MessageOnly", typeof(bool), typeof(OperationProgressBar), new PropertyMetadata(false));

        public bool ShowCancelButton
        {
            get
            {
                return (bool)this.GetValue(ShowCancelButonProperty);
            }
            set
            {
                this.SetValue(ShowCancelButonProperty, value);
            }
        }

        public static readonly DependencyProperty ShowCancelButonProperty = 
            DependencyProperty.Register(
                nameof(ShowCancelButton), 
                typeof(bool), 
                typeof(OperationProgressBar), 
                new PropertyMetadata(false));
    }
}
