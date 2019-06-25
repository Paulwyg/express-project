using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Ux.Statistics.Services
{
    public class MenuIdAssgin
    {
        /// <summary>
        /// 本模块的菜单起始Id，2100000 + 68*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int MenuIdAssignBaseId = 2100000 + 68 * 100;

        public const int NavToUxStatisticsViewModelMainId = MenuIdAssignBaseId + 1;

        public const int NavToRtuElectricityStatisticsViewModelMainId = MenuIdAssignBaseId + 2;

        //数据统计
        public const int NavToUxDataStatisticsViewModelMainId = MenuIdAssignBaseId + 3;
    }
}
