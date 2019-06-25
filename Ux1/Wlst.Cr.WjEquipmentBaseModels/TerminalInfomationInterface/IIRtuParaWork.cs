using System;

namespace Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface
{
    /// <summary>
    /// 终端工作参数
    /// </summary>
    public interface IIRtuParaWork
    {
        //public RtuParaWork()
        //{
        //    RtuId = 0;
        //}

        //public RtuParaWork(int rtuId)
        //{
        //    this.RtuId = rtuId;
        //}

        //public RtuParaWork(RtuParaWork rtuParaWork)
        //{
        //    this.Alarm = rtuParaWork.Alarm;
        //    this.Boot = rtuParaWork.Boot;
        //    this.Call = rtuParaWork.Call;
        //    this.Display = rtuParaWork.Display;
        //    this.ErrorDelays = rtuParaWork.ErrorDelays;
        //    this.Report = rtuParaWork.Report;
        //    this.Route = rtuParaWork.Route;
        //    this.RtuId = rtuParaWork.RtuId;
        //    this.Selfcheck = rtuParaWork.Selfcheck;
        //    this.Sound = rtuParaWork.Sound;
        //}

        int RtuId { get; set; }

        /// <summary>
        /// 工作参数 滚动显示
        /// </summary>
        Boolean Display { get; set; }

        /// <summary>
        /// 工作参数 开机申请
        /// </summary>
        Boolean Boot { get; set; }

        /// <summary>
        /// 工作参数 声响报警
        /// </summary>
        Boolean Sound { get; set; }

        /// <summary>
        /// 工作参数 进入自检
        /// </summary>
        Boolean Selfcheck { get; set; }

        /// <summary>
        /// 工作参数 允许报警
        /// </summary>
        Boolean Alarm { get; set; }

        /// <summary>
        /// 工作参数 允许主报
        /// </summary>
        Boolean Report { get; set; }

        /// <summary>
        /// 工作参数 允许呼叫
        /// </summary>
        Boolean Call { get; set; }

        /// <summary>
        /// 工作参数 禁止路由
        /// </summary>
        Boolean Route { get; set; }

    }
}
