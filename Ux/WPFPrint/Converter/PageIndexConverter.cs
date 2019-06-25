using System;
using System.Globalization;
using System.Windows.Data;

namespace HappyPrint.Converter
{
    class PageIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                int index = (int)value;

                index++;

                return index;
            }
            catch (Exception)
            {
                return 1;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int index = (int)value;

            return --index;
        }
    }
}
