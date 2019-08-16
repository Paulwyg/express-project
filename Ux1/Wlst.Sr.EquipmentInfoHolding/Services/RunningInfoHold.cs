using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.ModuleServices;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.client;
using WriteLog = Wlst.Cr.Core.UtilityFunction.WriteLog;


namespace Wlst.Sr.EquipmentInfoHolding.Services
{
    public class RunningInfoHold : EventHandlerHelperExtendNotifyProperyChanged
    {
        public static ConcurrentDictionary<int, Model.RunInfo> Info = new ConcurrentDictionary<int, RunInfo>();

        private static RunningInfoHold _myself;

        protected RunningInfoHold()
        {
            // InitAction();
            //LoadXml();
            Wlst.Cr.Coreb .AsyncTask.Qtz.AddQtz("fff", 0, DateTime.Now.AddSeconds(10).Ticks, 3, Ac1);

            Wlst.Cr.Coreb.AsyncTask.Qtz.AddQtz("fff", 0, DateTime.Now.AddSeconds(10).Ticks, 2, Ac2);

        }

        private static ConcurrentQueue<long> NeedPushCtrl = new ConcurrentQueue<long>();
        private static ConcurrentQueue<int> NeedPushRtu = new ConcurrentQueue<int>();  

        public void Ac1(object obj)
        {
            if (NeedPushCtrl.Count > 0)
            {
                var lst = new List<long>();
                while (NeedPushCtrl.Count > 0)
                {
                    long s = 0;
                    if (NeedPushCtrl.TryDequeue(out s))
                    {
                        lst.Add(s);
                    }
                }

                //发布事件  
                var argsctrl = new PublishEventArgs()
                                   {
                                       EventType = PublishEventType.Core,
                                       EventId = EventIdAssign.RunningInfoUpdate3,
                                   };
                argsctrl.AddParams(lst );
                EventPublish.PublishEvent(argsctrl); 
            }
        }

        /// <summary>
        /// 每两秒 发布 终端状态变化。 lvf 2018年11月30日14:52:08
        /// </summary>
        /// <param name="obj"></param>
        public void Ac2(object obj)
        {
            if (NeedPushRtu.Count > 0)
            {
                var lst = new List<int>();
                while (NeedPushRtu.Count > 0)
                {
                    int s = 0;
                    if (NeedPushRtu.TryDequeue(out s))
                    {
                        lst.Add(s);
                    }
                }

                //发布事件  
                var argsrtu = new PublishEventArgs()
                {
                    EventType = PublishEventType.Core,
                    EventId = EventIdAssign.RunningInfoUpdate4,
                };
                argsrtu.AddParams(lst);
                EventPublish.PublishEvent(argsrtu);
            }
        }


        public static RunInfo GetRunInfo(int rtuId)
        {
            return Info.ContainsKey(rtuId) ? Info[rtuId] : null;
        }

        internal static void InitServices()
        {
            if (_myself == null) _myself = new RunningInfoHold();
            _myself.InitAction();
        }

        private void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxSys.wlst_sys_rtu_online,
                //.ClientPart.wlst_Measures_server_ans_clinet_request_RtuOnLine,
                OrderRtuOnLine,
                typeof (RunningInfoHold), this);
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxSlu.wst_svr_ans_slu_ctrl_measure, OnSluMeasure,
                typeof (RunningInfoHold), this);

            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxLeak.wst_leak_order_zcOrSet, OnLeakMeasure,
                typeof (RunningInfoHold), this);

            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_states, OrderRtuStatesOrder,
                typeof (RunningInfoHold), this);

            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxEqu.wst_update_equ, OnEquUp,
                typeof (RunningInfoHold), this);

            //ProtocolServer.RegistProtocol(
            //    Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_data, // .wlst_svr_ans_cnt_request_wj3090_measure ,
            //    OrderRtuMeasure,
            //    typeof (RunningInfoHold), this, true);
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_orders, // .wlst_svr_ans_cnt_request_wj3090_measure ,
                OrderRtuMeasureOrder,
                typeof (RunningInfoHold), this, true);
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxLdu.wst_svr_ans_ldu_orders,
                //.ClientPart.wlst_Wj1090_server_ans_clinet_order_Measure,
                OrderLduMeasure,
                typeof (RunningInfoHold), this);

            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(RequestRtuOnLine, 0,
                                                                      DelayEventHappen.EventOne);
            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(RequestEquipmentNewData, 3,
                                                                      DelayEventHappen.EventOne);


            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(RequestEquipmentSluNewData, 4,
                                                                      DelayEventHappen.EventOne);

            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(RequestEquipmentStates, 5,
                                                                      DelayEventHappen.EventOne);
        }


        private long countx = 0;
        public void OrderRtuOnLine(string session, Wlst.mobile.MsgWithMobile infos)
        {
            var rtusOnLine = infos.Args.Addr;
            if (rtusOnLine == null) return;
            countx++;

            var eventlst = new List<int>();
            //eventlst.AddRange(rtusOnLine);
            foreach (var t in Info)
            {
                if (!rtusOnLine.Contains(t.Key))
                {
                    if (t.Value.IsOnLine) eventlst.Add(t.Key);
                    t.Value.IsOnLine = false;

                }
                else
                {
                    if (t.Value.IsOnLine) continue;
                    t.Value.IsOnLine = true;
                    eventlst.Add(t.Key);
                }

            }
            foreach (var f in rtusOnLine)
            {
                if (Info.ContainsKey(f)) continue;
                Info.TryAdd(f, new RunInfo(f) { IsOnLine = true });
                eventlst.Add(f);
                //eventlst.Add(f);
            }

            if (countx % 100 == 0)
            {
                eventlst.Clear();
                eventlst.AddRange(rtusOnLine);
            }
            PublishEvent(eventlst, false);


            //if (eventlst.Count > 0)
            //{
            //    foreach (var f in eventlst)
            //    {
            //        NeedPushRtu.Enqueue(f);
            //    }
            //}
        }


        private void OnLeakMeasure(string sessionid, Wlst.mobile.MsgWithMobile info)
        {
            var data = info.WstLeakOrderZcOrSet;
            if (data == null) return;

            var lst = new List<int>();
            foreach (var t in data.Item)
            {
                if (t.Op != 11) continue;
                if (t.Items.Count == 0) continue;
                if (Info.ContainsKey(t.RtuId) == false) Info.TryAdd(t.RtuId, new RunInfo(t.RtuId) {IsOnLine = true});
                else Info[t.RtuId].IsOnLine = true;

                Info[t.RtuId].AddLeakNewData(t.Items[0]);
                lst.Add(t.RtuId);
            }
            if (lst.Count > 0) PublishEvent(lst, true);
        }

        /// <summary>
        /// 控制器状态 信息
        /// </summary>
        private static ConcurrentDictionary<long, int> DicConnstate = new ConcurrentDictionary<long, int>();

        public static int GetCtrlImageCode(long keys,int lampcount =1 )
        {
            
            if (DicConnstate.ContainsKey(keys)) return DicConnstate[keys];
            int sluid = (int)(keys/10000);
            int ctrlid = (int)(keys%10000);
            
            //lvf 2019年5月27日16:34:13   判断灯头

            if (Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(sluid))
            {
                var tmpp = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[sluid] as Wlst.Sr.EquipmentInfoHolding.Model.Wj2090Slu;
                if (tmpp != null && tmpp.WjSluCtrls.ContainsKey(ctrlid))
                {
                    lampcount = tmpp.WjSluCtrls[ctrlid].LightCount>1?2:1;
                }
            }
            


            // int errorIndex = 2090 * 1000 + lampnum * 100 + code;
            return 2090 * 1000 + lampcount * 100 + Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetCtrlErrorCode(sluid, ctrlid,lampcount);
        }

        /// <summary>
        /// 终端状态 信息
        /// </summary>
        private static ConcurrentDictionary<int, int> DicRtustate = new ConcurrentDictionary<int, int>();

        public static int GetTmlImageCode(int keys)
        {

            if (DicRtustate.ContainsKey(keys)) return DicRtustate[keys];

            return 3005;
        }

        private void OnSluMeasure(string sessionid, Wlst.mobile.MsgWithMobile info)
        {

            if (info == null || info.WstSluSvrAnsSluMeasure == null) return;
            if (info.WstSluSvrAnsSluMeasure.Type == 7)
            {
                var ctrlTmp = new CtrlIconInfo();
                var lstslus = new List<int>();
                foreach (var g in info.WstSluSvrAnsSluMeasure.InfoBaseic5)
                {
                    if (g.Info == null) continue;
                    if (lstslus.Contains(g.Info.SluId) == false) lstslus.Add(g.Info.SluId);
                    if (Info.ContainsKey(g.Info.SluId) == false) Info.TryAdd(g.Info.SluId, new RunInfo(g.Info.SluId));

                    Info[g.Info.SluId].AddSluCtrlNewData(g.Info.CtrlId, g);

                    ////计算控制器的状态
                    //var code = Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetCtrlErrorCode(g.Info.SluId, g.Info.CtrlId);
                    //int lampnum = g.Items.Count;
                    ////   if (g.LightCount > 1) lampnum = 2;
                    //int errorIndex = 2090 * 1000 + lampnum * 100 + code;
                    //long sluctrlid = g.Info.SluId * 10000L + g.Info.CtrlId;
                    //if (DicConnstate.ContainsKey(sluctrlid))
                    //{
                    //    if (DicConnstate[sluctrlid] != errorIndex)
                    //    {
                    //        DicConnstate[sluctrlid] = errorIndex;
                    //       // lstctrls.Add(sluctrlid);
                    //    }
                    //}
                    //else
                    //{
                    //    DicConnstate[sluctrlid] = errorIndex;
                    //   // lstctrls.Add(sluctrlid);
                    //}

                }
                //IsSlu7Back = true;
                //if(IsRtuStateBack )
                //{
                //    OnRtuNewdataOrSluCtrlNewdataArriveChangeMapIcon(0 ,  null);
                //}

                return;
            }

            int sluId = info.WstSluSvrAnsSluMeasure.SluId;
            if (sluId < 1) return;

            if (Info.ContainsKey(sluId) == false) Info.TryAdd(sluId, new RunInfo(sluId) {IsOnLine = true});
            else Info[sluId].IsOnLine = true;

            //var tu5 = new List<Tuple<int, int>>();
            var rtulst = new List<int>();
            rtulst.Add(sluId);

            foreach (var f in info.WstSluSvrAnsSluMeasure.InfoSlu0)
            {
                Info[sluId].AddSluNewData(f);
                //lst.Add(1);
            }
            if (info.WstSluSvrAnsSluMeasure.UnknowCtrls != null &&
                info.WstSluSvrAnsSluMeasure.UnknowCtrls.Count > 1)
            {
                Info[sluId].AddSluNewData(info.WstSluSvrAnsSluMeasure.UnknowCtrls);
                // lst.Add(2);
            }


            var lst = new List<int>();
            var lstctrls = new List<long>();

            foreach (var g in info.WstSluSvrAnsSluMeasure.InfoBaseic5)
            {
                //Info[sluId].RtuOcStates = 3;
                Info[sluId].AddSluCtrlNewData(g.Info.CtrlId, g);
                if (!lst.Contains(g.Info.CtrlId)) lst.Add(g.Info.CtrlId);

                //获取控制器灯头数量
                int  LightCount = 1;
                var sluinfo =
                    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(
                        sluId);
                if (sluinfo == null) return;
                var cons = sluinfo as Wlst.Sr.EquipmentInfoHolding.Model.Wj2090Slu;
                if (cons == null) return;
                foreach (var t in cons.WjSluCtrls)
                {
                    if (t.Value.CtrlPhyId == g.Info.CtrlId)
                    {

                        LightCount = t.Value.LightCount;

                        break;
                    }
                }


                //单灯数据比较新，或者相差30分钟以内，则根据单灯数据 lvf 2019年6月19日15:24:55
                if (g.Info.DateTimeCtrl - Info[sluId].RtuOcStatesChangedtime > 0
                    || Info[sluId].RtuOcStatesChangedtime - g.Info.DateTimeCtrl < (long)1800 * (long)10000000)
                {
                    //计算控制器的状态
                    var code = Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetCtrlErrorCode(g.Info.SluId, g.Info.CtrlId,
                                                                                            LightCount);
                    int lampnum = LightCount > 1 ? 2 : 1;
                    //   if (g.LightCount > 1) lampnum = 2;
                    int errorIndex = 2090 * 1000 + lampnum * 100 + code;
                    long sluctrlid = g.Info.SluId * 10000L + g.Info.CtrlId;
                    if (DicConnstate.ContainsKey(sluctrlid))
                    {
                        if (DicConnstate[sluctrlid] != errorIndex)
                        {
                            DicConnstate[sluctrlid] = errorIndex;
                            lstctrls.Add(sluctrlid);
                        }
                    }
                    else
                    {
                        DicConnstate.TryAdd(sluctrlid, errorIndex);
                        //DicConnstate[sluctrlid] = errorIndex;
                        lstctrls.Add(sluctrlid);
                    }
                }
            }


            //发布事件  
            var args = new PublishEventArgs()
                           {
                               EventType = PublishEventType.Core,
                               EventId = EventIdAssign.RunningInfoUpdate2,
                           };
            args.AddParams(rtulst);
            if (lst.Count > 0) args.AddParams(lst);
            EventPublish.PublishEvent(args);

            if (lstctrls.Count > 0)
            {
                //if (lstctrls.Count > 15)
                //{
                //    //发布事件  
                //    var argsctrl = new PublishEventArgs()
                //                       {
                //                           EventType = PublishEventType.Core,
                //                           EventId = EventIdAssign.RunningInfoUpdate3,
                //                       };
                //    argsctrl.AddParams(lstctrls);
                //    EventPublish.PublishEvent(argsctrl);
                //}
                //else 
                    //延迟发布事件
                {
                    foreach (var f in lstctrls )
                    {
                        NeedPushCtrl.Enqueue(f);
                    }
                }
            }

            //if (lst.Count > 0)
            //{
            //    Thread.Sleep(200);
            //    OnRtuNewdataOrSluCtrlNewdataArriveChangeMapIcon(sluId, lst);
            //}


        }

        //void OnRtuNewdataOrSluCtrlNewdataArriveChangeMapIcon(int sluid, List<int> ctrllst)
        //{
        //    if (sluid == 0 && ctrllst == null) // rtu state & slu measure7
        //    {
        //        var rtus = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems;
        //        foreach (var f in rtus)
        //        {
        //            if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsSlu(f.Key))
        //                OnRtuNewdataOrSluCtrlNewdataArriveChangeMapIconPri(f.Key);
        //        }

        //        return;
        //    }
        //    if (sluid > 0) //rtu state 
        //    {

        //        OnRtuNewdataOrSluCtrlNewdataArriveChangeMapIconPri(sluid);

        //    }
        //}

        //void OnRtuNewdataOrSluCtrlNewdataArriveChangeMapIconPri(int sluid)
        //{
        //    var ctrlTmp = new CtrlIconInfo();
        //    if (Info.ContainsKey(sluid) == false || Info[sluid].SluCtrlNewData == null)
        //    {
        //        return;
        //    }

        //    var lstUseRtu = new List<int>();
        //    var lstCtrl = new List<int>();
        //    if (Info[sluid].RtuOcStates != 3)
        //    {
        //        var tmplst = new List<int>();
        //        var runinfoslu1 = Info[sluid].SluCtrlNewData;
        //        var sluinfo =
        //       Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(sluid);
        //        var cons = sluinfo as Wlst.Sr.EquipmentInfoHolding.Model.Wj2090Slu;
        //        if (cons == null) return;
        //        foreach (var g in cons.WjSluCtrls.Values)   //所有控制器 默认跟终端
        //        {
        //            tmplst.Add(g.CtrlId);
        //        }

        //        foreach (var g in runinfoslu1)
        //        {
        //            //if (g.Value.Data5 == null || g.Value.Data5.Info == null || g.Value.Data5.Info.DateTimeCtrl == 0)
        //            //{
        //            //    tmplst.Add(g.Key);
        //            //} 
        //            //else
        //            //{
        //            if (g.Value.Data5.Info.DateTimeCtrl > Info[sluid].RtuOcStatesChangedtime)
        //            {

        //                //排除有最新数据的控制器  跟控制器自己状态走
        //                tmplst.Remove(g.Key);
        //                //tmplst.Add(g.Key);
        //                if (g.Value.Data5.Info == null || g.Value.Data5.Info.Status == 3 || g.Value.Data5.Info.DateTimeCtrl == 0)
        //                {
        //                    ctrlTmp.UnConnected = true;
        //                }
        //                //ctrlTmp = new CtrlIconInfo();
        //                if (UxTreeSetting.IsRutsNotShowError == false)
        //                {
        //                    var errs = Ioc.GetSluLampErrors(sluid, g.Key, 0);

        //                    //(from t in Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary
        //                    //            where
        //                    //                t.Value.IsThisUserShow && t.Value.RtuId == sluid && t.Value.LoopId == g.Key &&
        //                    //                t.Value.LampId > 0
        //                    //            select t.Value.FaultId - 54).ToList(); //55-功率越上限 56功率越下限  57灯具漏电 58光源故障  59补偿电容故障 60意外灭灯  61意外亮灯  62自熄灯 63 控制器断电告警  64 继电器故障
        //                    if (errs.Count > 0)
        //                    {
        //                        for (int i = 0; i < errs.Count; i++)
        //                        {
        //                            if (errs[i] == 4 || errs[i] == 6)  //光源故障 意外灭灯
        //                            {
        //                                if (errs[0] != 4 && errs[0] != 6)
        //                                {
        //                                    int tmp = errs[0];
        //                                    errs[0] = errs[i];
        //                                    errs[i] = tmp;
        //                                }

        //                            }
        //                        }
        //                    }

        //                    ctrlTmp.Errors = errs;
        //                }
        //                else
        //                {
        //                    ctrlTmp.Errors = new List<int>();
        //                }

        //                foreach (var f in g.Value.Data5.Items)
        //                {
        //                    ctrlTmp.IsIconUseRtuStateTo = false;
        //                    ctrlTmp.states = f.StateWorkingOn;      //todo
        //                    ctrlTmp.UnConnected = false;
        //                    //ctrlTmp.Errors = new List<int>();
        //                    //if (!ctrlTmp.Errors.Contains(f.Fault)) ctrlTmp.Errors.Add(f.Fault);
        //                }
        //                Info[sluid].AddCtrlNewState(g.Key, ctrlTmp);
        //            }

        //            //}
        //        }

        //        bool IsAuto = false;
        //        ConcurrentDictionary<int, bool> conIsAutoOpenWhenElec = new ConcurrentDictionary<int, bool>();
        //        //todo check f is  sdkd 
        //        foreach (var g in cons.WjSluCtrls.Values)
        //        {
        //            switch (g.LightCount)
        //            {
        //                case 1:
        //                    if (g.IsAutoOpenLightWhenElec1) IsAuto = true;
        //                    break;
        //                case 2:
        //                    if (g.IsAutoOpenLightWhenElec1 || g.IsAutoOpenLightWhenElec2) IsAuto = true;
        //                    break;
        //                case 3:
        //                    if (g.IsAutoOpenLightWhenElec1 || g.IsAutoOpenLightWhenElec2 || g.IsAutoOpenLightWhenElec3) IsAuto = true;
        //                    break;
        //                case 4:
        //                    if (g.IsAutoOpenLightWhenElec1 || g.IsAutoOpenLightWhenElec2 || g.IsAutoOpenLightWhenElec3 || g.IsAutoOpenLightWhenElec4) IsAuto = true;
        //                    break;
        //            }
        //            if (IsAuto)
        //            {
        //                conIsAutoOpenWhenElec.TryAdd(g.CtrlId, true);
        //            }

        //        }
        //        //todo shangdian kaideng lstUseRtu

        //        foreach (var f in tmplst)
        //        {
        //            if (!conIsAutoOpenWhenElec.ContainsKey(f)) continue;
        //            if (conIsAutoOpenWhenElec[f])
        //            {
        //                lstUseRtu.Add(f);
        //                lstCtrl.Add(f);
        //            }
        //        }

        //        foreach (var g in lstUseRtu)
        //        {
        //            ctrlTmp = new CtrlIconInfo();
        //            ctrlTmp.UnConnected = false;
        //            ctrlTmp.RtuState = Info[sluid].RtuOcStates;
        //            ctrlTmp.IsIconUseRtuStateTo = true;
        //            ctrlTmp.states = Info[sluid].RtuOcStates;
        //            ctrlTmp.Errors = new List<int>();
        //            Info[sluid].AddCtrlNewState(g, ctrlTmp);
        //            //if (Info[sluid].SluCtrlIconStates.ContainsKey(g))
        //            //{

        //            //    Info[sluid].SluCtrlIconStates[g] = ctrlTmp;
        //            //}
        //            //else
        //            //{
        //            //    Info[sluid].SluCtrlIconStates.TryAdd(g, ctrlTmp);
        //            //}
        //        }


        //    }

        //    foreach (var g in Info[sluid].SluCtrlNewData)
        //    {
        //        ctrlTmp = new CtrlIconInfo();
        //        if (lstUseRtu.Contains(g.Key)) continue;
        //        //todo icon use slu ctrl data 
        //        lstCtrl.Add(g.Key);
        //        if (g.Value.Data5 == null) continue;
        //        var data5 = g.Value.Data5;
        //        ctrlTmp.IsIconUseRtuStateTo = false;
        //        if (data5.Info == null || data5.Info.Status == 3 || data5.Info.DateTimeCtrl == 0)
        //        {
        //            ctrlTmp.UnConnected = true;
        //        }
        //        else if (g.Value.Data5.Info.DateTimeCtrl < Info[sluid].RtuOcStatesChangedtime)
        //        {
        //            if (!IsIconFollowTheRtu && Info[sluid].RtuOcStates == 2)
        //            {
        //                if (UxTreeSetting.IsRutsNotShowError == false)
        //                {
        //                    var errs = Ioc.GetSluLampErrors(sluid, g.Key, 0);
        //                    ctrlTmp.Errors = errs;
        //                }
        //                ctrlTmp.UnConnected = false;
        //                ctrlTmp.RtuState = Info[sluid].RtuOcStates;
        //                ctrlTmp.IsIconUseRtuStateTo = false;
        //                ctrlTmp.states = data5.Items[0].StateWorkingOn;

        //            }
        //            else
        //            {
        //                ctrlTmp.UnConnected = false;
        //                ctrlTmp.RtuState = Info[sluid].RtuOcStates;
        //                ctrlTmp.IsIconUseRtuStateTo = true;
        //                ctrlTmp.states = Info[sluid].RtuOcStates;
        //                ctrlTmp.Errors = new List<int>();
        //            }

        //            //if (UxTreeSetting.IsRutsNotShowError == false)
        //            //{
        //            //    var errs = (from t in Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary
        //            //                where
        //            //                    t.Value.IsThisUserShow && t.Value.RtuId == sluid && t.Value.LoopId == g.Key &&
        //            //                    t.Value.LampId > 0
        //            //                select t.Value.FaultId -54).ToList();
        //            //    ctrlTmp.Errors = errs;
        //            //}


        //        }
        //        else
        //        {
        //            if (UxTreeSetting.IsRutsNotShowError == false)
        //            {
        //                var errs = Ioc.GetSluLampErrors(sluid, g.Key, 0);
        //                //var errs = (from t in Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary
        //                //        where
        //                //            t.Value.IsThisUserShow && t.Value.RtuId == sluid && t.Value.LoopId == g.Key &&
        //                //            t.Value.LampId > 0
        //                //        select t.Value.FaultId - 54).ToList(); //55-功率越上限 56功率越下限  57灯具漏电 58光源故障  59补偿电容故障 60意外灭灯  61意外亮灯  62自熄灯 63 控制器断电告警  64 继电器故障
        //                ctrlTmp.Errors = errs;
        //            }
        //            else
        //            {
        //                ctrlTmp.Errors = new List<int>();
        //            }

        //            foreach (var f in data5.Items)
        //            {
        //                ctrlTmp.IsIconUseRtuStateTo = false;
        //                ctrlTmp.states = f.StateWorkingOn;
        //                ctrlTmp.UnConnected = false;
        //                //ctrlTmp.Errors = new List<int>();
        //                //if (!ctrlTmp.Errors.Contains(f.Fault)) ctrlTmp.Errors.Add(f.Fault);
        //            }
        //        }
        //        Info[sluid].AddCtrlNewState(g.Key, ctrlTmp);
        //        //if (Info[sluid].SluCtrlIconStates.ContainsKey(g.Key))
        //        //{
        //        //    Info[sluid].SluCtrlIconStates[g.Key] = ctrlTmp;
        //        //}
        //        //else
        //        //{
        //        //    Info[sluid].SluCtrlIconStates.TryAdd(g.Key, ctrlTmp);
        //        //}
        //    }
        //    var args = new Publish1EventArgs()
        //    {
        //        EventType = Publish1EventType.Core,
        //        EventId = EventIdAssign.MapNeedChangeIcon,
        //    };
        //    args.AddParams(sluid);
        //    args.AddParams(lstCtrl);
        //    EventPublish.Publish1Event(args);

        //}

        //////private bool IsRtuStateBack = false;
        //////private bool IsSlu7Back = false;
        //////private bool IsIconFollowTheRtu;
        public void OrderRtuStatesOrder(string session, Wlst.mobile.MsgWithMobile infos)
        {
            var rtuData = infos.WstRtuStates;
            if (rtuData == null) return;
            var lst = new List<Tuple<int, long, int>>();
            var slulstClose = new List<int>();
            var ctrlstClose = new List<int>();
            foreach (var f in rtuData.CloseRtus)
            {
                if (f == null) continue;
                lst.Add(new Tuple<int, long, int>(f.RtuId, f.ThatStateCreateTime, 2));
            }
            foreach (var f in rtuData.OpenRtus)
            {
                if (f == null) continue;
                lst.Add(new Tuple<int, long, int>(f.RtuId, f.ThatStateCreateTime, 1));
            }

            foreach (var fff in lst)
            {
                if (tmpdata.ContainsKey(fff.Item1) == false) UpdateDic(fff.Item1, 0);
                List<int> slus = tmpdata[fff.Item1]; // GetRtuWithSluBanding(fff.Item1);
                if (slus.Count > 0)
                    foreach (var sluid in slus)
                    {

                        if (sluid == 0) continue;
                        if (!Info.ContainsKey(sluid))
                            Info.TryAdd(sluid, new RunInfo(sluid));

                        if (Info[sluid].RtuOcStates != fff.Item3)
                        {
                            Info[sluid].RtuOcStates = fff.Item3;
                            Info[sluid].RtuOcStatesChangedtime = fff.Item2;
                        }
                        else
                        {
                            Info[sluid].RtuOcStatesChangedtime = fff.Item2;

                        }
                        if (Info[sluid].RtuOcStates == 2) //如果终端状态是全关时 清空控制器最新数据 
                        {
                            if (slulstClose.Contains(sluid) == false) slulstClose.Add(sluid);
                            foreach (var g in Info[sluid].SluCtrlNewData)
                            {
                                if (ctrlstClose.Contains(g.Value.CtrlId) == false) ctrlstClose.Add(g.Value.CtrlId);
                                //g.Value.Data5.Info.Status = 0;   //喻总说 要用关灯图标，所以用4

                            }
                            //
                        }


                    }

            }
            if (slulstClose.Count == 0) return;
            var args = new PublishEventArgs()
                           {
                               EventType = PublishEventType.Core,
                               EventId = EventIdAssign.RunningInfoUpdate2,
                           };
            args.AddParams(slulstClose);
            if (ctrlstClose.Count > 0) args.AddParams(ctrlstClose);
            EventPublish.PublishEvent(args);


        }





        private ConcurrentDictionary<int, List<int>> tmpdata = new ConcurrentDictionary<int, List<int>>();

        private void OnEquUp(string sessionid, Wlst.mobile.MsgWithMobile info)
        {
            var obj = info.WstEquUpdate;
            if (obj == null) return;
            foreach (var f in obj.EquLst)
            {
                if (f.Wj2090Slu == null || f.Wj2090Slu.RelatedRtuId < 100) continue;
                int sluid = f.RtuId;
                int bandingrtuid = f.Wj2090Slu.RelatedRtuId;
                UpdateDic(bandingrtuid, sluid);
            }

        }

        private void UpdateDic(int rtuid, int sluid)
        {
            if (tmpdata.ContainsKey(rtuid) == false) tmpdata.TryAdd(rtuid, new List<int>());
            if (sluid == 0)
            {
                var lst = GetRtuWithSluBanding(rtuid);
                tmpdata[rtuid] = lst;
            }
            else
            {
                var rtus = (from t in tmpdata where t.Value.Contains(sluid) select t.Key).ToList();
                foreach (var f in rtus)
                    if (tmpdata.ContainsKey(f) && tmpdata[f].Contains(sluid))
                        tmpdata[f].Remove(sluid);
                tmpdata[rtuid].Add(sluid);
            }
        }

        //int GetSluWithRtuBanding(int sluId)
        //{
        //    return sluId;
        //}
        private List<int> GetRtuWithSluBanding(int rtuId) //通过终端地址获取集中器地址
        {
            List<int> sluId = new List<int>();
            if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(rtuId))
                return new List<int>();
            var rtus = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems;
            var rtuInfo = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuId];
            foreach (var g in rtus)
            {
                var ps = g.Value as Wlst.Sr.EquipmentInfoHolding.Model.Wj2090Slu;
                if (ps == null) continue;
                if (ps.WjSlu.RelatedRtuId == rtuInfo.RtuPhyId)
                {
                    if (!sluId.Contains(ps.WjSlu.RtuId)) sluId.Add(ps.WjSlu.RtuId);
                }
            }
            return sluId;
        }


        public void OrderRtuMeasureOrder(string session, Wlst.mobile.MsgWithMobile infos)
        {

            var rtuData = infos.WstRtuOrders;
            if (rtuData == null) return;

            // lvf 2018年8月13日09:47:56   收到停运启运后,直接更新缓存.
            if(rtuData.Op == 6 ||rtuData.Op ==7)
            {
                if (rtuData.RtuIds.Count == 0) return;
                var rtuid = rtuData.RtuIds[0];
                if (EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(rtuid) == false) return;
                EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuid].RtuRealState = rtuData.Op == 6
                                                                                                        ? 1
                                                                                                        : 2;
                return;
            }
            // lvf 2018年8月24日11:16:52 更改3006设备型号 直接更新
            if (rtuData.Op == 81)
            {
                if (rtuData.RtuIds.Count == 0) return;
                var rtuid = rtuData.RtuIds[0];
                if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(rtuid) == false) return;
                if (rtuData.Date == "3006")
                {
                    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuid].RtuModel =
                        EnumRtuModel.Wj3006;
                }
                return;
            }



            if (rtuData.Op < 31 || rtuData.Op > 32) return;


            //读取全局屏蔽小电流大小 lvf 2018年9月5日13:09:29
            var GlobalAShield = 0.0;
            var tmp = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOption(3302, 3, "0", null);
            if (tmp != null) GlobalAShield = Convert.ToDouble(tmp.Trim());
      

            //如果型号发生变化 需要进一步修改 
            if (rtuData.Items == null) return;
            var lstrtus = new List<int>();
            foreach (var fff in rtuData.Items)
            {
                try
                {
                    // var fff = t.Clone() as TmlNewData;
                    if (fff == null) continue;
                    if (fff.LstNewLoopsData == null) continue;

                    var info = new RtuNewDataInfo(fff);

                    if (!Info.ContainsKey(fff.RtuId))
                        Info.TryAdd(fff.RtuId, new RunInfo(fff.RtuId));
                    if (rtuData.Items.Count == 1) Info[fff.RtuId].IsOnLine = true;


                    bool isLightHasElectric = false;
                    var data = Services.EquipmentDataInfoHold.GetInfoById(fff.RtuId);
                    if (data != null)
                    {

                        var amps = data as Wj3005Rtu;
                        if (amps != null && amps.WjLoops != null)
                        {
                            var dic = new Dictionary<int, double>();
                            //记录需要呈现的回路数据 lvf 2018年9月4日09:12:22
                            foreach (var f in amps.WjLoops)
                                if (f.Value.IsShieldLoop == 0 && f.Value.SwitchOutputId > 0)
                                {
                                    if (dic.ContainsKey(f.Value.LoopId) == false)
                                        dic.Add(f.Value.LoopId, f.Value.ShieldLittleA);
                                }
                           
                            //判断回路是否开灯 
                            foreach (var f in fff.LstNewLoopsData)
                            {
                                //if (dic.ContainsKey(f.LoopId) && f.A > dic[f.LoopId])
                                //{
                                //    isLightHasElectric = true;
                                //    break;
                                //}

                                //是否是屏蔽回路
                                if ( dic.ContainsKey(f.LoopId))
                                {
                                    //是否不存在终端独立的屏蔽小电流
                                    if (dic[f.LoopId] == 0)
                                    {
                                        if (f.A>GlobalAShield)
                                        {
                                            isLightHasElectric = true;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if ( f.A > dic[f.LoopId])
                                        {
                                            isLightHasElectric = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //电流  

                    Info[fff.RtuId].IsLightHasElectric = isLightHasElectric;


                    Info[fff.RtuId].AddRtuNewData(fff);

                    if (rtuData.Items.Count == 1)
                    {
                        Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                            fff.RtuId, info.RtuName, Wlst.Cr.CoreMims.ShowMsgInfo.OperatrType.ServerReply, "选测数据");
                    }

                    //if (fff.IsAllSwitchOpen != 3)
                    //{

                        if (tmpdata.ContainsKey(fff.RtuId) == false) UpdateDic(fff.RtuId, 0);
                        List<int> slus = new List<int>();
                        if (tmpdata.ContainsKey(fff.RtuId)) slus = tmpdata[fff.RtuId];

                        // List<int> slus = GetRtuWithSluBanding(fff.RtuId);
                        if (slus.Count > 0)
                            foreach (var sluid in slus)
                            {
                                if (sluid != 0)
                                {
                                    if (!Info.ContainsKey(sluid))
                                        Info.TryAdd(sluid, new RunInfo(sluid));

                                    if (Info[sluid].RtuOcStates != fff.IsAllSwitchOpen)
                                    {
                                        Info[sluid].RtuOcStates = fff.IsAllSwitchOpen;
                                        Info[sluid].RtuOcStatesChangedtime = fff.DateCreate;
                                    }
                                    else
                                    {
                                        Info[sluid].RtuOcStatesChangedtime = fff.DateCreate;

                                    }

                                // lvf  2019年5月27日16:22:37 终端选测 触发单灯变色

                                int lampcount = 1;
                                if (Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(sluid))
                                {
                                    var lstctrls = new List<long>();
                                    var tmpp = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[sluid] as Wlst.Sr.EquipmentInfoHolding.Model.Wj2090Slu;
                                    if (tmpp != null )
                                    {
                                        foreach (var g in tmpp.WjSluCtrls)
                                        {

                                            lampcount = g.Value.LightCount>1?2:1;

                                          
                                            //计算控制器的状态
                                            var Imagecode = Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetCtrlErrorCode(sluid, g.Value.CtrlId,
                                                                                                                        lampcount);
                                         
                                            int errorIndex = 2090 * 1000 + lampcount * 100 + Imagecode;
                                            long sluctrlid = sluid * 10000L + g.Value.CtrlId;
                                            if (DicConnstate.ContainsKey(sluctrlid))
                                            {
                                                //if (DicConnstate[sluctrlid] != errorIndex)
                                                //{
                                                    DicConnstate[sluctrlid] = errorIndex;
                                                    lstctrls.Add(sluctrlid);
                                                //}
                                            }
                                            else
                                            {
                                                DicConnstate.TryAdd(sluctrlid, errorIndex);
                                                //DicConnstate[sluctrlid] = errorIndex;
                                                lstctrls.Add(sluctrlid);
                                            }



                                        }
                                        if (lstctrls.Count > 0)
                                        {
                                            {
                                                foreach (var f in lstctrls)
                                                {
                                                    NeedPushCtrl.Enqueue(f);
                                                }
                                            }
                                        }
                                    }
                                }


                              



                                //OnRtuNewdataOrSluCtrlNewdataArriveChangeMapIcon(sluid, null);
                            }
                            }

                    //}

                        //计算终端的状态
                        var code = Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetImageIconByState(fff.RtuId);

                        if (DicRtustate.ContainsKey(fff.RtuId))
                        {
                            if (DicRtustate[fff.RtuId] != code)
                            {
                                DicRtustate[fff.RtuId] = code;
                                lstrtus.Add(fff.RtuId);
                            }
                        }
                        else
                        {
                            DicRtustate.TryAdd(fff.RtuId, code);
                            //DicRtustate[fff.RtuId] = code;
                            lstrtus.Add(fff.RtuId);
                        }



                }
                catch (Exception ex)
                {
                    WriteLog.WriteLogError("Add tml new data info error:" +
                                           ex.ToString());
                }


            }
            var rtus = (from t in rtuData.Items select t.RtuId).ToList();
            PublishEvent(rtus, true);

            //lvf 添加延迟发布
            if(lstrtus.Count>0)
            {
                foreach (var f in lstrtus)
                {
                    NeedPushRtu.Enqueue(f);
                }
            }

            //OnRtuNewdataOrSluCtrlNewdataArriveChangeMapIcon(0 ,null);

            //OnRtuNewdataOrSluCtrlNewdataArriveChangeMapIcon(rtus, null, null);
            //var args1 = new PublishEventArgs()
            //{
            //    EventType = PublishEventType.Core,
            //    EventId = EventIdAssign.MapNeedChangeIcon,
            //};
            //args1.AddParams(rtus);
            //args1.AddParams(new List<int>());
            //args1.AddParams(new List<int>());
            //EventPublish.Publish1Event(args1);

        }

        public void OrderLduMeasure(string session, Wlst.mobile.MsgWithMobile infos)
        {
            var tmlInfoExchangeforUpdatetmlinfo = infos.WstLduSvrAnsOrders;
            if (tmlInfoExchangeforUpdatetmlinfo.Op != 1) return;

            try
            {
                var lst = new List<int>();
                foreach (var fff in tmlInfoExchangeforUpdatetmlinfo.ItemsData)
                {
                    try
                    {

                        if (fff == null) continue;


                        if (Info.ContainsKey(fff.LduId) == false) Info.TryAdd(fff.LduId, new RunInfo(fff.LduId));
                        Info[fff.LduId].AddLduNewData(fff);

                        if (!lst.Contains(fff.LduId))
                            lst.Add(fff.LduId);

                        //Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                        //    fff.LduId, info.RtuName, Wlst.Cr.CoreMims.ShowMsgInfo.OperatrType.ServerReply, "集中器数据");
                    }
                    catch (Exception ex)
                    {
                        WriteLog.WriteLogError("Add tml new data info error:" +
                                               ex.ToString());
                    }
                }

                PublishEvent(lst, true);


                //var lstUpdate = new List<Tuple<int, bool>>();
                //foreach (var t in lst)
                //{
                //    var info = GetRtuAttachInfo(t);
                //    if (info == null || info.Count == 0) continue;
                //    bool error = false;
                //    foreach (var g in info)
                //    {
                //        //更新终端最新数据 回路 附加信息 中的防盗状态信息
                //        Wlst.Sr.EquipmentInfoHolding.Services.RtuNewDataService.UpdateAttachInfo(g.Item1, g.Item2,
                //                                                                                 g.Item3, g.Item4, g.Item5);
                //        if (g.Item4) error = true;
                //    }
                //    lstUpdate.Add(new Tuple<int, bool>(t, error));
                //    //  Wlst.Sr.EquipemntLightFault.Services.TmlErrorStates.UpdateEquipmentError(t, error);

                //}
                //if (lstUpdate.Count > 0)
                //{
                //    var ar = new PublishEventArgs()
                //    {
                //        EventId = Wlst.Sr.EquipemntLightFault.Services.EventIdAssign.RtuErrorStateChanged,
                //        EventType = PublishEventType.Core
                //    };
                //    ar.AddParams(lstUpdate);
                //    EventPublish.Publish1Event(ar);
                //}
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("Error to update tml New data ,ex:" + ex);
            }
        }

        public static void PublishEventForOut(List<int> rtus, bool isNewData)
        {
            _myself.PublishEvent(rtus, isNewData);
        }


        private void PublishEvent(List<int> rtus, bool isNewData)
        {
            //发布事件  
            var args = new PublishEventArgs()
                           {
                               EventType = PublishEventType.Core,
                               EventId = isNewData ? EventIdAssign.RunningInfoUpdate2 : EventIdAssign.RunningInfoUpdate1,
                           };
            args.AddParams(rtus);
            EventPublish.PublishEvent(args);
        }

        /// <summary>
        /// 与服务器交互数据 触发点
        /// </summary>
        private void RequestRtuOnLine()
        {

            var info = Wlst.Sr.ProtocolPhone.LxSys.wlst_sys_rtu_online;
            //.ServerPart.wlst_Measures_clinet_request_RtuOnLine;
            SndOrderServer.OrderSnd(info);
        }

        /// <summary>
        /// 与服务器交互数据 触发点
        /// </summary>
        private void RequestEquipmentNewData()
        {
            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_orders; //.wlst_cnt_request_wj3090_measure;
            //  info.Args.Addr.Add(-1);
            info.WstRtuOrders.Op = 32;
            info.WstRtuOrders.RtuIds.Add(0);
            // SndOrderServer.OrderSnd(info);

            var md5 = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.OrderSndHttp(info);
            if (md5 != null)
            {
                OrderRtuMeasureOrder(null, md5);
                //  step1Md5back(md5);
            }

        }


        /// <summary>
        /// 与服务器交互数据 触发点
        /// </summary>
        private void RequestEquipmentSluNewData()
        {
            var info = Wlst.Sr.ProtocolPhone.LxSlu.wst_slu_ctrl_measure; //.wlst_cnt_request_wj3090_measure;
            //  info.Args.Addr.Add(-1);
            info.WstSluMeasure.Type = 7;
            SndOrderServer.OrderSnd(info);
        }

        /// <summary>
        /// 与服务器交互数据 触发点
        /// </summary>
        private void RequestEquipmentStates()
        {
            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_states; //.wlst_cnt_request_wj3090_measure;
            SndOrderServer.OrderSnd(info);
        }

        //private void LoadXml()
        //{
        //    var infos = Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read("Wj2090SetConfg");
        //    if (infos.ContainsKey("IsIconFollowTheRtu"))
        //    {
        //        IsIconFollowTheRtu = infos["IsIconFollowTheRtu"].Contains("yes");
        //    }
        //    else IsIconFollowTheRtu = true;

        //}

        /// <summary>
        /// 获取控制器故障编号
        /// </summary>
        /// <param name="sluId"></param>
        /// <param name="ctrlid"></param>
        /// <param name="lampcount"></param>
        /// <returns></returns>
        public static int GetCtrlErrorCode(int sluId, int ctrlid,int lampcount =1)
        {

            try
            {
                var data = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(sluId);
                if (data == null) return 1;

                int errorindex = 0;
                if (data.SluCtrlNewData.ContainsKey(ctrlid) == false ||
                    data.SluCtrlNewData[ctrlid].Data5 == null ||
                    data.SluCtrlNewData[ctrlid].Data5.Info == null ||
                    data.SluCtrlNewData[ctrlid].Data5.Info.Status == 3 ||
                    data.SluCtrlNewData[ctrlid].Data5.Info.DateTimeCtrl < 10)
                {
                    //如果单灯没有数据  先判断绑定终端是否为全关  如果是 就采用关灯状态 lvf 2018年4月27日14:18:38
                    if (data.RtuOcStates == 2) return 1;
                    return 1;
                }
                var errs1 = Ioc.GetSluLampErrors(sluId, ctrlid, 1);
                var errs2 = Ioc.GetSluLampErrors(sluId, ctrlid, 2);
                bool hasError1 = false;
                bool hasError2 = false;
                bool hasError = false;
                if (UxTreeSetting.IsRutsNotShowError == false)
                {
                    hasError1 = errs1.Count > 0;
                    hasError2 = errs2.Count > 0;
                }
                if (lampcount > 1)
                {
                    if (hasError1 || hasError2) hasError = true; // 目前图标不区分 灯1 灯2故障
                }
                else
                {
                    if (hasError1) hasError = true;
                }



                var ctrldata = data.SluCtrlNewData[ctrlid].Data5;
                // 如果终端状态数据比较新  则判断终端是否是全关，如果是全关状态  就采用关灯状态 lvf 2018年10月31日15:21:09
                if (ctrldata.Info.DateCreate < data.RtuOcStatesChangedtime)
                {
                    if (data.RtuOcStates == 2) return 1;
                }

                bool isLight1 = false;
                bool isLight2 = false;
                bool isSavePower1 = false;
                bool isSavePower2 = false;

                if (ctrldata.Items != null)
                {
                    foreach (var l in ctrldata.Items)
                    {
                        if (l.LampId == 1)
                        {
                            if (l.Current > 0.01) isLight1 = true;
                            if (l.StateWorkingOn == 1 || l.StateWorkingOn == 2) isSavePower1 = true;
                        }
                        if (l.LampId == 2)
                        {
                            if (l.Current > 0.01) isLight2 = true;
                            if (l.StateWorkingOn == 1 || l.StateWorkingOn == 2) isSavePower2 = true;
                        }

                        //if (l.LampId == 1 && l.Current > 0.01) isLight1 = true;
                        //if (l.LampId == 2 && l.Current > 0.01) isLight2 = true;

                    }
                }


                if (hasError == false && isLight1 == false && isLight2 == false) errorindex = 1; //全关  无故障
                if (hasError && isLight1 == false && isLight2 == false) errorindex = 2; //全关  有故障
                if (hasError == false && isLight1 && isLight2 == false) errorindex = 3; //灯1亮  无故障
                if (hasError && isLight1 && isLight2 == false) errorindex = 4; //灯1亮  有故障
                if (hasError == false && isLight1 == false && isLight2) errorindex = 5; //灯2亮  无故障
                if (hasError && isLight1 == false && isLight2) errorindex = 6; //灯2亮  有故障
                if (hasError == false && isLight1 && isLight2) errorindex = 7; //全亮  无故障
                if (hasError && isLight1 && isLight2) errorindex = 8; //全亮  有故障
                if (isSavePower1 || isSavePower2)   //有节能，暂时不细化到灯头
                {
                    if (hasError == false)
                    {
                        errorindex = 13;
                    }
                    else
                    {
                        errorindex = 14;
                    }

                }

                //  errorindex = (int)DateTime.Now.Ticks % 7;
                return errorindex;
            }
            catch (Exception ex)
            {
                return 1;
            }
           

        }

        /// <summary>
        /// 获取终端故障编号
        /// </summary>
        /// <param name="rtuid"></param>
        /// <returns></returns>
        public static int GetImageIconByState(int rtuid)
        {

            var TerInfo = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(rtuid);
            if (TerInfo == null) return 0;
            var runninfo = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(rtuid);

            // int modelId = (int) TerInfo.EquipmentType;
            if (TerInfo.EquipmentType == WjParaBase.EquType.Rtu)
            {
                var s = TerInfo.RtuStateCode;
                if (s == 0)
                {
                    return 3001;

                }
                if (s == 1)
                {
                    return 3002;

                }

                var online = runninfo != null && runninfo.IsOnLine;
                if (online == false)
                {
                    return 3003;

                }
                var haserror = false;
                if (UxTreeSetting.IsRutsNotShowError == false)
                    haserror = runninfo.ErrorCount > 0;
                var lighton = runninfo.IsLightHasElectric; // RtuNewDataService.IsRtuHasElectric(this.NodeId);
                int errorindex = 0;
                // var ShieldAList = new Dictionary<int, Tuple<double, double>>();
                if (haserror && lighton) errorindex = 3;
                if (haserror && !lighton) errorindex = 1;
                if (!haserror && lighton)
                {
                    //foreach(var t in runninfo.RtuNewData.LstNewLoopsData   )
                    //{
                    //    ShieldAList.Add(t.LoopId, new Tuple<double, double>(t.A, t.ShieldLittleA));
                    //}
                    //foreach(var t in ShieldAList )
                    //{
                    //    int count = 0;
                    //    if (t.Value.Item1 < t.Value.Item2 || t.Value.Item1 == 0.0) count++;
                    //}

                    errorindex = 2;
                }
                if (!haserror && !lighton) errorindex = 0;

                return 3005 + errorindex;
            }
            else if (TerInfo.EquipmentType == WjParaBase.EquType.Slu)
            {
                if (TerInfo.RtuStateCode != 2)
                {
                    return (int)WjParaBase.EquType.Slu + 2;

                }
                var online = runninfo != null && runninfo.IsOnLine;
                if (online == false)
                {
                    return (int)WjParaBase.EquType.Slu + 3;

                }
                var haserror = false;
                if (UxTreeSetting.IsRutsNotShowError == false)
                    haserror = runninfo.ErrorCount > 0;
                if (haserror)
                {
                    return (int)WjParaBase.EquType.Slu + 1;
                }
                else
                {
                    return (int)WjParaBase.EquType.Slu;
                }

            }
            else
            {
                var tmp = runninfo != null && runninfo.ErrorCount > 0;
                ;
                return (int)TerInfo.EquipmentType + (tmp ? 1 : 0);
            }
        }

        //todo
        public static Dictionary<int, int> GetCtrlIcon(int sluId, List<int> ctrlList)
        {
            //var data = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(sluId);
            //if (data == null) return new Dictionary<int, int>();
            
            //var errors =
            //    Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultLstInfoByRtuId(sluid);

            Dictionary<int, int> namevalue =
    new Dictionary<int, int>();

            //lvf 2018年4月27日11:13:45  如果绑定终端为全关，则全部呈现关灯状态
            //if (Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(sluId) == false) return new Dictionary<int, int>();
            //var tmp = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[sluId] as Wj2090Slu;
            //if (tmp == null) return new Dictionary<int, int>();
            //var relateRtuId = tmp.WjSlu.RelatedRtuId;
            //var rtudata = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(relateRtuId);
            //if (rtudata == null) return new Dictionary<int, int>();
            //bool allcClose = rtudata.RtuOcStates == 3;

            // lvf 2018年10月31日15:26:56  注销
            //var data = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(sluId);
            //if (data == null) return new Dictionary<int, int>();
            //var allClose = data.RtuOcStates == 2;
            //if (allClose)
            //{
            //    foreach (var ctrlid in ctrlList)
            //    {

            //        if (!namevalue.ContainsKey(ctrlid))
            //        {
            //            namevalue.Add(ctrlid, 1);   //1 为关灯状态
            //        }
            //        else
            //        {
            //            namevalue[ctrlid] = 1;
            //        }

            //    }
            //    return namevalue;
            //}


            foreach (var ctrlid in ctrlList)
            {
                int errorindex = GetCtrlErrorCode(sluId ,ctrlid);

                
                if (!namevalue.ContainsKey(ctrlid))
                {
                    namevalue.Add(ctrlid, errorindex);
                }
                else
                {
                    namevalue[ctrlid] = errorindex;
                }
                
            }
            return namevalue;
        }
    }
}
