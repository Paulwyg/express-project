using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Wlst.Ux.PrivilegesManage.SelfInfoChangeViewModel.Converter
{
     [ValueConversion(typeof(int), typeof(bool))]
    public class MobileRightConverter : IValueConverter
    {
         public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
         {
             int param = int.Parse(parameter.ToString());
          
             if (value != null)
             {
                 int res = int.Parse(value.ToString());
                 return param == res;
             }
             return false;
         }

         public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
         {
             if(value !=null)
             {
                if((bool)value)
                {
                    return int.Parse(parameter.ToString());
                }

             }
             return null;
         }
    }
}
