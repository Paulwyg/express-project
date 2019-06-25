//using System;
//using System.Collections.Generic;
//using Wlst.Cr.Core.Services;


//using Wlst.Ux.Wj1050Module.Wj1050MruEventScheduleViewModel.Models;

//namespace Wlst.Ux.Wj1050Module.Protocols
//{
//    /// <summary>
//    /// 核心数据注册协议池:主设备信息  地址池地址为 523xx
//    /// 附属设备信息  533XX
//    /// </summary>
//    public class PPProtocol
//    {
//        private List<Tuple<int, string, string, Type>> lstProtocol;

//        public PPProtocol()
//        {
//            lstProtocol = new List<Tuple<int, string, string, Type>>();

//            //提交服务器数据  供服务器比对时间以确定是否应该让客户端更新终端数据 ；服务器返回数据则为客户端需要更新的数据
//            lstProtocol.Add(new Tuple<int, string, string, Type>(
//                                Ux.Wj1050Module.Services.EventIdAssign.MruEventTaskInstanceInfoRequsetOrReply,
//                                "wlst.sql.Wj1050Module.MruEventTaskInstanceInfoRequset",
//                                "wlst.sql.Wj1050Module.MruEventTaskInstanceInfoRequset",
//                                typeof (MruEventTaskInstanceInfoforExchange)));

//            //删除数据  列表中仅具有Id即可
//            lstProtocol.Add(new Tuple<int, string, string, Type>(
//                                Ux.Wj1050Module.Services.EventIdAssign.MruEventTaskInstanceInfoUpdate,
//                                "wlst.sql.Wj1050Module.MruEventTaskInstanceInfoUpdate",
//                                "wlst.sql.Wj1050Module.MruEventTaskInstanceInfoUpdate",
//                                typeof (MruEventTaskInstanceInfoforExchange)));




//            //删除数据  列表中仅具有Id即可
//            lstProtocol.Add(new Tuple<int, string, string, Type>(
//                                Ux.Wj1050Module.Services.EventIdAssign.RequestMruDataId,
//                                "wlst.sql.Wj1050Module.MruReadData",
//                                "wlst.sql.Wj1050Module.MruReadData",
//                                typeof(Models .Exchange .ReplyMruData )));

//            //删除数据  列表中仅具有Id即可
//            lstProtocol.Add(new Tuple<int, string, string, Type>(
//                                Ux.Wj1050Module.Services.EventIdAssign.MruReadAddrId,
//                                "wlst.rtu.MruReadAddr",
//                                "wlst.rtu.MruReadAddr",
//                                typeof(Models .Exchange .ReplyReadMruAddr )));

//            //删除数据  列表中仅具有Id即可
//            lstProtocol.Add(new Tuple<int, string, string, Type>(
//                                Ux.Wj1050Module.Services.EventIdAssign.MruReadDataId,
//                                "wlst.rtu.MruReadData",
//                                "wlst.rtu.MruReadData",
//                                typeof(Models .Exchange .ReplyReadMruData )));
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