using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;


namespace Wlst.Ux.Wj9001Module.Resources
{
    public class ImageResources
    {
        private static BitmapImage _rtuIcon30051;

        /// <summary>
        /// Open NoErr 1
        /// </summary>
        public static BitmapImage RtuIcon3005
        {
            get
            {
                if (_rtuIcon30051 == null)
                {
                    _rtuIcon30051 = new BitmapImage(new Uri(@"pack://siteoforigin:,,,/Image/EquipmentImage/3005.png"));
                }
                return _rtuIcon30051;
            }
        }

        protected static Dictionary<int, BitmapImage> Info = new Dictionary<int, BitmapImage>();
        public static BitmapImage GetEquipmentIcon(int state)
        {
            try
            {
                if (Info.ContainsKey(state)) return Info[state];
                var tmp = new BitmapImage(new Uri(@"pack://siteoforigin:,,,/Image/EquipmentImage/" + state + ".png"));
                Info.Add(state, tmp);
                return tmp;
            }
            catch (Exception ex)
            {
            }

            return null;
        }
        private static BitmapImage _goupIcon;

        /// <summary>
        /// Open NoErr 1
        /// </summary>
        public static BitmapImage GroupIcon
        {
            get
            {
                if (_goupIcon == null)
                {
                    try
                    {
                        _goupIcon =
                            new BitmapImage(new Uri(@"pack://siteoforigin:,,,/Image/EquipmentImage/RtuGroupIcon.png"));
                    }
                    catch (Exception ex)
                    {

                    }
                }
                return _goupIcon;
            }
        }


        private static BitmapImage _leakIcon9001;

        /// <summary>
        /// Open NoErr 1
        /// </summary>
        public static BitmapImage LeakIcon9001
        {
            get
            {
                if (_leakIcon9001 == null)
                {
                    _leakIcon9001 = new BitmapImage(new Uri(@"pack://siteoforigin:,,,/Image/EquipmentImage/9001.png"));
                }
                return _leakIcon9001;
            }
        }
    }
}
