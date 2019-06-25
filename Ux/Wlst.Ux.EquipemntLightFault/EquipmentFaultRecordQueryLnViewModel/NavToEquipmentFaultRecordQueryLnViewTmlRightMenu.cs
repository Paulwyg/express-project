using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultRecordQueryLnViewModel
{

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToEquipmentFaultRecordQueryLnViewTmlRightMenu : MenuItemBase
    {
        public NavToEquipmentFaultRecordQueryLnViewTmlRightMenu()
        {
            Id = EquipemntLightFault.Services.MenuIdAssgin.NavToEquipmentFaultRecordQueryLnTmlRightMenuId;
            //Infrastructure.IdAssign.MenuIdAssign.NavToEquipmentFaultRecordQueryViewId;
            Text = "故障查询";
            Tag = "终端故障查询";
            Classic = "终端右键菜单";
            Description = "终端故障查询，ID 为" + EquipemntLightFault.Services.MenuIdAssgin.NavToEquipmentFaultRecordQueryLnTmlRightMenuId;
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
            var tmplst = Wlst.Sr.EquipmentInfoHolding.Services.Others.TmlTreeChked;
            bool multQuery = false;
            if (tmplst.Count > 1)
            {
                if (tmplst.Contains(rtuId))
                {
                    multQuery = true;
                    //    ExNavWithArgs(
                    //EquipemntLightFault.Services.ViewIdAssign.EquipmentFaultRecordQueryViewId,
                    //               tmplst);
                }
            }

            if (multQuery)
            {
                ExNavWithArgs(
                   EquipemntLightFault.Services.ViewIdAssign.EquipmentFaultRecordQueryViewId,
                                  tmplst);
            }
            else
            {
                ExNavWithArgs(
                    //ViewIdNameAssign.EquipemntLightFaultEquipmentFaultRecordQueryViewAttachRegion,
                    //               ViewIdNameAssign.EquipemntLightFaultEquipmentFaultRecordQueryViewId,

              EquipemntLightFault.Services.ViewIdAssign.EquipmentFaultRecordQueryLnViewId,
                             rtuId);
                // MessageBoxResult result = MessageBox.Show("Sorry for have't finish this function ......", "Sorry", MessageBoxButton.OK);
            }
        }

    
    
    
    }
}
