using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Wlst.Cr.WjEquipmentBaseModels.Models;
using Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationParts;
using Wlst.client;


namespace Wlst.Cr.WjEquipmentBaseModels.WjEquipment.Wj1090
{
    /// <summary>
    /// 本设备支持1090型，3090型防盗设备；
    /// <para>1090型防盗设备支持2路防盗 只能作为附属设备 集中控制器型号为 1090</para>
    /// <para>3090型防盗设备支持6路防盗 可以作为附属设备呈现  作为附属设备的时候虚拟集中控制器型号为 30920 即本类模型</para>
    /// <para>3090型防盗设备支持6路防盗 可以作为主设备呈现  作为主设备的时候虚拟集中控制器型号为 30910 即本类模型</para>
    /// </summary>
    [Serializable]
    public partial class Wj1090TerminalInformation : EquipmentInfomation,TerminalInfomationInterface.IILduConcentrator
    {
        /// <summary>
        /// 检测回路信息 
        /// </summary>
        public List<LduLineParameter> LduLines { get; set; }
  

        //public override int RtuModel
        //{ get; set; }

        public Wj1090TerminalInformation()
            : base()
        {
            LduLines = new List<LduLineParameter>();
        }

        /// <summary>
        /// 设备参数构造函数
        /// </summary>
        /// <param name="rtuId">设备地址</param>
        /// <param name="rtuName">设备名称</param>
        /// <param name="state">终端工作状态</param>
        public Wj1090TerminalInformation(int rtuId, string rtuName, int state)
            : base(rtuId, rtuName, state)
        {
            LduLines = new List<LduLineParameter>();
        }


        public Wj1090TerminalInformation(Interface.IIEquipmentInfo info)
            : base(info)
        {
            LduLines = new List<LduLineParameter>();
        }

        public override string GetRtuLoopName(int loopId)
        {
            //return base.GetRtuLoopLampName(loopId, lampId);
            if(LduLines !=null )
            foreach (var g in LduLines )
            {
                if(g.LduLineID ==loopId )
                {
                    return g.LduLineName;
                }
            }
            return base.GetRtuLoopName(loopId);
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


}
