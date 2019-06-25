using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.WJ3005Module.TmlInfoValidQuery
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToTmlInfoValidQuery : MenuItemBase
    {
        public NavToTmlInfoValidQuery()
        {
            Id = WJ3005Module.Services.MenuIdAssgin.NavToTmlInfoValidQuery;
            Text = "终端参数合法性查询";
            Tag = "终端参数合法性查询";
            Classic = "主菜单";
            Description = "终端参数合法性查询，ID 为" + WJ3005Module.Services.MenuIdAssgin.NavToTmlInfoValidQuery;
            Tooltips = "终端参数合法性查询";
            IsEnabled = true;
            IsCheckable = false;
            base.Command = new RelayCommand(Ex, CanEx, true);
            
        }
        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaX.Count > 0;
        }
        bool CanEx()
        {
            return true;
        }


        protected void Ex()
        {
            ExNavWithArgs(WJ3005Module.Services.ViewIdAssign.TmlInfoValidQueryView,
                               1);
        }
    }
}