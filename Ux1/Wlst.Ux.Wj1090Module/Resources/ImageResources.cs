using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Wlst.Ux.Wj1090Module.Resources
{
    public class ImageResources
    {
        #region tml tree node icon image


        //public const string ImageBasePaht = @"pack://siteoforigin:,,,/Image/EquipmentImage/";
        //public const string ImageEndType = ".png";

        //protected static Dictionary<int, BitmapImage> Info = new Dictionary<int, BitmapImage>();


        public static BitmapImage GetEquipmentIcon(int state)
        {
            return Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapImage(state); 
            
        }
        private static BitmapImage _goupIcon;

        /// <summary>
        /// Open NoErr 1
        /// </summary>
        public static BitmapImage GroupIcon
        {
            get
            {
                return Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapImage("RtuGroupIcon"); 

            }
        }
        #endregion


    }
}
