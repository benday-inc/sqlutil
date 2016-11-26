using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Benday.SqlServerUtilities.WpfUi.Controls
{
    public sealed partial class TextboxField : UserControl
    {
        public TextboxField()
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
                if (value == null)
                {
                    this.SetValue(LabelTextProperty, String.Empty);
                    m_label.Text = String.Empty;
                }
                else
                {
                    // string newValueToUpper = value.ToUpper();

                    // this.SetValue(LabelTextProperty, newValueToUpper);

                    // m_label.Text = newValueToUpper;

                    this.SetValue(LabelTextProperty, value);
                }
            }
        }

        public static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register(
          "LabelText", typeof(string), typeof(TextboxField), new PropertyMetadata(String.Empty, LabelTextPropertyChanged));

        private static void LabelTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextboxField target = d as TextboxField;

            var tempValue = e.NewValue as string;

            if (target != null && tempValue != null)
            {
                string newValueToUpper = tempValue.ToUpper();

                target.m_label.Text = newValueToUpper;
            }
        }

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
    }
}
