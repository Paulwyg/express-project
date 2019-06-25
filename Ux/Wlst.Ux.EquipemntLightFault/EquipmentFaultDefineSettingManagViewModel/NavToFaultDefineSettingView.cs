using System.ComponentModel.Composition;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultDefineSettingManagViewModel
{

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToFaultDefineSettingView : MenuItemBase
    {
        public NavToFaultDefineSettingView()
        {
            Id = EquipemntLightFault.Services.MenuIdAssgin.NavToFaultDefineSettingManagViewId;
            // Infrastructure.IdAssign.MenuIdAssign.NavToEquipmentFaultDefineSettingViewId;
            Text = "自定义故障管理";
            Tag = "自定义故障管理";
            Classic = "主菜单";
            Description = "自定义故障管理，ID 为" + EquipemntLightFault.Services.MenuIdAssgin.NavToFaultDefineSettingManagViewId;
            // Infrastructure.IdAssign.MenuIdAssign.NavToEquipmentFaultDefineSettingViewId;
            Tooltips = "自定义故障管理";
            IsEnabled = true;
            IsCheckable = false;
            base.Command = new RelayCommand(Ex,CanEx,true );
            IsPrivilegLeave = true;
        }

        private static bool CanEx()
        {
            return true;
        }

        protected void Ex()
        {
            ExNavWithArgs(
                EquipemntLightFault .Services .ViewIdAssign.FaultDefineSettingManagViewAttachRegion ,
                EquipemntLightFault .Services .ViewIdAssign.FaultDefineSettingManagViewId ,
                               1);
        }


    }
}
