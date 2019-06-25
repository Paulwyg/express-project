using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationParts;

namespace Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface
{
    public interface IISluRegulators
    {
        /// <summary>
        /// 单灯控制器
        /// </summary>
        SluRegulators SluRegulators { get; set; }
    }
}
