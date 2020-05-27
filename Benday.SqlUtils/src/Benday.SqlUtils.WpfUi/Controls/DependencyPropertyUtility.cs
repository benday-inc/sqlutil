using System;
using System.Linq;
using System.Windows;

namespace Benday.SqlUtils.WpfUi.Controls
{
    public static class DependencyPropertyUtility
    {
        public static void LabelTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ILabeledField target = d as ILabeledField;

            var tempValue = e.NewValue as string;

            if (target != null && tempValue != null)
            {
                string newValueToUpper = tempValue.ToUpper();
                
                target.SetLabelText(newValueToUpper);
            }
        }

        public static void MultiLinePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = d as TextboxField;

            if (target != null && e.NewValue is bool)
            {
                var tempValue = (bool)e.NewValue;

                target.SetMultiLineMode(tempValue);
            }
        }
    }
}
