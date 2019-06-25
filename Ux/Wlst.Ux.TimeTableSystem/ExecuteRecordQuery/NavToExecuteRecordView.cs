using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.TimeTableSystem.ExecuteRecordQuery
{

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToExecuteRecordView : MenuItemBase //TreeModuleGrpMulitManageView
    {
        public NavToExecuteRecordView()
        {
            Id = TimeTableSystem.Services.MenuIdAssgin.NavToExecuteRecordViewId;
            Text = "开关灯统计";
            Classic = "主菜单";
            Tag = "时间表开关灯统计";
            Description = "时间表开关灯统计，ID 为" +
                          TimeTableSystem.Services.MenuIdAssgin.NavToExecuteRecordViewId;
            Tooltips = "时间表开关灯统计";
            IsCheckable = false;
            IsEnabled = true;
            base.Command = new RelayCommand(Ex, CanEx, true);
            IsPrivilegLeave = false;
        }

        private bool CanEx()
        {
            return true;
        }

        protected void Ex()
        {
            ExNavWithArgs(
                TimeTableSystem.Services.ViewIdAssign.ExecuteRecordViewAttachRegion,
                TimeTableSystem.Services.ViewIdAssign.ExecuteRecordViewId,
                1);
        }

    }
}
