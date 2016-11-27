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

namespace Benday.SqlServerUtilities.WpfUi.Controls
{
    public partial class CheckboxField : UserControl
    {
        public CheckboxField()
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
                    m_labelTopBottom.Text = String.Empty;
                }
                else
                {
                    string newValueToUpper = value.ToUpper();

                    this.SetValue(LabelTextProperty, newValueToUpper);
                    m_labelTopBottom.Text = newValueToUpper;
                    m_labelLeftRight.Text = newValueToUpper;
                }
            }
        }

        public static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register(
          "LabelText", typeof(string), typeof(CheckboxField), new PropertyMetadata(String.Empty));

        public Orientation Orientation
        {
            get
            {
                return (Orientation)this.GetValue(OrientationProperty);
            }
            set
            {
                var originalValue = this.Orientation;

                if (originalValue != value)
                {
                    this.SetValue(OrientationProperty, value);

                    if (value == System.Windows.Controls.Orientation.Horizontal)
                    {
                        m_gridLeftRight.Visibility = System.Windows.Visibility.Visible;
                        m_stackPanelTopBottom.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    else
                    {
                        m_gridLeftRight.Visibility = System.Windows.Visibility.Collapsed;
                        m_stackPanelTopBottom.Visibility = System.Windows.Visibility.Visible;
                    }
                }
            }
        }

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
          "Orientation", typeof(Orientation), typeof(CheckboxField), new PropertyMetadata(Orientation.Horizontal));

        public GridLength LabelColumnWidth
        {
            get
            {
                return (GridLength)this.GetValue(LabelColumnWidthProperty);
            }
            set
            {
                this.SetValue(LabelColumnWidthProperty, value);
                if (this.Orientation == Orientation.Horizontal)
                {
                    m_columnForLabel.Width = value;
                }
            }
        }

        private static readonly GridLength DefaultLabelColumnWidthValue = new GridLength((double)200);
        public static readonly DependencyProperty LabelColumnWidthProperty = DependencyProperty.Register(
          "LabelColumnWidth", typeof(GridLength), typeof(CheckboxField), new PropertyMetadata(DefaultLabelColumnWidthValue));

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
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
