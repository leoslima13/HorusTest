using System;
using System.Globalization;
using Horus.Helpers;
using Xamarin.Forms;

namespace Horus.Converters
{
    public class IsCompletedToTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Color.Default;
            var bValue = (bool) value;

            return bValue ? Colors.WhiteColor : Colors.SecondaryDarkColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}