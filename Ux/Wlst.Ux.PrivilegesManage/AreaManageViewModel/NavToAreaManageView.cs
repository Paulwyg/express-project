using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Ux.PrivilegesManage.Services;

namespace Wlst.Ux.PrivilegesManage.AreaManageViewModel
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToAreaManageView : MenuItemBase
    {
        public NavToAreaManageView()
        {
            Id = PrivilegesManage.Services.MenuIdAssgin.NavToAreaManageViewId; 
            Text = "区域分组";
            Tag = "区域分组";
            Description = "区域分组，ID 为" + PrivilegesManage.Services.MenuIdAssgin.NavToAreaManageViewId;            
            Tooltips = "区域分组";
            Classic = "主菜单";
            IsEnabled = true;
            IsCheckable = false;
            base.Command = new RelayCommand(Ex, CanEx, true);
        }



        public override bool IsCanBeShowRwx()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.Cand();
        }

        private bool CanEx()
        {
            //return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D;
            return true;
        }

        protected void Ex()
        {
            this.ExNavWithArgs(ViewIdAssign.AreaManageViewId,
                               1);

        }
    }
}
