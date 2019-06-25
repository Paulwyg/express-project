using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;


using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreMims.ShowMsgInfo;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
 

namespace Wlst.Ux.WJ3005Module.ZOrders.FaultCollection
{



    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class MeasureControllerForMenu : MenuItemBase
    {

        public MeasureControllerForMenu()
        {
            Id = Services.MenuIdAssgin.MenuRtuCalFaultCount;
            // Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMenuId;
            Tag = "当前故障数统计";
            Description = "当前故障数，ID为" + Services.MenuIdAssgin.MenuRtuCalFaultCount
                          // Infrastructure.IdAssign.MenuIdAssign.MeasureControllerForMenuId 
                          + ",类型为终端右键菜单。";
            Text = "当前故障数:";
            this.Classic = "右键菜单-监控设备通用";
            Tooltips = "当前故障数统计";
            base.Command = new RelayCommand(Ex, CanEx, true);
        }

        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanR();
            var equipment = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (equipment == null || equipment.RtuStateCode  == 0) return false;
            var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(equipment.RtuId);
            return Wlst.Cr.CoreMims.Services.UserInfo.CanR(areaId );
        }

        public override void InitDataWhenBeforeUse(object argu)
        {
            base.InitDataWhenBeforeUse(argu);
            //base.IsEnabled = SysRunInfo.CanOperator;
            var ter = argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (ter == null) return;

            this.ExText = GetErrorNewCount(ter.RtuId) + "";
        }

        private int GetErrorNewCount(int rtuId)
        {
 
            var ff =
                Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(rtuId);
            if (ff != null) return ff.ErrorCount;
            return 0;

        }

        protected bool CanEx()
        {
            var equipment = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (equipment == null || equipment.RtuStateCode == 0) return false;
            return true;

            return true;
        }

        private void Ex()
        {
        }
    }
}
