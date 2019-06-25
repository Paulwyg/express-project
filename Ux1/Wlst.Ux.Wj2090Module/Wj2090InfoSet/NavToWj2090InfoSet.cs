using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.CoreMims.Commands;
 

namespace Wlst.Ux.Wj2090Module.Wj2090InfoSet
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToWj2090InfoSet : MenuItemBase
    {
        public  NavToWj2090InfoSet() 
        {
            Id = Wj2090Module.Services.MenuIdAssign.NavToWj2090SluInfoSetMenuId;
            Text = "参数设置";
            Tag = "WJ2090单灯参数设置";
            this.Classic = "右键菜单-WJ2090-专有";
            Description = "WJ2090单灯参数设置,ID为" + Wj2090Module.Services.MenuIdAssign.NavToWj2090SluInfoSetMenuId;
            Tooltips = "WJ2090终端参数设置";
            base.IsEnabled = true;
            base.IsCheckable = false;
            base.Command = new RelayCommand(Ex);
            //IsPrivilegLeave = true;
        }
        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanW();
            var equipment = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (equipment == null || equipment.RtuStateCode  == 0) return false;
            var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(equipment.RtuId);
            return Wlst.Cr.CoreMims.Services.UserInfo.CanW(areaId);
        }
        protected void Ex()
        {
            
            var equipment = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (equipment == null) return;
            int rtuId = equipment.RtuId;
            if (rtuId < 1) return;
            this.ExNavWithArgs(
                Wj2090Module.Services.ViewIdAssign.Wj2090SluInfoSetViewId,
                rtuId);
        }
    }
}
