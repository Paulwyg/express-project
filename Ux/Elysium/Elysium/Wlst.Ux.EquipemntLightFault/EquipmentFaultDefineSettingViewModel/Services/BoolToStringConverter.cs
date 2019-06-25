using System;
using System.Globalization;
using System.Windows.Data;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultDefineSettingViewModel.Services
{
    [ValueConversion(typeof(bool), typeof(string))]
    public class BoolToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool) value ? "报警" : "不报警";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (string)value == "报警";
        }
    }
}
