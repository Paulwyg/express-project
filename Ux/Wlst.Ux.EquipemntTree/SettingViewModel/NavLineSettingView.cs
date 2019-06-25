using System.ComponentModel.Composition;
using Wlst.Cr.Setting.Interfaces;
using Wlst.Cr.Setting.Models;

namespace Wlst.Ux.EquipemntTree.SettingViewModel
{
    [Export(typeof (IISetting))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavLineSettingView : SettingBase
    {

        public NavLineSettingView()
        {
           this.Id = EquipemntTree.Services.MenuIdAssgin.NavLineTreeSettingViewId;
            this.ViewId = EquipemntTree.Services.ViewIdAssign.SettingViewId;// Infrastructure.IdAssign.ViewIdNameAssign.TreeModuleViewsSettingViewId;
            this.PathSetting = "分组选项";
        }
    }
}
