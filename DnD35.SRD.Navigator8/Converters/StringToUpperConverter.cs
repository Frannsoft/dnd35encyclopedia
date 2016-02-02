
using DnDNavigator.Runtime.Model.DnDEntry;
using System;
using System.Globalization;
using System.Windows.Data;
namespace DnD35.SRD.Navigator8.Converters
{
    public class StringToUpperConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value != null && value.GetType() == typeof(EntryType.Types))
            {
                return ((EntryType.Types)value).ToString().ToUpper();
            }
            else if(value != null && value.GetType() == typeof(string))
            {
                return ((string)value).ToUpper();
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //uh, no I want them to remain capitalized...
            return value;
        }
    }
}
