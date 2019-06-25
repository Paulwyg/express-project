using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Wlst.Ux.EquipemntTree.SettingViewModel.Services
{
    /// <summary>
    /// 要求参数为 int
    /// </summary>
    public class RadioBoolToIntConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            int integer = (int)value;

            if (integer == int.Parse(parameter.ToString()))

                return true;

            else

                return false;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            return parameter;

        }

    }
}
