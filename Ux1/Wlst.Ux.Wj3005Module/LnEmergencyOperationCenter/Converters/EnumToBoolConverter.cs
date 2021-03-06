﻿using System;
using System.Globalization;
using System.Windows.Data;
using Wlst.Ux.WJ3005Module.LnEmergencyOperationCenter.Services;

namespace Wlst.Ux.WJ3005Module.LnEmergencyOperationCenter.Converters
{

    /// <summary>
    /// 要求参数为 int
    /// </summary>
    public class EnumToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (EnumTmlState)value == EnumTmlState.Use;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

    }
}
