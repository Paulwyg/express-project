using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Ux.Wj2090Module.Services;

namespace Wlst.Ux.Wj2090Module.ZOperatorLarge.Partols
{

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToPartolsView : MenuItemBase
    {
        public NavToPartolsView()
        {
            Id = Wj2090Module.Services.MenuIdAssign.NavToPartolsViewId;
            Text = "单灯巡测";
            Tag = "单灯巡测";
            this.Classic = "主菜单";
            Description = "单灯巡测，ID 为" + Id;
            Tooltips = "单灯巡测";
            base.IsEnabled = true;
            base.IsCheckable = false;
            base.Command = new RelayCommand(Ex);
        }
        public override bool IsCanBeShowRwx()
        {
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanR();
           return true;
        }

        protected void Ex()
        {

            ExNavWithArgs(
                          ViewIdAssign.PartolsViewId,
                          0);
        }

    }
}
