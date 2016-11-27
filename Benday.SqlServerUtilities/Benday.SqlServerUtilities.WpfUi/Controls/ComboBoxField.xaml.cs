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

namespace Benday.SqlServerUtilities.WpfUi.Controls
{
    public partial class ComboBoxField : UserControl
    {
        public ComboBoxField()
        {
            InitializeComponent();
        }

        public string LabelText
        {
            get
            {
                return (string)this.GetValue(LabelTextProperty);
            }
            set
            {
                if (value == null)
                {
                    this.SetValue(LabelTextProperty, String.Empty);                    
                }
                else
                {
                    string newValueToUpper = value.ToUpper();

                    this.SetValue(LabelTextProperty, newValueToUpper);
                    m_labelLeftRight.Text = newValueToUpper;
                }
            }
        }

        public static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register(
          "LabelText", typeof(string), typeof(ComboBoxField), new PropertyMetadata(String.Empty));                   
    }
}
