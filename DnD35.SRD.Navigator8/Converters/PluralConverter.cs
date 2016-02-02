
using DnDNavigator.Runtime.Model.DnDEntry;
using System;
using System.Globalization;
using System.Windows.Data;
namespace DnD35.SRD.Navigator8.Converters
{
    public class PluralConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = (string)value;
            if (s.Equals(EntryType.Types.Equipment.ToString(), StringComparison.OrdinalIgnoreCase) || 
                s.Equals(EntryType.Types.Class.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return s;
            }
            else
            {
                return s = s + "S"; //pluralize
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //nothing here for now
            return value;
        }
    }
}
