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
    public sealed partial class TextboxField : UserControl, ILabeledField
    {
        public TextboxField()
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
          "LabelText", typeof(string), typeof(TextboxField), new PropertyMetadata(String.Empty, DependencyPropertyUtility.LabelTextPropertyChanged));
        

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

        public void SetMultiLineMode(bool value)
        {
            string styleKey;

            if (value == false)
            {
                styleKey = "FieldTextboxStyle";
            }
            else
            {
                styleKey = "FieldTextboxMultiLineStyle";
            }

            Style style = FindResource(styleKey) as Style;

            if (style == null)
            {
                throw new InvalidOperationException("Could not find style");
            }

            _Textbox.Style = style;

            this.SetValue(MultiLineProperty, value);           
        }

        public bool MultiLine
        {
            get
            {
                return (bool)this.GetValue(MultiLineProperty);
            }
            set
            {
                SetMultiLineMode(value);
            }
        }

        public static readonly DependencyProperty MultiLineProperty = DependencyProperty.Register(
          "MultiLine", typeof(bool), typeof(TextboxField), new PropertyMetadata(false, DependencyPropertyUtility.MultiLinePropertyChanged));        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var currentMultiLineValue = this.MultiLine;

            this.MultiLine = !currentMultiLineValue;
        }
    }
}
