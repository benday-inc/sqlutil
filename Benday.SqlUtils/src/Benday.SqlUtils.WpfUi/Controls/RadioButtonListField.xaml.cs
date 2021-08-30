using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Benday.SqlUtils.WpfUi.Controls
{
    /// <summary>
    /// Interaction logic for RadioButtonListField.xaml
    /// </summary>
    public partial class RadioButtonListField : UserControl, ILabeledField
    {
        public RadioButtonListField()
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
                SetLabelText(value);
            }
        }

        public static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register(
          "LabelText", typeof(string), typeof(RadioButtonListField), 
          new PropertyMetadata(String.Empty, DependencyPropertyUtility.LabelTextPropertyChanged));

        public void SetLabelText(string value)
        {
            if (value == null)
            {
                this.SetValue(LabelTextProperty, String.Empty);
                _Label.Text = String.Empty;
            }
            else
            {
                this.SetValue(LabelTextProperty, value);
                _Label.Text = value;
            }
        }

        public Boolean IsLabelVisible
        {
            get { return (bool)GetValue(IsLabelVisibleProperty); }
            set { SetValue(IsLabelVisibleProperty, value); }
        }

        public static readonly DependencyProperty IsLabelVisibleProperty =
            DependencyProperty.Register("IsLabelVisible", typeof(Boolean), typeof(RadioButtonListField), new PropertyMetadata(true));

        public Orientation ItemsOrientation
        {
            get
            {
                var temp = (Orientation)this.GetValue(ItemsOrientationProperty);
                return temp;
            }
            set
            {
                this.SetValue(ItemsOrientationProperty, value);
            }
        }

        public static readonly DependencyProperty ItemsOrientationProperty = DependencyProperty.Register(
          "ItemsOrientation", typeof(Orientation), typeof(RadioButtonListField), new PropertyMetadata(Orientation.Vertical));
    }
}
