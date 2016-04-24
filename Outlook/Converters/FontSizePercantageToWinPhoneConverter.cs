using System;
using System.Windows.Data;

namespace Outlook.Converters
{
    public class FontSizePercantageToWinPhoneConverter : IValueConverter
    {
        private const double RATIO = 1.5625;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                double d = 0;
                d = (double)value;
                return d * RATIO;
            }
            catch { return value; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                var d = (double)value;
                return d / RATIO;
            }
            catch { return value; }
        }
    }
}