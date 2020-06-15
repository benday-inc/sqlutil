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
    public class VisibilityValueConverter : BendayValueConverterBase
    {        
        protected override object ConvertTo(object value, Type targetType)
        {
            bool valueAsBoolean = false;

            if (value is bool)
            {
                valueAsBoolean = (bool)value;
            }
            else if (value != null)
            {
                Boolean.TryParse(value.ToString(), out valueAsBoolean);
            }

            if (valueAsBoolean == true)
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
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
