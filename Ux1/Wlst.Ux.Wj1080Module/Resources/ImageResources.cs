using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Wlst.Ux.Wj1080Module.Resources
{
   public  class ImageResources
    {
     //  protected static Dictionary<int, BitmapImage> Info = new Dictionary<int, BitmapImage>();
       public static BitmapImage GetEquipmentIcon(int state)
       {
           return Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapImage(state); 

       }

       private static BitmapImage _mruIcon1080;

        /// <summary>
        /// Open NoErr 1
        /// </summary>
        public static BitmapImage MruIcon1080
        {
            get
            {
                return Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapImage(1080); 
            }
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
    }
}
