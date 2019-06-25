using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Wlst.Ux.RadMapJpeg.Resources
{
    public class ImageResource
    {
        #region tml tree node icon image



        public static BitmapFrame GetEquipmentIcon(int state)
        {

                return Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapFrame(state); 

        }

        #endregion


      
    }
}