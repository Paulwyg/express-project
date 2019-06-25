using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.EquipemntLightFault.Model;
using Wlst.Sr.EquipemntLightFault.Services;
using Wlst.client;
using WriteLog = Wlst.Cr.Core.UtilityFunction.WriteLog;


namespace Wlst.Sr.EquipemntLightFault.InfoHold
{

    /// <summary>
    /// 故障类型信息 故障编码 故障类型信息；使用时 只全局发布 update事件  无参数
    /// </summary>
    public partial class FaultTypeInfo : InfoDictionaryBase<Wlst.client.FaultTypes.FaultTypeItem >
    {



        private int timelong;

        /// <summary>
        /// 故障报警查询统计时间段
        /// </summary>
        public int TimeLongAlarm
        {
            get
            {
                if (timelong < 1) timelong = 1;
                if (timelong > 1440) timelong = 1440;
                return timelong;
            }
            set
            {
                if (value < 1) value = 1;
                if (value > 1440) value = 1440;
                timelong = value;

            }
        }


        /// <summary>
        /// 检测电压 认为电压为0的 最低条件
        /// </summary>
        public int VolBelow;

        /// <summary>
        /// 亮灯率报警下限值
        /// </summary>
        public double LdlBelow;

        /// <summary>
        /// 为外界提供故障类型信息服务
        /// </summary>
        public void InitStartService()
        {
            this.InitAction();
            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(RequestFaultTypeInfo, 1);


           EventPublish.AddEventTokener( 
                Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandler,
                FundOrderFilter);
        }
        /// <summary>
        /// 事件过滤
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private bool FundOrderFilter(PublishEventArgs args)
        {
            if (args.EventId == 100 && args.EventType == PublishEventType.ReCn)
                return true;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        private void FundEventHandler(PublishEventArgs args)
        {
            try
            {
                RequestFaultTypeInfo();
                //   Async.Run(new Action<object>(ExExecuteEvent), args);
            }
            catch (Exception ex)
            {
                Cr.Core.UtilityFunction.WriteLog.WriteLogError("Publish Evenet Error without Invock:" + ex);
            }
        }

        public override Wlst.client.FaultTypes .FaultTypeItem GetInfoById(int id)
        {
            return base.GetInfoById(id);
            //var sss = (from t in Info where t.Value.IsEnable select t.Value.FaultId).ToList();
            //if (sss.Count == 0)
            //{
            //    return base.GetInfoById(id);
            //}
            //else
            //{
            //    if (sss.Contains(id)) return base.GetInfoById(id);

            //}
            //return null;
        }
    }

    /// <summary>
    /// Event
    /// </summary>
    public partial class FaultTypeInfo
    {

        public    ConcurrentDictionary<int, client.FaultTypes.FaultSettingRuleOne> Ruls =
            new ConcurrentDictionary<int, FaultTypes.FaultSettingRuleOne>();
 
        private void InitAction()
        {
            //ProtocolServer.RegistProtocol(
            //    Wlst.Sr.ProtocolPhone .ClientListen .wlst_svr_ans_cnt_request_fault_type ,
            //    RequestOrUpdateFaultTypeInfo,
            //    typeof(FaultTypeInfo), this);
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone .LxFault  .wlst_fault_type ,
                RequestOrUpdateFaultTypeInfo,
                typeof(FaultTypeInfo), this);
        }

        private void RequestOrUpdateFaultTypeInfo(string session,Wlst .mobile .MsgWithMobile  infos)
        {
            var tmlInfoExchangeforUpdatetmlinfo = infos.WstFaultTypes;
   
            if (tmlInfoExchangeforUpdatetmlinfo == null) return;
            //如果型号发生变化 需要进一步修改 
            if (tmlInfoExchangeforUpdatetmlinfo.FalutItems  == null)
                return;

            TimeLongAlarm = tmlInfoExchangeforUpdatetmlinfo.HourLong;
            VolBelow = tmlInfoExchangeforUpdatetmlinfo.VolBelow;
            LdlBelow = tmlInfoExchangeforUpdatetmlinfo.LdlBelow;


            try
            {
                Info.Clear();
                foreach (var t in tmlInfoExchangeforUpdatetmlinfo.FalutItems)
                {
                    try
                    {
                        if (Info.ContainsKey(t.FaultId))
                        {
                            Info[t.FaultId] = t;
                        }
                        else
                        {
                            Info.TryAdd( t.FaultId, t);
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLog.WriteLogError("Add tml falut type info error:" +
                                               ex.ToString());
                    }
                }

                Ruls.Clear();
                foreach (var f in tmlInfoExchangeforUpdatetmlinfo .RuleItems)
                {
                    if (Ruls.ContainsKey(f.RuleId)) Ruls[f.RuleId] = f;
                    else Ruls.TryAdd(f.RuleId, f);
                }
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("Error to update tml falut type ,ex:" + ex);
            }

            var ar = new PublishEventArgs()
                         {
                             EventId = EventIdAssign.FaultTypeUpdateId,
                             EventType = PublishEventType.Core
                         };
            EventPublish.PublishEvent(ar);


           // EquipmentIsHasErrors.UserIndividuationOrBandingChange();

            TmlExistFaultsInfoServices.OnUserInviOrTypeChange();
        }
    }

    /// <summary>
    /// 通信
    /// </summary>
    public partial class FaultTypeInfo
    {
        /// <summary>
        /// 请求所有故障类型信息
        /// </summary>
        protected void RequestFaultTypeInfo()
        {
            var info = Wlst.Sr.ProtocolPhone.LxFault .wlst_fault_type ;
            info.WstFaultTypes.Op = 1;
            SndOrderServer.OrderSnd(info, 10, 6);
           // LogInfo.Log("正在请求故障类型信息!!!");
            LogInfo.Log("登录信息同步中...");
        }


        /// <summary>
        /// 更新故障类型 信息
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="hour">统计的时间 间隔 </param>
        /// <param name="volbelow">缺相电压下限 </param>
        /// <param name="ldlbelow">亮灯率报警下限 </param>
        public void UpdateFauleTypeInfo(List<Wlst.client.FaultTypes.FaultTypeItem> lst, int hour, int volbelow, double ldlbelow, List<client.FaultTypes.FaultSettingRuleOne> rules)
        {
            var info = Wlst.Sr.ProtocolPhone.LxFault.wlst_fault_type;
            info.WstFaultTypes.Op = 2;
            foreach (var t in lst)
            {
                info.WstFaultTypes.FalutItems.Add(t);
            }
            info.WstFaultTypes.HourLong = hour;
            info.WstFaultTypes.LdlBelow = ldlbelow;
            info.WstFaultTypes.VolBelow = volbelow;

            info.WstFaultTypes.RuleItems.AddRange(rules);

            SndOrderServer.OrderSnd(info, 10, 6);
            //  LogInfo.Log("正在更故障报警设置信息!!!");
        }
    }



}
