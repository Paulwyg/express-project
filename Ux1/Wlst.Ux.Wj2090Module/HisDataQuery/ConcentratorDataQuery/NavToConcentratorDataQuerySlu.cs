using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Ux.Wj2090Module.Services;

namespace Wlst.Ux.Wj2090Module.HisDataQuery.ConcentratorDataQuery
{
  
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToConcentratorDataQuerySlu : MenuItemBase
    {
        public NavToConcentratorDataQuerySlu()
        {
            Id = MenuIdAssign.NavToConcentratorDataQuerySluId;
            Text = "历史数据查询";
            Tag = "单灯数据查询";
            this.Classic = "右键菜单-单灯集中器";
            Description = "单灯数据查询，ID 为" + MenuIdAssign.NavToConcentratorDataQuerySluId;
            Tooltips = "单灯数据查询";
            base.IsEnabled = true;
            base.IsCheckable = false;
            base.Command = new RelayCommand(Ex);
           // IsPrivilegLeave = false;
        }

        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanR();
            var equipment = this.Argu as Wlst.Sr .EquipmentInfoHolding .Model .WjParaBase ;
            if (equipment == null || equipment.RtuStateCode  == 0) return false;
            var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(equipment.RtuId);
            return Wlst.Cr.CoreMims.Services.UserInfo.CanR(areaId);
        }

        protected void Ex()
        {

            var ars = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (ars == null) return;
            int sluId = ars.RtuId;
            if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsSlu(sluId) == false) return;

            ExNavWithArgs(
                          ViewIdAssign.ControlDataQueryViewId,
                          sluId);
        }

    }
}
