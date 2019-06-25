using System.Collections.Concurrent;
using System.Collections.Generic;
using MS.Internal.Xml.XPath;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.ModuleServices;
using Wlst.Cr.CoreMims.Services;
using Wlst.client;

namespace Wlst.Ux.Wj2090Module.SrInfo
{

    //public partial class CtrGrpInfo : EventHandlerHelper
    //{
    //    #region single instances

    //    private static CtrGrpInfo _mySlef = null;
    //    private static object obj = 1;

    //    public static CtrGrpInfo MySelf
    //    {
    //        get
    //        {
    //            if (_mySlef == null)
    //            {
    //                lock (obj)
    //                {
    //                    if (_mySlef == null)
    //                        new CtrGrpInfo();
    //                }

    //            }
    //            return _mySlef;
    //        }
    //    }

    //    #endregion


    //    /// <summary>
    //    /// 存放单灯时间方案
    //    /// </summary>
    //    public ConcurrentDictionary<int, List<SluCtrlGrpInfo.SluCtrlGrpInfoItem>> Info = new ConcurrentDictionary<int, List<SluCtrlGrpInfo.SluCtrlGrpInfoItem>>();

    //    private CtrGrpInfo()
    //    {
    //        if (_mySlef != null) return;

    //        _mySlef = this;
    //        this.InitAciton();
    //        this.InitEvent();
    //        Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(RequestGrpInfofromSvr, 5,
    //                                                                  DelayEventHappen.EventSvAc);
    //    }

    //    public void Init()
    //    {

    //    }

    //    private void RequestGrpInfofromSvr()
    //    {
    //        var info = Wlst.Sr.ProtocolPhone .LxSlu .wst_slu_ctrl_grp ;// .wlst_cnt_wj2090_request_con_grps ;//.ServerPart.wlst_Wj2090_clinet_request_con_grp_info ;
    //        info.WstSluCtrlGrp.Op = 2;
    //        SndOrderServer.OrderSnd(info, 10, 3);
    //    }

    //    /// <summary>
    //    /// 更新所有的分组信息
    //    /// </summary>
    //    /// <param name="timeInfo"></param>
    //    public void UpdateGroupInfo(List<SluCtrlGrpInfo.SluCtrlGrpInfoItem> timeInfo, int sluId)
    //    {
    //        var info = Wlst.Sr.ProtocolPhone .LxSlu .wst_slu_ctrl_grp ;// .wlst_cnt_wj2090_update_con_grps ;//.ServerPart.wlst_Wj2090_clinet_update_con_grp_info;
    //        info.WstSluCtrlGrp  .ItemsGrp .AddRange(timeInfo);
    //        info.WstSluCtrlGrp.SluId = sluId;
    //        info.WstSluCtrlGrp.Op = 12;
    //        SndOrderServer.OrderSnd(info, 10, 3);
    //    }


    //    public List<SluCtrlGrpInfo.SluCtrlGrpInfoItem> GetGrpInfoBySluId(int sluId)
    //    {
    //        if (Info.ContainsKey(sluId)) return Info[sluId];
    //        return new List<SluCtrlGrpInfo.SluCtrlGrpInfoItem>();
    //    }
    //}


    //public partial class CtrGrpInfo
    //{
    //    private void InitAciton()
    //    {

    //        ProtocolServer.RegistProtocol(
    //            Wlst.Sr.ProtocolPhone .LxSlu .wst_slu_ctrl_grp ,// .wlst_svr_ans_cnt_wj2090_request_con_grps ,
    //       //.ClientPart.wlst_Wj2090_svr_ans_clinet_request_con_grp_info, 
    //       OnGrpRequestOrUpdate,
    //            typeof (CtrGrpInfo), this);

    //    }
    //    private void InitEvent()
    //    {
    //        this.AddEventFilterInfo(-1, PublishEventType.ReCn);

    //    }
    //    /// <summary>
    //    /// 事件数据处理
    //    /// </summary>
    //    /// <param name="args"></param>
    //    public override void ExPublishedEvent(PublishEventArgs args)
    //    {
    //        if (args.EventType == PublishEventType.ReCn)
    //        {
    //            this.RequestGrpInfofromSvr();
    //            return;
    //        }
    //    }

    //    private void OnGrpRequestOrUpdate(string sessionid, Wlst.mobile.MsgWithMobile info)
    //    {
    //        if (info == null || info.WstSluCtrlGrp == null) return;
    //        if (info.WstSluCtrlGrp.Op == 2)
    //        {
    //            Info.Clear();
    //            foreach (var g in info.WstSluCtrlGrp.ItemsGrp)
    //            {

    //                if (!Info.ContainsKey(g.SluId))
    //                    Info.TryAdd(g.SluId, new List<SluCtrlGrpInfo.SluCtrlGrpInfoItem>());
    //                Info[g.SluId].Add(g);
    //            }


    //            var ar = new PublishEventArgs()
    //                         {
    //                             EventId = Wj2090Module.Services.EventIdAssign.GrpConInfoRequestId,
    //                             EventType = PublishEventType.Core
    //                         };
    //            EventPublish.PublishEvent(ar);
    //        }
    //        if (info.WstSluCtrlGrp.Op == 2)
    //        {


    //            if (Info.ContainsKey(info.WstSluCtrlGrp.SluId))
    //                Info[info.WstSluCtrlGrp.SluId].Clear();

    //            foreach (var g in info.WstSluCtrlGrp.ItemsGrp )
    //            {
    //                if (!Info.ContainsKey(g.SluId))
    //                    Info.TryAdd(g.SluId, new List<SluCtrlGrpInfo.SluCtrlGrpInfoItem>( ));
    //                Info[g.SluId].Add(g);
    //            }


    //            var ar = new PublishEventArgs()
    //                         {
    //                             EventId = Wj2090Module.Services.EventIdAssign.GrpConInfoUpdateId,
    //                             EventType = PublishEventType.Core
    //                         };
    //            ar.AddParams(info.WstSluCtrlGrp.SluId);
    //            EventPublish.PublishEvent(ar);
    //        }
    //    }
    //}
}

