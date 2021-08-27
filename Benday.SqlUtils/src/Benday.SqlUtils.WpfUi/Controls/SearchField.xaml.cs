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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Benday.SqlUtils.WpfUi.Controls
{
    /// <summary>
    /// Interaction logic for SearchField.xaml
    /// </summary>
    public partial class SearchField : UserControl, ILabeledField
    {
        public SearchField()
        {
            InitializeComponent();
            this.IsEnabledChanged += TextboxField_IsEnabledChanged;
        }

        private void TextboxField_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _Textbox.IsEnabled = (bool)e.NewValue;
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
          "LabelText", typeof(string), typeof(SearchField), new PropertyMetadata(String.Empty, DependencyPropertyUtility.LabelTextPropertyChanged));


        private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                RaiseEnterKeyPressedEvent(sender as TextBox);
            }
        }

        /// <summary>
        /// Raised when Enter key is pressed in the textbox.
        /// </summary>
        public event EventHandler OnEnterKey;
        private void RaiseEnterKeyPressedEvent(TextBox sender)
        {
            var temp = OnEnterKey;

            if (temp != null)
            {
                if (sender != null)
                {
                    // force the viewmodel binding to refresh
                    BindingExpression binding = sender.GetBindingExpression(TextBox.TextProperty);

                    if (binding != null)
                    {
                        binding.UpdateSource();
                    }
                }

                OnEnterKey(this, new EventArgs());
            }
        }

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
    }
}
