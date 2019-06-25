using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Wlst.Ux.EquipmentInfo.DailyStatistics.TerminalViewModel.ViewModel;

namespace Wlst.Ux.EquipmentInfo.SystemData.MainViewModel.ViewModel
{
    public class AxisLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var _op = (string)value;

            switch (_op)
            {
                default:
                    return "0";
                case "0":
                    return "0";
                case "120":
                    return "2";
                case "240":
                    return "4";
                case "360":
                    return "6";
                case "480":
                    return "8";
                case "600":
                    return "10";
                case "720":
                    return "12";
                case "840":
                    return "14";
                case "960":
                    return "16";
                case "1080":
                    return "18";
                case "1200":
                    return "20";
                case "1320":
                    return "22";
                case "1440":
                    return "24";


            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
