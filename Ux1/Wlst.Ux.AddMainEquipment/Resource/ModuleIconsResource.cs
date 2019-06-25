using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace Wlst.Ux.AddMainEquipment.Resource
{
    public  class ModuleIconsResource
    {
        public static BitmapImage GetTmlTreeIcon()
        {
            switch (state)
            {
                case 1:
                    return TmlOpenWithNoErr;
                    break;
                default:
                    return null;
            }
            return null;
        }
    }
}
