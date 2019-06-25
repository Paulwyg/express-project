//using System;
//using System.Collections.Generic;
//using System.Linq;
//
//
//using Wlst.Cr.Core.EventHandlerHelper;
//using Wlst.Cr.Core.UtilityFunction;
//using Wlst.Cr.CoreMims.Services;
//using Wlst.Cr.CoreOne.Services;
//using Wlst.Sr.EquipemntLightFault.Model;
//using Wlst.Sr.EquipemntLightFault.Services;

//namespace Wlst.Sr.EquipemntLightFault.InfoHold
//{
//    /// <summary>
//    /// 故障类型信息 故障编码 故障类型信息；使用时 只全局发布 update事件  无参数
//    /// </summary>
//    public partial class UserIndividuationFaultInfo : InfoDictionaryBase<Wlst.client.UserSelfDefineFalutAlarm .UserselfDefineFalutAlarmItem >
//    {


//        /// <summary>
//        /// 获取该故障是否对本用户设置显示
//        /// 根据设备地址获取设备信息；不存在返回null；
//        /// </summary>
//        /// <param name="id"></param>
//        /// <returns>不存在返回null</returns>
//        public override Wlst.client.UserSelfDefineFalutAlarm.UserselfDefineFalutAlarmItem  GetInfoById(int id)
//        {
//            bool someSelected = Info.Any(t => t.Value.IsDisplay );

//            if (someSelected == false || Info.Count == 0)
//            {
//                return new Wlst.client.UserSelfDefineFalutAlarm.UserselfDefineFalutAlarmItem() 
//                {
//                    AlarmTimes = 3,
//                    FaultCode = id,
//                    IsDisplay  = true
//                };
//            }

//            if (Info.ContainsKey(id))
//            {
//                return Info[id];
//            }
//            return null;
//        }

//        /// <summary>
//        /// <para>获取对本用户设置显示的所有故障列表</para>
//        /// </summary>
//        public override List<Wlst.client.UserSelfDefineFalutAlarm.UserselfDefineFalutAlarmItem > GetInfoList
//        {
//            get
//            {
//                var lstReturn
//                    = (from pair in Info where pair.Value.IsDisplay  orderby pair.Key select pair.Value).ToList();
//                if (lstReturn.Count > 0) return lstReturn;
//                return (from pair in Info orderby pair.Key select pair.Value).ToList();
//            }
//        }



//        /// <summary>
//        /// 为外界提供服务
//        /// </summary>
//        public void InitStartService()
//        {
//            this.InitAction();
//            //RequestUserIndividuationFaultTypeInfo();
//            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(RequestUserIndividuationFaultTypeInfo, 1);
//        }

//    }


//    /// <summary>
//    /// Event
//    /// </summary>
//    public partial class UserIndividuationFaultInfo
//    {

//        private  void InitAction()
//        {
//            ProtocolServer.RegistProtocol(
//                Wlst.Sr.ProtocolPhone .LxFault  .wlst_user_define_fault_alarms  ,//.ProtocolCnt.ClientPart.wlst_EquipemntLightFault_server_ans_clinet_request_UserIndividuationFault,
//                RequestOrUpdateUserIndividuationFault,
//                typeof(UserIndividuationFaultInfo), this);
//            //ProtocolServer.RegistProtocol(
//            //    Wlst.Sr.ProtocolPhone .ClientListen .wlst_svr_ans_cnt_update_user_define_alarms ,
//            //    RequestOrUpdateUserIndividuationFault,
//            //    typeof(UserIndividuationFaultInfo), this);
//        }

//        private  void RequestOrUpdateUserIndividuationFault(string session,Wlst .mobile .MsgWithMobile  infos)
//        {
//            var tmlInfoExchangeforUpdatetmlinfo = infos.WstUserDefineFaultAlarms;
     
       
//            try
//            {
//                Info.Clear();
//                foreach (var t in tmlInfoExchangeforUpdatetmlinfo.Items  )
//                {
//                    try
//                    {
//                        if (Info.ContainsKey(t.FaultCode))
//                        {
//                            Info[t.FaultCode] = t;
//                        }
//                        else
//                        {
//                            Info.Add(t.FaultCode, t);
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        WriteLog.WriteLogError("Add tml falut type info error:" +
//                                               ex.ToString());
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                WriteLog.WriteLogError("Error to update tml falut type ,ex:" + ex);
//            }

//            var ar = new PublishEventArgs()
//                         {
//                             EventId = EventIdAssign.UserIndividuationFaultUpdateId,
//                             EventType = PublishEventType.Core
//                         };
//            EventPublish.PublishEvent(ar);


//          //  EquipmentIsHasErrors.UserIndividuationOrBandingChange();

//            TmlExistFaultsInfoServices.OnUserInviOrTypeChange();
//        }
//    }

//    /// <summary>
//    /// 通信
//    /// </summary>
//    public partial class UserIndividuationFaultInfo
//    {
//        /// <summary>
//        /// 与服务器交互数据 触发点
//        /// </summary>
//        protected void RequestUserIndividuationFaultTypeInfo()
//        {

//            var info = Wlst.Sr.ProtocolPhone.LxFault .wlst_user_define_fault_alarms ;
//            info.WstUserDefineFaultAlarms.Op  = 1;
//            SndOrderServer.OrderSnd(info, 10, 6);
//          //  LogInfo.Log("正在请求用户个性化故障报警信息!!!");
//            LogInfo.Log("登录信息同步中...");
//        }


//        /// <summary>
//        /// 更新信息
//        /// </summary>
//        /// <param name="lst">用户自定义信息</param>
//        /// <param name="username">修改的用户名称</param>
//        public void UpdateUserIndividuationFauleTypeInfo(List<Wlst .client .UserSelfDefineFalutAlarm .UserselfDefineFalutAlarmItem   > lst,string username)
//        {
//            var info = Wlst.Sr.ProtocolPhone.LxFault .wlst_user_define_fault_alarms ;
//            info.WstUserDefineFaultAlarms.Op  = 2;
//            foreach (var t in lst)
//            {
//                info.WstUserDefineFaultAlarms.Items .Add(t);
//            }
//            SndOrderServer.OrderSnd(info, 10, 6);

//          //  LogInfo.Log("正在更新用户故障报警个性化设置信息!!!");
//            LogInfo.Log("登录信息同步中...");
//        }
//    }
//}
