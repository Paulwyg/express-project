using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Wlst.Ux.EquipmentInfo.SystemData.MainViewModel.ViewModel
{
    public class FaultTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var op = (string)value;
            var name = "";
            if (MainViewModel.PassTypeItems == null) return "";
            foreach (var f in MainViewModel.PassTypeItems)
            {
                if (f.Name.Split('-')[0]==op)
                {
                    name = f.Name.Split('-')[1];
                    break;
                }
            }
            return name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
