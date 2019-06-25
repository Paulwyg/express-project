using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Wlst.Ux.EmergencyDispatch.ControlCenterManagDemo2.Converters
{
   
    [ValueConversion(typeof(bool), typeof(bool))]
    public class OpposittBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                return false ;
            var source = (bool)value;
            return !source ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool ))
                return false;
            var source = (bool  )value;
            return !source  ;
        }
    }
}
