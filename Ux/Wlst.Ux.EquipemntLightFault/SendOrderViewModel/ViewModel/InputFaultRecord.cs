using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.CoreServices;

namespace Wlst.Ux.EquipemntLightFault.SendOrderViewModel.ViewModel
{
    public class InputFaultRecord
    {
        public int RtuId;

        public string RtuName;

        public int LoopID;

        public string FaultName;

        public int FaultID;

        public int PriorityLevel;

        public ulong OrderID;

        public long Time;

        //lvf 2018年9月19日14:02:23  添加备注
        public string Remarks;
    }
}
