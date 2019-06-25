using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.Nr6005Module.ZDataQuery.RtuOpenCloseLightOneDayReportQuery
{

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToRtuOpenCloseLightOneDayReportQuery : MenuItemBase
    {
        public NavToRtuOpenCloseLightOneDayReportQuery()
        {
            Id = Nr6005Module.Services.MenuIdAssgin.NavToRtuOpenCloseLightOneDayReportQueryId;
            // Infrastructure.IdAssign.MenuIdAssign.NavToEquipmentDailyDataQueryViewId;
            Text = "开关灯结果查询";
            Tag = "开关灯结果查询";
            this.Classic = "主菜单";
            Description = "开关灯结果查询，ID 为" +
                          Nr6005Module.Services.MenuIdAssgin.NavToRtuOpenCloseLightOneDayReportQueryId;
            // Infrastructure.IdAssign.MenuIdAssign.NavToEquipmentDailyDataQueryViewId;
            Tooltips = "开关灯结果查询";
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
                               //ViewIdNameAssign.EquipmentDataQueryEquipmentDailyDataQueryViewAttachRegion,
                               Nr6005Module.Services.ViewIdAssign.RtuOpenCloseLightSuccQueryViewId,
                               //   ViewIdNameAssign.EquipmentDataQueryEquipmentDailyDataQueryViewId,
                               0);
            // MessageBoxResult result = MessageBox.Show("Sorry for have't finish this function ......", "Sorry", MessageBoxButton.OK);
        }
    }
}