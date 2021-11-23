/// ---------------------------
/// Author: Szilveszter Dezsi
/// Created: 2019-09-14
/// Modified: n/a
/// ---------------------------

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace PL
{
    /// <summary>
    /// Class for converting TreeView icon-images.
    /// </summary>
    [ValueConversion(typeof(string), typeof(bool))]
    public class HeaderToImageConverter : IValueConverter
    {
        public static HeaderToImageConverter Instance = new HeaderToImageConverter();

        /// <summary>
        /// Returns a disk drive image in TreeView when user selects a local drive as root directory.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value as string).Contains(@"\"))
            {
                Uri uri = new Uri
                ("pack://application:,,,/images/diskdrive.png");
                BitmapImage source = new BitmapImage(uri);
                return source;
            }
            else
            {
                Uri uri = new Uri("pack://application:,,,/images/directory.png");
                BitmapImage source = new BitmapImage(uri);
                return source;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Cannot convert back");
        }
    }
}