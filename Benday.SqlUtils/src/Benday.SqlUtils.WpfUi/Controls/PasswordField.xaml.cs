using Benday.Presentation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Benday.SqlUtils.WpfUi.Controls
{
    public sealed partial class PasswordField : UserControl, ILabeledField
    {
        public PasswordField()
        {
            this.InitializeComponent();
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
          "LabelText", typeof(string), typeof(PasswordField), new PropertyMetadata(String.Empty, DependencyPropertyUtility.LabelTextPropertyChanged));
        

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

        private ViewModelField<string> _DataContext;

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var field = e.NewValue as ViewModelField<string>;

            if (field != null)
            {
                _DataContext = field;
                RefreshPasswordControl();
            }
        }

        private void RefreshPasswordControl()
        {
            if (_DataContext != null)
            {
                _Textbox.Password = _DataContext.Value;
            }
        }

        private void _Textbox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (_DataContext != null)
            {
                _DataContext.Value = _Textbox.Password;
            }

            
        }
    }
}
