using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.MefExtensions.Event;
using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.ModuleServices;
using Wlst.Cr.Core.UtilityFunction;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreMims.ShowMsgInfo;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Cr.CoreOne.Services;
using Wlst.Cr.PPProtocolSvrCnt.Common;
using Wlst.client;


namespace Wlst.Sr.PrivilegesCrl.PrivilegeInfo
{

    /// <summary>
    /// 
    /// </summary>
    public partial class PrivilegeInfomation : EventHandlerHelper
    {
        /// <summary>
        /// 执行初始化并向服务器求情数据
        /// </summary>
        public void InitStart()
        {

            this.InitEvent();
            this.InitAction();
          
        }
  
    }


    /// <summary>
    /// Action
    /// </summary>
    public partial class PrivilegeInfomation
    {      private void InitEvent()
        {
              this.AddEventFilterInfo(-1, PublishEventType.ReCn);
        }

    



        public void InitAction()
        {
 


            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone .ClientListen .wlst_svr_ans_cnt_request_update_user_info ,//.ClientPart.wlst_PrivilegeInfo_server_ans_clinet_update_UserInfomation,
                ModflyUserInfomationId,
                typeof(PrivilegeInfomation), this);
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone .ClientListen .wlst_svr_ans_cnt_request_all_user_info ,//.ClientPart.wlst_PrivilegeInfo_server_ans_clinet_request_UserInfomation,
                RequestAllUserInfomationId,
                typeof(PrivilegeInfomation), this);



            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone .ClientListen .wlst_svr_ans_cnt_request_delete_user_info ,//.ClientPart.wlst_PrivilegeInfo_server_ans_clinet_delete_UserInfomation,
                DeleteUserInfoId,
                typeof(PrivilegeInfomation), this);
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone .ClientListen .wlst_svr_ans_cnt_request_add_user_info ,//.ClientPart.wlst_PrivilegeInfo_server_ans_clinet_add_UserInfomation,
                AddNewUserInfoId,
                typeof(PrivilegeInfomation), this);


        }


        public void ModflyUserInfomationId(string session,Wlst .mobile .MsgWithMobile  infos)
        {
            var info = infos.WstSvrAnsCntAddOrUpdateOrRequestUserInfo ;
            //var info = args.GetParams()[1] as ModfliedUserInfo;
            if (info == null) return;
            if (info.SvrRtnModified  && info.UserInfo.UserName == UserInfo.UserLoginInfo.UserName)
            {
                if (info.ManagerUserName != UserInfo.UserLoginInfo.UserName)
                {
                    //别人修改了自己的用户信息
                    if (info.UserInfo.UserPasswrod != UserInfo.UserLoginInfo.UserPasswrod)
                    {
                        Wlst.Sr.PPPandSocketSvr.Server.SocketClient.Close();
                        UMessageBox.Show( "用户已经被修改","您的用户名已经被管理员修改，系统已停止对您的数据服务，请从新登陆...",
                            UMessageBoxButton.Ok);
                        return;
                    }
                }

                UserInfo.UserLoginInfo = info.UserInfo;
    
            }


            //      UserInfo.UserLoginInfo = info.UserInfo;
            var ar = new PublishEventArgs()
                         {
                             EventId = Services.EventIdAssign.ModflyUserInfomationId,
                             EventType = PublishEventType.Core
                         };
            ar.AddParams(info);
            ar.AddParams(infos.Head .Gid );
            EventPublisher.EventPublish(ar);
        }

        public void RequestAllUserInfomationId(string session,Wlst .mobile .MsgWithMobile  infos)
        {
            var info = infos.WstSvrAnsCntRequestUserInfo ;
            // var info = args.GetParams()[1] as AllUserInfo;
            if (info == null) return;
            if (info.ManagerUserName == UserInfo.UserLoginInfo.UserName)
            {
                var ar = new PublishEventArgs()
                             {
                                 EventId = Services.EventIdAssign.RequestAllUserInfomationId,
                                 EventType = PublishEventType.Core
                             };
                ar.AddParams(info);
                EventPublisher.EventPublish(ar);

            }
        }

        public void DeleteUserInfoId(string session,Wlst .mobile .MsgWithMobile  infos)
        {
            var info = infos.WstSvrAnsCntDeleteUserInfo ;
            // var info = args.GetParams()[1] as BaseRequestInfo;
            if (info == null) return;
            if (info.TargetUserName == UserInfo.UserLoginInfo.UserName)
            {
                //var ar = new PublishEventArgs()
                //{
                //    EventId = Services.EventIdAssign.RequestAllUserInfomationId,
                //    EventType = PublishEventType.Core
                //};
                //ar.AddParams(info);
                //EventPublisher.EventPublish(ar);
                Wlst.Sr.PPPandSocketSvr.Server.SocketClient.Close();


               UMessageBox.Show(
                    
                                                          "用户已经注销",
                   
                                                          "您的用户名已经被管理员注销，系统已停止对您提供数据服务...",
                    UMessageBoxButton.Ok);


            }
            else
            {
                var ar = new PublishEventArgs()
                             {
                                 EventId = Services.EventIdAssign.DeleteUserInfoId,
                                 EventType = PublishEventType.Core
                             };
                ar.AddParams(info.TargetUserName);
                EventPublisher.EventPublish(ar);
            }
        }


        public void AddNewUserInfoId(string session,Wlst .mobile .MsgWithMobile  infos)
        {
            var info = infos.WstSvrAnsCntAddOrUpdateOrRequestUserInfo ;
            //var info = args.GetParams()[1] as ModfliedUserInfo;
            if (info == null) return;
            if (info.ManagerUserName != UserInfo.UserLoginInfo.UserName) //fei本用户在修改用户信息
            {
                return;
            }
            var ar = new PublishEventArgs()
                         {
                             EventId = Services.EventIdAssign.AddNewUserInfoId,
                             EventType = PublishEventType.Core
                         };
            ar.AddParams(info);
            ar.AddParams(infos.Head .Gid );
            EventPublisher.EventPublish(ar);
        }


   


    }

    /// <summary>
    /// Socket to Server
    /// </summary>
    public partial class PrivilegeInfomation
    {
       
        public void DeleteUserInfomaiton(string deleteUserName)
        {
            //int waitId = Infrastructure.UtilityFunction.TickCount.EnvironmentTickCount;
            //LogInfo.Log(I36N.Services.I36N.ConvertByCodingOne(Services.I36NIdAssign.I36NIdAssignBase + 1,
            //                                                  "正在删除用户信息!!!"));
            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(0, "权限信息", OperatrType.UserOperator, "正在删除用户信息!!!");
            //var info = new BaseRequestInfo();
            //info.ManagerUserName = Wlst.Cr.Core.Services.UserInfo.UserLoginInfo.UserName;
            //info.ManagerUserPassword = Wlst.Cr.Core.Services.UserInfo.UserLoginInfo.UserPasswrod;
            //info.TargetUserName = deleteUserName;
            //SndOrderServer.OrderSnd(Services.EventIdAssign.DeleteUserInfoId, null,
            //                        info, waitId, 10, 12);


            var info = Wlst.Sr.ProtocolPhone .ServerListen .wlst_cnt_request_delete_user_info ;//.ServerPart.wlst_PrivilegeInfo_clinet_delete_UserInfomation;
            info.WstCntDeleteUserInfo .ManagerUserName = UserInfo.UserLoginInfo.UserName;
            info.WstCntDeleteUserInfo.ManagerUserPassword = UserInfo.UserLoginInfo.UserPasswrod;
            info.WstCntDeleteUserInfo.TargetUserName = deleteUserName;
            SndOrderServer.OrderSnd(info, 10, 6);
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="managerInfo"></param>
        public long ModflyUserInfo(UserInfomation managerInfo)
        {
           // int waitId = Environment.TickCount;
            //LogInfo.Log(I36N.Services.I36N.ConvertByCodingOne(Services.I36NIdAssign.I36NIdAssignBase + 1,
            //                                                  "正在验证用户权限信息!!!"));
            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(0, "权限信息", OperatrType.UserOperator, "正在验证用户权限信息!!!");
            //var info = new ModflyOrAddUserInfo();
            //info.ManagerUserName = Wlst.Cr.Core.Services.UserInfo.UserLoginInfo.UserName;
            //info.ManagerUserPassword = Wlst.Cr.Core.Services.UserInfo.UserLoginInfo.UserPasswrod;

            //info.UserInfo = managerInfo;
            //SndOrderServer.OrderSnd(Services.EventIdAssign.ModflyUserInfomationId, null,
            //                        info, waitId, 10, 12);


            var info = Wlst.Sr.ProtocolPhone .ServerListen .wlst_cnt_request_update_user_info ;//.ServerPart.wlst_PrivilegeInfo_clinet_update_UserInfomation;
            info.WstCntAddOrUpdateOrRequestUserInfo .ManagerUserName = UserInfo.UserLoginInfo.UserName;
            info.WstCntAddOrUpdateOrRequestUserInfo.ManagerUserPassword = UserInfo.UserLoginInfo.UserPasswrod;
            info.WstCntAddOrUpdateOrRequestUserInfo.UserInfo = managerInfo;
            SndOrderServer.OrderSnd(info, 10, 6);
            return info.Head .Gid ;
        }

        /// <summary>
        /// 使用本账户请求所有的用户信息
        /// </summary>
        public void RequestAllUserInfomation()
        {
            //int waitId = Infrastructure.UtilityFunction.TickCount.EnvironmentTickCount;
            //LogInfo.Log(I36N.Services.I36N.ConvertByCodingOne(Services.I36NIdAssign.I36NIdAssignBase + 1,
            //                                                  "正在请求所有用户信息!!!"));

            //var info = new BaseRequestInfo
            //               {
            //                   ManagerUserName = Wlst.Cr.Core.Services.UserInfo.UserLoginInfo.UserName,
            //                   ManagerUserPassword = Wlst.Cr.Core.Services.UserInfo.UserLoginInfo.UserPasswrod,
            //                   TargetUserName = Wlst.Cr.Core.Services.UserInfo.UserLoginInfo.UserName
            //               };
            //SndOrderServer.OrderSnd(Services.EventIdAssign.RequestAllUserInfomationId, null,
            //                        info, waitId, 10, 3);


            var info = Wlst.Sr.ProtocolPhone .ServerListen .wlst_cnt_request_all_user_info ;//.ServerPart.wlst_PrivilegeInfo_clinet_request_UserInfomation ;
            info.WstCntRequestUserInfo .ManagerUserName = UserInfo.UserLoginInfo.UserName;
            info.WstCntRequestUserInfo.ManagerUserPassword = UserInfo.UserLoginInfo.UserPasswrod;
            info.WstCntRequestUserInfo.TargetUserName = UserInfo.UserLoginInfo.UserName;
            SndOrderServer.OrderSnd(info, 10, 6);
        }



        /// <summary>
        /// AddUserInfo
        /// </summary>
        /// <param name="addUserInfo"></param>
        public long AddUserInfo(UserInfomation addUserInfo)
        {
            //int waitId = Infrastructure.UtilityFunction.TickCount.EnvironmentTickCount;
            //LogInfo.Log(I36N.Services.I36N.ConvertByCodingOne(Services.I36NIdAssign.I36NIdAssignBase + 1,
            //                                                  "正在增加新用户"));
            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(0, "用户", OperatrType.UserOperator, "正在增加新用户!!!");
            //var info = new ModflyOrAddUserInfo();
            //info.ManagerUserName = Wlst.Cr.Core.Services.UserInfo.UserLoginInfo.UserName;
            //info.ManagerUserPassword = Wlst.Cr.Core.Services.UserInfo.UserLoginInfo.UserPasswrod;
            //info.UserInfo = addUserInfo;
            //SndOrderServer.OrderSnd(Services.EventIdAssign.AddNewUserInfoId, null,
            //                        info, waitId, 10, 12);


            var info = Wlst.Sr.ProtocolPhone .ServerListen .wlst_cnt_request_add_user_info ;//.ServerPart.wlst_PrivilegeInfo_clinet_add_UserInfomation;
            info.WstCntAddOrUpdateOrRequestUserInfo .ManagerUserName = UserInfo.UserLoginInfo.UserName;
            info.WstCntAddOrUpdateOrRequestUserInfo.ManagerUserPassword = UserInfo.UserLoginInfo.UserPasswrod;
            info.WstCntAddOrUpdateOrRequestUserInfo.UserInfo = addUserInfo;
            SndOrderServer.OrderSnd(info, 10, 6);
            return info.Head .Gid ;

        }


        //public void RequestPrivilegeInfoByPriGroupId(int requestPriId)
        //{
        //    int waitId = Infrastructure.UtilityFunction.TickCount.EnvironmentTickCount;
        //    LogInfo.Log("正在请求权限信息!!!");
        //    var info = new BaseRequestPrivilegeGroupPriInfo();
        //    info.ManagerUserName = Wlst.Cr.Core.Services.UserInfo.UserLoginInfo.UserName;
        //    info.ManagerUserPassword = Wlst.Cr.Core.Services.UserInfo.UserLoginInfo.UserPasswrod;
        //    info.TargetPrivilegeId = requestPriId;
        //    SndOrderServer.OrderSnd(Services.EventIdAssign.RequestPrivilegeInfoByPriGroupId, null,
        //                            info, waitId, 10, 12);
        //}


    }
}





