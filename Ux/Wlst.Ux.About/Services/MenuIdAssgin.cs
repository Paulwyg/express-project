using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Ux.About.Services
{
    public class MenuIdAssgin
    {
        /// <summary>
        /// 本模块的菜单起始Id，2100000 + 67*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int MenuIdAssignBaseId = 2100000 + 67 * 100;

        public const int NavToUxAboutViewModelMainId = MenuIdAssignBaseId + 1;
        public const int NavToHelpViewModelMainId = MenuIdAssignBaseId + 2;

        public const int NavToShowErrViewModelMainId = MenuIdAssignBaseId + 3;

        public const int NavToThreeLvViewModelMainId = MenuIdAssignBaseId + 4;


        public const int NavToWebPageId = MenuIdAssignBaseId + 5;
    }
}
