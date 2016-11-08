using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Benday.Presentation.ValueConverters
{
    public class StringOrInt32ValueConverter : BendayValueConverterBase
    {

        private object ConvertBackForInt32(object value)
        {
            if (value == null)
            {
                return 0;
            }
            else
            {
                string valueAsString = value.ToString();

                if (String.IsNullOrEmpty(valueAsString) == true)
                {
                    return 0;
                }
                else
                {
                    int returnValue = 0;

                    if (Int32.TryParse(valueAsString, out returnValue) == true)
                    {
                        return returnValue;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        protected override object ConvertTo(object value, Type targetType)
        {
            if (value == null)
            {
                return String.Empty;
            }
            else
            {
                return value.ToString();
            }
        }

        protected override object ConvertFrom(object value, Type targetType)
        {
            if (targetType == typeof(Int32))
            {
                return ConvertBackForInt32(value);
            }
            else
            {
                return value;
            }
        }
    }
}
