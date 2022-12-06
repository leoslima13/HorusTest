using System;
using System.Globalization;
using Xamarin.Forms;

namespace Horus.Converters
{
    public class IsCompletedToArrowSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            var bValue = (bool) value;

            return bValue ? "arrow_right_w.png" : "arrow_right_g.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}