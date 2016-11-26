using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Benday.SqlServerUtilities.WpfUi.Controls
{
    public sealed partial class LabelField : UserControl
    {
        public LabelField()
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
                    m_label.Text = String.Empty;
                }
                else
                {
                    string newValueToUpper = value.ToUpper();

                    this.SetValue(LabelTextProperty, newValueToUpper);
                    m_label.Text = newValueToUpper;
                }
            }
        }

        public static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register(
          "LabelText", typeof(string), typeof(LabelField), new PropertyMetadata(String.Empty));
    }
}
