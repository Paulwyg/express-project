using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.client;

namespace Wlst.Sr.AssetManageInfoHold.Model
{
    public class LampInfo : ZcDyxx.ZcDyxxItem
    {
        public LampInfo(ZcDyxx.ZcDyxxItem item)
        {
            this.Id = item.Id ;
            this.RtuId = item.RtuId;
            this.Cqj = item.Cqj;
            this.Dygh = item.Dygh;
            this.Dbbh = item.Dbbh;
            this.IsYj = item.IsYj;
        }
    }
}
