using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace IsTakipWpf.Infrastructure
{
    public class CountToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int count)
            {
                bool invert = parameter?.ToString() == "Invert";
                if (invert)
                    return count == 0 ? Visibility.Visible : Visibility.Collapsed;
                return count > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
