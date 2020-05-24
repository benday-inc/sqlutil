using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Benday.Presentation.ValueConverters
{
    public class ShortDateValueConverter : BendayValueConverterBase
    {        
        protected override object ConvertTo(object value, Type targetType)
        {
            if (value == null || (value is DateTime) == false)
            {
                return String.Empty;
            }
            else
            {
                DateTime temp = (DateTime)value;

                return temp.Date.ToString(DateTimeFormatInfo.CurrentInfo.ShortDatePattern);
            }
        }

        protected override object ConvertFrom(object value, Type targetType)
        {
            return value;
        }
    }
}
