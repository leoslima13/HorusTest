using System;
using System.Globalization;
using Horus.Helpers;
using Xamarin.Forms;

namespace Horus.Converters
{
    public class IsCompletedToCardBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            var bValue = (bool) value;

            return bValue ? Colors.PrimaryColor : Colors.WhiteColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}