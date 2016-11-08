using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Benday.Presentation.ValueConverters
{
    public class StringFormatValueConverter : BendayValueConverterBase
    {
        private readonly string formatString;

        /// <summary>
        /// Creates a new <see cref="StringFormatValueConverter"/>
        /// </summary>
        /// <param name="formatString">Format string, it can take zero or one parameters, the first one being replaced by the source value</param>
        public StringFormatValueConverter(string formatString) : base()
        {
            this.formatString = formatString;
        }
                
        protected override object ConvertTo(object value, Type targetType)
        {
            return string.Format(System.Globalization.CultureInfo.CurrentUICulture, this.formatString, value);
        }

        protected override object ConvertFrom(object value, Type targetType)
        {
            throw new NotImplementedException();
        }
    }
}
