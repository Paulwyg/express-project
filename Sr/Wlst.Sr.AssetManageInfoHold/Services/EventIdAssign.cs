using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Sr.AssetManageInfoHold.Services
{
    /// <summary>
    /// 本模块的全局事件发布起始Id，3100000 + 71*100, 每个模块均发放100个Id值。
    /// </summary>
    public class EventIdAssign
    {
        /// <summary>
        /// 本模块的全局事件发布起始Id，3100000 + 71*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int EventIdAssignBaseId = 3100000 + 71*100;

        public const int LampNeedUpdate = EventIdAssignBaseId + 1;

        public const int SimNeedUpdate = EventIdAssignBaseId + 1;
    }
}
