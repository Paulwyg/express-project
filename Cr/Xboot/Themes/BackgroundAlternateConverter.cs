using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Xboot.Themes
{

    /// <summary>
    /// 斑马线
    /// </summary>
    public sealed class BackgroundAlternateConverter : IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var item = (Control) value;
            var itemsControl = ItemsControl.ItemsControlFromItemContainer(item);

            if (itemsControl == null) return Brushes.LightBlue;
            int index = itemsControl.ItemContainerGenerator.IndexFromContainer(item);
            if (index == -1)
            {
                index = itemsControl.Items.Count;
            }

            if (index%2 == 0)
            {
                return Brushes.LightBlue;
            }
            else
            {
                return Brushes.Beige;
            }
        }



        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Brushes.Transparent;
        }
    }
}
