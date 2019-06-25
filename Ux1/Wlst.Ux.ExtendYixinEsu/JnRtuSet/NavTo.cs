using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.CoreServices;

namespace Wlst.Ux.ExtendYixinEsu.JnRtuSet
{
    public class NavTo
    {
        internal static void NavToLdl()
        {
            RegionManage.ShowViewByIdAttachRegionWithArgu(ExtendYixinEsu.Services.ViewIdAssign.JnRtuSetViewId, 0);
        }
    }
}
