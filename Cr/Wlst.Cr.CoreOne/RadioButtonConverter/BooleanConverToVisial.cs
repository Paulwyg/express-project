using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Wlst.Cr.CoreOne.RadioButtonConverter
{



    [ValueConversion(typeof (bool), typeof (Visibility))]
    public class BooleanConverToVisial : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool param = bool.Parse(parameter.ToString());
            if (value == null)
            {
                return Visibility.Collapsed;
            }
            else
            {
                var x = !((bool) value ^ param);
                if (x) return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool param = bool.Parse(parameter.ToString());
            Visibility vs;
            var vsg = Visibility.TryParse(value.ToString(), out vs);
            if (vs == Visibility.Visible && param) return true;
            if (vs == Visibility.Collapsed && param == false) return true;
            return false;
        }

    }
}
