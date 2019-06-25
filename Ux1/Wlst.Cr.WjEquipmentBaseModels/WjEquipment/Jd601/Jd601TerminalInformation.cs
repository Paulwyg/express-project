using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Wlst.Cr.WjEquipmentBaseModels.Models;
using Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface;
using Wlst.client;


namespace Wlst.Cr.WjEquipmentBaseModels.WjEquipment.Jd601
{

    /// <summary>
    /// 本设备为节能设备  可作为附属设备
    /// 同时实现了主设备与附属设备属性
    /// </summary>
    [Serializable]
    public partial class Jd601TerminalInformation : EquipmentInfomation 
    {

       

        public override int RtuModel
        {
            get { return 601; }
            set { }
        }

        public Jd601TerminalInformation()
            : base()
        {

            this.EsyValidIdentify = true;
            this.PreheatingTime = 2;
            this.EsyOpentTime = 0;
            this.CloseTime = 0;
            this.CtRadioA = 150;
            this.CtRadioB = 150;
            this.CtRadioC = 150;
            this.TimeMode =1;
            this.RunMode =0;
            this.FanSatrtTemp = 45;
            this.FanClosedTemp = 35;
            this.EnerySaveTemp = 70;
            this.MandatoryProtectTemp = 85;
            this.RecoverEnergySaveTemp = 50;
            this.InputOvervoltageLimit = 270;
            this.InputUndervoltageLimit = 170;
            this.OutputUndervoltageLimit = 160;
            this.OutputOverloadLimit = 144;
            this.RegulatingSpeed = 10;
            this.PowerSupplyPhases = 3;
            this.CommTypeCode =0;
            this.WorkMode = 0;
            this.IsActiveAlarm = false ;
            this.AlarmDelay = 10;
            this.Mode = 1;
        }

        /// <summary>
        /// 终端参数构造函数
        /// </summary>
        /// <param name="rtuId">终端地址</param>
        /// <param name="rtuName">终端名称</param>
        /// <param name="state">终端工作状态</param>
        public Jd601TerminalInformation(int rtuId, string rtuName, int state)
            : base(rtuId, rtuName, state)
        {

            this.EsyValidIdentify = true;
            this.PreheatingTime = 2;
            this.EsyOpentTime = 0;
            this.CloseTime = 0;
            this.CtRadioA = 150;
            this.CtRadioB = 150;
            this.CtRadioC = 150;
            this.TimeMode = 1;
            this.RunMode = 0;
            this.FanSatrtTemp = 45;
            this.FanClosedTemp = 35;
            this.EnerySaveTemp = 70;
            this.MandatoryProtectTemp = 85;
            this.RecoverEnergySaveTemp = 50;
            this.InputOvervoltageLimit = 270;
            this.InputUndervoltageLimit = 170;
            this.OutputUndervoltageLimit = 160;
            this.OutputOverloadLimit = 144;
            this.RegulatingSpeed = 10;
            this.PowerSupplyPhases = 3;
            this.CommTypeCode =0;
            this.WorkMode =0;
            this.IsActiveAlarm = false ;
            this.AlarmDelay = 10;
            this.Mode = 1;
        }

        public Jd601TerminalInformation (Interface.IIEquipmentInfo info):base (info )
        {
            this.EsyValidIdentify = true;
            this.PreheatingTime = 2;
            this.EsyOpentTime = 0;
            this.CloseTime = 0;
            this.CtRadioA = 150;
            this.CtRadioB = 150;
            this.CtRadioC = 150;
            this.TimeMode = 1;
            this.RunMode = 0;
            this.FanSatrtTemp = 45;
            this.FanClosedTemp = 35;
            this.EnerySaveTemp = 70;
            this.MandatoryProtectTemp = 85;
            this.RecoverEnergySaveTemp = 50;
            this.InputOvervoltageLimit = 270;
            this.InputUndervoltageLimit = 170;
            this.OutputUndervoltageLimit = 160;
            this.OutputOverloadLimit = 144;
            this.RegulatingSpeed = 10;
            this.PowerSupplyPhases = 3;
            this.CommTypeCode = 0;
            this.WorkMode = 0;
            this.IsActiveAlarm = false;
            this.AlarmDelay = 10;
            this.Mode = 1;
        }

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
    };


    public partial class Jd601TerminalInformation : IIJd601
    {
        /// <summary>
        /// 有效标示  指示是否使用
        /// </summary>

        public bool EsyValidIdentify { get; set; }

        /// <summary>
        /// 预热时间 默认2分钟
        /// </summary>

        public int PreheatingTime { get; set; }

        /// <summary>
        /// 开机时间 不设置
        /// </summary>

        public int EsyOpentTime { get; set; }

        /// <summary>
        /// 关机时间 不设置
        /// </summary>

        public int CloseTime { get; set; }

        /// <summary>
        /// A相接触器变比 50~500 默认150
        /// </summary>

        public int CtRadioA { get; set; }

        /// <summary>
        /// B相接触器变比 50~500 默认150
        /// </summary>

        public int CtRadioB { get; set; }

        /// <summary>
        /// C相接触器变比 50~500 默认150
        /// </summary>

        public int CtRadioC { get; set; }

        /// <summary>
        /// 时间模式：0 为定时模式 ；1为延时模式；默认1
        /// </summary>

        public int TimeMode { get; set; }

        /// <summary>
        /// 运行模式：0 自动，1 手动； 默认 0
        /// </summary>

        public int RunMode { get; set; }

        /// <summary>
        /// 风机启动温度 默认45
        /// </summary>

        public int FanSatrtTemp { get; set; }

        /// <summary>
        /// 风机关闭温度 默认35
        /// </summary>

        public int FanClosedTemp { get; set; }

        /// <summary>
        /// 退出节能温度 默认 70 界面设置
        /// </summary>

        public int EnerySaveTemp { get; set; }

        /// <summary>
        /// 强制保护温度 默认85
        /// </summary>

        public int MandatoryProtectTemp { get; set; }

        /// <summary>
        /// 恢复节能温度 默认50
        /// </summary>

        public int RecoverEnergySaveTemp { get; set; }

        /// <summary>
        /// 输入过压门限值 默认270
        /// </summary>

        public int InputOvervoltageLimit { get; set; }

        /// <summary>
        /// 输入欠压门限值 默认170
        /// </summary>

        public int InputUndervoltageLimit { get; set; }

        /// <summary>
        /// 输出欠压门限值 默认160
        /// </summary>

        public int OutputUndervoltageLimit { get; set; }

        /// <summary>
        /// 输入过载门限值 默认 144 电流
        /// </summary>

        public int OutputOverloadLimit { get; set; }

        /// <summary>
        /// 调压速度 仅模式为延时模式时有效 默认10 6~60
        /// </summary>

        public int RegulatingSpeed { get; set; }

        /// <summary>
        /// 供电相数  默认3 不提供界面设置
        /// </summary>

        public int PowerSupplyPhases { get; set; }

        /// <summary>
        /// 通信模式 ：0 通过终端；1 通过通信模块 默认0
        /// </summary>

        public int CommTypeCode { get; set; }

        /// <summary>
        /// 工作模式：0 通用模式；1 特殊模式；不提供界面设置 默认0
        /// </summary>

        public int WorkMode { get; set; }

        /// <summary>
        /// 是否主动报警 默认false
        /// </summary>

        public bool IsActiveAlarm { get; set; }

        /// <summary>
        /// 报警延时时间  默认10秒钟
        /// </summary>

        public int AlarmDelay { get; set; }

        /// <summary>
        /// 节能模式：0 接触器模式；1 IGBT模式 默认1
        /// </summary>

        public int Mode { get; set; }




    };
}
