using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.ExtendYixinEsu.JnDataQuery
{

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToJnQueryView : MenuItemBase
    {
        public NavToJnQueryView()
        {
            Id = ExtendYixinEsu.Services.MenuIdAssgin.NavToJnQueryViewId;
                // Infrastructure.IdAssign.MenuIdAssign.NavToWj1050InfoSetViewId;
            Text = "节能数据";
            Tag = "节能数据查询";
            Classic = "主菜单";
            Description = "节能数据查询，ID 为" + ExtendYixinEsu.Services.MenuIdAssgin.NavToJnQueryViewId;
            Tooltips = "节能数据";
            IsEnabled = true;
            IsCheckable = false;
            base.Command = new RelayCommand(Ex);
        }
        public override bool IsCanBeShowRwx()
        {
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanR();
            return true;
        }
        protected void Ex()
        {
           // JnRtuSet.NavTo.NavToLdl();
            ExNavWithArgs(ExtendYixinEsu.Services.ViewIdAssign.JnQueryViewId,
                          0);
        }
    }
}
