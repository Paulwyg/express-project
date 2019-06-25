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
    public class WeekSndAnsConverter : IValueConverter
    {

        private static BitmapSource _readyBitmap;

        private static BitmapSource ReadyBitmap
        {
            get { return _readyBitmap ?? (_readyBitmap = ImageSourceHelper.MySelf.GetBitmapSourceById(10000001)); }
        }

        private static BitmapSource _k1K3AnsBitmap;

        private static BitmapSource K1K3AnsBitmap
        {
            get
            {
                if (_k1K3AnsBitmap == null)
                {
                    _k1K3AnsBitmap = ImageSourceHelper.MySelf.GetBitmapSourceById(10000002);
                }
                return _k1K3AnsBitmap;
            }

        }


        private static BitmapSource _k4K6AnsBitmap;

        private static BitmapSource K4K6AnsBitmap
        {
            get
            {
                if (_k4K6AnsBitmap == null)
                {
                    _k4K6AnsBitmap = ImageSourceHelper.MySelf.GetBitmapSourceById(10000003);
                }
                return _k4K6AnsBitmap;
            }

        }

        private static BitmapSource _allAnsBitmap;

        private static BitmapSource AllAnsBitmap
        {
            get { return _allAnsBitmap ?? (_allAnsBitmap = ImageSourceHelper.MySelf.GetBitmapSourceById(10000004)); }
        }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return ReadyBitmap;
            }
            try
            {
                EnumWeekSndAns param = (EnumWeekSndAns)value;
                if (param == EnumWeekSndAns.K1K3Ans) return "K1K3应答";// K1K3AnsBitmap;
                else if (param == EnumWeekSndAns.K4K6Ans) return "K4K6应答";// K4K6AnsBitmap;
                else if (param == EnumWeekSndAns.AllAns ) return "全部应答";
                else return "";// AllAnsBitmap;
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
