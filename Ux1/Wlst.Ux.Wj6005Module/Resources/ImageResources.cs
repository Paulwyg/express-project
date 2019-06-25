using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace Wlst.Ux.Wj6005Module.Resources
{
   public  class ImageResources
    {

       private static BitmapImage _esuIcon6005;

        /// <summary>
        /// Open NoErr 1
        /// </summary>
        public static BitmapImage EsuIcon6005
        {
            get
            {
                return Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapImage("601"); 
  
            }
        }
    }
}
