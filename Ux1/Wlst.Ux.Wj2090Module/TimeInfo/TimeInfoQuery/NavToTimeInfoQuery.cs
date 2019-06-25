using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
 

namespace Wlst.Ux.Wj2090Module.TimeInfo.TimeInfoQuery
{


    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToTimeInfoQuery : MenuItemBase
    {
        public NavToTimeInfoQuery()
        {
            Id = Wj2090Module.Services.MenuIdAssign.TimeInfoQueryViewId;
            Text = "查询时间方案";
            Tag = "查询时间方案";
            Classic = "右键菜单-WJ2090-专有";
            Description = "单灯集中器右键菜单，ID 为"
                          + Wj2090Module.Services.MenuIdAssign.TimeInfoQueryViewId;
            Tooltips = "查询时间方案";
            IsCheckable = false;
            IsEnabled = true;
            base.Command = new RelayCommand(Ex, CanEx, true);
            //IsPrivilegLeave = false;

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
        private bool CanEx()
        {
            return true;
        }

        protected void Ex()
        {
            var equipment = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (equipment == null) return;
            int rtuId = equipment.RtuId;
            if (rtuId < 1) return;

            ExNavWithArgs(
                Wj2090Module.Services.ViewIdAssign.TimeInfoQueryViewId,
                rtuId);
        }
    }
}
