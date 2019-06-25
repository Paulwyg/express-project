using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Ux.MapGis.Services
{
   public class MenuIdAssign
    { /// <summary>
        /// 本模块的视图起始Id，2100000 + 71*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int ViewIdAssignBaseId = 2100000 + 171 * 100;

        //1107105 电子地图编辑角色权限
        public const int MapEditId = ViewIdAssignBaseId + 1;

     

    }
}
