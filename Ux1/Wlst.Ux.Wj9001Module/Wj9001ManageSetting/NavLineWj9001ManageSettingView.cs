using System.ComponentModel.Composition;
using Wlst.Cr.Setting.Interfaces;
using Wlst.Cr.Setting.Models;

namespace Wlst.Ux.Wj9001Module.Wj9001ManageSetting
{
    [Export(typeof (IISetting))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavLineWj9001ManageSettingView : SettingBase
    {

        public NavLineWj9001ManageSettingView()
        {
            this.Id = Wj9001Module.Services.MenuIdAssgin.NavLineWj9001ManageSettingViewId;
            this.ViewId = Wj9001Module.Services.ViewIdAssign.Wj9001ManageSettingViewId;// Infrastructure.IdAssign.ViewIdNameAssign.TreeModuleViewsSettingViewId;
            this.PathSetting = "设备选项#漏电设备";
        }
    }
}
