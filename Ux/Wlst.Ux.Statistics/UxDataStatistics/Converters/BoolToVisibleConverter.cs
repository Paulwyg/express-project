using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Wlst.Ux.Statistics.UxDataStatistics.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                return Visibility.Visible;
            var source = (bool)value;
            return source ? Visibility.Collapsed :Visibility.Visible ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Visibility))
                return true ;
            var source = (Visibility)value;
            return source == Visibility.Collapsed;
        }
    }
}
