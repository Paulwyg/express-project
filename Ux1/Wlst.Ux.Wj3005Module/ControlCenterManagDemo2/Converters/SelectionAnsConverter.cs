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
    public class SelectionAnsConverter : IValueConverter
    {
        private static BitmapSource _readyBitmap;

        private static BitmapSource ReadyBitmap
        {
            get
            {
                if (_readyBitmap == null)
                {
                    _readyBitmap = ImageSourceHelper.MySelf.GetBitmapSourceById(10000001);
                }
                return _readyBitmap;
            }

        }


        private static BitmapSource _openBitmap;

        private static BitmapSource OpenBitmap
        {
            get
            {
                if (_openBitmap == null)
                {
                    _openBitmap = ImageSourceHelper.MySelf.GetBitmapSourceById(10000008);
                }
                return _openBitmap;
            }

        }


        private static BitmapSource _replyBitmap;

        private static BitmapSource ReplyBitmap
        {
            get
            {
                if (_replyBitmap == null)
                {
                    _replyBitmap = ImageSourceHelper.MySelf.GetBitmapSourceById(10000009);
                }
                return _replyBitmap;
            }

        }

        private static BitmapSource _closeBitmap;

        private static BitmapSource CloseBitmap
        {
            get
            {
                if (_closeBitmap == null)
                {
                    _closeBitmap = ImageSourceHelper.MySelf.GetBitmapSourceById(10000007);
                }
                return _closeBitmap;
            }

        }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return ReadyBitmap;
            }
            try
            {
                EnumSelectionTestAns param = (EnumSelectionTestAns)value;
                if (param == EnumSelectionTestAns.Ready) return ReadyBitmap ;
                else if (param == EnumSelectionTestAns.Open) return OpenBitmap;
                else if (param == EnumSelectionTestAns.Reply) return ReplyBitmap;
                else return CloseBitmap;
            }
            catch (Exception ex)
            {

            }
            return ReadyBitmap;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

    }


    /// <summary>
    /// 要求参数为 int
    /// </summary>
    public class SelectionAnsConverterOne : IValueConverter
    {
        private static BitmapSource _readyBitmap;

        private static BitmapSource ReadyBitmap
        {
            get
            {
                if (_readyBitmap == null)
                {
                    _readyBitmap = ImageSourceHelper.MySelf.GetBitmapSourceById(10000001);
                }
                return _readyBitmap;
            }

        }





        private static BitmapSource _replyBitmap;

        private static BitmapSource ReplyBitmap
        {
            get
            {
                if (_replyBitmap == null)
                {
                    _replyBitmap = ImageSourceHelper.MySelf.GetBitmapSourceById(10000009);
                }
                return _replyBitmap;
            }

        }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return ReadyBitmap;
            }
            try
            {
                EnumSelectionTestAns param = (EnumSelectionTestAns)value;
                if (param == EnumSelectionTestAns.Ready) return ReadyBitmap;
                else if (param == EnumSelectionTestAns.Open) return ReplyBitmap;
                else if (param == EnumSelectionTestAns.Reply) return ReplyBitmap;
                else return ReplyBitmap;
            }
            catch (Exception ex)
            {

            }
            return ReadyBitmap;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

    }
}
