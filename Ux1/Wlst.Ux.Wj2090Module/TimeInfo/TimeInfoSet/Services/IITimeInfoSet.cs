using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Ux.Wj2090Module.TimeInfo.Services
{
    public interface IITimeInfoSet : Wlst.Cr.Core.CoreInterface.IINavOnLoad, Wlst.Cr.Core.CoreInterface.IITab,
                                     Wlst.Cr.Core.CoreInterface.IIOnHideOrClose
    {
        event EventHandler OnBackNeedShowCtrlView;
        void OnUserSetOverSelectedSefDef();
        bool OnUserSetOverSlus();
        bool HaveSlu();
    }
}
