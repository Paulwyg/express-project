using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Wlst.Cr.WjEquipmentBaseModels.Interface;
using Wlst.Cr.WjEquipmentBaseModels.Models;
using Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface;
using Wlst.client;

namespace Wlst.Cr.WjEquipmentBaseModels.WjEquipment.Wj1080
{
    /// <summary>
    /// 本设备为光控设备  可作为附属设备 也可作为主设备  ，
    /// 同时实现了主设备与附属设备属性
    /// </summary>
    [Serializable]
    public partial class Wj1080TerminalInformation : EquipmentInfomation 
    {

      
        public override  int RtuModel
        {
            get { return 1080; }
             set { }
        }

        public Wj1080TerminalInformation()
            : base()
        {

        }

        /// <summary>
        /// 终端参数构造函数
        /// </summary>
        /// <param name="rtuId">终端地址</param>
        /// <param name="rtuName">终端名称</param>
        /// <param name="state">终端工作状态</param>
        public Wj1080TerminalInformation(int rtuId, string rtuName, int state)
            : base(rtuId, rtuName, state)
        {

        }

        public Wj1080TerminalInformation(Interface.IIEquipmentInfo info)
            : base(info)
         {
             
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


    public partial class Wj1080TerminalInformation :IILux 
    {
        /// <summary>
        /// 光控安装位置 
        /// </summary>
        public string LuxLocation { get; set; }
        /// <summary>
        /// 通信方式 0 保留，1 电台，2 串口232，3 串口485，4 Zigbee，5 电力载波，6 Socket
        /// </summary>
        public int RtuCommucationType { get; set; }

        /// <summary>
        /// 光控端口号
        /// </summary>
        public int LuxPort { get; set; }

        /// <summary>
        /// 光控工作模式 0 每隔5秒主报，1 选测应答 ，2 根据设定的时间主动山包，默认10秒，GPRS通信，3 根据设定的时间主动上报，默认10秒，485通信
        /// </summary>
        public int LuxWorkMode { get; set; }

        /// <summary>
        /// 光控量程
        /// </summary>
        public int LuxRange { get; set; }

    };

}
