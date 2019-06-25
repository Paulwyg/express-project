using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Ux.fuyang.Services
{
    public class MenuIdAssgin
    {
        /// <summary>
        /// 本模块的菜单起始Id，2100000 + 80*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int MenuIdAssignBaseId = 2100000 + 150 * 100;


        public const int NavToBroadcastStrategyViewId = MenuIdAssignBaseId + 1;

        public const int NavToOnlineStatusViewId = MenuIdAssignBaseId + 2;

    }
}
