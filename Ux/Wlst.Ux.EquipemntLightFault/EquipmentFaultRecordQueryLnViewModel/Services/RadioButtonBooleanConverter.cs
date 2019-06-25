using System;
using System.Globalization;
using System.Windows.Data;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultRecordQueryLnViewModel.Services
{
    [ValueConversion(typeof (bool), typeof (bool))]
    public class RadioButtonBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var param = bool.Parse(parameter.ToString());
            if (value == null)
            {
                return false;
            }
            return !((bool) value ^ param);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool param = bool.Parse(parameter.ToString());
            return !((bool) value ^ param);
        }

    }
}
