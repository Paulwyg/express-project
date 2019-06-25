using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Ux.fuyang.BroadcastStrategy.Services
{
    public interface IIBroadcastStrategy : Wlst.Cr.Core.CoreInterface.IINavOnLoad, Wlst.Cr.Core.CoreInterface.IITab,
                                     Wlst.Cr.Core.CoreInterface.IIOnHideOrClose
    {
    }
}
