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
    public class NavToConcentratorDataQuerySluCtrl : MenuItemBase
    {
        public NavToConcentratorDataQuerySluCtrl()
        {
            Id = MenuIdAssign.NavToConcentratorDataQuerySluCtrlId;
            Text = "历史数据查询";
            Tag = "单灯数据查询";
            this.Classic = "右键菜单-单灯控制器";
            Description = "单灯数据查询，ID 为" + MenuIdAssign.NavToConcentratorDataQuerySluCtrlId;
            Tooltips = "单灯数据查询";
            base.IsEnabled = true;
            base.IsCheckable = false;
            base.Command = new RelayCommand(Ex); //IsPrivilegLeave = false;
        }

        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanR();
            var equipment = this.Argu as  Wlst .Sr .EquipmentInfoHolding .Model .WjParaBase ;
            if (equipment == null || equipment.RtuStateCode  == 0) return false;
            var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(equipment.RtuId);
            return Wlst.Cr.CoreMims.Services.UserInfo.CanR(areaId);
        }
        protected void Ex()
        {

            var terminalInfo = this.Argu as Tuple<int, int>;
            if (terminalInfo == null)
            {
                // LogInfo.Log("无法执行关灯命令，参数错误....");
                return;
            }

            if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsSlu(terminalInfo.Item1) == false) return;

            ExNavWithArgs(
                          ViewIdAssign.ControlDataQueryViewId,
                          terminalInfo.Item1, terminalInfo.Item2);
        }

    }
}
