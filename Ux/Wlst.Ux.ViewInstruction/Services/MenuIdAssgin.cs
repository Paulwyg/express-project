using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Ux.ViewInstruction.Services
{
    public class MenuIdAssgin
    {
        /// <summary>
        /// 本模块的全局事件发布起始Id，3100000 + 70*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int MenuIdAssignBaseId = 2100000 + 70 * 100;

        public const int ShowViewInstructionMenuId = MenuIdAssignBaseId + 1;
    }
}
