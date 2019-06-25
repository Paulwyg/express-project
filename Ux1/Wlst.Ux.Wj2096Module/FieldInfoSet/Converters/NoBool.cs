using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Wlst.Ux.Wj2096Module.FieldInfoSet.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    
    public class NoBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                return true;
            var source = (bool)value;
            return source ? false : true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
                return false;

        }
    }
}
