using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.Setting.Interfaces;
using Wlst.Cr.Setting.Models;

namespace Wlst.Ux.Wj2090Module.TreeTab.Set
{

    [Export(typeof(IISetting))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavLineWj2090TreeSet : SettingBase
    {

        public NavLineWj2090TreeSet()
        {
            this.Id = Wj2090Module.Services.MenuIdAssign.NavLineWj2090TreeSetId;
            this.ViewId = Wj2090Module.Services.ViewIdAssign.Wj2090TreeSetViewId;// Infrastructure.IdAssign.ViewIdNameAssign.TreeModuleViewsSettingViewId;
            this.PathSetting = "设备选项#单灯设备";
        }
    }
}
