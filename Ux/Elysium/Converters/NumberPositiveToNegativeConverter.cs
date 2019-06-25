using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

using JetBrains.Annotations;

namespace Elysium.Converters
{
    [PublicAPI]
    [ValueConversion(typeof(byte), typeof(byte))]
    [ValueConversion(typeof(short), typeof(short))]
    [ValueConversion(typeof(int), typeof(int))]
    [ValueConversion(typeof(long), typeof(long))]
    [ValueConversion(typeof(float), typeof(float))]
    [ValueConversion(typeof(double), typeof(double))]
    [ValueConversion(typeof(decimal), typeof(decimal))]
    public sealed class NumberPositiveToNegativeConverter : IValueConverter
    {
        [PublicAPI]
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is byte)
            {
                return (byte)((byte)value * -1);
            }
            if (value is short)
            {
                return (short)((short)value * -1);
            }
            if (value is int)
            {
                return (int)value * -1;
            }
            if (value is long)
            {
                return (long)value * -1L;
            }
            if (value is float)
            {
                return (float)value * -1f;
            }
            if (value is double)
            {
                return (double)value * -1d;
            }
            if (value is decimal)
            {
                return (decimal)value * -1m;
            }
            return DependencyProperty.UnsetValue;
        }

        [PublicAPI]
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}
