using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Ux.EquipmentInfo.Services
{
    public class ViewIdAssign
    {
        /// <summary>
        /// 本模块的视图起始Id，1100000 + 76*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int ViewIdAssignBaseId = 1100000 + 76 * 100;

        public const int MainViewId = ViewIdAssignBaseId + 1;

        public const int MainStatisticsViewId = ViewIdAssignBaseId + 2;

        public const int SystemDataViewId = ViewIdAssignBaseId + 3;
    }
}
