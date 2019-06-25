using System.ComponentModel.Composition;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultRecordQuerySZViewModel
{

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToEquipmentFaultRecordQueryView : MenuItemBase
    {
        public NavToEquipmentFaultRecordQueryView()
        {
            Id = EquipemntLightFault.Services.MenuIdAssgin.NavToEquipmentFaultRecordQuerySzViewId;
            //Infrastructure.IdAssign.MenuIdAssign.NavToEquipmentFaultRecordQueryViewId;
            Text = "故障查询";
            Tag = "故障查询";
            Classic = "主菜单";
            Description = "终端故障查询，ID 为" + EquipemntLightFault.Services.MenuIdAssgin.NavToEquipmentFaultRecordQuerySzViewId;
            //Infrastructure.IdAssign.MenuIdAssign.NavToEquipmentFaultRecordQueryViewId;
            Tooltips = "故障查询";
            IsEnabled = true;
            IsCheckable = false;
            base.Command = new RelayCommand(Ex, CanEx, true);
        }
        public override bool IsCanBeShowRwx()
        {
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanR();
            return true;
        }
        static bool CanEx()
        {
            return true;
        }

        protected void Ex()
        {
            ExNavWithArgs(
                //ViewIdNameAssign.EquipemntLightFaultEquipmentFaultRecordQueryViewAttachRegion,
                //               ViewIdNameAssign.EquipemntLightFaultEquipmentFaultRecordQueryViewId,
                
                EquipemntLightFault .Services .ViewIdAssign .EquipmentFaultRecordQuerySzViewId ,
                               0);
            // MessageBoxResult result = MessageBox.Show("Sorry for have't finish this function ......", "Sorry", MessageBoxButton.OK);
        }


    }
}
