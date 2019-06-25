using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.ModuleServices;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Cr.PPProtocolSvrCnt.Common;
using Wlst.Sr.EquipemntLightFault.Model;
using Wlst.Sr.EquipemntLightFault.Services;
using Wlst.Sr.EquipmentInfoHolding.Model;
using WriteLog = Wlst.Cr.Core.UtilityFunction.WriteLog;


namespace Wlst.Sr.EquipemntLightFault.InfoHold
{

    /// <summary>
    /// 终端现场故障保存  结构为 终端地址 回路 故障编码
    /// </summary>
    public partial class TmlExistFaultsInfo : InfoDictionaryBase<FaultInfoBase>
    {
        internal ConcurrentDictionary<int, FaultInfoBase> FaultNotShow = new ConcurrentDictionary<int, FaultInfoBase>();

        /// <summary>
        /// 根据设备逻辑地址获取设备报警信息；不存在返回null
        /// </summary>
        /// <param name="rtuId">终端地址</param>
        /// <returns></returns>
        public List<FaultInfoBase> GetFaultLstInfoByRtuId(int rtuId)
        {
            return (from t in Info where t.Value.RtuId == rtuId && t.Value.IsThisUserShow select t.Value).ToList();
        }


        /// <summary>
        /// 注册启动事件
        /// </summary>
        public void InitStartSerive()
        {
            this.InitAction();
            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(RequestTmlExistFaultsInfo, 4,
                                                                      DelayEventHappen.EventOne);
            //每半小时  请求最新故障
            Wlst.Cr.Coreb.Servers.QtzLp.AddQtz("null", 8888, DateTime.Now.Ticks+1800,1800, RequestTmlExistFaultsInfos);
        }
        private void RequestTmlExistFaultsInfos(object obj)
        {
            var info = Wlst.Sr.ProtocolPhone.LxFault.wlst_fault_curr;
            info.WstFaultCurr.Op = 1;
            SndOrderServer.OrderSnd(info, 10, 6);

            //LogInfo.Log("正在请求终端所有故障信息!!!");
            LogInfo.Log("登录信息同步中... 请求故障");

            var info1 = Wlst.Sr.ProtocolPhone.LxSpe.wst_spe_special_rtus;//.wlst_fault_curr;
            info.Args.Cid = 1;
            SndOrderServer.OrderSnd(info1, 10, 6);

            //记录请求最新故障时间 lvf 2018年8月9日15:45:39
            Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.ReqExistFaultsTime = DateTime.Now;

        }

        private void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxFault.wlst_fault_curr,
                RequestOrUpdateTmlExistFaultsData,
                typeof (TmlExistFaultsInfo), this);

            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxSpe.wst_spe_special_rtus,
                RequestOrUpdateTmlExistFaultsData1,
                typeof (TmlExistFaultsInfo), this);
            //lvf 2018年6月28日13:26:35  处理火零不平衡
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxFault.wst_fault_curr_update,
                RequestTmlExistFaultsLnData,
                typeof (TmlExistFaultsInfo), this);
        }

        private void RequestTmlExistFaultsLnData(string session, Wlst.mobile.MsgWithMobile infos)
        {
            if(infos.WstFaultCurrUpdate.ItemsUpdate.Count ==0 ) return;
            var t = infos.WstFaultCurrUpdate.ItemsUpdate[0];
                            var ss =
                                (from tt in Info.Values
                                 where
                                     tt.FaultId == t.FaultId && tt.RtuId == t.RtuId && tt.LoopId == t.LoopId &&
                                     tt.LampId == t.LampId
                                 select tt).ToList();
                            if (ss.Count == 0) return;



            ss[0].V = t.Ahx;
            ss[0].A = t.Alx;
            ss[0].AUpper = t.Adiff;
            ss[0].Remark = infos.Head.Why;
        }



        public List<int> SpeRtus = new List<int>();

        private void RequestOrUpdateTmlExistFaultsData1(string session, Wlst.mobile.MsgWithMobile infos)
        {
            SpeRtus.Clear();
            SpeRtus.AddRange(infos.Args.Addr);
        }

 
        private int _index = 1;

        private void RequestOrUpdateTmlExistFaultsData(string session,Wlst .mobile .MsgWithMobile  infos)
        {

            //todo
            var tmlInfoExchangeforUpdatetmlinfo = infos.WstFaultCurr;
       
            if (tmlInfoExchangeforUpdatetmlinfo == null) return;

            ////清空缓存 如果op==1 lvf 2018年8月8日10:33:54
            //if( tmlInfoExchangeforUpdatetmlinfo.Op ==1)
            //{
            //    Info.Clear();
            //    FaultNotShow.Clear();
            //}


            //如果型号发生变化 需要进一步修改 
            if (tmlInfoExchangeforUpdatetmlinfo.FaultItemsAdd   == null ||
                tmlInfoExchangeforUpdatetmlinfo.FaultItemsDelete  == null) return;

            if (tmlInfoExchangeforUpdatetmlinfo.FaultItemsAdd .Count == 0 &&
                tmlInfoExchangeforUpdatetmlinfo.FaultItemsDelete .Count == 0) return;
            //   var lstUpdatesss = new List<int>();

            //    var lstUpdatRtus = new List<int>();

            var lstUpdateStates = new List<Tuple<int, bool>>();
            var lstaddindex = new List<int>();
            var lstdeleteindex = new List<int>();
            var rtus = new List<int>();

            try
            {
               
                foreach (var t in tmlInfoExchangeforUpdatetmlinfo.FaultItemsAdd )
                {
                    if (rtus.Contains(t.RtuId) == false) rtus.Add(t.RtuId);
                    try
                    {
                        while (Info.ContainsKey(_index) || FaultNotShow.ContainsKey( _index )) _index++;
                        //t.Id = _index;
                        var tmp = new FaultInfoBase(t,_index );
                        if (tmp.IsThisUserShow)
                        {
                            var ss =
                                (from tt in Info.Values
                                 where
                                     tt.FaultId == t.FaultId && tt.RtuId == t.RtuId && tt.LoopId == t.LoopId &&
                                     tt.LampId == t.LampId
                                 select tt).ToList();
                            if (ss.Count == 0)
                            {

                                //   var tmp = new Model.EquipmentFaultCnt(t);
                                Info.TryAdd( _index , tmp);
                                lstaddindex.Add(_index);
                                lstUpdateStates.Add(new Tuple<int, bool>(t.RtuId, true));
                                //if (!lstUpdatesss.Contains(t.RtuId))
                                //    lstUpdatesss.Add(t.RtuId);
                            }
                        }
                        else
                        {
                            var ss =
                                (from tt in FaultNotShow.Values
                                 where
                                     tt.FaultId == t.FaultId  && tt.RtuId == t.RtuId && tt.LoopId == t.LoopId &&
                                     tt.LampId == t.LampId
                                 select tt).ToList();
                            if (ss.Count == 0)
                            {
                                FaultNotShow.TryAdd( _index, tmp);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLog.WriteLogError("Add tml faults info error:" +
                                               ex.ToString());
                    }
                }
                foreach (var t in tmlInfoExchangeforUpdatetmlinfo.FaultItemsDelete )
                {
                    if (rtus.Contains(t.RtuId) == false) rtus.Add(t.RtuId);
                    try
                    {
                        var ss =
                            (from tt in Info.Values
                             where
                                 tt.FaultId == t.FaultId  && tt.RtuId == t.RtuId && tt.LoopId == t.LoopId &&
                                 tt.LampId == t.LampId
                             select tt).ToList();

                        if (ss.Count > 0)
                        {
                            FaultInfoBase outx;
                            //  if (!lstUpdatesss.Contains(ss[0].RtuId)) lstUpdatesss.Add(ss[0].RtuId);
                            Info.TryRemove(ss[0].Id,out outx );
                            lstdeleteindex.Add(ss[0].Id);
                            lstUpdateStates.Add(new Tuple<int, bool>(t.RtuId, false));
                            break;
                        }

                        var gga =
                            (from tt in FaultNotShow.Values
                             where
                                 tt.FaultId == t.FaultId  && tt.RtuId == t.RtuId && tt.LoopId == t.LoopId &&
                                 tt.LampId == t.LampId
                             select tt).ToList();

                        if (gga.Count > 0)
                        {
                            FaultInfoBase outx;
                            FaultNotShow.TryRemove(gga[0].Id,out outx );
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLog.WriteLogError("Add tml faults info error:" +
                                               ex.ToString());
                    }
                }

                //如果是全部请求 不发布添加事件
                if (lstaddindex.Count > 0 )
                {
                    var ar = new PublishEventArgs()
                                 {
                                     EventId = EventIdAssign.EquipmentExistFaultAddId,
                                     EventType = PublishEventType.Core
                                 };
                    ar.AddParams(lstaddindex);
                    EventPublish.PublishEvent(ar);
                }


                if (lstdeleteindex.Count > 0)
                {
                    var ar = new PublishEventArgs()
                                 {
                                     EventId = EventIdAssign.EquipementExistFaultDeleteId,
                                     EventType = PublishEventType.Core
                                 };
                    ar.AddParams(lstdeleteindex);
                    EventPublish.PublishEvent(ar);
                }


                //var arg = new PublishEventArgs()
                //              {
                //                  EventId = EventIdAssign.RtuErrorStateChanged,
                //                  EventType = PublishEventType.Core
                //              };
                //arg.AddParams(lstUpdateStates);
                //EventPublish.PublishEvent(arg);
                // EquipmentIsHasErrors.RtuErrorsChange(lstUpdatRtus);
                //TmlExistFaultsInfoServices.RtuErrorsChangeAttachShowError(
                //    (from t in lstUpdateStates select t.Item1).ToList());

                foreach (var l in rtus)
                {
                    var xcount = (from t in Info where t.Value.RtuId == l && t.Value.IsThisUserShow select t).Count();
                   if( Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.Info.ContainsKey(l)==false )
                   {
                       Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.Info.TryAdd(l,
                                                                                         new RunInfo(l)
                                                                                             {ErrorCount = xcount});
                   }
                    else
                   {
                       Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.Info[l].ErrorCount = xcount;
                   }
                }
                Sr.EquipmentInfoHolding.Services.RunningInfoHold.PublishEventForOut(rtus,false );

                //foreach (var l in  lstdelete )
                //{
                //    var xcount = (from t in Info where t.Value.RtuId == l && t.Value.IsThisUserShow select t).Count();
                //    if (Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.Info.ContainsKey(l) == false)
                //    {
                //        Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.Info.TryAdd(l,
                //                                                                          new RunInfo(l) { ErrorCount = xcount });
                //    }
                //    else
                //    {
                //        Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.Info[l].ErrorCount = xcount;
                //    }
                //}

            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("Error to update tml faults ,ex:" + ex);
            }

        }


        internal void OnTypeOrUserIndividuationChange()
        {
            foreach (var g in Info) g.Value.UpdatePriInfo() ;
            foreach (var g in FaultNotShow) g.Value.UpdatePriInfo() ;
            var lstAdd = (from t in FaultNotShow where t.Value.IsThisUserShow select t.Key).ToList();
            var lstDelte = (from t in Info where t.Value.IsThisUserShow == false select t.Key).ToList();
         //   var lstUpdatRtus = new List<int>();
            var lstUpdate = new List<int>();
            //var lstadd = new List<int>();
            //var lstdelete = new List<int>();

            foreach (var g in lstDelte)
            {
                if (!FaultNotShow.ContainsKey(g) && Info.ContainsKey(g))
                {
                    FaultNotShow.TryAdd( g, Info[g]);

                }
                if (Info.ContainsKey(g))
                {
                    lstUpdate.Add(Info[g].RtuId);

                    FaultInfoBase outx;
                    Info.TryRemove(g,out outx );  
                    
                }
            
            }

            foreach (var g in lstAdd)
            {
                if (FaultNotShow.ContainsKey(g) && !Info.ContainsKey(g))
                {
                    Info.TryAdd( g, FaultNotShow[g]);

                }
                if (FaultNotShow.ContainsKey(g))
                {
                    lstUpdate.Add(FaultNotShow[g].RtuId);

                    FaultInfoBase outx;
                    FaultNotShow.TryRemove( g,out outx );
                }
                lstUpdate.Add(g);
            }


            if (lstAdd.Count > 0)
            {
                var ar = new PublishEventArgs()
                {
                    EventId = EventIdAssign.EquipmentExistFaultAddId,
                    EventType = PublishEventType.Core
                };
                ar.AddParams(lstAdd);
                EventPublish.PublishEvent(ar);
            }


            if (lstDelte.Count > 0)
            {
                var ar = new PublishEventArgs()
                {
                    EventId = EventIdAssign.EquipementExistFaultDeleteId,
                    EventType = PublishEventType.Core
                };
                ar.AddParams(lstDelte);
                EventPublish.PublishEvent(ar);
            }

            //var arr = new PublishEventArgs()
            //{
            //    EventId = EventIdAssign.RtuErrorStateChanged,
            //    EventType = PublishEventType.Core
            //};
            //arr.AddParams(lstUpdate);
            //EventPublish.PublishEvent(arr);


            foreach (var l in lstUpdate)
            {
   
                var xcount = (from t in Info where t.Value.RtuId == l && t.Value.IsThisUserShow select t).Count();
                if (Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.Info.ContainsKey(l) == false)
                {
                    Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.Info.TryAdd(l,
                                                                                      new RunInfo(l) { ErrorCount = xcount });
                }
                else
                {
                    Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.Info[l].ErrorCount = xcount;
                }
            }
            Sr.EquipmentInfoHolding.Services.RunningInfoHold.PublishEventForOut(lstUpdate, false);

            //var lstsdfsd = (from t in lstUpdate select t.Item1).ToList();
            //TmlExistFaultsInfoServices.RtuErrorsChangeAttachShowError(lstsdfsd);

        }
    }

    /// <summary>
    /// 请求故障数据 向服务器
    /// </summary>
    public partial class TmlExistFaultsInfo
    {
        /// <summary>
        /// 与服务器交互数据 触发点
        /// </summary>
        private  void RequestTmlExistFaultsInfo()
        {

            var info = Wlst.Sr.ProtocolPhone.LxFault .wlst_fault_curr ;
            info.WstFaultCurr.Op = 1;
            SndOrderServer.OrderSnd(info, 10, 6);
            
            //LogInfo.Log("正在请求终端所有故障信息!!!");
            LogInfo.Log("登录信息同步中... 请求故障");

            var info1 = Wlst.Sr.ProtocolPhone.LxSpe.wst_spe_special_rtus;//.wlst_fault_curr;
            info.Args.Cid = 1;
            SndOrderServer.OrderSnd(info1, 10, 6);

            //记录请求最新故障时间 lvf 2018年8月9日15:45:39
            Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.ReqExistFaultsTime = DateTime.Now;
        }
    }
}
