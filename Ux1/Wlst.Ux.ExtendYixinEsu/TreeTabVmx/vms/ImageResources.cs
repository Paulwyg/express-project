using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace Wlst.Ux.ExtendYixinEsu.TreeTabVmx.vms
{
    public class ImageResources
    {
        #region tml tree node icon image


        public const string ImageBasePaht = @"pack://siteoforigin:,,,/Image/EquipmentImage/";
        public const string ImageEndType = ".png";

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

        #endregion

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

        //new BitmapImage(new Uri(@"pack://siteoforigin:,,,/Image/EquipmentImage/RtuGroupIcon.png"));
    }


    public class TerminalPartsInfomation
    {
        public int Id;
        public string Name;
        public string RightMenuKey;
        public BitmapSource ImagesIcon;
    }
}
