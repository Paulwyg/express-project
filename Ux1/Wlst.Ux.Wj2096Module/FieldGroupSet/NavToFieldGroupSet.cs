using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.Wj2096Module.FieldGroupSet
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToFieldGroupSet : MenuItemBase
    {
        public NavToFieldGroupSet()
        {
            Id = Ux.Wj2096Module.Services.MenuIdAssign.NavToFieldGroupSetMenuId;
            Text = "域分组设置";
            Tag = "WJ2096单灯域分组设置";
            this.Classic = "主菜单";
            Description = "WJ2096单灯域分组设置,ID为" + Ux.Wj2096Module.Services.MenuIdAssign.NavToFieldGroupSetMenuId;
            Tooltips = "WJ2096单灯域分组设置";
            base.IsCheckable = false;
            base.IsEnabled = true;
            base.Command = new RelayCommand(Ex, CanEx, true);
        }
        public override bool IsCanBeShowRwx()
        {
            return (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaR.Count > 0 || Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D);
        }

        bool CanEx()
        {
            return (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaR.Count > 0 || Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D);
        }

        protected void Ex()
        {
            this.ExNavWithArgs(
                Ux.Wj2096Module.Services.ViewIdAssign.FieldGroupSetViewId,
                -1);
        }
    }
}
