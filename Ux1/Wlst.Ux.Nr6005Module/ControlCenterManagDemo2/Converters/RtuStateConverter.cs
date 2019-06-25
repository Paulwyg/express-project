using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Wlst.Cr.CoreOne.Services;
using Wlst.Ux.Nr6005Module.ControlCenterManagDemo2.Services;

namespace Wlst.Ux.Nr6005Module.ControlCenterManagDemo2.Converters
{
    public class RtuStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "停运";
            }
            try
            {
                EnumTmlState param = (EnumTmlState)value;
                if (param == EnumTmlState.Use) return "使用";
                else if (param == EnumTmlState.Disable) return "停运";
                else return "不用";
            }
            catch (Exception ex)
            {

            }
            return "不用";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

    }
}
