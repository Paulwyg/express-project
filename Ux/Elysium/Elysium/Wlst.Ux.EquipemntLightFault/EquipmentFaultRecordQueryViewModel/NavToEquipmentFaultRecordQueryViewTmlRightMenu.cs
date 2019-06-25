using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;


namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultRecordQueryViewModel
{
   
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToEquipmentFaultRecordQueryViewTmlRightMenu : MenuItemBase
    {
        public NavToEquipmentFaultRecordQueryViewTmlRightMenu()
        {
            Id = EquipemntLightFault.Services.MenuIdAssgin.NavToEquipmentFaultRecordQueryViewTmlRightMenuId;
            //Infrastructure.IdAssign.MenuIdAssign.NavToEquipmentFaultRecordQueryViewId;
            Text = "故障查询";
            Tag = "终端故障查询";
            Classic = "终端右键菜单";
            Description = "终端故障查询，ID 为" + EquipemntLightFault.Services.MenuIdAssgin.NavToEquipmentFaultRecordQueryViewTmlRightMenuId;
            //Infrastructure.IdAssign.MenuIdAssign.NavToEquipmentFaultRecordQueryViewId;
            Tooltips = "终端故障查询";
            IsEnabled = true;
            IsCheckable = false;
            base.Command = new RelayCommand(Ex, CanEx, true);
        }
        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanR();
            var equipment = this.Argu as Wlst.Sr .EquipmentInfoHolding .Model .WjParaBase ;
            if (equipment == null || equipment.RtuStateCode  == 0) return false;
            var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(equipment.RtuId);
            return Wlst.Cr.CoreMims.Services.UserInfo.CanR(areaId );
        }
        static bool CanEx()
        {
            return true;
        }

        protected void Ex()
        {

            var equipment = this.Argu as Wlst.Sr .EquipmentInfoHolding .Model .WjParaBase ;
            if (equipment == null)
                return;
            int rtuId = equipment.RtuId;
            if (rtuId < 1) return;
            ExNavWithArgs(
                //ViewIdNameAssign.EquipemntLightFaultEquipmentFaultRecordQueryViewAttachRegion,
                //               ViewIdNameAssign.EquipemntLightFaultEquipmentFaultRecordQueryViewId,
                
                EquipemntLightFault.Services.ViewIdAssign.EquipmentFaultRecordQueryViewId,
                               rtuId);
            // MessageBoxResult result = MessageBox.Show("Sorry for have't finish this function ......", "Sorry", MessageBoxButton.OK);
        }


    }
}
