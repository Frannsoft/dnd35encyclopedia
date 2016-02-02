using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace DnD35.SRD.Navigator8.Converters
{
    public class EnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value != null)
            {
                Type type = value.GetType();
                string name = Enum.GetName(type, value);
                
                if(name != null)
                {
                    FieldInfo fieldInfo = type.GetField(name);
                    if(fieldInfo != null)
                    {
                        DescriptionAttribute attr = Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)) as DescriptionAttribute;
                        if(attr != null)
                        {
                            return attr.Description;
                        }
                    }
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
