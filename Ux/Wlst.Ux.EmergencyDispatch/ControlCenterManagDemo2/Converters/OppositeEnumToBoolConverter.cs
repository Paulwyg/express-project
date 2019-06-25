using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Wlst.Cr.CoreOne.Services;
using Wlst.Ux.EmergencyDispatch.ControlCenterManagDemo2.Services;

namespace Wlst.Ux.EmergencyDispatch.ControlCenterManagDemo2.Converters
{

    /// <summary>
    /// 要求参数为 int
    /// </summary>
    public class OppositeEnumToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (EnumTmlState)value != EnumTmlState.Use;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

    }
}
