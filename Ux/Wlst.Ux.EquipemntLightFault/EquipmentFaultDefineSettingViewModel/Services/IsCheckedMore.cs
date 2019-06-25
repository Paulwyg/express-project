using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultDefineSettingViewModel.Services
{
    [ValueConversion(typeof(bool), typeof(int))]
    public class IsCheckedMore : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool) value ? 150 : 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    
    }
}
