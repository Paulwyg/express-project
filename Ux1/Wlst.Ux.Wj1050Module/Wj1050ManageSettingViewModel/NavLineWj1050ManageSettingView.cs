using System.ComponentModel.Composition;
using Wlst.Cr.Setting.Interfaces;
using Wlst.Cr.Setting.Models;

namespace Wlst.Ux.Wj1050Module.Wj1050ManageSettingViewModel
{
    [Export(typeof (IISetting))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavLineWj1050ManageSettingView : SettingBase
    {

        public NavLineWj1050ManageSettingView()
        {
            this.Id = Wj1050Module.Services.MenuIdAssgin.NavLineWj1050ManageSettingViewId;
            this.ViewId = Wj1050Module.Services.ViewIdAssign.Wj1050ManageSettingViewId;// Infrastructure.IdAssign.ViewIdNameAssign.TreeModuleViewsSettingViewId;
            this.PathSetting = "设备选项#电表设备";
        }
    }
}
