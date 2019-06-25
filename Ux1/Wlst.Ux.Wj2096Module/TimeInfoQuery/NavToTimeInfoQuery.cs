using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
 

namespace Wlst.Ux.Wj2096Module.TimeInfoQuery
{


    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToTimeInfoQuery : MenuItemBase
    {
        public NavToTimeInfoQuery()
        {
            Id = Wj2096Module.Services.MenuIdAssign.TimeInfoQueryMenuId;
            Text = "查询时间方案";
            Tag = "查询时间方案";
            Classic = "右键菜单-WJ2096-专有";
            Description = "单灯集中器右键菜单，ID 为"
                          + Wj2096Module.Services.MenuIdAssign.TimeInfoQueryMenuId;
            Tooltips = "查询时间方案";
            IsCheckable = false;
            IsEnabled = true;
            base.Command = new RelayCommand(Ex, CanEx, true);
            //IsPrivilegLeave = false;

        }
        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            var equipment = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField((int)this.Argu); ;
            if (equipment == null) return false;
            var areaId = equipment.AreaId;
            return Wlst.Cr.CoreMims.Services.UserInfo.CanW(areaId);
        }
        private bool CanEx()
        {
            return true;
        }

        protected void Ex()
        {
            var equipment = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField((int)this.Argu);
            if (equipment == null) return;
            int rtuId = equipment.FieldId;
            if (rtuId < 1) return;

            ExNavWithArgs(
                Wj2096Module.Services.ViewIdAssign.TimeInfoQueryViewId,
                rtuId);
        }
    }
}
