using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.EquipemntLightFault.Services;
using Wlst.client;
using WriteLog = Wlst.Cr.Core.UtilityFunction.WriteLog;

namespace Wlst.Sr.EquipemntLightFault.Services
{

     /// <summary>
    /// 故障类型信息 故障编码 故障类型信息；使用时 只全局发布 update事件  无参数
    /// </summary>
    public  class UserDisplayErrorSelfSetInfoHold
     {

         private static UserDisplayErrorSelfSetInfoHold _mySlef;
         public static UserDisplayErrorSelfSetInfoHold MySelf
         {
             get
             {
                 if (_mySlef == null) _mySlef = new UserDisplayErrorSelfSetInfoHold();
                 return _mySlef;
             }
         }

        protected UserDisplayErrorSelfSetInfoHold ()
        {
            
        }

         public static   Wlst.client.UserSelfDefineFalutAlarm.UserselfDefineFalutAlarmItem  GetInfoById(int id)
         {
             return MySelf.GetInfoByIdPri(id);
         }

         public static   List<Wlst.client.UserSelfDefineFalutAlarm.UserselfDefineFalutAlarmItem > GetInfoList()
         {
             return MySelf.GetInfoListPri();
         }

         protected Dictionary<int, Wlst.client.UserSelfDefineFalutAlarm.UserselfDefineFalutAlarmItem> Info =
             new Dictionary<int, UserSelfDefineFalutAlarm.UserselfDefineFalutAlarmItem>();

         public List<Wlst.client.UserSelfDefineFalutAlarm.UserselfDefineFalutAlarmGroupItem> InfoAlarmGroups =
             new List<UserSelfDefineFalutAlarm.UserselfDefineFalutAlarmGroupItem>();

            
            /// <summary>
        /// 获取该故障是否对本用户设置显示
        /// 根据设备地址获取设备信息；不存在返回null；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>不存在返回null</returns>
          Wlst.client.UserSelfDefineFalutAlarm.UserselfDefineFalutAlarmItem  GetInfoByIdPri(int id)
        {
            bool someSelected = Info.Any(t => t.Value.IsDisplay );

            if (someSelected == false || Info.Count == 0)
            {
                return new Wlst.client.UserSelfDefineFalutAlarm.UserselfDefineFalutAlarmItem() 
                {
                    AlarmTimes = 3,
                    FaultCode = id,
                    IsDisplay  = true
                };
            }

            if (Info.ContainsKey(id))
            {
                return Info[id];
            }
            return null;
        }

        /// <summary>
        /// <para>获取对本用户设置显示的所有故障列表</para>
        /// </summary>
          List<Wlst.client.UserSelfDefineFalutAlarm.UserselfDefineFalutAlarmItem > GetInfoListPri()
        {
         
                var lstReturn
                    = (from pair in Info where pair.Value.IsDisplay  orderby pair.Key select pair.Value).ToList();
                if (lstReturn.Count > 0) return lstReturn;
                return (from pair in Info orderby pair.Key select pair.Value).ToList();
            
        }


        /// <summary>
        /// 为外界提供服务
        /// </summary>
        public void InitStartService()
        {
            this.InitAction();
            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(RequestUserIndividuationFaultTypeInfo, 1);
        }

         public static bool IsShieldAlarmsThatUserOcLightCause = false;

        private  void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone .LxFault  .wlst_user_define_fault_alarms  ,//.ProtocolCnt.ClientPart.wlst_EquipemntLightFault_server_ans_clinet_request_UserIndividuationFault,
                RequestOrUpdateUserIndividuationFault,
                typeof(UserDisplayErrorSelfSetInfoHold), this);
        }

        private  void RequestOrUpdateUserIndividuationFault(string session,Wlst .mobile .MsgWithMobile  infos)
        {
            var tmlInfoExchangeforUpdatetmlinfo = infos.WstFaultUserDefineFaultAlarms;
            if (infos.WstFaultUserDefineFaultAlarms == null)
            {
                return;
            }

            //管理级别 集中修改
            if (string.IsNullOrEmpty(infos.WstFaultUserDefineFaultAlarms.RequestOrSetUserName) == false)
            {
                //和自己无关的 
                if (
                    Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.UserName.Equals(
                        infos.WstFaultUserDefineFaultAlarms.RequestOrSetUserName) == false)
                    return;
            }

            IsShieldAlarmsThatUserOcLightCause = infos.WstFaultUserDefineFaultAlarms.IsShieldAlarmsThatUserOcLightCause;

            try
            {
                Info.Clear();
                foreach (var t in tmlInfoExchangeforUpdatetmlinfo.Items  )
                {
                    try
                    {
                        if (Info.ContainsKey(t.FaultCode))
                        {
                            Info[t.FaultCode] = t;
                        }
                        else
                        {
                            Info.Add(t.FaultCode, t);
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLog.WriteLogError("Add tml falut type info error:" +
                                               ex.ToString());
                    }
                }

                InfoAlarmGroups.Clear();
                foreach (var f in tmlInfoExchangeforUpdatetmlinfo.ItemsAlarmGroup) InfoAlarmGroups.Add(f);

            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("Error to update tml falut type ,ex:" + ex);
            }

            var ar = new PublishEventArgs()
                         {
                             EventId = EventIdAssign.UserDisplayErrorSelfSetInfoUpdated,
                             EventType = PublishEventType.Core
                         };
            EventPublish.PublishEvent(ar);


            TmlExistFaultsInfoServices.OnUserInviOrTypeChange();
        }

        /// <summary>
        /// 与服务器交互数据 触发点
        /// </summary>
        protected void RequestUserIndividuationFaultTypeInfo()
        {

            var info = Wlst.Sr.ProtocolPhone.LxFault .wlst_user_define_fault_alarms ;
            info.WstFaultUserDefineFaultAlarms.Op = 1;
            SndOrderServer.OrderSnd(info, 10, 6);
            LogInfo.Log("登录信息同步中...");
        }



    }

    
}
