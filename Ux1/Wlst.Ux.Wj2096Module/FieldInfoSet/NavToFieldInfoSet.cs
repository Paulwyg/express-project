using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.Wj2096Module.FieldInfoSet
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToFieldInfoSet : MenuItemBase
    {
        public NavToFieldInfoSet()
        {
            Id = Ux.Wj2096Module.Services.MenuIdAssign.NavToWj2096SluInfoSetMenuId;
            Text = "参数设置";
            Tag = "WJ2096单灯参数设置";
            this.Classic = "右键菜单-WJ2096-专有";
            Description = "WJ2096单灯参数设置,ID为" + Ux.Wj2096Module.Services.MenuIdAssign.NavToWj2096SluInfoSetMenuId;
            Tooltips = "WJ2096单灯参数设置";
            base.IsCheckable = false;
            base.IsEnabled = true;
            base.Command = new RelayCommand(Ex);
        }
        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;

            var equipment = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField((int)this.Argu); ;
            if (equipment == null) return false;

            var areaId = equipment.AreaId;
            return Wlst.Cr.CoreMims.Services.UserInfo.CanW(areaId);


        }

        protected void Ex()
        {
            var equipment = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField((int)this.Argu); ;
            if (equipment == null) return;


            int rtuId = equipment.FieldId;
            if (rtuId < 1) return;
            this.ExNavWithArgs(
                Ux.Wj2096Module.Services.ViewIdAssign.Wj2096SluInfoSetViewId,
                rtuId);
        }
    }
}
