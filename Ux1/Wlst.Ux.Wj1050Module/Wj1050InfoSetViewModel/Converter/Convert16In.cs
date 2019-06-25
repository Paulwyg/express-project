using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Wlst.Ux.Wj1050Module.Wj1050InfoSetViewModel.Converter
{
   /// <summary>
    /// 要求参数为 int
    /// </summary>
    public class Convert16In : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            int integer = (int) value;
            var vs = System.Convert.ToString(integer, 16).Trim();
            //if (integer == int.Parse(parameter.ToString()))

            //    return true;

            //else

            return vs;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int x = 0;
            var str = value as string;
            try
            {
                x = System.Convert.ToInt32(str, 16);

            }
            catch (Exception ex)
            {

            }
            return x;

        }

    }

}
