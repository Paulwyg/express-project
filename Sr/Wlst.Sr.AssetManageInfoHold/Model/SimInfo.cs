using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.client;

namespace Wlst.Sr.AssetManageInfoHold.Model
{
    public class SimInfo:ZcSim.ZcSimItem
    {
        public SimInfo(ZcSim.ZcSimItem item)
        {
            this.Id = item.Id ;
            this.RtuId = item.RtuId;
            this.SimNum = item.SimNum;
            this.DtKt = item.DtKt;
            this.DtXf = item.DtXf;
            this.State = item.State;
            this.IpAddr = item.IpAddr;
        }
    }
}
