using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Wlst.Ux.WJ4005Module.Wj4005TmlInfoSetViewModel.Services
{
   [ValueConversion(typeof(bool), typeof(string))]
    public class BoolToComboBox : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var flag = (bool)value;
            if (flag) return "常闭";
            else return "常开";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "常闭")
                return false;
            else
                return true;
        }
    }
}
