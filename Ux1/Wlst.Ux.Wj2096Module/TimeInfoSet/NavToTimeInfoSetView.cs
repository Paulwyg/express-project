using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
 

namespace Wlst.Ux.Wj2096Module.TimeInfoSet
{

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToTimeInfoSetView : MenuItemBase
    {
        public NavToTimeInfoSetView()
        {
            Id = Wj2096Module.Services.MenuIdAssign.NavToTimeInfoSetMenuId;
            Text = "物联网单灯方案设置";
            Tag = "物联网单灯方案设置";
            Classic = "主菜单";
            Description = "物联网单灯方案设置，线路检测通用，ID 为"
                          + Wj2096Module.Services.MenuIdAssign.NavToTimeInfoSetMenuId;
            Tooltips = "物联网单灯方案设置";
            IsCheckable = false;
            IsEnabled = true;
            base.Command = new RelayCommand(Ex, CanEx, true);
           // IsPrivilegLeave = true ;

        }
        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanW();
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW .Count >0 ;;
        }
        private bool CanEx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW .Count >0 ;
        }

        protected void Ex()
        {

            ExNavWithArgs(
                Wj2096Module.Services.ViewIdAssign.TimeInfoSetViewId,
                0);
        }
    }
}
