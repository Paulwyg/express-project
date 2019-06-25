using System.ComponentModel.Composition;
using Wlst.Cr.Setting.Interfaces;
using Wlst.Cr.Setting.Models;

namespace Wlst.Ux.LhEquipemntTree.SettingViewModel
{
    [Export(typeof (IISetting))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavLineSettingView : SettingBase
    {

        public NavLineSettingView()
        {
             this.Id = LhEquipemntTree.Services.MenuIdAssgin.NavLineTreeSettingViewId;
            this.ViewId = LhEquipemntTree.Services.ViewIdAssign.SettingViewId;// Infrastructure.IdAssign.ViewIdNameAssign.TreeModuleViewsSettingViewId;
            this.PathSetting = "灯饰选项";
        }
    }
}
