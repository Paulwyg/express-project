using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.Setting.Interfaces;
using Wlst.Cr.Setting.Models;

namespace Wlst.Ux.StateBarModule.AreaSet
{

    [Export(typeof (IISetting))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavLineAreaSet : SettingBase
    {

        public NavLineAreaSet()
        {
            this.Id = Services.MenuIdAssgin.NavLineAreaSetId;
            this.ViewId = Services.ViewIdAssign.AreaSetViewId;
            // Infrastructure.IdAssign.ViewIdNameAssign.TreeModuleViewsSettingViewId;
            this.PathSetting = "主界面布局";
        }
    }
}
