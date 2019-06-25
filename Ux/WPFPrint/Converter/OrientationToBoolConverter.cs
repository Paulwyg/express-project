using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Printing;
using System.Text;
using System.Windows.Data;

namespace HappyPrint.Converter
{
    /// <summary>
    /// 方向转换
    /// </summary>
    class OrientationToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var orientation = (PageOrientation)value;

            return System.Enum.GetName(typeof(PageOrientation), orientation) == parameter.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isChecked = (bool)value;

            if (!isChecked)
            {
                return null;
            }

            return System.Enum.Parse(typeof(PageOrientation),parameter.ToString());
        }
    }
}
