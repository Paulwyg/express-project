using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Ux.AssetManagementModule.Services
{
    /// <summary>
    /// 本模块的视图起始Id，1100000 + 35*100, 每个模块均发放100个Id值。
    /// </summary>
    public class ViewIdAssign
    {
        /// <summary>
        /// 本模块的视图起始Id，1100000 + 35*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int ViewIdAssignBaseId = 1100000 + 34 * 100;

        public const int LampManageViewId = ViewIdAssignBaseId + 1;

        public const int SimManageViewId = ViewIdAssignBaseId + 2;
    }
}
