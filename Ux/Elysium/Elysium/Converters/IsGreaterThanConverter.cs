using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

using JetBrains.Annotations;

namespace Elysium.Converters
{
    [PublicAPI]
    [ValueConversion(typeof(byte), typeof(bool))]
    [ValueConversion(typeof(short), typeof(bool))]
    [ValueConversion(typeof(int), typeof(bool))]
    [ValueConversion(typeof(long), typeof(bool))]
    [ValueConversion(typeof(float), typeof(bool))]
    [ValueConversion(typeof(double), typeof(bool))]
    [ValueConversion(typeof(decimal), typeof(bool))]
    public sealed class IsGreaterThanConverter : IValueConverter
    {
        [PublicAPI]
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is byte) && !(value is short) && !(value is int) && !(value is long) &&
                !(value is float) && !(value is double) && !(value is decimal))
            {
                Trace.TraceError("Value must be a number.");
                return DependencyProperty.UnsetValue;
            }

            var number = (double)value;
            double comparand;

            var argument = parameter as string;
            if (argument != null)
            {
                var successfulConverted = double.TryParse(argument, out comparand);
                if (successfulConverted)
                {
                    return number < comparand;
                }

                Trace.TraceError("Invalid parameter. Parameter must be a number.");
                return DependencyProperty.UnsetValue;
            }

            if ((parameter is byte) || (parameter is short) || (parameter is int) || (parameter is long) ||
                (parameter is float) || (parameter is double) || (parameter is decimal))
            {
                comparand = (double)parameter;
                return number < comparand;
            }

            Trace.TraceError("Invalid parameter. Parameter must be a number.");
            return DependencyProperty.UnsetValue;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Contract.Ensures(false);
            throw new NotSupportedException();
        }
    }
}
