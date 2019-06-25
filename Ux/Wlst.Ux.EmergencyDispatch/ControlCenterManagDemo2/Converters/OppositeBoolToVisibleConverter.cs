using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Wlst.Ux.EmergencyDispatch.ControlCenterManagDemo2.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class OppositeBoolToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                return Visibility.Collapsed;
            var source = (bool)value;
            return source ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Visibility))
                return false;
            var source = (Visibility)value;
            return source!=Visibility.Visible;
        }
    }
}
