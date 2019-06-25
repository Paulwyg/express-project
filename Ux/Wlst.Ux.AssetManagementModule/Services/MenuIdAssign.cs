using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Ux.AssetManagementModule.Services
{
    /// <summary>
    /// 本模块的菜单起始Id，2100000 + 35*100, 每个模块均发放100个Id值。
    /// </summary>
    public class MenuIdAssign
    {
        /// <summary>
        /// 本模块的菜单起始Id，2100000 + 35*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int MenuIdAssignBaseId = 2100000 + 34*100;

        public const int NavToLampManageViewId = MenuIdAssignBaseId + 1;

        public const int NavToSimManageViewId = MenuIdAssignBaseId + 2;
    }
}
