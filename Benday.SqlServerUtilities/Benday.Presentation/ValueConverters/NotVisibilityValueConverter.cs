using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Benday.Presentation.ValueConverters
{
    public class NotVisibilityValueConverter : BendayValueConverterBase
    {
        protected override object ConvertTo(object value, Type targetType)
        {
            bool valueAsBoolean = (bool)value;

            if (valueAsBoolean == false)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        protected override object ConvertFrom(object value, Type targetType)
        {
            Visibility valueAsVisibility = (Visibility)value;

            if (valueAsVisibility == Visibility.Visible)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
