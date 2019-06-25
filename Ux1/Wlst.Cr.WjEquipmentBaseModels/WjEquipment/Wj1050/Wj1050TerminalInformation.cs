using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Wlst.Cr.WjEquipmentBaseModels.Models;
using Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface;
using Wlst.client;


namespace Wlst.Cr.WjEquipmentBaseModels.WjEquipment.Wj1050
{
    /// <summary>
    ///  本设备可作为附加设备  ；使用时确认该设备是附属设备还是主设备
    /// </summary>
    [Serializable]
    public partial class Wj1050TerminalInformation : EquipmentInfomation 
    {

     

        public override int RtuModel
        {
            get { return 1050; }
            set { }
        }

        public Wj1050TerminalInformation()
            : base()
        {

        }

        /// <summary>
        /// 终端参数构造函数
        /// </summary>
        /// <param name="rtuId">终端地址</param>
        /// <param name="rtuName">终端名称</param>
        /// <param name="state">终端工作状态</param>
        public Wj1050TerminalInformation(int rtuId, string rtuName, int state)
            : base(rtuId, rtuName, state)
        {

        }

         public Wj1050TerminalInformation (Interface.IIEquipmentInfo info):base (info )
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


    public partial class Wj1050TerminalInformation : IIWj1050Equipment
    {

        /// <summary>
        /// 电表序号
        /// </summary>
        public int MruBandRate { get; set; }

        /// <summary>
        /// 电表变比
        /// </summary>
        public int MruRatio { get; set; }

        /// <summary>
        /// 电表类型 1 ：1997协议；2 ：2007 协议
        /// </summary>
        public int MruType { get; set; }

        ///// <summary>
        ///// 备注
        ///// </summary>
        //public string MruRemark { get; set; }

        /// <summary>
        /// 电表地址
        /// </summary>
        public int MruAddr1 { get; set; }

        /// <summary>
        /// 电表地址
        /// </summary>
        public int MruAddr2 { get; set; }

        /// <summary>
        /// 电表地址
        /// </summary>
        public int MruAddr3 { get; set; }

        /// <summary>
        /// 电表地址
        /// </summary>
        public int MruAddr4 { get; set; }

        /// <summary>
        /// 电表地址
        /// </summary>
        public int MruAddr5 { get; set; }

        /// <summary>
        /// 电表地址
        /// </summary>
        public int MruAddr6 { get; set; }

    };

}
