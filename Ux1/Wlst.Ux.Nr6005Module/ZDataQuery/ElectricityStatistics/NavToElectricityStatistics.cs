using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.Nr6005Module.ZDataQuery.ElectricityStatistics
{


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToElectricityStatistics : MenuItemBase
    {
        public NavToElectricityStatistics()
        {
            Id = Wlst.Ux.Nr6005Module.Services.MenuIdAssgin.NavToElectricityStatistics;
            Text = "电能统计";
            Tag = "电能统计";
            Classic = "主菜单";
            Description = "电能统计，ID 为" + Nr6005Module.Services.MenuIdAssgin.NavToElectricityStatistics;
            Tooltips = "电能统计";
            IsEnabled = true;
            IsCheckable = false;
            base.Command = new RelayCommand(Ex, CanEx, true);
            //   IsPrivilegLeave = true;
        }
        public override bool IsCanBeShowRwx()
        {
            return true;
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaR.Count > 0;
            return false;
        }
        bool CanEx()
        {
            return true;
        }


        protected void Ex()
        {
            ExNavWithArgs(Nr6005Module.Services.ViewIdAssign.NavToElectricityStatisticsViewId,
                               1);
        }
    }
}
