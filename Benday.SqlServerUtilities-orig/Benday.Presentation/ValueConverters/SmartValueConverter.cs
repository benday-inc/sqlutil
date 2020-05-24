using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Benday.Presentation.ValueConverters
{
    public class SmartValueConverter : BendayValueConverterBase
    {
        public bool ZeroIsEmptyString
        {
            get;
            set;
        }

        private object ConvertForInt32(int value)
        {
            if (ZeroIsEmptyString == true && value == 0)
            {
                return String.Empty;
            }
            else
            {
                return value.ToString();
            }
        }

        private object ConvertForDateTime(DateTime value)
        {
            if (value == default(DateTime))
            {
                return String.Empty;
            }
            else
            {
                return value.ToString(DateTimeFormatInfo.CurrentInfo.ShortDatePattern);
            }
        }

        private object ConvertBackForInt32(object value)
        {
            try
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
                        return int.Parse(valueAsString);
                    }
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private object ConvertBackForDateTime(object value)
        {
            try
            {
                if (value == null)
                {
                    return new DateTime(1900, 1, 1);
                }
                else
                {
                    string valueAsString = value.ToString();

                    if (String.IsNullOrEmpty(valueAsString) == true)
                    {
                        return new DateTime(1900, 1, 1);
                    }
                    else
                    {
                        return DateTime.Parse(valueAsString);
                    }
                }
            }
            catch (Exception)
            {
                return new DateTime(1900, 1, 1);
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
                if (value is Int32)
                {
                    return ConvertForInt32((int)value);
                }
                else if (value is DateTime)
                {
                    return ConvertForDateTime((DateTime)value);
                }
                else
                {
                    return value.ToString();
                }
            }
        }

        protected override object ConvertFrom(object value, Type targetType)
        {
            if (targetType == typeof(Int32))
            {
                return ConvertBackForInt32(value);
            }
            else if (targetType == typeof(DateTime))
            {
                return ConvertBackForDateTime(value);
            }
            else
            {
                return value;
            }
        }
    }
}
