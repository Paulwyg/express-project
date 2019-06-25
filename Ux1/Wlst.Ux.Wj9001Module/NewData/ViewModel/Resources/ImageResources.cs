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
            get { return Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapImage("3005"); }
        }

       // protected static Dictionary<int, BitmapImage> Info = new Dictionary<int, BitmapImage>();
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


        private static BitmapImage _leakIcon9001;

        /// <summary>
        /// Open NoErr 1
        /// </summary>
        public static BitmapImage LeakIcon9001
        {
            get
            {
                return Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapImage("90011"); 
      
            }
        }
    }
}
