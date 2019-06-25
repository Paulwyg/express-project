using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.client;

namespace Wlst.Ux.PrivilegesManage.AreaManageViewModel.ViewModels
{
    public class AreaInformation : AreaInfo.AreaItem
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Index;

        public AreaInformation(AreaInfo.AreaItem item)
        {

            this.AreaId = item.AreaId;
            this.AreaName = item.AreaName;
            this.LstTml = item.LstTml;


        }
    }


}
