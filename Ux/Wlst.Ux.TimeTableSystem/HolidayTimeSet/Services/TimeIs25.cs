using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Wlst.Ux.TimeTableSystem.HolidayTimeSet.Services
{
    [ValueConversion(typeof(int), typeof(string))]
    public class TimeIs25 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo info)
        {
            try
            {
                var bol = (int)value;
                if (bol == 25) return "--";
                else 
                    return value.ToString();
                
            }
            catch (Exception ex)
            {
                Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
            return 25;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo info)
        {
            try
            {
                var strtime = (string)value;
                if (strtime.Equals("--"))
                    return 25;
                else
                    return Int32.Parse(strtime);
            }
            catch (Exception ex)
            {
                Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
            return 25;
        }
    }
}
