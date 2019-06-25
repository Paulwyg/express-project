using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Wlst.Cr.WjEquipmentBaseModels.Models;
using Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationParts;
using Wlst.client;


namespace Wlst.Cr.WjEquipmentBaseModels.WjEquipment.Wj2090
{
    /// <summary>
    /// 本设备支持2090型单灯设备；
    /// <para>2090型单灯设备支持最多256个控制器</para>
    /// </summary>
    [Serializable]
    public partial class Wj2090TerminalInformation : EquipmentInfomation, 
        TerminalInfomationInterface.IISluRegulators ,
        TerminalInfomationInterface .IISlu 
    {

        /// <summary>
        /// 单灯控制器
        /// </summary>
        public SluRegulators SluRegulators { get; set; }
      

        //public override int RtuModel
        //{ get; set; }

        public Wj2090TerminalInformation()
            : base()
        {
            SluRegulators = new SluRegulators();
        }

        /// <summary>
        /// 设备参数构造函数
        /// </summary>
        /// <param name="rtuId">设备地址</param>
        /// <param name="rtuName">设备名称</param>
        /// <param name="state">终端工作状态</param>
        public Wj2090TerminalInformation(int rtuId, string rtuName, int state)
            : base(rtuId, rtuName, state)
        {
            SluRegulators = new SluRegulators(rtuId );
        }


        public Wj2090TerminalInformation(Interface.IIEquipmentInfo info)
            : base(info)
        {
            SluRegulators = new SluRegulators(info .RtuId );
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


        public override string GetRtuLoopName(int loopId)
        {
            //return base.GetRtuLoopLampName(loopId, lampId);
            if (SluRegulators != null && SluRegulators.DicRtuParaSluRegulator.ContainsKey(loopId))
            {
                return SluRegulators.DicRtuParaSluRegulator[loopId].RtuName;
            }

            return base.GetRtuLoopName(loopId);
        }

        public override string GetRtuLoopNameandLampCode(int loopId)
        {
            
            if (SluRegulators != null && SluRegulators.DicRtuParaSluRegulator.ContainsKey(loopId))
            {
                return ", "+SluRegulators.DicRtuParaSluRegulator[loopId].LampCode ;
            }
            return base.GetRtuLoopNameandLampCode(loopId);

        }
    };

    public partial class Wj2090TerminalInformation
    {
       
        /// <summary>
        /// 是否启用集中器巡测
        /// </summary>

        public bool IsPartrolMeasured { get; set; }


        /// <summary>
        /// 是否投运
        /// </summary>
        public bool IsUsed { get; set; }


        /// <summary>
        /// 是否主动报警
        /// </summary>
        public bool IsAlarmAuto { get; set; }


        /// <summary>
        /// 是否自动补发指令
        /// </summary>
        public bool IsSndOrderAuto { get; set; }


        /// <summary>
        /// zigbee 地址
        /// </summary>
        public long  ZigbeeAddress { get; set; }

        /// <summary>
        ///  1 Zigbee, 0 线路载波
        /// </summary>
        public int IsZigbee { get; set; }


        /// <summary>
        /// 控制器数目 最多256
        /// </summary>
        public int SumOfControls { get; set; }


        /// <summary>
        /// 域名  1-65383
        /// </summary>
        public int DomainName { get; set; }


        //[AttrisColumn("rtu_id")]
        //public int domain_nameLastUsed { get; set; }

        ////域名 上一次使用的域名
        //[AttrisColumn("rtu_id")]
        //public int DomainNamePrepare { get; set; }

        //未生效的新域名 1默认等待设置

        /// <summary>
        /// 电压上限
        /// </summary>
        public int UpperVoltage { get; set; }

        //电压上限
        /// <summary>
        /// 电压下限
        /// </summary>
        public int LowerVoltage { get; set; }


  
        public double Longitude { get; set; }

        //纬度
       
        public double Latitude { get; set; }



        /// <summary>
        /// 蓝牙Pin码
        /// </summary>
        public int BluetoothPin { get; set; }


        /// <summary>
        /// 安全模式 : 0-无安全模式-配对成功即可查询但不可设置；1-安全模式1-配对成功即可查询设置；2-安全模式2-配对成功并经主台确认可查询设置；默认0
        /// </summary>
        public int SecurityPattern { get; set; }


        /// <summary>
        /// 理由运行模式：1-标准模式；2-扩展模式；3-III代模式；4-IV代模式；5自适应模式；默认1
        /// </summary>
        public int RouteRunPattern { get; set; }


        /// <summary>
        /// 启用的信道  1-16，存在于该列表中则启用，不存在则不启用 
        /// </summary>
        public List<int> ChannelUsed { get; set; }


        /// <summary>
        /// 连续通信失败多少次后报警  1-50；
        /// </summary>
        public int AlarmCountCommucationFail { get; set; }


        /// <summary>
        /// 功率因数低于多少后报警； 40-100
        /// </summary>
        public int AlarmPowerfactorLower { get; set; }


        /// <summary>
        /// 电流量程；0.1-20
        /// </summary>
        public double CurrentUpper { get; set; }

        /// <summary>
        /// 功率量程  ；10-2000
        /// </summary>
        public int PowerUpper { get; set; }




        /// <summary>
        /// 1 -PWM ;2 -485 
        /// </summary>
        public int PowerAdjustType { get; set; }

        /// <summary>
        /// 功率调节频率或波特率
        /// </summary>
        public int PowerAdjustBound { get; set; }

        /// <summary>
        /// Ip地址 
        /// </summary>

        public string StaticIp { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>

        public string MobileNo { get; set; }
    }


}
