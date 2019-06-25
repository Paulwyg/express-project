using System.ComponentModel.Composition;
using Wlst.Cr.Setting.Interfaces;
using Wlst.Cr.Setting.Models;
using Wlst.Ux.Wj3005ExNewDataExcelModule.Services;

namespace Wlst.Ux.Wj3005ExNewDataExcelModule.ZNewData.TmlNewDataViewModel
{

    [Export(typeof (IISetting))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavLineNewDataSettingView : SettingBase
    {

        public NavLineNewDataSettingView()
        {
            this.Id = MenuIdAssgin.NavLineNewDataSettingViewId;
            this.ViewId = ViewIdAssign.NewDataSettingViewId;
                // Infrastructure.IdAssign.ViewIdNameAssign.TreeModuleViewsSettingViewId;
            this.PathSetting = "最新数据";
        }
    }
}
