using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Wlst.Ux.EquipmentInfo.SystemData.MainViewModel.ViewModel
{
    public class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var _op  = System.Convert.ToInt32(value);
            return (_op / 60).ToString(CultureInfo.InvariantCulture).PadLeft(2,'0') + ":" + (_op % 60).ToString(CultureInfo.InvariantCulture).PadLeft(2,'0');
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
