using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Wlst.Ux.Wj1090Module.Resources
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


    }
}
