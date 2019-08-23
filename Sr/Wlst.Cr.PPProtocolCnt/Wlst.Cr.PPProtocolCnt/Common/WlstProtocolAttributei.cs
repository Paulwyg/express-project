//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace wlst.Common
//{
//    /// <summary>
//    /// 客户端与服务器通信
//    /// </summary>
//    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field )]
//    public class WlstProtocolAttribute : Attribute
//    {
//        /// <summary>
//        /// 本条协议信息
//        /// </summary>
//        /// <param name="command">本条协议识别码</param>
//        /// <param name="dataType">本条协议承载的数据类型</param>
//        /// <param name="moniterBy"> 确定是由哪一方监听本条协议</param>
//        public WlstProtocolAttribute(string command, Type dataType, MoniterAside moniterBy)
//        {
//            this.Cmd = command;
//            this.DataType = dataType;
//            this.MoniterBy = moniterBy;
//        }

//        /// <summary>
//        /// 协议识别码
//        /// </summary>
//        public string Cmd { get; private set; }

//        /// <summary>
//        /// 本条协议数据类型 
//        /// </summary>
//        public Type DataType { get; private set; }

//        /// <summary>
//        /// 确定是由哪一方监听本条协议
//        /// </summary>
//        public MoniterAside MoniterBy { get; private set; }
//    }

//    public class WlstProtocolData
//    {
//        /// <summary>
//        /// 协议识别码
//        /// </summary>
//        public string Cmd { get; private set; }

//        /// <summary>
//        /// 本条协议数据类型 
//        /// </summary>
//        public Type DataType { get; private set; }

//        /// <summary>
//        /// 确定是由哪一方监听本条协议
//        /// </summary>
//        public MoniterAside MoniterBy { get; private set; }

//        /// <summary>
//        /// 通过类型获取其下的所有协议
//        /// </summary>
//        /// <param name="instances"></param>
//        /// <returns></returns>
//        public static List<WlstProtocolData> GetWlstProtocol(Type instances)
//        {
//            return ForObject(instances);
//        }

//        private static List<WlstProtocolData> ForObject(Type instances)
//        {
//            var lst = new List<WlstProtocolData>();
//            if (instances == null) return lst;
//            var properties = instances.GetProperties();

//            foreach (var f in properties)
//            {
//                foreach (var g in f.GetCustomAttributes(false))
//                {
//                    if (g is WlstProtocolAttribute)
//                    {
//                        var gg = g as WlstProtocolAttribute;
//                        var wlstProtocolData = new WlstProtocolData()
//                                                   {
//                                                       Cmd = gg.Cmd,
//                                                       DataType = gg.DataType,
//                                                       MoniterBy = gg.MoniterBy,
//                                                   };
//                        lst.Add(wlstProtocolData);
//                        break;
//                    }
//                }
//            }

//            return lst;
//        }
//    }

//    public enum MoniterAside
//    {
//        /// <summary>
//        /// 客户端监听本条协议
//        /// </summary>
//        ByClientSide = 1,

//        /// <summary>
//        /// 服务端监听本条协议
//        /// </summary>
//        ByServerSide,

//        /// <summary>
//        /// 客户端与服务器段均不监听本条协议
//        /// </summary>
//        None
//    }
//}
