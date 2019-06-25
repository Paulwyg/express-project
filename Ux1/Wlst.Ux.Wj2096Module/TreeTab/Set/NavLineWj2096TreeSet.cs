using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.Setting.Interfaces;
using Wlst.Cr.Setting.Models;

namespace Wlst.Ux.Wj2096Module.TreeTab.Set
{

    [Export(typeof(IISetting))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavLineWj2096TreeSet : SettingBase
    {

        public NavLineWj2096TreeSet()
        {
            this.Id =Ux.Wj2096Module.Services.MenuIdAssign.NavToWj2096TreeSetMenuId;
            this.ViewId = Ux.Wj2096Module.Services.ViewIdAssign.Wj2096TreeSetViewId;
            this.PathSetting = "设备选项#物联网单灯设备";
        }
    }
}
