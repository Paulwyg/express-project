using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Wlst.Ux.EmergencyDispatch.ControlCenterManag.Converters
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
            return Enum.Parse(targetType, (String)parameter, true);
        }

    }
}
