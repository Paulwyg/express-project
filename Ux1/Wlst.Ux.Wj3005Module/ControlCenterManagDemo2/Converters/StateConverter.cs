using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Wlst.Cr.CoreOne.Services;
using Wlst.Ux.WJ3005Module.ControlCenterManagDemo2.Services;

namespace Wlst.Ux.WJ3005Module.ControlCenterManagDemo2.Converters
{

    /// <summary>
    /// 要求参数为 int
    /// </summary>
    public class StateConverter : IValueConverter
    {
        private static BitmapSource _useBitmap;

        private static BitmapSource UseBitmap
        {
            get
            {
                if (_useBitmap == null)
                {
                    _useBitmap = ImageSourceHelper.MySelf.GetBitmapSourceById(10000005);
                }
                return _useBitmap;
            }

        }


        private static BitmapSource _disableBitmap;

        private static BitmapSource DisableBitmap
        {
            get
            {
                if (_disableBitmap == null)
                {
                    _disableBitmap = ImageSourceHelper.MySelf.GetBitmapSourceById(10000006);
                }
                return _disableBitmap;
            }

        }

        private static BitmapSource _notUseBitmap;

        private static BitmapSource NotUseBitmap
        {
            get
            {
                if (_notUseBitmap == null)
                {
                    _notUseBitmap = ImageSourceHelper.MySelf.GetBitmapSourceById(10000010);//notuse
                }
                return _notUseBitmap;
            }

        }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return DisableBitmap;
            }
            try
            {
                EnumTmlState param = (EnumTmlState)value;
                if (param == EnumTmlState.Use) return UseBitmap;
                else if (param == EnumTmlState.Disable) return DisableBitmap;
                else return NotUseBitmap;
            }
            catch (Exception ex)
            {

            }
            return NotUseBitmap;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

    }
}
