using System;
using System.Globalization;
using System.Windows.Data;

namespace Wlst.Cr.CoreOne.RadioButtonConverter
{
    /// <summary>
    /// <para>(1) Add converter in resources: RadioButtonBooleanConverter x:Key="RadioButtonBooleanConverter"  </para>
    ///<para>(2) Use converter in RadioButton: </para>
    ///<para> RadioButton GroupName="rbGroupNew" </para>
    ///<para>IsChecked="{Binding Path=xxx, Mode=TwoWay, Converter={StaticResource RadioButtonBooleanConverter}, ConverterParameter=true}" Content="New" /> </para>
    /// <para>RadioButton GroupName="rbGroupOld" IsChecked="{Binding Path=xxxx, Mode=TwoWay, Converter={StaticResource RadioButtonBooleanConverter}, ConverterParameter=false}" Content="Old"  </para>

    /// </summary>
    [ValueConversion(typeof (bool), typeof (bool))]
    public class RadioButtonBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool param = bool.Parse(parameter.ToString());
            if (value == null)
            {
                return false;
            }
            else
            {
                return !((bool) value ^ param);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool param = bool.Parse(parameter.ToString());
            return !((bool) value ^ param);
        }

    }
}
