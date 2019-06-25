using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.Nr6005Module.ZDataQuery.RtuOpenCloseLightQuery
{

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToRtuOpenCloseLightQueryView : MenuItemBase
    {
        public NavToRtuOpenCloseLightQueryView()
        {
            Id = Nr6005Module.Services.MenuIdAssgin.NavToRtuOpenCloseLightQueryViewId ;
                // Infrastructure.IdAssign.MenuIdAssign.NavToSndWeekTimeQueryViewId;
            Text = "时间统计";
            Tag = "开关灯时间统计";
            this.Classic = "主菜单";
            Description = "开关灯时间统计，ID 为" + Nr6005Module.Services.MenuIdAssgin.NavToRtuOpenCloseLightQueryViewId;
                // Infrastructure.IdAssign.MenuIdAssign.NavToSndWeekTimeQueryViewId;
            Tooltips = "开关灯时间统计";
            base.IsEnabled = true;
            base.IsCheckable = false;
            base.Command = new RelayCommand(Ex, CanEx, true);
        }
        public override bool IsCanBeShowRwx()
        {
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanR();
            return true;
        }
        private bool CanEx()
        {
            return true;
        }


        protected void Ex()
        {

            this.ExNavWithArgs(
                               Nr6005Module.Services.ViewIdAssign.RtuOpenCloseLightQueryViewId,
                               0);
            // MessageBoxResult result = MessageBox.Show("Sorry for have't finish this function ......", "Sorry", MessageBoxButton.OK);
        }


    }
}
