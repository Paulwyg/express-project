using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Ux.EquipmentInfo.Services
{
    public class MenuIdAssign
    {
        /// <summary>
        /// 本模块的菜单起始Id，2100000 + 76*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int MenuIdAssignBaseId = 2100000 + 76 * 100;

        public const int NavToMainViewId = MenuIdAssignBaseId + 1;

        public const int NavToMainStatisticsViewId = MenuIdAssignBaseId + 2;

        public const int NavToSystemDataViewId = MenuIdAssignBaseId + 3;
    }
}
