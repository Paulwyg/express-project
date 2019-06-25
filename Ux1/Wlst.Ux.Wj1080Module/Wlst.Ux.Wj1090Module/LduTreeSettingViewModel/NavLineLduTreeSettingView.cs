using System.ComponentModel.Composition;
using Wlst.Cr.Setting.Interfaces;
using Wlst.Cr.Setting.Models;

namespace Wlst.Ux.Wj1090Module.LduTreeSettingViewModel
{
    [Export(typeof (IISetting))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavLineLduTreeSettingView : SettingBase
    {

        public NavLineLduTreeSettingView()
        {
            this.Id = Wj1090Module.Services.MenuIdAssgin.NavLineLduTreeSettingViewId;
            this.ViewId = Wj1090Module.Services.ViewIdAssign.LduTreeSettingViewId;// Infrastructure.IdAssign.ViewIdNameAssign.TreeModuleViewsSettingViewId;
            this.PathSetting = "设备选项#防盗设备";
        }
    }
}
