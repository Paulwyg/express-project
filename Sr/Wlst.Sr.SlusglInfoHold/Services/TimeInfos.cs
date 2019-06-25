using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.ModuleServices;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.client;

namespace Wlst.Sr.SlusglInfoHold.Services
{
    public partial class TimeInfos : EventHandlerHelperExtendNotifyProperyChanged
    {
        #region single instances
        private static TimeInfos _mySlef = null;
        private static object obj = 1;

        public static TimeInfos MySelf
        {
            get
            {
                if (_mySlef == null)
                {
                    lock (obj)
                    {
                        if (_mySlef == null)
                            new TimeInfos();
                    }

                }
                return _mySlef;
            }
        }
        #endregion

        public void OnInit()
        {
        }

        /// <summary>
        /// 存放单灯时间方案  areaid - plan id   -plan
        /// </summary>
        public ConcurrentDictionary<Tuple<int, int>, VSluTimeScheme.VSluTimeSchemeItem> Info = new ConcurrentDictionary<Tuple<int, int>, VSluTimeScheme.VSluTimeSchemeItem>();

        private TimeInfos()
        {
            if (_mySlef != null) return;

            _mySlef = this;
            this.InitAciton();
            this.InitEvent();
            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(RequestTimeInfofromSvr, 5,
                                                                      DelayEventHappen.EventSvAc);
        }

        private void RequestTimeInfofromSvr()
        {
            var info = Wlst.Sr.ProtocolPhone.LxSluSgl.wst_slusgl_time_scheme;// .wlst_cnt_wj2090_request_time;//.ServerPart.wlst_Wj2090_clinet_request_time_info_all;
            info.WstVsluTimeSchemeInfo.Op = 1;
            SndOrderServer.OrderSnd(info, 10, 3);
        }


        /// <summary>
        /// 获取集中器绑定的方案地址列表
        /// </summary>
        /// <param name="sluId"></param>
        /// <returns></returns>
        public List<Tuple<int, int>> GetSluBandingSchemeToday(int sluId)
        {
            int dayofweek = (int)DateTime.Now.DayOfWeek;
            List<Tuple<int, int>> rtn = new List<Tuple<int, int>>();//var rtn = new List<int>();
            foreach (var g in Info)
            {
                //if (g.Value.IsNotUsed) continue;
                if (!g.Value.SluTimePlanInfo.OperationWeekSet.Contains(dayofweek)) continue;
                foreach (var f in g.Value.SluCtrls)
                {
                    if (f.VSluId == sluId)
                    {
                        rtn.Add(g.Key);
                        break;
                    }
                }
            }
            return rtn;
        }


        /// <summary>
        /// 获取集中器绑定的方案地址列表
        /// </summary>
        /// <param name="sluId"></param>
        /// <param name="ctrlPhyId"> </param>
        /// <returns></returns>
        public List<Tuple<int, int>> GetSluBandingSchemeToday(int sluId, int ctrlPhyId)
        {
            List<Tuple<int, int>> rtn = new List<Tuple<int, int>>();//var rtn = new  List<int>();
            int dayofweek = (int)DateTime.Now.DayOfWeek;
            foreach (var g in Info)
            {
                //if ( g.Value.IsNotUsed) continue;
                if (!g.Value.SluTimePlanInfo.OperationWeekSet.Contains(dayofweek)) continue;
                foreach (var f in g.Value.SluCtrls)
                {
                    if (f.VSluId == sluId)
                    {
                        bool has = false;
                        if (f.OperatorType == 10) has = true;
                        else if (f.OperatorType == 21)
                        {
                            if (ctrlPhyId % 2 == 1) has = true;
                        }
                        else if (f.OperatorType == 20)
                        {
                            if (ctrlPhyId % 2 == 0) has = true;
                        }
                        else if (f.OperatorType == 31)
                        {
                            if (ctrlPhyId % 3 == 1) has = true;
                        }
                        else if (f.OperatorType == 41)
                        {
                            if (ctrlPhyId % 4 == 1) has = true;
                        }
                        //else if (f.OperatorType == 4)
                        //{
                        //    var info = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(sluId);
                        //    var para = info as Wlst.Sr.EquipmentInfoHolding.Model.Wj2090Slu;
                        //    if (para != null)
                        //    {


                        //        var lst = new List<int>();


                        //        foreach (var gx in para.WjSluCtrlGrps.Values)
                        //        {

                        //            lst.AddRange(gx.CtrlPhyLst);
                        //        }

                        //        if (lst.Contains(ctrlPhyId)) has = true;
                        //    }
                        //}
                        var tu = new Tuple<int, int>(g.Key.Item1, g.Key.Item2);   //lvf
                        if (has) rtn.Add(tu);

                        break;
                    }
                }
            }
            return rtn;
        }

        /// <summary>
        /// 获取详细信息
        /// </summary>
        /// <param name="timeids">时间表地址列表</param>
        /// <param name="sluId">集中器地址</param>
        /// <param name="isCtrlInfo">是否为显示控制器的时间信息</param>
        /// <returns>时控-18:10-开关灯控制灯头-控制器列表</returns>
        public List<Tuple<string, string, string, string>> GetSluBandingSchemeDetailToday(List<Tuple<int, int>> timeids, int sluId, bool isCtrlInfo)
        {
            var infortn = new List<Tuple<string, string, string, string>>();
            foreach (var g in timeids)
            {
                if (!Info.ContainsKey(g)) continue;
                var f = Info[g];

                var strMethod = "";
                var strTime = "";
                if (f.SluTimePlanInfo.OperationMethod == 1)
                {
                    strMethod = "定时";
                    strTime = (f.SluTimePlanInfo.OperationArgu / 60).ToString("D2") + ":" + (f.SluTimePlanInfo.OperationArgu % 60).ToString("D2");
                }
                else if (f.SluTimePlanInfo.OperationMethod == 2)
                {
                    strMethod = "日出日落";
                    strTime = "偏移:" + f.SluTimePlanInfo.OperationArgu;
                }
                else if (f.SluTimePlanInfo.OperationMethod == 11)
                {
                    strMethod = "光控";
                    strTime = (f.SluTimePlanInfo.LightStartEffect / 60).ToString("D2") + ":" + (f.SluTimePlanInfo.LightStartEffect % 60).ToString("D2") +
                              "-" +
                              (f.SluTimePlanInfo.LightEndEffect / 60).ToString("D2") + ":" + (f.SluTimePlanInfo.LightEndEffect % 60).ToString("D2") +
                              " 操作值:" +
                              (f.SluTimePlanInfo.OperationArgu / 10000) + "-" + (f.SluTimePlanInfo.OperationArgu % 10000);
                }
                else continue;


                var strMixInfo = "";
                if (f.SluTimePlanInfo.CmdType == 4)
                {
                    var strInfo = "";
                    bool isthesame = true;
                    int samevalue = 0;
                    string dengtou = "灯";

                    int index = 0;
                    foreach (var ff in f.SluTimePlanInfo.CmdMix)
                    {
                        index++;
                        if (ff == 0) continue;
                        strInfo += "灯" + index + ":" + GetMixName(ff) + ";";
                        dengtou += "" + index + ",";
                        if (samevalue == 0)
                        {
                            samevalue = ff;
                            continue;
                        }
                        if (samevalue != ff)
                        {
                            isthesame = false;
                        }
                    }
                    if (isthesame)
                    {
                        strInfo = GetMixName(samevalue) + ":" + dengtou;
                    }
                    strMixInfo = "开关灯控制:" + strInfo;
                }
                else if (f.SluTimePlanInfo.CmdType == 5)
                {
                    var straaa = "";
                    if (f.SluTimePlanInfo.CmdPwmScaleValue == 0) straaa = "0%";
                    else straaa = f.SluTimePlanInfo.CmdPwmScaleValue + "%";
                    strMixInfo = "调光" + straaa + ": 灯:";
                    foreach (var t in f.SluTimePlanInfo.CmdPwmScale)
                    {
                        strMixInfo += t + ",";
                    }
                }
                else continue;


                var ctrlsInfo = "";
                if (isCtrlInfo == false)
                {
                    foreach (var t in f.SluCtrls)
                    {
                        if (t.VSluId != sluId) continue;
                        if (t.OperatorType == 10) ctrlsInfo = "全部";
                        else if (t.OperatorType == 21)
                        {
                            ctrlsInfo = "单数";
                        }
                        else if (t.OperatorType == 20)
                        {
                            ctrlsInfo = "双数";
                        }
                        else if (t.OperatorType == 31)
                        {
                            ctrlsInfo = "隔二亮一";
                        }
                        else if (t.OperatorType == 41)
                        {
                            ctrlsInfo = "隔三亮一";
                        }
                        else if (t.OperatorType == 4)
                        {
                            ctrlsInfo = "自定义:";
                            int index = 1;
                            foreach (var fff in t.CtrlOrGrp)
                            {
                                index++;
                                if (index > 5) break;
                                ctrlsInfo += fff + "-";
                            }
                            ctrlsInfo += ".......";
                        }
                        break;
                    }
                }
                infortn.Add(new Tuple<string, string, string, string>(strMethod, strTime, strMixInfo, ctrlsInfo));
            }
            return infortn;
        }

        private string GetMixName(int x)
        {
            if (x == 1)
                return "开灯"
                    ;
            if (x == 2) return "节能";
            if (x == 3) return "节能";
            if (x == 4) return "关灯";
            return "不操作";

        }
    }


    public partial class TimeInfos
    {
        private void InitAciton()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxSluSgl.wst_slusgl_time_scheme,// .wlst_svr_ans_cnt_wj2090_request_time,//.ClientPart.wlst_Wj2090_svr_ans_clinet_request_time_info_all, 
                OnTimeInfoRequestOrUpdate,
                typeof(TimeInfos), this);
        }

        private void InitEvent()
        {
            this.AddEventFilterInfo(100, PublishEventType.ReCn);

        }
        /// <summary>
        /// 事件数据处理
        /// </summary>
        /// <param name="args"></param>
        public override void ExPublishedEvent(PublishEventArgs args)
        {
            if (args.EventType == PublishEventType.ReCn)
            {
                this.RequestTimeInfofromSvr();
                return;
            }
        }

        private void OnTimeInfoRequestOrUpdate(string sessionid, Wlst.mobile.MsgWithMobile info)
        {
            if (info == null || info.WstVsluTimeSchemeInfo == null) return;
            if (info.WstVsluTimeSchemeInfo.Op == 1)
            {
                Info.Clear();
            }
            else if (info.WstVsluTimeSchemeInfo.Op == 2 || info.WstVsluTimeSchemeInfo.Op == 3)
            {
                if (info.WstVsluTimeSchemeInfo.Items.Count > 0)
                {
                    //int areaId = info.WstVsluTimeSchemeInfo.Items[0].AreaId;
                    //var keys = (from t in Info where t.Key.Item1 == areaId select t.Key).ToList();
                    foreach (var f in info.WstVsluTimeSchemeInfo.Items)
                    {
                        var tu = new Tuple<int, int>(f.AreaId, f.SchemeId);
                        if (Info.ContainsKey(tu))
                        {
                            VSluTimeScheme.VSluTimeSchemeItem vl;
                            Info.TryRemove(tu, out vl);
                        }
                    }
                }
            }
            else if (info.WstVsluTimeSchemeInfo.Op == 4)
            {
                foreach (var t in Info)
                {
                    if (t.Key.Item2 == info.WstVsluTimeSchemeInfo.Argu)
                    {
                        VSluTimeScheme.VSluTimeSchemeItem vl;
                        Info.TryRemove(t.Key, out vl);
                    }
                }
            }
            foreach (var g in info.WstVsluTimeSchemeInfo.Items)
            {
                var tu = new Tuple<int, int>(g.AreaId, g.SchemeId);
                if (Info.ContainsKey(tu)) Info[tu] = g;
                else Info.TryAdd(tu, g);
            }
            if (info.WstVsluTimeSchemeInfo.Op == 1)
            {
                var ar = new PublishEventArgs()
                             {
                                 EventId =Services.EventIdAssign.SluSglTimeInfoRequest,                                   
                                 EventType = PublishEventType.Core
                             };
                EventPublish.PublishEvent(ar);
            }
            if (info.WstVsluTimeSchemeInfo.Op == 2 || info.WstVsluTimeSchemeInfo.Op == 3)
            {
                var ar = new PublishEventArgs()
                {
                    EventId = Services.EventIdAssign.SluSglTimeInfoUpdate,
                    EventType = PublishEventType.Core
                };
                EventPublish.PublishEvent(ar);
            }
            if (info.WstVsluTimeSchemeInfo.Op == 4)
            {
                var ar = new PublishEventArgs()
                {
                    EventId = Services.EventIdAssign.SluSglTimeInfoDelete,
                    EventType = PublishEventType.Core
                };
                EventPublish.PublishEvent(ar);
            }
        }



    }
}
