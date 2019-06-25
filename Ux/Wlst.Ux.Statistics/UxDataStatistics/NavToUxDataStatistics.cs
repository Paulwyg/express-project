using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.Models;
using System.ComponentModel.Composition;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Sr.EquipmentInfoHolding.Model;


namespace Wlst.Ux.Statistics.UxDataStatistics
{
    //[Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToUxDataStatistics : MenuItemBase
    {

        public NavToUxDataStatistics()
        {
            Id = Wlst.Ux.Statistics.Services.MenuIdAssgin.NavToUxDataStatisticsViewModelMainId;
            Text = "数据统计";
            Tag = "数据统计";
            Classic = "主菜单";
            Description = "数据统计，ID 为" + Wlst.Ux.Statistics.Services.MenuIdAssgin.NavToUxDataStatisticsViewModelMainId;
            Tooltips = "数据统计";
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
            ExNavWithArgs(Statistics.Services.ViewIdAssign.UxDataStatisticsViewId, 0);
        }
    }
}
