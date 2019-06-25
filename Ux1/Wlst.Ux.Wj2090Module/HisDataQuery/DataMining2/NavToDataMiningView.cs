using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.Wj2090Module.HisDataQuery.DataMining2
{
   

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToDataMiningView : MenuItemBase
    {
        public NavToDataMiningView()
        {
            Id = Wj2090Module.Services.MenuIdAssign.NavToDataMining2ViewId;
                // Infrastructure.IdAssign.MenuIdAssign.NavToEquipmentDailyDataQueryViewId;
            Text = "单灯电量统计";
            Tag = "单灯电量统计";
            this.Classic = "主菜单";
            Description = "单灯电量统计2，ID 为" + Wj2090Module.Services.MenuIdAssign.NavToDataMining2ViewId;
                // Infrastructure.IdAssign.MenuIdAssign.NavToEquipmentDailyDataQueryViewId;
            Tooltips = "单灯电量统计";
            base.IsEnabled = true;
            base.IsCheckable = false;
            //IsPrivilegLeave = false;
            base.Command = new RelayCommand(Ex, CanEx, true);
        }
        public override bool IsCanBeShowRwx()
        {
            return true;
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanR();
            //var equipment = this.Argu as Wlst.Sr .EquipmentInfoHolding .Model .WjParaBase ;
            //if (equipment == null || equipment.RtuStateCode  == 0) return false;
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanR(equipment.AreaId);
        }
        bool CanEx()
        {
            return true;
        }


        protected void Ex()
        {
            this.ExNavWithArgs(
                //ViewIdNameAssign.EquipmentDataQueryEquipmentDailyDataQueryViewAttachRegion,
                               Wj2090Module.Services.ViewIdAssign.DataMining2ViewId,
                //   ViewIdNameAssign.EquipmentDataQueryEquipmentDailyDataQueryViewId,
                               0);
            // MessageBoxResult result = MessageBox.Show("Sorry for have't finish this function ......", "Sorry", MessageBoxButton.OK);
        }


    }
}
