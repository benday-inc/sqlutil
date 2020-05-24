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
    public abstract class BendayValueConverterBase : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConvertTo(value, targetType);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConvertFrom(value, targetType);
        }

        protected abstract object ConvertTo(object value, Type targetType);
        protected abstract object ConvertFrom(object value, Type targetType);
    }
}
