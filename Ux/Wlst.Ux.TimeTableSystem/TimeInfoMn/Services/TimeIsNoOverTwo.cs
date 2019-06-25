using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Wlst.Ux.TimeTableSystem.TimeInfoMn.Services
{
    [ValueConversion(typeof(bool), typeof(string))]
    public class TimeIsNoOverTwo : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo info)
        {
            try
            {
                var bol = (bool)value;
                switch (bol)
                {
                    case true:
                        return 0;

                    case false:
                        return "Auto";

                    default:
                        return " ";
                }
                //}
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
