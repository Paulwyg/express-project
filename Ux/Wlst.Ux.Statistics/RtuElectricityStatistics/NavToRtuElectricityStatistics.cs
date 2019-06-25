using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.Statistics.RtuElectricityStatistics
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToRtuElectricityStatistics : MenuItemBase
    {
        public NavToRtuElectricityStatistics()
        {
            Id = Wlst.Ux.Statistics.Services.MenuIdAssgin.NavToRtuElectricityStatisticsViewModelMainId;
            Text = "统计123";
            Tag = "统计123";
            Classic = "主菜单";
            Description = "统计，ID 为" + Wlst.Ux.Statistics.Services.MenuIdAssgin.NavToRtuElectricityStatisticsViewModelMainId;
            Tooltips = "统计";
            IsCheckable = false;
            IsEnabled = true;
            base.Command = new RelayCommand(Ex,CanEx,true);

        }
        public override bool IsCanBeShowRwx()
        {
            return true;
        }

        private static bool CanEx()
        {
            return true;
        }

        protected void Ex()
        {
            ExNavWithArgs(Statistics.Services.ViewIdAssign.UxRtuElectricityStatisticsViewId, 0);
        }
    }
}
