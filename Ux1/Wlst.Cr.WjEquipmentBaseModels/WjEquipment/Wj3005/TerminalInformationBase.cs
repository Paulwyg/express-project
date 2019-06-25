using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Wlst.Cr.WjEquipmentBaseModels.Interface;
using Wlst.Cr.WjEquipmentBaseModels.Models;
using Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface;
using Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationParts;
using Wlst.client;


namespace Wlst.Cr.WjEquipmentBaseModels.WjEquipment.Wj3005
{

    /// <summary>
    /// 本设备为终端设备，支持主设备以及附属设备
    /// <para> 可扩展附属设备</para>
    /// </summary>
    [Serializable]
    public partial class TerminalInformationBase : EquipmentInfomation, IIRtuParaAnalogueAmps, IISwitchOut
    {

       


        public TerminalInformationBase()
            : base()
        {
            //SwitchIn = new SwitchIn(0);
            SwitchOut = new SwitchOut(0);
            RtuParaAnalogueAmps = new RtuParaAnalogueAmps(0);
        }

        /// <summary>
        /// 终端参数构造函数
        /// </summary>
        /// <param name="rtuId">终端地址</param>
        /// <param name="rtuName">终端名称</param>
        /// <param name="state">终端工作状态</param>
        public TerminalInformationBase(int rtuId, string rtuName, int state)
            : base(rtuId, rtuName, state)
        {
            //SwitchIn = new SwitchIn(rtuId);
            SwitchOut = new SwitchOut(rtuId);
            RtuParaAnalogueAmps = new RtuParaAnalogueAmps(rtuId);
        }

        public TerminalInformationBase(IIEquipmentInfo baseTerminalInfomation)
            : base(baseTerminalInfomation)
        {
            //SwitchIn = new SwitchIn(baseTerminalInfomation.RtuId);
            SwitchOut = new SwitchOut(baseTerminalInfomation.RtuId);
            RtuParaAnalogueAmps = new RtuParaAnalogueAmps(baseTerminalInfomation.RtuId);
        }


        /// <summary>
        /// 开关量输入
        /// </summary>
        //public SwitchIn SwitchIn { get;  set; }

        /// <summary>
        /// 开关量输出
        /// </summary>

        public SwitchOut SwitchOut { get;  set; }

        /// <summary>
        /// 回路数据
        /// </summary>

        public RtuParaAnalogueAmps RtuParaAnalogueAmps { get;  set; }

        /// <summary>
        /// 克隆本类的实例 即创建了一个原对象的深表副本
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                object CloneObject;
                BinaryFormatter bf = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
                bf.Serialize(ms, this);
                ms.Seek(0, SeekOrigin.Begin);
                // 反序列化至另一个对象(即创建了一个原对象的深表副本) 
                CloneObject = bf.Deserialize(ms);
                // 关闭流 
                ms.Close();
                return CloneObject;
            }
        }


        public override string GetRtuLoopName(int loopId)
        {
            if (RtuParaAnalogueAmps != null && RtuParaAnalogueAmps.DicRtuParaAnalogueAmp.ContainsKey(loopId))
            {
                return RtuParaAnalogueAmps.DicRtuParaAnalogueAmp[loopId].LoopName;
            }
            return base.GetRtuLoopName(loopId);
        }
    };

    public partial class TerminalInformationBase : IIRtuParaGprs
    {
        #region IIRtuParaGprs

        /// <summary>
        /// 通讯参数 通讯服务商
        /// </summary>
        public string ServiceProvider { get; set; }

        /// <summary>
        /// 通讯参数 静态IP地址
        /// </summary>

        public string Ip { get; set; }

        /// <summary>
        /// 通讯参数 端口号
        /// </summary>

        public int Port { get; set; }

        /// <summary>
        /// 通讯参数 手机号
        /// </summary>

        public string SimNumber { get; set; }

        /// <summary>
        /// 通讯参数 心跳周期
        /// </summary>

        public int HeartBeatPeriod { get; set; }

        /// <summary>
        /// 通讯参数 主报周期
        /// </summary>

        public int ReportDataPeriod { get; set; }

        /// <summary>
        /// 通讯参数 优先次序
        /// </summary>

        public int Priority { get; set; }


        /// <summary>
        /// 工作参数 报警延时（秒）
        /// </summary>

        public byte ErrorDelays { get; set; }

        #endregion
    };

    public partial class TerminalInformationBase : IIRtuParaWork
    {
        #region IIRtuParaWork

        /// <summary>
        /// 工作参数 滚动显示
        /// </summary>

        public Boolean Display { get; set; }

        /// <summary>
        /// 工作参数 开机申请
        /// </summary>

        public Boolean Boot { get; set; }

        /// <summary>
        /// 工作参数 声响报警
        /// </summary>

        public Boolean Sound { get; set; }

        /// <summary>
        /// 工作参数 进入自检
        /// </summary>

        public Boolean Selfcheck { get; set; }

        /// <summary>
        /// 工作参数 允许报警
        /// </summary>

        public Boolean Alarm { get; set; }

        /// <summary>
        /// 工作参数 允许主报
        /// </summary>

        public Boolean Report { get; set; }

        /// <summary>
        /// 工作参数 允许呼叫
        /// </summary>

        public Boolean Call { get; set; }

        /// <summary>
        /// 工作参数 禁止路由
        /// </summary>

        public Boolean Route { get; set; }

        //public long RecentUpdateTime { get; set; }

        #endregion
    };

    public partial class TerminalInformationBase : IIRtuParaAnalogueVoltage
    {
        #region IIRtuParaAnalogueVoltage

        /// <summary>
        /// 电压参数 显示名称
        /// </summary>

        public string RtuVoltageName { get; set; }

        /// <summary>
        /// 电压参数 量程
        /// </summary>
        public int Range { get; set; }

        /// <summary>
        /// 电压参数 报警上限
        /// </summary>

        public int UpperLimit { get; set; }

        /// <summary>
        /// 电压参数 报警下限
        /// </summary>

        public int LowerLimit { get; set; }


        /// <summary>
        /// 是否开关量输入状态有电流来判断 >0.3  由于其他参悟无法放入故放入电压参数中
        /// </summary>
        public bool IsSwitchinputJudgebyA { get; set; }

        /// <summary>
        /// 是否启用屏蔽小电流
        /// </summary>
        public bool IsShieldLittleA { get; set; }

        /// <summary>
        /// 屏蔽值
        /// </summary>
        public double AShield { get; set; }
        #endregion
    };
}
