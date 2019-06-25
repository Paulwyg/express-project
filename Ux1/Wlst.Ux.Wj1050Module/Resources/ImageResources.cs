using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using Wlst.client;

namespace Wlst.Ux.Wj1050Module.Resources
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
                return Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapImage("3005"); 


            }
        }

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


        private static BitmapImage _mruIcon1050;

        /// <summary>
        /// Open NoErr 1
        /// </summary>
        public static BitmapImage MruIcon1050
        {
            get
            {
                return Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapImage("10501"); 
            }
        }

        public static BitmapImage MruIconOn1050
        {
            get
            {
                return Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapImage("10501");
            }
        }
        public static BitmapImage MruIconOff1050
        {
            get
            {
                return Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapImage("10500");
            }
        }
    }
}
