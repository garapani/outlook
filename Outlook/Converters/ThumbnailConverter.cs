using System;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Outlook.Converters
{
    public class ThumbnailConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value != null && value.ToString() != String.Empty)
                {
                    Uri uri = null;
                    string url = value.ToString();
                    if (url.StartsWith("/"))
                    {
                        uri = new Uri(url, UriKind.Relative);
                    }
                    else
                    {
                        uri = new Uri(url, UriKind.Absolute);
                    }
                    var bm = new BitmapImage(uri)
                    {
                        CreateOptions = BitmapCreateOptions.DelayCreation,
                        DecodePixelHeight = System.Convert.ToInt32(parameter)
                    };

                    return bm;
                }
            }
            catch (Exception)
            {
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}