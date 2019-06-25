//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationParts
//{
//    /// <summary>
//    /// 线路检测回路参数
//    /// </summary>
//    public class  LduLine
//    {
//        /// <summary>
//        /// 集中器地址
//        /// </summary>
//        public int LduConcentratorId { get; set; }

//        /// <summary>
//        /// 线路序号
//        /// </summary>
//        public int LduLineID { get; set; }


//        /// <summary>
//        /// 是否使用
//        /// </summary>
//        public bool IsUsed { get; set; }
//        /// <summary>
//        /// 控制类型 0保留，1 1控1,2 1控2，3 1控3 。。。。。。
//        /// </summary>
//        public int LduControlTypeCode { get; set; }

//        /// <summary>
//        /// 线路名称
//        /// </summary>
//        public string LduLineName { get; set; }

//        /// <summary>
//        /// 通信方式 0 保留，1 电台，2 串口232，3 串口485，4 Zigbee，5 电力载波，6 Socket  一般为3或6
//        /// </summary>
//        public Sr.ProtocolCnt.AexchangeModels.ModelEnum.EnumCommunicationMode LduCommTypeCode { get; set; }

//        /// <summary>
//        /// 互感器比值
//        /// </summary>
//        public int MutualInductorRadio { get; set; }

//        /// <summary>
//        /// 相位
//        /// </summary>
//        public int LduPhase { get; set; }

//        /// <summary>
//        /// 开灯信号强度门限
//        /// </summary>
//        public int LduLightonSingleLimit { get; set; }

//        /// <summary>
//        /// 开灯阻抗报警门限
//        /// </summary>
//        public int LduLightonImpedanceLimit { get; set; }

//        /// <summary>
//        /// 亮灯率报警门限
//        /// </summary>
//        public int LduBrightRateAlarmLimit { get; set; }

//        /// <summary>
//        /// 关灯信号强度门限
//        /// </summary>
//        public int LduLightoffSingleLimit { get; set; }

//        /// <summary>
//        /// 关灯阻抗报警门限
//        /// </summary>
//        public int LduLightoffImpedanceLimit { get; set; }

//        ///// <summary>
//        ///// 故障参数
//        ///// </summary>
//        //public int LduFaultParam { get; set; }

//        /// <summary>
//        /// 线路短路主动告警
//        /// </summary>
//        public bool AlarmLineShortCircuit { get; set; }
//        /// <summary>
//        /// 关灯阻抗主动报警
//        /// </summary>
//        public bool AlarmLineLightOffImpedance { get; set; }
//        /// <summary>
//        /// 关灯信号强度主动告警
//        /// </summary>
//        public bool AlarmLineLightOffSingle { get; set; }
//        /// <summary>
//        /// 线路失电主动告警
//        /// </summary>
//        public bool AlarmLineLosePower { get; set; }
//        /// <summary>
//        /// 亮灯率变化主动告警
//        /// </summary>
//        public bool AlarmLineBrightRate { get; set; }
//        /// <summary>
//        /// 开灯阻抗主动报警
//        /// </summary>
//        public bool AlarmLineLightOpenImpedance { get; set; }
//        /// <summary>
//        /// 开灯信号强度主动告警
//        /// </summary>
//        public bool AlarmLineLightOpenSingel { get; set; }


//        /// <summary>
//        /// 回路序号  本防盗检测设备检测的终端回路的回路序号
//        /// </summary>
//        public int LduLoopID { get; set; }

//        /// <summary>
//        /// 本防盗设备的末端 安装的灯杆序号
//        /// </summary>
//        public string LduEndLampportSn { get; set; }


//        /// <summary>
//        /// 备注
//        /// </summary>
//        public string Remark { get; set; }
//    }
//}
