using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Wlst.Ux.WJ3005Module.Wj3005TmlInfoSetViewModel.Services
{

    [ValueConversion(typeof(string), typeof(string))]
    public class LittleAConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo info)
        {
            try
            {
                var inttime = double.Parse((string)value);

                if (inttime <= 0.0)
                    return "不屏蔽";
                return inttime;
            }
            catch (Exception ex)
            {
                Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
            return "不屏蔽";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo info)
        {
            ////var date = (bool)value;
            ////return !date;
            //return null; //如果只是显示，return   null就可以了。
            try
            {
                var strtime = (string)value;
                if (strtime.Equals("不屏蔽"))return "0";
                if (strtime.Last().Equals('.')) return strtime + "0";
                return value.ToString();
            }
            catch (Exception ex)
            {
                Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
            return 0;
        }
    }
}
