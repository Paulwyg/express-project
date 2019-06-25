using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace Wlst.Ux.Wj2090Module.Services
{
    public class ImageResources
    {
        #region tml tree node icon image


        //public const string ImageBasePaht = @"pack://siteoforigin:,,,/Image/EquipmentImage/";
        //public const string ImageEndType = ".png";

        //protected static Dictionary<int, BitmapImage> Info = new Dictionary<int, BitmapImage>();


        /// <summary>
        /// 2090 正常 2091 故障
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static BitmapImage GetEquipmentIcon(int state)
        {

                return Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapImage(state ); 


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
                return Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapImage("RtuGroupIcon"); 

            }
        }

        private static BitmapImage _ctrlIcon;

        /// <summary>
        /// 控制器
        /// </summary>
        public static BitmapImage CtrlIcon
        {
            get
            {
                return Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapImage("2090100"); 

            }
        }

        /// <summary>
        /// 单灯头控制器
        /// </summary>
        public static BitmapImage CtrlIconOne
        {
            get
            {
                return Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapImage("2090101");

            }
        }
        /// <summary>
        /// 双灯头控制器
        /// </summary>
        public static BitmapImage CtrlIconTwo
        {
            get
            {
                return Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapImage("2090201");

            }
        }
        private static BitmapImage _sluIcon;

        /// <summary>
        /// 控制器
        /// </summary>
        public static BitmapImage SluIcon
        {
            get
            {
                return Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapImage("2090"); 

            }
        }

         
    }
}
