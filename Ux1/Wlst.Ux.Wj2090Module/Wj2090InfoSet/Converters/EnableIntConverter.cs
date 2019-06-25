using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace Wlst.Ux.Wj2090Module.Wj2090InfoSet.Converters
{
    /// <summary>
    /// 要求参数为 int
    /// </summary>
    public class EnableIntConverter : IValueConverter
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
            if ((bool)value)
                return parameter;
            return null;
        } 
    }
}
