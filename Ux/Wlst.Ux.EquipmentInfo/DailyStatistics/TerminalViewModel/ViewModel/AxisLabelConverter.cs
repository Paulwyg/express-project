using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Wlst.Ux.EquipmentInfo.DailyStatistics.TerminalViewModel.ViewModel;

namespace Wlst.Ux.EquipmentInfo.DailyStatistics.TerminalViewModel.ViewModel
{
    public class AxisLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string _op = (string)value;
            //if (TerminalViewModel.OperatorTypes == null) return " ";
            //if (TerminalViewModel.OperatorTypes.ContainsKey(_op) == false) return " ";
            //return TerminalViewModel.OperatorTypes[_op];
            switch (_op)
            {
                default:
                    return "";
                case "1":
                    return "开灯";
                case "2":
                    return "关灯";
                case "3":
                    return "开关灯应答";
                case "4":
                    return "对时";
                case "5":
                    return "对时应答";
                case "6":
                    return "发送周设置";
                case "7":
                    return "发送周设置应答";
                case "8":
                    return "发送参数";
                case "9":
                    return "发送参数应答";
                case "10":
                    return "终端复位";
                case "11":
                    return "终端复位应答";

            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
