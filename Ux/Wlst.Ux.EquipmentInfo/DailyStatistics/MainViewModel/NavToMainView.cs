using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Ux.EquipmentInfo.Services;

namespace Wlst.Ux.EquipmentInfo.DailyStatistics.MainViewModel
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToMainView: MenuItemBase
    {
        public NavToMainView()
        {
            Id = MenuIdAssign.NavToMainStatisticsViewId;
            Text = "终端日数据统计";
            Tag = "终端日数据统计";
            Description = "终端日数据统计，ID 为" + MenuIdAssign.NavToMainStatisticsViewId;
            Tooltips = "终端日数据统计";
            Classic = "主菜单";
            base.IsEnabled = true;
            base.IsCheckable = false;
            base.Command = new RelayCommand(Ex);
            //IsPrivilegLeave = false ;
        }

        public override bool IsCanBeShowRwx()
        {
            return true;

        }
        protected void Ex()
        {
            this.ExNavWithArgs(ViewIdAssign.MainStatisticsViewId, 1);
        }
    }
}
