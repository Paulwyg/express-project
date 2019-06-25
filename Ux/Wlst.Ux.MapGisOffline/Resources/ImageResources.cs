using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Wlst.Ux.MapGisLocal.Resources
{
    public class ImageResources
    {
        #region tml tree node icon image



        public static BitmapImage GetEquipmentIcon(int state)
        {
            return Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapImage(state);
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

        private static BitmapImage _esuIcon;

        /// <summary>
        /// 节电
        /// </summary>
        public static BitmapImage EsuIcon
        {
            get
            {
                return Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapImage("601");
            }
        }

        private static BitmapImage _mruIcon;

        /// <summary>
        /// 电表
        /// </summary>
        public static BitmapImage MruIcon
        {
            get
            {
                return Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapImage("1050");
            }
        }

        private static BitmapImage _luxIcon;

        /// <summary>
        /// 光控
        /// </summary>
        public static BitmapImage LuxIcon
        {
            get
            {
                return Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapImage("1080");
            }
        }

        private static BitmapImage _lduIcon;

        /// <summary>
        /// 线路检测
        /// </summary>
        public static BitmapImage LduIcon
        {
            get
            {
                return Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapImage("1090");
            }
        }

        private static BitmapImage _sluIcon;

        //public static ESRI.ArcGIS.Client.Symbols.PictureMarkerSymbol SluIcon
        //{
        //    get
        //    {
        //        return Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapImage("2090");
        //    }
        //}
    }
}
