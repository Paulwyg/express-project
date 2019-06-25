using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.Nr6005Module.ZDataQuery.ElectricityQuery
{


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToElectricityQuery : MenuItemBase
    {
        public NavToElectricityQuery()
        {
            Id = Wlst.Ux.Nr6005Module.Services.MenuIdAssgin.NavToElectricityQuery;
            Text = "电能查询";
            Tag = "电能查询";
            Classic = "主菜单";
            Description = "电能查询，ID 为" + Nr6005Module.Services.MenuIdAssgin.NavToElectricityQuery;
            Tooltips = "电能查询";
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
            ExNavWithArgs(Nr6005Module.Services.ViewIdAssign.NavToElectricityQueryViewId,
                               1);
        }
    }
}
