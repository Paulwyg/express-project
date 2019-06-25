using System;
using System.Globalization;
using System.Windows.Data;
using Wlst.Ux.Nr6005Module.LnEmergencyOperationCenter.Services;

namespace Wlst.Ux.Nr6005Module.LnEmergencyOperationCenter.Converters
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
