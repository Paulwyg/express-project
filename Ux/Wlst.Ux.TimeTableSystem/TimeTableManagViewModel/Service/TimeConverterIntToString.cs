using System;
using System.Windows.Data;

namespace Wlst.Ux.TimeTableSystem.TimeTableManagViewModel.Service
{
    [ValueConversion(typeof(int), typeof(string))]
    public class TimeConverterIntToString : IValueConverter
    {
        
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo info)
        {
            try
            {
                var inttime = (int)value;
                int hour = inttime / 60;
                int minute = inttime % 60;

                if (hour == 25)
                    return "不操作";
                return hour.ToString("D2") + ":" + minute.ToString("D2");
            }
            catch (Exception ex)
            {
                Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
            return "25:00";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo info)
        {
            ////var date = (bool)value;
            ////return !date;
            //return null; //如果只是显示，return   null就可以了。
            try
            {
                var strtime = (string)value;
                if (strtime.Equals("不操作"))
                    return 1500;
                int hour = 25;
                int minute = 0;
                string hourstr = "";
                string minutestr = "";

                string[] sp = strtime.Split(':', ',', '.');
                foreach (var t in sp)
                {
                    if (!string.IsNullOrEmpty(t))
                    {
                        if (string.IsNullOrEmpty(hourstr))
                        {
                            hourstr = t;
                        }
                        else
                        {
                            minutestr = t;
                            break;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(hourstr))
                {
                    hour = Int32.Parse(hourstr);
                }
                if (!string.IsNullOrEmpty(minutestr))
                {
                    minute = Int32.Parse(minutestr);
                }
                
                return hour * 60 + minute;
            }
            catch (Exception ex)
            {
                Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
            return 1500;
        }
    }
}