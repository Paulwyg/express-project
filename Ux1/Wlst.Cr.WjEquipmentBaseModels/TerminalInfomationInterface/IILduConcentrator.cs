using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.client;


namespace Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface
{
    /// <summary>
    /// 线路检测集中控制器参数
    /// </summary>
    public interface IILduConcentrator
    {
        /// <summary>
        /// 线路检测回路信息
        /// </summary>
        List<LduLineParameter> LduLines { get; set; }
    }



}
