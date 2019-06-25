using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.Setting.Interfaces;
using Wlst.Cr.Setting.Models;

namespace Wlst.Ux.StateBarModule.CommonSet
{

    [Export(typeof (IISetting))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavLineCommonSettingView : SettingBase
    {

        public NavLineCommonSettingView()
        {
            this.Id = Services.MenuIdAssgin.NavLineCommonSettingViewId;
            this.ViewId = Services.ViewIdAssign.CommonSettingViewId;
            // Infrastructure.IdAssign.ViewIdNameAssign.TreeModuleViewsSettingViewId;
            this.PathSetting = "系统常规";
        }
    }
}
