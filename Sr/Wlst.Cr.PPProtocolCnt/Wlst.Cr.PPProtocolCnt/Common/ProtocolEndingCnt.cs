//using System;
//using System.Collections.Generic;

//namespace Wlst.Cr.PPProtocolSvrCnt.Common
//{

//    /// <summary>
//    /// 客户端与中间层通信上层协议
//    /// </summary>
//    public class ProtocolEncodingCnt<T> : ProtocolEncodingCntBase where T :new ()
//    {

//        /// <summary>
//        /// 设备地址列表
//        /// </summary>
//        public List<int> AddrLst;

//        /// <summary>
//        /// 如果系统需要中间层返回数据 则中间层返回数据会携带该唯一编号  
//        /// 获取类的时候赋当前系统时间Ticks值  构造函数写入
//        /// </summary>
//        public long Guid;

//        /// <summary>
//        /// 数据  本条数据数据区域 如果有数据的话
//        /// </summary>
//        public T Data;


//        /// <summary>
//        /// 如果中间层返回数据值为2 则表示中间层收到客户端数据 要求客户端放弃再次下发
//        /// </summary>
//        public int RcvDataAndDoNotSndAgain;


//        /// <summary>
//        /// 协议识别码 构造函数写入
//        /// </summary>
//        public string PrivateCmd { get; private set; }

//        ///// <summary>
//        ///// 本条协议数据类型  构造函数写入
//        ///// </summary>
//        //public Type PrivateDataType { get; private set; }


//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="cmd">协议识别码</param>
//        public ProtocolEncodingCnt(string cmd)
//        {
//            PrivateCmd = cmd;
//            // PrivateDataType = typeof (T);
//            //Data = default(T);
//            Data = new T();
//            AddrLst = new List<int>();
//            Guid = DateTime.Now.Ticks;
//            RcvDataAndDoNotSndAgain = 0;
//        }


//        /// <summary>
//        /// 设备地址列表
//        /// </summary>
//        /// <param name="addrLst"></param>
//        public override void SetAddrLst(List<int> addrLst)
//        {
//            if (addrLst == null) return;
//            if (AddrLst == null) AddrLst = new List<int>();
//            foreach (var t in addrLst) this.AddrLst.Add(t);
//        }

//        /// <summary>
//        /// 数据  本条数据数据区域 如果有数据的话
//        /// </summary>
//        /// <param name="data"></param>
//        public override void SetData(object data)
//        {
//            try
//            {
//                if (data is T)
//                {
//                    this.Data = (T) data;
//                }
//            }
//            catch (Exception ex)
//            {

//            }
//        }

//        public override void SetGuid(long guid)
//        {
//            this.Guid = guid;
//            //base.SetGuid(guid);
//        }

//        public override void SetRcvDataAndDoNotSndAgain(int rcvDataAndDoNotSndAgain)
//        {
//            this.RcvDataAndDoNotSndAgain = rcvDataAndDoNotSndAgain;
//            //base.SetRcvDataAndDoNotSndAgain(rcvDataAndDoNotSndAgain);
//        }

//    }

//    public class ProtocolEncodingCntBase
//    {
//        /// <summary>
//        /// 设备地址列表
//        /// </summary>
//        /// <param name="addrLst"></param>
//        public virtual void SetAddrLst(List<int> addrLst)
//        {

//        }

//        /// <summary>
//        /// 数据  本条数据数据区域 如果有数据的话
//        /// </summary>
//        /// <param name="data"></param>
//        public virtual void SetData(object data)
//        {

//        }

//        /// <summary>
//        /// 获取类的时候赋当前系统时间Ticks值  构造函数写入
//        /// </summary>
//        /// <param name="guid"></param>
//        public virtual void SetGuid(long guid)
//        {

//        }

//        /// <summary>
//        /// 如果中间层返回数据值为2 则表示中间层收到客户端数据 要求客户端放弃再次下发
//        /// </summary>
//        /// <param name="rcvDataAndDoNotSndAgain"></param>
//        public virtual void SetRcvDataAndDoNotSndAgain(int rcvDataAndDoNotSndAgain)
//        {

//        }
//    }
//}
