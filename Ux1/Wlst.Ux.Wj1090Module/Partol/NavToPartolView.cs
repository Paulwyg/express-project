using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.Wj1090Module.Partol
{


    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToPartolView : MenuItemBase
    {
        public NavToPartolView()
        {
            Id = Wj1090Module.Services.MenuIdAssgin.NavToPartolViewId;
            Text = "线路巡测";
            Tag = "巡测线路检测集中器";
            Classic = "主菜单";
            Description = "线路巡测，主菜单，ID 为"
                          + Wj1090Module.Services.MenuIdAssgin.NavToPartolViewId;
            Tooltips = "线路巡测";
            IsCheckable = false;
            IsEnabled = true;
            base.Command = new RelayCommand(Ex, CanEx, true);
            //IsPrivilegLeave = false;

        }

        public override bool IsCanBeShowRwx()
        {
            //if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            ////return Wlst.Cr.CoreMims.Services.UserInfo.CanR();
            //var equipment = this.Argu as Sr.EquipmentInfoHolding.Model.WjParaBase;//Wlst.Cr.WjEquipmentBaseModels.Interface.IIEquipmentInfo;
            //if (equipment == null || equipment.RtuStateCode == 0) return false;
            //var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(equipment.RtuId);
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanX(areaId);

            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaX.Count > 0;
        }

        private bool CanEx()
        {
            return true;
        }

        protected void Ex()
        {
            ExNavWithArgs(
            
                Wj1090Module.Services.ViewIdAssign.PartolViewId,
                0);
        }
    }
}
