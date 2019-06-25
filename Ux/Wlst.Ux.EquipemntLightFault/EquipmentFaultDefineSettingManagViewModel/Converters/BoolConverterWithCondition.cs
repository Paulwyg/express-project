using System;
using System.Globalization;
using System.Windows.Data;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultDefineSettingManagViewModel.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class BoolConverterWithCondition : IValueConverter
    {
        private bool IsTrue { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isEnable = (bool) value;
            IsTrue = ((string) parameter == "True");
            return isEnable && IsTrue || !isEnable && !IsTrue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isEnable = (bool)value;
            IsTrue = ((string)parameter == "True");

            return IsTrue ? isEnable : !isEnable;
        }
    }
}
