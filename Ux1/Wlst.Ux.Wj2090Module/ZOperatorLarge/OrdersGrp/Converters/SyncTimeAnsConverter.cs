using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Wlst.Cr.CoreOne.Services;

namespace Wlst.Ux.Wj2090Module.ZOperatorLarge.OrdersGrp.Converters
{

    /// <summary>
    /// 要求参数为 int
    /// </summary>
    public class SyncTimeAnsConverter : IValueConverter
    {
        private static BitmapSource _noAnswerBitmap;

        private static BitmapSource NoAnswerBitmap
        {
            get
            {
                if (_noAnswerBitmap == null)
                {
                    _noAnswerBitmap = ImageSourceHelper.MySelf.GetBitmapSourceById(10000001);
                }
                return _noAnswerBitmap;
            }

        }




































































































        private static BitmapSource _yesAnswerBitmap;

        private static BitmapSource YesAnswerBitmap
        {
            get
            {
                if (_yesAnswerBitmap == null)
                {
                    _yesAnswerBitmap = ImageSourceHelper.MySelf.GetBitmapSourceById(10000009);
                }
                return _yesAnswerBitmap;
            }

        }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null ;
            }
            try
            {
                bool param = (bool)value;
                if (param) return YesAnswerBitmap;
                return null ;
            }
            catch (Exception ex)
            {

            }
            return null ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

    }
}
