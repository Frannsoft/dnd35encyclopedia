using HtmlAgilityPack;
using System;
using System.Windows.Data;

namespace DnD35.SRD.Navigator8.Converters
{
    public class StripHtmlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string val = value as string;
            if (!string.IsNullOrEmpty(val))
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(val);

                //no tags, no leading/trailing whitespaces
                val = doc.DocumentNode.InnerText.Trim();

                return val;
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
