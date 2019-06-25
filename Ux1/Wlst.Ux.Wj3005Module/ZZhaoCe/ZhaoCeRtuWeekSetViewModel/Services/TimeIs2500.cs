using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Wlst.Ux.WJ3005Module.ZZhaoCe.ZhaoCeRtuWeekSetViewModel.Services
{
    [ValueConversion(typeof(int), typeof(string))]
    public class TimeIs2500 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo info)
        {
            try
            {
                var bol = (string)value;
                var bol1 = bol.Substring(0, 5);
                var bol2 = bol.Substring(6, 5);

                if (System.Convert.ToInt32(bol.Substring(0, 2)) == 25)
                {
                    bol1 = "25:00";
                }

                if (System.Convert.ToInt32(bol.Substring(6, 2)) == 25)
                {
                    bol2 = "25:00";
                }

                return bol1+"-"+bol2;
            }
            catch (Exception ex)
            {
                Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
            return 25;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo info)
        {

            return null;
        }
    }
}
