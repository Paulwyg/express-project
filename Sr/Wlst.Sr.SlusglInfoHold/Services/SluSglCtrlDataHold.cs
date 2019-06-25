using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.ModuleServices;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.client;

namespace Wlst.Sr.SlusglInfoHold.Services
{
    public partial class SluSglCtrlDataHold
    {
        #region single instance

        private static SluSglCtrlDataHold _mySlef = null;
        private static object _obj = 1;

        public static SluSglCtrlDataHold MySlef
        {
            get
            {
                if (_mySlef == null)
                {
                    lock (_obj)
                    {
                        if (_mySlef == null) _mySlef = new SluSglCtrlDataHold();
                    }
                }
                return _mySlef;
            }
        }

        public SluSglCtrlDataHold(int rtuId)
        {
            this.RtuId = rtuId;

        }
        public int RtuId;

        protected SluSglCtrlDataHold()
        {
            this.InitAciotn();
            Wlst.Cr.Coreb.AsyncTask.Qtz.MySelf.AddQtz("fff", 0, DateTime.Now.AddSeconds(10).Ticks, 3, Ac1);
        }

        #endregion


        public void OnInit()
        {
         
        }


        private static ConcurrentQueue<long> NeedPushCtrl = new ConcurrentQueue<long>();
        /// <summary>
        /// 单灯控制器参数列表  独立单灯设备
        /// </summary>
        public static ConcurrentDictionary<int, RunInfo> Info = new ConcurrentDictionary<int, RunInfo>();

        #region Get

        public static RunInfo GetRunInfo(int ctrlid)
        {
            return Info.ContainsKey(ctrlid) ? Info[ctrlid] : null;
        }

        #endregion

    }

    /// <summary>
    /// 与服务器交互数据
    /// </summary>
    public partial class SluSglCtrlDataHold
    {


        private void InitAciotn()
        {

            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxSluSgl.wst_slu_sgl_ctrl_measure ,
                OnSluSglMeasure,
                typeof(SluSglCtrlDataHold), this);

            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(RequestEquipmentSluSglNewData, 4,
                                                                DelayEventHappen.EventOne);


        }

       // private bool IsSlu7Back = false;
        /// <summary>
        /// 控制器状态 信息
        /// </summary>
        private static ConcurrentDictionary<long, int> DicConnstate = new ConcurrentDictionary<long, int>();

        public void OnSluSglMeasure(string session, Wlst.mobile.MsgWithMobile info)
        {
            if (info == null || info.WstSluSvrAnsSluMeasure == null) return;
            if (info.WstSluSvrAnsSluMeasure.Type == 7)
            {
                //var ctrlTmp = new CtrlIconInfo();
                var lstslus = new List<int>();
                foreach (var g in info.WstSluSvrAnsSluMeasure.InfoBaseic5)
                {
                    if (g.Info == null) continue;
                    if (lstslus.Contains(g.Info.CtrlId) == false) lstslus.Add(g.Info.CtrlId);
                    if (Info.ContainsKey(g.Info.CtrlId) == false) Info.TryAdd(g.Info.CtrlId, new RunInfo(g.Info.CtrlId));

                    Info[g.Info.CtrlId].AddSluCtrlNewData(g.Info.CtrlId, g);                  
                }
                return;
            }

            int sluId = info.WstSluSvrAnsSluMeasure.SluId;
            if (sluId < 1) return;

            var lst = new List<int>();
            var lstctrls = new List<long>();

            foreach (var g in info.WstSluSvrAnsSluMeasure.InfoBaseic5)
            {
                if (Info.ContainsKey(g.Info.CtrlId) == false) Info.TryAdd(g.Info.CtrlId, new RunInfo(g.Info.CtrlId) { IsOnLine = true });
                else Info[g.Info.CtrlId].IsOnLine = true;

                var rtulst = new List<int>();
                rtulst.Add(g.Info.CtrlId);

                Info[g.Info.CtrlId].AddSluCtrlNewData(g.Info.CtrlId, g);
                if (!lst.Contains(g.Info.CtrlId)) lst.Add(g.Info.CtrlId);

                try
                {
                    //计算控制器的状态
                    var code = GetCtrlErrorCode(g.Info.SluId, g.Info.CtrlId, g.Items.Count);
                    int lampnum = g.Items.Count;
                    //   if (g.LightCount > 1) lampnum = 2;
                    int errorIndex = 2090 * 1000 + lampnum * 100 + code;
                    long sluctrlid = g.Info.CtrlId;// g.Info.SluId * 10000L + 
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
                }catch(Exception ex)
                {
                    
                } 

                
            }

            var args = new PublishEventArgs()
            {
                EventType = PublishEventType.Core,
                EventId = EventIdAssign.SluSglMeasure
            };

            args.AddParams(lst);
            EventPublish.PublishEvent(args);
            //foreach (var f in info.WstSluSvrAnsSluMeasure.InfoSlu0)
            //{
            //    Info[sluId].AddSluNewData(f);
            //    //lst.Add(1);
            //}
            //if (info.WstSluSvrAnsSluMeasure.UnknowCtrls != null &&
            //    info.WstSluSvrAnsSluMeasure.UnknowCtrls.Count > 1)
            //{
            //    Info[sluId].AddSluNewData(info.WstSluSvrAnsSluMeasure.UnknowCtrls);
            //    // lst.Add(2);
            //}


            //foreach (var g in info.WstSluSvrAnsSluMeasure.InfoBaseic5)
            //{
            //    //Info[sluId].RtuOcStates = 3;
            //    Info[sluId].AddSluCtrlNewData(g.Info.CtrlId, g);
            //    if (!lst.Contains(g.Info.CtrlId)) lst.Add(g.Info.CtrlId);
            //}

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
                    foreach (var f in lstctrls)
                    {
                        NeedPushCtrl.Enqueue(f);
                    }
                }
            }


        }
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
                    EventId = EventIdAssign.SluSglRunningInfoUpdate,
                };
                argsctrl.AddParams(lst);
                EventPublish.PublishEvent(argsctrl);
            }
        }

        /// <summary>
        /// 获取控制器故障编号
        /// </summary>
        /// <param name="sluId"></param>
        /// <param name="ctrlid"></param>
        /// <param name="lampcount"></param>
        /// <returns></returns>
        public static int GetCtrlErrorCode(int sluId, int ctrlid,int lampcount =1)
        {


            var data = GetRunInfo(ctrlid);
            if (data == null) return 0;

            int errorindex = 0;
            if (data.SluCtrlNewData.ContainsKey(ctrlid) == false ||
                data.SluCtrlNewData[ctrlid].Data5 == null ||
                data.SluCtrlNewData[ctrlid].Data5.Info == null ||
                data.SluCtrlNewData[ctrlid].Data5.Info.Status == 3 ||
                data.SluCtrlNewData[ctrlid].Data5.Info.DateTimeCtrl < 10)
            {
                //如果单灯没有数据  先判断绑定终端是否为全关  如果是 就采用关灯状态 lvf 2018年4月27日14:18:38
                if (data.RtuOcStates == 2) return 1;
                return 0;
            }
            var errs1 = Ioc.GetSluLampErrors(sluId, ctrlid, 1);
            var errs2 = Ioc.GetSluLampErrors(sluId, ctrlid, 2);
            bool hasError1 = false;
            bool hasError2 = false;
            bool hasError = false;
            //if (UxTreeSetting.IsRutsNotShowError == false)
            //{
                hasError1 = errs1.Count > 0;
                hasError2 = errs2.Count > 0;
            //}
            if (lampcount>1)
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


        public static int GetCtrlImageCode(long keys)
        {

            if (DicConnstate.ContainsKey(keys)) return DicConnstate[keys];
            //int sluid = (int)(keys / 10000);
            //int ctrlid = (int)(keys % 10000);

            int ctrlid = (int) (keys);

            // int errorIndex = 2090 * 1000 + lampnum * 100 + code;
            return 2090 * 1000 + 1 * 100 + GetCtrlErrorCode(0, ctrlid);
        }

        /// <summary>
        /// 与服务器交互数据 触发点
        /// </summary>
        private void RequestEquipmentSluSglNewData()
        {
            var info = Wlst.Sr.ProtocolPhone.LxSluSgl.wst_slu_sgl_ctrl_measure; 
            info.WstSluMeasure.Type = 7;

            SndOrderServer.OrderSnd(info);
        }


    }
}
