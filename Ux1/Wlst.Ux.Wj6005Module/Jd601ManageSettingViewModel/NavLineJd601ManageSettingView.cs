using System.ComponentModel.Composition;
using Wlst.Cr.Setting.Interfaces;
using Wlst.Cr.Setting.Models;

namespace Wlst.Ux.Wj6005Module.Jd601ManageSettingViewModel
{
    [Export(typeof (IISetting))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavLineJd601ManageSettingView : SettingBase
    {

        public NavLineJd601ManageSettingView()
        {
            this.Id = Wj6005Module.Services.MenuIdAssgin.NavLineJd601ManageSettingView;
            this.ViewId = Wj6005Module.Services.ViewIdAssign.Jd601ManageSettingViewId;// Infrastructure.IdAssign.ViewIdNameAssign.TreeModuleViewsSettingViewId;
            this.PathSetting = "设备选项#节电设备";
        }
    }
}
