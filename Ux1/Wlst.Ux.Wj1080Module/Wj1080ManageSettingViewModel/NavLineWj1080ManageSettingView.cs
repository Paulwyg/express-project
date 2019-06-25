using System.ComponentModel.Composition;
using Wlst.Cr.Setting.Interfaces;
using Wlst.Cr.Setting.Models;

namespace Wlst.Ux.Wj1080Module.Wj1080ManageSettingViewModel
{
    [Export(typeof (IISetting))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavLineJd601ManageSettingView : SettingBase
    {

        public NavLineJd601ManageSettingView()
        {
            this.Id = Wj1080Module.Services.MenuIdAssgin.NavLineJd601ManageSettingView;
            this.ViewId = Wj1080Module.Services.ViewIdAssign.Wj1080ManageSettingViewId;// Infrastructure.IdAssign.ViewIdNameAssign.TreeModuleViewsSettingViewId;
            this.PathSetting = "设备选项#光控设备";
        }
    }
}
