using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Benday.Presentation.ValueConverters
{
    public class ToUpperCaseValueConverter : BendayValueConverterBase
    {
        protected override object ConvertTo(object value, Type targetType)
        {
            if (value == null)
            {
                return String.Empty;
            }
            else
            {
                return value.ToString().ToUpper();
            }
        }

        protected override object ConvertFrom(object value, Type targetType)
        {
            return value;
        }
    }
}
