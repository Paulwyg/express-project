//using System.Collections.Generic;

//namespace Wlst.Cr.PPProtocolSvrCnt.Common
//{
//    /// <summary>
//    /// 与服务器端通信协议标准
//    /// </summary>
//    public class ProtocolEncodingCnt
//    {
//        /// <summary>
//        /// 协议基本参数区域
//        /// 已经完成  发送时需要制定mod值  该协议类型 ProtocolMod
//        /// </summary>
//        public ProtocolEncodingBase head;

//        /// <summary>
//        /// 参数 已完成初始化 需要完成对参数中的addr lst赋值
//        /// </summary>
//        public ProtocolEncodingArgs args;

//        /// <summary>
//        /// 数据 需要按照给定的协议格式完成data  此自定义类型
//        /// </summary>
//        public object one_data_us_zx_ksdf_jhhga_wagawag;

//        public ProtocolEncodingCnt()
//        {
//            head = new ProtocolEncodingBase();
//            args = new ProtocolEncodingArgs();
//            one_data_us_zx_ksdf_jhhga_wagawag = null;
//        }

//    };

//    /// <summary>
//    /// 协议基本参数区域
//    /// </summary>
//    public class ProtocolEncodingBase
//    {
//        /// <summary>
//        /// 0 未知， 1 来自正常电脑客户端,2 来自Android手机平台，3 来自Iphone手机平台，4 来自websocket1平台,5 来自websocket2平台，6 来自Android pad平台，7 来自Iphone pad平台，
//        /// </summary>
//        public int src;
//        /// <summary>
//        /// 协议版本号
//        /// </summary>
//        public int ver;
//        /// <summary>
//        /// 指令域 或 系统内部指令
//        /// </summary>
//        public string cmd;
//        /// <summary>
//        /// 用户有效识别码
//        /// </summary>
//        public string idf;
//        /// <summary>
//        /// 下发命令携带的唯一编号
//        /// 如果系统需要中间层返回数据 则中间层必须携带该唯一编号返回
//        /// </summary>
//        public long  gid;

//        /// <summary>
//        /// 如果中间层返回数据 rcv值为2 则表示中间层收到客户端数据 要求客户端放弃再次下发  无携带数据
//        /// </summary>
//        public int rcv;

 

//        public ProtocolEncodingBase()
//        {
//            ver = 1;
//            gid = 0;
//            cmd = string.Empty;
//            rcv = 0;
//            idf = string.Empty;
//            src = 1;
//        }
//    };



//    public class ProtocolEncodingArgs
//    {
//        public List<int> addr;

//        public ProtocolEncodingArgs()
//        {
//            addr = new List<int>();
//        }
//    };

//}
