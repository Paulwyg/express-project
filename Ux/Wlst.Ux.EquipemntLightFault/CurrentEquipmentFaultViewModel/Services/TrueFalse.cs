using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Wlst.Ux.EquipemntLightFault.CurrentEquipmentFaultViewModel.Services
{
    [ValueConversion(typeof(bool), typeof(bool))]
    class TrueFalse: IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo info)
        {
            try
            {
                var inttime = (bool)value;
                if (inttime) return false;
                else return true;
            }
            catch (Exception ex)
            {
                Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo info)
        {
            ////var date = (bool)value;
            ////return !date;
            //return null; //如果只是显示，return   null就可以了。
            
            return null;
        }
    }
}
