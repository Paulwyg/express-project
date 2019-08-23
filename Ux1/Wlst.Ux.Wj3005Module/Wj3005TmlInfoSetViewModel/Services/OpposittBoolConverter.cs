using System;
using System.Globalization;
using System.Windows.Data;

namespace Wlst.Ux.WJ3005Module.Wj3005TmlInfoSetViewModel.Services
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class OpposittBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                return false;
            var source = (bool)value;
            return !source;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                return false;
            var source = (bool)value;
            return !source;
        }
    }
}
