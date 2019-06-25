using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.Models;
using System.ComponentModel.Composition;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Sr.EquipmentInfoHolding.Model;

namespace Wlst.Ux.Statistics.UxStatistics
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToUxStatistics : MenuItemBase
    {
        public NavToUxStatistics()
        {
            Id = Wlst.Ux.Statistics.Services.MenuIdAssgin.NavToUxStatisticsViewModelMainId;
            Text = "统计test";
            Tag = "统计";
            Classic = "主菜单";
            Description = "统计，ID 为" + Wlst.Ux.Statistics.Services.MenuIdAssgin.NavToUxStatisticsViewModelMainId;
            Tooltips = "统计";
            IsCheckable = false;
            IsEnabled = true;
            base.Command = new RelayCommand(Ex, CanEx, true);

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
            ExNavWithArgs(Statistics.Services.ViewIdAssign.UxStatisticsViewId, 0);
        }
    }
}
