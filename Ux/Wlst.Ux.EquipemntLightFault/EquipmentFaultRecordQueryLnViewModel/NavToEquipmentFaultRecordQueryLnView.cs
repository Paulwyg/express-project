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
    public class NavToEquipmentFaultRecordQueryLnView : MenuItemBase
    {
        public NavToEquipmentFaultRecordQueryLnView()
        {
            Id = EquipemntLightFault.Services.MenuIdAssgin.NavToEquipmentFaultRecordQueryLnViewId;
            //Infrastructure.IdAssign.MenuIdAssign.NavToEquipmentFaultRecordQueryViewId;
            Text = "火零不平衡查询";
            Tag = "或零不平衡查询";
            Classic = "主菜单";
            Description = "火零不平衡故障查询，ID 为" + EquipemntLightFault.Services.MenuIdAssgin.NavToEquipmentFaultRecordQueryLnViewId;
            //Infrastructure.IdAssign.MenuIdAssign.NavToEquipmentFaultRecordQueryViewId;
            Tooltips = "火零不平衡故障查询";
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

                EquipemntLightFault.Services.ViewIdAssign.EquipmentFaultRecordQueryLnViewId,
                               0);

            // MessageBoxResult result = MessageBox.Show("Sorry for have't finish this function ......", "Sorry", MessageBoxButton.OK);
        }
    
    }
}
