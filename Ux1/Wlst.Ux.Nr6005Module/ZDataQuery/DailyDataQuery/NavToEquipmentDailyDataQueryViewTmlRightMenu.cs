using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;


namespace Wlst.Ux.Nr6005Module.ZDataQuery.DailyDataQuery
{
   
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToEquipmentDailyDataQueryViewTmlRightMenu : MenuItemBase
    {
        public NavToEquipmentDailyDataQueryViewTmlRightMenu()
        {
            Id = Nr6005Module.Services.MenuIdAssgin.NavToEquipmentDailyDataQueryViewTmlRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.NavToEquipmentDailyDataQueryViewId;
            Text = "终端数据";
            Tag = "数据查询";
            this.Classic = "终端右键菜单";
            Description = "数据查询，ID 为" + Nr6005Module.Services.MenuIdAssgin.NavToEquipmentDailyDataQueryViewTmlRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.NavToEquipmentDailyDataQueryViewId;
            Tooltips = "数据查询";
            base.IsEnabled = true;
            base.IsCheckable = false;
            base.Command = new RelayCommand(Ex, CanEx, true);
        }
        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanR();
            var equipment = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (equipment == null || equipment.RtuStateCode  == 0) return false;
            var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(equipment.RtuId);
            return Wlst.Cr.CoreMims.Services.UserInfo.CanR(areaId);
        }
        bool CanEx()
        {
            return true;
        }


        protected void Ex()
        {
            var equipment = this.Argu as Wlst .Sr .EquipmentInfoHolding .Model .WjParaBase ;
            if (equipment == null)
                return;
            int rtuId = equipment.RtuId;
            if (rtuId < 1) return;
            this.ExNavWithArgs(
                //ViewIdNameAssign.EquipmentDataQueryEquipmentDailyDataQueryViewAttachRegion,
                Nr6005Module.Services.ViewIdAssign.EquipmentDailyDataQueryViewId,
                //   ViewIdNameAssign.EquipmentDataQueryEquipmentDailyDataQueryViewId,
                               rtuId);
            // MessageBoxResult result = MessageBox.Show("Sorry for have't finish this function ......", "Sorry", MessageBoxButton.OK);
        }


    }
}
