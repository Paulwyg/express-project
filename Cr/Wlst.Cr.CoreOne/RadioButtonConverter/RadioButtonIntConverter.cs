using System;
using System.Globalization;
using System.Windows.Data;

namespace Wlst.Cr.CoreOne.RadioButtonConverter
{

    /// <summary>
    /// 要求参数为 int
    /// </summary>
    public class RadioButtonIntConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            int integer = (int) value;

            if (integer == int.Parse(parameter.ToString()))

                return true;

            else

                return false;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool) value)
                return parameter
                    ;
            return null;

        }

    }
}
