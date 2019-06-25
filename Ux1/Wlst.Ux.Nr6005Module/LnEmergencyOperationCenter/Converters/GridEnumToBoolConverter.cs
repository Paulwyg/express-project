using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Wlst.Ux.Nr6005Module.LnEmergencyOperationCenter.Converters
{

    /// <summary>
    /// 要求参数为 int
    /// </summary>
    public class GridEnumToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return Visibility.Collapsed;

            if( value.ToString() == parameter.ToString())
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

    }
}
