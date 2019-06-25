using System.ComponentModel.Composition;
using Wlst.Cr.Setting.Interfaces;
using Wlst.Cr.Setting.Models;

namespace Wlst.Ux.CrissCrossEquipemntTree.SettingViewModel
{
    [Export(typeof (IISetting))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavLineSettingView : SettingBase
    {

        public NavLineSettingView()
        {
            this.Id = CrissCrossEquipemntTree.Services.MenuIdAssgin.NavLineTreeSettingViewId;
            this.ViewId = CrissCrossEquipemntTree.Services.ViewIdAssign.SettingViewId;// Infrastructure.IdAssign.ViewIdNameAssign.TreeModuleViewsSettingViewId;
            this.PathSetting = "交叉分组选项";
        }
    }
}
