using System.ComponentModel.Composition;
using Wlst.Cr.Setting.Interfaces;
using Wlst.Cr.Setting.Models;

namespace Wlst.Ux.LdEquipemntTree.SettingViewModel
{
    [Export(typeof (IISetting))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavLineSettingView : SettingBase
    {

        public NavLineSettingView()
        {
             this.Id = LdEquipemntTree.Services.MenuIdAssgin.NavLineTreeSettingViewId;
            this.ViewId = LdEquipemntTree.Services.ViewIdAssign.SettingViewId;// Infrastructure.IdAssign.ViewIdNameAssign.TreeModuleViewsSettingViewId;
            this.PathSetting = "路灯选项";
        }
    }
}
