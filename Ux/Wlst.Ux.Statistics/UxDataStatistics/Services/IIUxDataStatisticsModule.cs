using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Ux.Statistics.UxDataStatistics.ViewModel;

namespace Wlst.Ux.Statistics.UxDataStatistics.Services
{
    public interface IIUxDataStatisticsModule : IINavOnLoad, IITab, IIOnHideOrClose 

    {

        /// <summary>
        /// 亮灯率统计
        /// </summary>
        ObservableCollection<LightRateStatisticsViewModel> LightRateStatisticsData { get; }
    
    }
}
