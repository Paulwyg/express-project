using System;
using System.ComponentModel.Composition;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.client;

namespace Wlst.Ux.TimeTableSystem.OpenCloseReportQuery
{

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToOpenCloseReportQueryView : MenuItemBase //TreeModuleGrpMulitManageView
    {
        public NavToOpenCloseReportQueryView()
        {
            Id = TimeTableSystem.Services.MenuIdAssgin.NavToOpenCloseReportQueryViewId;
            Text = "报表查询";
            Classic = "主菜单";
            Tag = "系统时间表自动开关灯历史报表查询";
            Description = "时间表报表查询，ID 为" +
                          TimeTableSystem.Services.MenuIdAssgin.NavToOpenCloseReportQueryViewId;
            Tooltips = "时间表报表查询";
            IsCheckable = false;
            IsEnabled = true;
            base.Command = new RelayCommand(Ex,CanEx ,true );
            //IsPrivilegLeave =false ;
        }
        public override bool IsCanBeShowRwx()
        {
            return true;
        }

        bool CanEx()
        {
            return true;
        }

        protected void Ex()
        {
            ExNavWithArgs(
              
                TimeTableSystem.Services.ViewIdAssign.OpenCloseReportQueryViewId,
                1);
        }

    }
}
