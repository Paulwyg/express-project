using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Wlst.Cr.CoreOne.Services;

namespace Wlst.Ux.WJ3005Module.ZPartol.PartolViewMoel.Services
{

    /// <summary>
    /// 要求参数为 int
    /// </summary>
    public class ImageConverter : IValueConverter
    {
        private static BitmapSource _offBitmap;

        private static BitmapSource OffBitmap
        {
            get
            {
                if (_offBitmap == null)
                {
                    _offBitmap = ImageSourceHelper.MySelf.GetBitmapSourceById(1001);
                }
                return _offBitmap;
            }

        }


        private static BitmapSource _onBitmap;

        private static BitmapSource OnBitmap
        {
            get
            {
                if (_onBitmap == null)
                {
                    _onBitmap = ImageSourceHelper.MySelf.GetBitmapSourceById(1002);
                }
                return _onBitmap;
            }

        }



        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return OffBitmap;
            }
            try
            {
                bool param = (bool) value;
                if (param) return OnBitmap;
                else return OffBitmap;
            }
            catch (Exception ex)
            {

            }
            return OffBitmap;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

    }
}
