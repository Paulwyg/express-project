using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Ux.fuyang.Services
{
    public class ViewIdAssign
    {
        /// <summary>
        /// 本模块的视图起始Id，1100000 + 80*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int ViewIdAssignBaseId = 1100000 + 150 * 100;

        public const int BroadcastStrategyViewId = ViewIdAssignBaseId + 1;

        public const int OnlineStatusViewId = ViewIdAssignBaseId + 2;

    }
}
