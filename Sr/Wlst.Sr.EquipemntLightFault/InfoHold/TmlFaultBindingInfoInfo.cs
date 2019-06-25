using System;
using System.Collections.Generic;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.ModuleServices;
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
    /// 终端报警故障参数保存 终端地址 故障编码 是否报警
    /// </summary>
    public partial class TmlFaultBindingInfo : InfoDictionaryBase<List<int>>
    {

        /// <summary>
        /// 查询该终端对该故障是否报警
        /// </summary>
        /// <param name="rtuId">地址</param>
        /// <param name="faultCodeId">故障编码地址</param>
        /// <returns>不存在返回false</returns>
        public bool GetIsAlarmByIdAndFaultId(int rtuId, int faultCodeId)
        {

            if (Info.ContainsKey(rtuId))
            {
                if (Info[rtuId].Contains(faultCodeId)) return true;
            }
            else
            {
                if (Info.ContainsKey(0) && Info[0].Contains(faultCodeId)) return true;
            }
            return false;
        }

        /// <summary>
        /// 为外界提供服务
        /// </summary>
        public void InitStartService()
        {
            this.InitAction();
             Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(RequestFaultBandTmlInfo, 1,DelayEventHappen.EventOne );
        }
    }

    /// <summary>
    /// Event
    /// </summary>
    public partial class TmlFaultBindingInfo
    {


        private  void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone .LxFault  .wst_fault_rtu_banding  ,
                RequestTmlFaultBindingInfo,
                typeof(TmlFaultBindingInfo), this);
            //ProtocolServer.RegistProtocol(
            //    Wlst.Sr.ProtocolPhone .ClientListen .wlst_svr_ans_cnt_update_fault_banding_equipment ,
            //    UpdateTmlFaultBindingInfo,
            //    typeof(TmlFaultBindingInfo), this);
        }

       
   
 private  void RequestTmlFaultBindingInfo(string session, Wlst .mobile .MsgWithMobile info)
        {
            var tmlInfoExchangeforUpdatetmlinfo = info.WstFaultRtuBanding;
     bool update = tmlInfoExchangeforUpdatetmlinfo.Op == 2;


            if (tmlInfoExchangeforUpdatetmlinfo.RtuOrGrpFalutBandingItems   == null)
                return;
            try
            {
               
                    Info.Clear();
                

                foreach (var t in tmlInfoExchangeforUpdatetmlinfo.RtuOrGrpFalutBandingItems)
                {
                    try
                    {
                        if (!Info.ContainsKey(t.RtuOrGrpId )) Info.TryAdd( t.RtuOrGrpId , new List<int>());
                        if (!Info[t.RtuOrGrpId  ].Contains(t.FaultId )) Info[t.RtuOrGrpId  ].Add(t.FaultId );
                        
                    }
                    catch (Exception ex)
                    {
                        WriteLog.WriteLogError("Add tml Isalarm info error:" +
                                               ex.ToString());
                    }
                }

                if (!update)
                {
                    var ar = new PublishEventArgs()
                                 {
                                     EventId = EventIdAssign.FaultBandtoEquipmentRequest,
                                     EventType = PublishEventType.Core
                                 };
                    EventPublish.PublishEvent(ar);
                }
                else
                {
                    var ar = new PublishEventArgs()
                                 {
                                     EventId = EventIdAssign.FaultBandtoEquipmentUpdate,
                                     EventType = PublishEventType.Core
                                 };
                    EventPublish.PublishEvent(ar);
                }
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("Error to update tml Isalarm ,ex:" + ex);
            }
        }


    }

    /// <summary>
    /// 通信
    /// </summary>
    public partial class TmlFaultBindingInfo
    {

        /// <summary>
        /// 与服务器交互数据 触发点
        /// </summary>
        public void RequestFaultBandTmlInfo()
        {
            var info = Wlst.Sr.ProtocolPhone.LxFault.wst_fault_rtu_banding;
            info.WstFaultRtuBanding.Op = 1;
            SndOrderServer.OrderSnd(info, 10, 6);
            // LogInfo.Log("正在请求终端与故障类型绑定信息!!!");
            LogInfo.Log("登录信息同步中...");
        }


        /// <summary>
        /// 更新信息到服务器
        /// </summary>
        /// <param name="infoss">需要更新的信息</param>
        public void UpdateFaultBandTmlInfo(List<Tuple<int, int>> infoss,int areaId)
        {
            var info = Wlst.Sr.ProtocolPhone.LxFault.wst_fault_rtu_banding;
            info.WstFaultRtuBanding.Op = 2;

            foreach (var t in infoss)
            {
                info.WstFaultRtuBanding.RtuOrGrpFalutBandingItems.Add(new FaultRtuBanding.FaultRtuOrGrpBanding()
                                                                          {RtuOrGrpId = t.Item1, FaultId = t.Item2,AreaId =areaId });
            }

            SndOrderServer.OrderSnd(info, 50, 6);
            //   LogInfo.Log("正在更新终端与故障类型绑定信息!!!");
            LogInfo.Log("登录信息同步中...");
        }


    }
}
