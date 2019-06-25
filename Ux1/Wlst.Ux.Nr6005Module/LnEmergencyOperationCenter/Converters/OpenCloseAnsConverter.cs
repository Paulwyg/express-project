using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Wlst.Cr.CoreOne.Services;
using Wlst.Ux.Nr6005Module.LnEmergencyOperationCenter.Services;

namespace Wlst.Ux.Nr6005Module.LnEmergencyOperationCenter.Converters
{

    /// <summary>
    /// 要求参数为 int
    /// </summary>
    public class OpenCloseAnsConverter : IValueConverter
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
                    _yesAnswerBitmap = ImageSourceHelper.MySelf.GetBitmapSourceById(10000005);
                }
                return _yesAnswerBitmap;
            }

        }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return NoAnswerBitmap;
            }
            try
            {
                EnumOpenOrCloseAns param = (EnumOpenOrCloseAns)value;
                if (param == EnumOpenOrCloseAns.NoAnswer) return NoAnswerBitmap;
                return YesAnswerBitmap;
            }
            catch (Exception ex)
            {

            }
            return NoAnswerBitmap;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

    }
}
