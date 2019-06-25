using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Xboot.Converter
{
    [ValueConversion(typeof(Visibility), typeof(bool))]
    public class VisibleToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Visibility))
                return Visibility.Collapsed;
            var source = (Visibility)value;
            return source==Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                return Visibility.Collapsed;
            var source = (bool)value;
            return source? Visibility.Visible:Visibility.Collapsed;
        }
    }


    [ValueConversion(typeof(Visibility), typeof(bool))]
    public class VisibleToBoolConverterOne : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Visibility))
                return Visibility.Collapsed;
            var source = (Visibility)value;
            return source == Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                return Visibility.Visible ;
            var source = (bool)value;
            return source ? Visibility.Collapsed  : Visibility.Visible ;
        }
    }
}
