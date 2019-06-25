

using Wlst.client;

namespace Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface
{
    /// <summary>
    /// 节电器设备起始地址 1 200 000
    /// </summary>
    public interface IIJd601
    {
        /// <summary>
        /// 附属设备逻辑ID
        /// </summary>

        int RtuId { get; set; }

        /// <summary>
        /// 附属设备逻辑地址
        /// </summary>

        int PhyId { get; set; }

        /// <summary>
        /// 附属设备名称
        /// </summary>

        string RtuName { get; set; }

        /// <summary>
        /// 有效标示  指示是否使用
        /// </summary>

        bool EsyValidIdentify { get; set; }

        /// <summary>
        /// 预热时间 默认2分钟
        /// </summary>

        int PreheatingTime { get; set; }

        /// <summary>
        /// 开机时间 不设置
        /// </summary>

        int EsyOpentTime { get; set; }

        /// <summary>
        /// 关机时间 不设置
        /// </summary>

        int CloseTime { get; set; }

        /// <summary>
        /// A相接触器变比 50~500 默认150
        /// </summary>

        int CtRadioA { get; set; }

        /// <summary>
        /// B相接触器变比 50~500 默认150
        /// </summary>

        int CtRadioB { get; set; }

        /// <summary>
        /// C相接触器变比 50~500 默认150
        /// </summary>

        int CtRadioC { get; set; }

        /// <summary>
        /// 时间模式：0 为定时模式 ；1为延时模式；默认1
        /// </summary>

        int TimeMode { get; set; }

        /// <summary>
        /// 运行模式：0 自动，1 手动； 默认 0
        /// </summary>

        int RunMode { get; set; }

        /// <summary>
        /// 风机启动温度 默认45
        /// </summary>

        int FanSatrtTemp { get; set; }

        /// <summary>
        /// 风机关闭温度 默认35
        /// </summary>

        int FanClosedTemp { get; set; }

        /// <summary>
        /// 退出节能温度 默认 70 界面设置
        /// </summary>

        int EnerySaveTemp { get; set; }

        /// <summary>
        /// 强制保护温度 默认85
        /// </summary>

        int MandatoryProtectTemp { get; set; }

        /// <summary>
        /// 恢复节能温度 默认50
        /// </summary>

        int RecoverEnergySaveTemp { get; set; }

        /// <summary>
        /// 输入过压门限值 默认270
        /// </summary>

        int InputOvervoltageLimit { get; set; }

        /// <summary>
        /// 输入欠压门限值 默认170
        /// </summary>

        int InputUndervoltageLimit { get; set; }

        /// <summary>
        /// 输出欠压门限值 默认160
        /// </summary>

        int OutputUndervoltageLimit { get; set; }

        /// <summary>
        /// 输入过载门限值 默认 144 电流
        /// </summary>

        int OutputOverloadLimit { get; set; }

        /// <summary>
        /// 调压速度 仅模式为延时模式时有效 默认10 6~60
        /// </summary>

        int RegulatingSpeed { get; set; }

        /// <summary>
        /// 供电相数  默认3 不提供界面设置
        /// </summary>

        int PowerSupplyPhases { get; set; }

        /// <summary>
        /// 通信模式 ：0 通过终端；1 通过通信模块 默认0
        /// </summary>

        int CommTypeCode { get; set; }

        /// <summary>
        /// 工作模式：0 通用模式；1 特殊模式；不提供界面设置 默认0
        /// </summary>

        int WorkMode { get; set; }

        /// <summary>
        /// 是否主动报警 默认false
        /// </summary>

        bool IsActiveAlarm { get; set; }

        /// <summary>
        /// 报警延时时间  默认10秒钟
        /// </summary>

        int AlarmDelay { get; set; }

        /// <summary>
        /// 节能模式：0 接触器模式；1 IGBT模式 默认1
        /// </summary>

        int Mode { get; set; }

    };

    //public enum EsyMode
    //{
    //    /// <summary>
    //    /// 0 接触器模式
    //    /// </summary>
    //    SwitchMode = 0,

    //    /// <summary>
    //    /// 1 IGBT模式 默认1
    //    /// </summary>
    //    IGBT,
    //};

    //public enum EsyWorkMode
    //{
    //    /// <summary>
    //    /// 通用模式 0
    //    /// </summary>
    //    CommonMode = 0,

    //    /// <summary>
    //    /// 特殊模式 1
    //    /// </summary>
    //    SpecialMode,
    //}

    //public enum EsyCommTypeCode
    //{
    //    /// <summary>
    //    /// 0 通过终端
    //    /// </summary>
    //    ThrouthRtu = 0,

    //    /// <summary>
    //    /// 1 通过通信模块
    //    /// </summary>
    //    ThrouthCommModel,
    //}

    //public enum EsyRunMode
    //{
    //    /// <summary>
    //    ///  0 自动，1 手动； 默认 0
    //    /// </summary>
    //    Auto = 0,

    //    /// <summary>
    //    ///  0 自动，1 手动； 默认 0
    //    /// </summary>
    //    Manuale,
    //};


    //public enum EsyTimeMode
    //{
    //    /// <summary>
    //    ///  0 为定时模式 ；1为延时模式；默认1
    //    /// </summary>
    //    TimeSetting = 0,

    //    /// <summary>
    //    /// 0 为定时模式 ；1为延时模式；默认1
    //    /// </summary>
    //    Delaying,
    //};
}
