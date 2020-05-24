using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Benday.SqlServerUtilities.WpfUi.Controls
{
    public sealed partial class ListboxField : UserControl, ILabeledField
    {
        public ListboxField()
        {
            this.InitializeComponent();
        }

        public string LabelText
        {
            get
            {
                return (string)this.GetValue(LabelTextProperty);
            }
            set
            {
                SetLabelText(value);
            }
        }

        public static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register(
          "LabelText", typeof(string), typeof(ListboxField), new PropertyMetadata(String.Empty,  DependencyPropertyUtility.LabelTextPropertyChanged));

        public void SetLabelText(string value)
        {
            if (value == null)
            {
                this.SetValue(LabelTextProperty, String.Empty);
                m_label.Text = String.Empty;
            }
            else
            {
                this.SetValue(LabelTextProperty, value);
            }
        }
    }
}
