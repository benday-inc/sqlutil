using System;
using System.Linq;
using System.Windows;

namespace Benday.SqlServerUtilities.WpfUi.Controls
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
    }
}
