namespace Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface
{
    /// <summary>
    /// 终端通讯参数
    /// </summary>
    public interface IIRtuParaGprs
    {
        //public RtuParaGprs()
        //{
        //    RtuId = 0;
        //}

        //public RtuParaGprs(int rtuId)
        //{
        //    this.RtuId = rtuId;
        //}

        //public RtuParaGprs(RtuParaGprs rtuParaGprs)
        //{
        //    this.HeartBeatPeriod = rtuParaGprs.HeartBeatPeriod;
        //    this.Ip = rtuParaGprs.Ip;
        //    this.Port = rtuParaGprs.Port;
        //    this.Priority = rtuParaGprs.Priority;
        //    this.HeartBeatPeriod = rtuParaGprs.HeartBeatPeriod;
        //    this.ReportDataPeriod = rtuParaGprs.ReportDataPeriod;
        //    this.RtuId = rtuParaGprs.RtuId;
        //    this.ServiceProvider = rtuParaGprs.ServiceProvider;
        //    this.SimNumber = rtuParaGprs.SimNumber;
        //}

        int RtuId { get; set; }

        /// <summary>
        /// 通讯参数 通讯服务商
        /// </summary>
        string ServiceProvider { get; set; }

        /// <summary>
        /// 通讯参数 静态IP地址
        /// </summary>
        string Ip { get; set; }

        /// <summary>
        /// 通讯参数 端口号
        /// </summary>
        int Port { get; set; }

        /// <summary>
        /// 通讯参数 手机号
        /// </summary>
        string SimNumber { get; set; }

        /// <summary>
        /// 通讯参数 心跳周期
        /// </summary>
        int HeartBeatPeriod { get; set; }

        /// <summary>
        /// 通讯参数 主报周期
        /// </summary>
        int ReportDataPeriod { get; set; }

        /// <summary>
        /// 通讯参数 优先次序
        /// </summary>
        int Priority { get; set; }

        /// <summary>
        /// 工作参数 报警延时（秒）
        /// </summary>
        byte ErrorDelays { get; set; }
    }
}
