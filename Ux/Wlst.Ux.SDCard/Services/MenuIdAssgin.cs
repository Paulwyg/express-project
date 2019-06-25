using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Ux.SDCard.Services
{
    public class MenuIdAssgin
    {
        /// <summary>
        /// 本模块的菜单起始Id，2100000 + 78*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int MenuIdAssignBaseId = 2100000 + 78 * 100;

        public const int NavToUxSDCardQueryViewModelMainId = MenuIdAssignBaseId + 1;
    }
}
