using System.ComponentModel.Composition;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.EquipemntLightFault.UserIndividuationFaultSettingViewModel
{

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToUserIndividuationFaultSettingView : MenuItemBase
    {
        public NavToUserIndividuationFaultSettingView()
        {
            Id = EquipemntLightFault.Services.MenuIdAssgin.NavToUserIndividuationFaultSettingViewId;
            // Infrastructure.IdAssign.MenuIdAssign.NavToEquipmentFaultDefineSettingViewId;
            Text = "用户故障报警";
            Tag = "用户故障报警";
            this.Classic = "主菜单";
            Description = "用户故障报警，ID 为" + EquipemntLightFault.Services.MenuIdAssgin.NavToUserIndividuationFaultSettingViewId;
            // Infrastructure.IdAssign.MenuIdAssign.NavToEquipmentFaultDefineSettingViewId;
            Tooltips = "用户故障报警";
            base.IsEnabled = true;
            base.IsCheckable = false;
            base.Command = new RelayCommand(Ex,CanEx ,true  );
            //this.IsPrivilegLeave = true;
        }
        public override bool IsCanBeShowRwx()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.Cand() == false;
            return true;// Wlst.Cr.CoreMims.Services.UserInfo.CanW();
        }
        bool CanEx()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.Cand() == false;
            return true;
        }

        protected void Ex()
        {
            this.ExNavWithArgs(
                //ViewIdNameAssign.EquipemntLightFaultEquipmentFaultDefineSettingViewAttachRegion,
                //               ViewIdNameAssign.EquipemntLightFaultEquipmentFaultDefineSettingViewId,
                EquipemntLightFault .Services .ViewIdAssign .UserIndividuationFaultSettingViewId  ,
                               1);
            // MessageBoxResult result = MessageBox.Show("Sorry for have't finish this function ......", "Sorry", MessageBoxButton.OK);
        }


    }
}
