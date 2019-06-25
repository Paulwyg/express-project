using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Wlst.Ux.WJ3005Module.ControlCenterManagDemo2.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class EnumFilterTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return value;

            return value.ToString() == parameter.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return value;
            return (bool)value ? Enum.Parse(targetType, (String)parameter, true):null;
        }

    }
}
