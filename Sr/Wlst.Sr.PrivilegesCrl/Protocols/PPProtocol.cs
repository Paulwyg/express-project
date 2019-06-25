//using System;
//using System.Collections.Generic;
//using Wlst.Cr.Core.Services;
//using Wlst.Sr.PrivilegesCrl.ExchangeModels.FrServer;
//using Wlst.Sr.PrivilegesCrl.ExchangeModels.ToServer;


//namespace Wlst.Sr.PrivilegesCrl.Protocols
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
//                                Services.EventIdAssign.RequestOrUpdateUserPrivilegInfoId,
//                                "wlst.sql.PrivilegesCrl.RequestUserPrivilegInfo",
//                                "wlst.sql.PrivilegesCrl.RequestUserPrivilegInfo",
//                                typeof (UserPrivilege)));

//            lstProtocol.Add(new Tuple<int, string, string, Type>(
//                                Services.EventIdAssign.RequestModifyUserInfomaotnAndPrivilegeRightId,
//                                "wlst.sql.PrivilegesCrl.RequestModifyUserInfomaotnAndPrivilegeRight",
//                                "wlst.sql.PrivilegesCrl.RequestModifyUserInfomaotnAndPrivilegeRight",
//                                typeof (UserRightOfPrivilege)));




//            lstProtocol.Add(new Tuple<int, string, string, Type>(
//                                Services.EventIdAssign.ModflyUserInfomationId,
//                                "wlst.sql.PrivilegesCrl.ModflyUserInfomation",
//                                "wlst.sql.PrivilegesCrl.ModflyUserInfomation",
//                                typeof (ModfliedUserInfo)));



//            lstProtocol.Add(new Tuple<int, string, string, Type>(
//                                Services.EventIdAssign.RequestAllUserInfomationId,
//                                "wlst.sql.PrivilegesCrl.RequestAllUserInfomation",
//                                "wlst.sql.PrivilegesCrl.RequestAllUserInfomation",
//                                typeof (AllUserInfo)));

//            lstProtocol.Add(new Tuple<int, string, string, Type>(
//                                Services.EventIdAssign.DeleteUserInfoId,
//                                "wlst.sql.PrivilegesCrl.DeleteUserInfo",
//                                "wlst.sql.PrivilegesCrl.DeleteUserInfo",
//                                typeof (BaseRequestInfo)));

//            lstProtocol.Add(new Tuple<int, string, string, Type>(
//                                Services.EventIdAssign.AddNewUserInfoId,
//                                "wlst.sql.PrivilegesCrl.AddNewUserInfo",
//                                "wlst.sql.PrivilegesCrl.AddNewUserInfo",
//                                typeof (ModfliedUserInfo)));


//            lstProtocol.Add(new Tuple<int, string, string, Type>(
//                                Services.EventIdAssign.ModifyPriGroupInfomation,
//                                "wlst.sql.PrivilegesCrl.ModifyPriGroupInfomation",
//                                "wlst.sql.PrivilegesCrl.ModifyPriGroupInfomation",
//                                typeof(ModifiedPriGroupInfomation)));



//            lstProtocol.Add(new Tuple<int, string, string, Type>(
//                                Services.EventIdAssign.RequestAllPriGroupInfomation,
//                                "wlst.sql.PrivilegesCrl.RequestAllPriGroupInfomation",
//                                "wlst.sql.PrivilegesCrl.RequestAllPriGroupInfomation",
//                                typeof(AllPriGroupInfomation )));

//            lstProtocol.Add(new Tuple<int, string, string, Type>(
//                                Services.EventIdAssign.DeletePriGroupInfomation ,
//                                "wlst.sql.PrivilegesCrl.DeletePriGroupInfomation",
//                                "wlst.sql.PrivilegesCrl.DeletePriGroupInfomation",
//                                typeof(BaseRequestPrivilegeGroupPriInfo)));

//            lstProtocol.Add(new Tuple<int, string, string, Type>(
//                                Services.EventIdAssign.AddPriGroupInfomation,
//                                "wlst.sql.PrivilegesCrl.AddPriGroupInfomation",
//                                "wlst.sql.PrivilegesCrl.AddPriGroupInfomation",
//                                typeof(ModifiedPriGroupInfomation )));




//            lstProtocol.Add(new Tuple<int, string, string, Type>(
//                                Services.EventIdAssign.RequestGourpInfoPrivilegeById,
//                                "wlst.sql.PrivilegesCrl.RequestGourpInfoPrivilegeById",
//                                "wlst.sql.PrivilegesCrl.RequestGourpInfoPrivilegeById",
//                                typeof (GroupPrivilege )));

//            lstProtocol.Add(new Tuple<int, string, string, Type>(
//                                Services.EventIdAssign.ModifyGourpInfoPrivilege ,
//                                "wlst.sql.PrivilegesCrl.ModifyGourpInfoPrivilege",
//                                "wlst.sql.PrivilegesCrl.ModifyGourpInfoPrivilege",
//                                typeof (GroupPrivilege  )));
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