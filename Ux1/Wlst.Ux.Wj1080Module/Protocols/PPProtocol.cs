//using System;
//using System.Collections.Generic;
//using Wlst.Cr.Core.Services;


//namespace Wlst.Ux.Wj1080Module.Protocols
//{
//    /// <summary>
//    /// 核心数据注册协议池:主设备信息  地址池地址为 523xx
//    /// 附属设备信息  533XX
//    /// </summary>
//    public class PPProtocol
//    {

//        public const string Wj1080DataRequest = "wlst.sql.wj1080.Wj1080LuxDataRequest";

//        public const string Wj1080SetAddr = "wlst.rtu.wj1080SetMulitAddr";
//        public const string Wj1080Measure = "wlst.rtu.Wj1080Measure";
//        public const string Wj1080ZcMode = "wlst.rtu.Wj1080ZcMode";

//        public const string Wj1080ZcReportDataTime = "wlst.rtu.Wj1080ZcReportDataTime";
//        public const string Wj1080ZcSoftVersion = "wlst.rtu.Wj1080ZcSoftVersion";

//        public const string Wj1080SetMulitLuxMode = "wlst.rtu.Wj1080SetMulitLuxMode";
//        public const string Wj1080SetReportDataTime = "wlst.rtu.Wj1080SetReportDataTime";

//        private List<Tuple<int, string, string, Type>> lstProtocol;

//        public PPProtocol()
//        {
//            lstProtocol = new List<Tuple<int, string, string, Type>>();

//            //提交服务器数据  供服务器比对时间以确定是否应该让客户端更新终端数据 ；服务器返回数据则为客户端需要更新的数据
//            lstProtocol.Add(new Tuple<int, string, string, Type>(
//                                Ux.Wj1080Module.Services.EventIdAssign.LuxDataRequsetOrReply,
//                                Wj1080DataRequest,
//                                Wj1080DataRequest,
//                                typeof (Model.Exchange.ReplyWj1080Data)));

//            //删除数据  列表中仅具有Id即可
//            lstProtocol.Add(new Tuple<int, string, string, Type>(
//                                Ux.Wj1080Module.Services.EventIdAssign.LuxDataMeasureEvent,
//                                Wj1080Measure,
//                                Wj1080Measure,
//                                typeof (Model.Exchange.ReplyCommuWj1080)));




//            //删除数据  列表中仅具有Id即可
//            lstProtocol.Add(new Tuple<int, string, string, Type>(
//                                Ux.Wj1080Module.Services.EventIdAssign.LuxSetModeEvent,
//                                Wj1080SetMulitLuxMode,
//                                Wj1080SetMulitLuxMode,
//                                typeof (Model.Exchange.ReplyCommuWj1080)));

//            //删除数据  列表中仅具有Id即可
//            lstProtocol.Add(new Tuple<int, string, string, Type>(
//                                Ux.Wj1080Module.Services.EventIdAssign.LuxSetReportTimeEvent,
//                                Wj1080SetReportDataTime,
//                                Wj1080SetReportDataTime,
//                                typeof (Model.Exchange.ReplyCommuWj1080)));

//            //删除数据  列表中仅具有Id即可
//            lstProtocol.Add(new Tuple<int, string, string, Type>(
//                                Ux.Wj1080Module.Services.EventIdAssign.LuxZcReportTimeEvent,
//                                Wj1080ZcReportDataTime,
//                                Wj1080ZcReportDataTime,
//                                typeof (Model.Exchange.ReplyCommuWj1080)));

//            //删除数据  列表中仅具有Id即可
//            lstProtocol.Add(new Tuple<int, string, string, Type>(
//                                Ux.Wj1080Module.Services.EventIdAssign.LuxZcModeEvent,
//                                Wj1080ZcMode ,
//                                Wj1080ZcMode,
//                                typeof(Model.Exchange.ReplyCommuWj1080)));

//            //删除数据  列表中仅具有Id即可
//            lstProtocol.Add(new Tuple<int, string, string, Type>(
//                                Ux.Wj1080Module.Services.EventIdAssign.LuxZcVersionEvent,
//                                Wj1080ZcSoftVersion,
//                                Wj1080ZcSoftVersion,
//                                typeof (Model.Exchange.ReplyCommuWj1080)));
//        }

//        /// <summary>
//        /// 将本模块的协议注册到全局协议池
//        /// </summary>
//        /// <returns></returns>
//        public bool RegistProtocol()
//        {
//            if (lstProtocol == null) return false;
//            return ProtocolServer.RegistProtocol(lstProtocol);
//        }
//    }
//}