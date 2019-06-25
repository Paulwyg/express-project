using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.Wj2096Module.ZcInfo.ZcConLocalArgs
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToTimeInfoQuery : MenuItemBase
    {
        public NavToTimeInfoQuery()
        {
            Id = Wj2096Module.Services.MenuIdAssign.ZcConnLocalArgsMenuId;
            Text = "召测时间方案";
            Tag = "召测时间方案";
            Classic = "单灯控制器-右键菜单-20960";
            Description = "单灯控制器右键菜单，ID 为"
                          + Wj2096Module.Services.MenuIdAssign.ZcConnLocalArgsMenuId;
            Tooltips = "召测时间方案";
            IsCheckable = false;
            IsEnabled = true;
            base.Command = new RelayCommand(Ex, CanEx, true);
            //IsPrivilegLeave = false;

        }

        private DateTime _dtEx = new DateTime(); 
        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;

            var equipment = this.Argu as Tuple<int, int>;
            if (equipment == null) return false;

            var areaId = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetCtrlAreaId(equipment.Item2);
            //var areaId = equipmentPara.AreaId;

            return Wlst.Cr.CoreMims.Services.UserInfo.CanX(areaId);
        }
        private bool CanEx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;

            var equipment = this.Argu as Tuple<int, int>;
            if (equipment == null) return false;

            return  DateTime.Now.Ticks - _dtEx.Ticks > 30000000;//true;
        }

        protected void Ex()
        {
            var terminalInfo = this.Argu as Tuple<int, int>;
            if (terminalInfo == null) return;
            var info = Wlst.Sr.ProtocolPhone.LxSluSgl.wst_slusgl_read_time_plan_in_ctrl;
            info.WstVsluTimeRead.SluId = 0;
            info.WstVsluTimeRead.CtrlId = terminalInfo.Item2;
            _dtEx = DateTime.Now;
            SndOrderServer.OrderSnd(info, 10, 3);
            ExNavWithArgs(
                Wj2096Module.Services.ViewIdAssign.ZcConnLocalArgsViewId,
                terminalInfo.Item1, terminalInfo.Item2);
        }
    }
}
