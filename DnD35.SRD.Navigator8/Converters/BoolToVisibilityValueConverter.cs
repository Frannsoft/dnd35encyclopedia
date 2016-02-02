using System;
using System.Windows;
using System.Windows.Data;

namespace DnD35.SRD.Navigator8.Converters
{
    public class BoolToVisibilityValueConverter : IValueConverter
    {
        /// <summary>
        /// Convert the bool passed
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility visibility = Visibility.Collapsed; //default
            bool val = false;

            if (value != null)
            {
                val = (bool)value;

                if (val)
                {
                    visibility = Visibility.Visible;
                }
                else
                {
                    visibility = Visibility.Collapsed;
                }
            }

            return visibility;
        }

        /// <summary>
        /// No need for this.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Returns visible if false, collapsed if true
    /// </summary>
    public class ReverseBoolToVisibilityValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility visibility = Visibility.Collapsed; //default
            bool val = false;

            if (value != null)
            {
                val = (bool)value;

                if (val)
                {
                    visibility = Visibility.Collapsed;
                }
                else
                {
                    visibility = Visibility.Visible;
                }
            }

            return visibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
