using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

using JetBrains.Annotations;

namespace Elysium.Converters
{
    [PublicAPI]
    [ValueConversion(typeof(short), typeof(short))]
    [ValueConversion(typeof(int), typeof(int))]
    [ValueConversion(typeof(long), typeof(long))]
    [ValueConversion(typeof(float), typeof(float))]
    [ValueConversion(typeof(double), typeof(double))]
    [ValueConversion(typeof(decimal), typeof(decimal))]
    public sealed class PercentToAngleConverter : IValueConverter
    {
        [PublicAPI]
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is short)
            {
                return (short)((short)value * 360);
            }
            if (value is int)
            {
                return (int)value * 360;
            }
            if (value is long)
            {
                return (long)value * 360L;
            }
            if (value is float)
            {
                return (float)value * 360f;
            }
            if (value is double)
            {
                return (double)value * 360d;
            }
            if (value is decimal)
            {
                return (decimal)value * 360m;
            }
            Trace.TraceError("Value must be a number.");
            return DependencyProperty.UnsetValue;
        }

        [PublicAPI]
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is short)
            {
                return (short)((short)value * (1d / 360d));
            }
            if (value is int)
            {
                return (int)((int)value * (1d / 360d));
            }
            if (value is long)
            {
                return (long)((long)value * (1d / 360d));
            }
            if (value is float)
            {
                return (float)value * (1f / 360f);
            }
            if (value is double)
            {
                return (double)value * (1d / 360d);
            }
            if (value is decimal)
            {
                return (decimal)value * (1m / 360m);
            }
            Trace.TraceError("Value must be a number.");
            return DependencyProperty.UnsetValue;
        }
    }
}
