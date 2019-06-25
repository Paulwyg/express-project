using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;
using System.Globalization;

namespace Wlst.Ux.Wj2090Module.Wj2090InfoSet.Converters
{
    [ValueConversion(typeof (bool), typeof(Visibility))]
    public class BoolToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
        {
            if (!(value is bool))
                return Visibility.Collapsed;
            var source = (bool)value;
            return source ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Visibility))
                return false;
            var source = (Visibility)value;
            return source == Visibility.Visible;
        }
    }



    [ValueConversion(typeof(bool), typeof(double))]
    public class IntToDouble : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if (!(value is bool))
            //    return Visibility.Collapsed;
            //var source = (bool)value;
            //return source ? Visibility.Visible : Visibility.Collapsed;

            int xg = 0;
            Int32.TryParse(value.ToString(), out xg);
            return xg*0.01;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if (!(value is Visibility))
            //    return false;
            //var source = (Visibility)value;
            //return source == Visibility.Visible;

            double xg = 0;
            Double.TryParse(value.ToString(), out xg);
            xg = xg*100;
            int gt = (int) xg;
            return gt;
        }
    }
}
