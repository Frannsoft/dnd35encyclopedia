using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Linq;

namespace DnD35.SRD.Navigator8.Converters
{
    public class ValueConverterGroup : List<IValueConverter>, IValueConverter
    {
        /// <summary>
        /// This applies a list of converters.  Applies them in the ORDER OF THE GROUP THEY ARE CONTAINED IN XAML.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.Aggregate(value, (current, converter) => converter.Convert(current, targetType, parameter, culture));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
