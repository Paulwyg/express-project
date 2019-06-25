using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultWithTmlPriorityLevelSettingViewModel.Services
{
    [ValueConversion(typeof(bool), typeof(string))]
    public class IsCheckedToNoValue : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo info)
        {
            try
            {
                var bol = (bool)value;
                switch (bol)
                {
                    case true:
                        return "Collapsed";

                    case false:
                        return "Visible";

                    default:
                        return "Visible";
                }
                //}
                return "X";
            }
            catch (Exception ex)
            {
                Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
            return "Visible";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo info)
        {
            ////var date = (bool)value;
            ////return !date;
            return null; //如果只是显示，return   null就可以了。
        }
    }
}
