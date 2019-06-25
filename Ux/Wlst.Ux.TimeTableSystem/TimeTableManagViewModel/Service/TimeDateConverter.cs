using System;
using System.Windows.Data;

namespace Wlst.Ux.TimeTableSystem.TimeTableManagViewModel.Service
{
    [ValueConversion(typeof(string), typeof(string))]
    public class TimeDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo info)
        {
            try
            {
                var date = (string)value;
                string[] sp = date.Split('.', '-', ',');
                if (sp.Length == 2)
                {
                    int year = DateTime.Now.Year;
                    int month = Int32.Parse(sp[0]);
                    int day = Int32.Parse(sp[1]);
                    if (month < DateTime.Now.Month - 2)
                    {
                        year = year + 1;
                    }
                    var dt = new DateTime(year, month, day);
                    int xxx = System.Convert.ToInt16(dt.DayOfWeek);
                    switch (xxx)
                    {
                        case 0:
                            return "星期日";
                          
                        case 1:
                            return "星期一";
                           
                        case 2:
                            return "星期二";
                           
                        case 3:
                            return "星期三";
                          
                        case 4:
                            return "星期四";
                          
                        case 5:
                            return "星期五";
                          
                        case 6:
                            return "星期六";
                          
                    }
                }
                return "X";
            }
            catch (Exception ex)
            {
                Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
            return "X";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo info)
        {
            ////var date = (bool)value;
            ////return !date;
            return null; //如果只是显示，return   null就可以了。
        }
    }
}