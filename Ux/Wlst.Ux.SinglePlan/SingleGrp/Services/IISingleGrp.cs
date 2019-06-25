using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Ux.SinglePlan.SingleGrp.Services
{
    public interface IISingleGrp : Wlst.Cr.Core.CoreInterface.IINavOnLoad, Wlst.Cr.Core.CoreInterface.IITab,
                                     Wlst.Cr.Core.CoreInterface.IIOnHideOrClose
    {
    }
}
