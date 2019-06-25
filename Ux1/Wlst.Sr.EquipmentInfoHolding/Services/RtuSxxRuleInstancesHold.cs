using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.client;
using Wlst.mobile;

namespace Wlst.Sr.EquipmentInfoHolding.Services
{


    public partial class RtuSxxRuleInstancesHold
    {
        private static RtuSxxRuleInstancesHold myself=null ;
        private static object  obj = 1;

        public static RtuSxxRuleInstancesHold Myself
        {
            get
            {
                if (myself == null)
                {
                    lock (obj)
                    {
                        if (myself == null) myself = new RtuSxxRuleInstancesHold();
                    }
                }
                return myself;
            }
        }

        public ConcurrentDictionary< int ,List <Wlst.client.RtuSets.LoopSxxRuleItem>> Rules = new ConcurrentDictionary<int, List<RtuSets.LoopSxxRuleItem>>( );

        /// <summary>
        /// 所有信息 区域-序号
        /// </summary>
        public ConcurrentDictionary<Tuple<int, int>, Wlst.client.RtuSetsNew.RtuLoopSxxSectionInfo> InfoItems =
            new ConcurrentDictionary<Tuple<int, int>, RtuSetsNew.RtuLoopSxxSectionInfo>();




        protected bool BolLoad = false;

        ///// <summary>
        ///// 是否与服务器同步了数据
        ///// </summary>
        //protected bool BolGetServerReturn = false;


        /// <summary>
        /// <para>获取升序排列的列表</para>
        /// </summary>
        public List<RtuSetsNew.RtuLoopSxxSectionInfo> GetList(int areaId)
        {
            return
                (from t in InfoItems where t.Key.Item1 == areaId orderby t.Key.Item2 ascending select t.Value).ToList();
        }

    }



    /// <summary>
    /// 实现对分组信息的事件捕获与更新
    /// </summary>
    public partial class RtuSxxRuleInstancesHold : EventHandlerHelperExtendNotifyProperyChanged //: GrpMultiInfoHoldingExtend
    {



        internal void InitEvent()
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
                RequestInfo();
                return;
            }
        }


        private bool _bolinitload = false;

        /// <summary>
        /// 程序初始化时必须执行一次
        /// </summary>
        internal void InitLoad()
        {
            if (_bolinitload) return;
            _bolinitload = true;

            InitEvent();
            this.InitAction();
            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(RequestInfo, 1);

        }


        private void InitAction()
        {

            ProtocolServer.RegistProtocol(Sr.ProtocolPhone.LxRtu.wst_ldl_sxx_avg_set_new,
                                          OnSxxInfoArrive,
                                          typeof (RtuSxxRuleInstancesHold), this);


            ProtocolServer.RegistProtocol(Sr.ProtocolPhone.LxRtu.wst_ldl_sxx_avg_set ,
                                   OnSxxInfoArrive1,
                                   typeof(RtuSxxRuleInstancesHold), this);

        }

        private void OnSxxInfoArrive(string session, MsgWithMobile infos)
        {

            if (infos.WstRtuLdlSxxAvgSetNew == null) return;

            if (infos.WstRtuLdlSxxAvgSetNew.Op == 14 || infos.WstRtuLdlSxxAvgSetNew.Op == 4)
            {
                if (infos.WstRtuLdlSxxAvgSetNew.SxxItems == null) return;
                var lstfromServer = infos.WstRtuLdlSxxAvgSetNew.SxxItems;

                var aras = (from t in lstfromServer select t.AreaId).ToList().Distinct().ToList();
                var keys = (from t in InfoItems where aras.Contains(t.Key.Item1) select t.Key).ToList();

                foreach (var f in keys)
                {
                    RtuSetsNew.RtuLoopSxxSectionInfo fx;
                    if (InfoItems.ContainsKey(f)) InfoItems.TryRemove(f, out fx);
                }
                foreach (var f in lstfromServer)
                {
                    var tu = new Tuple<int, int>(f.AreaId, f.SectionId);
                    if (InfoItems.ContainsKey(tu)) InfoItems[tu] = f;
                    else InfoItems.TryAdd(tu, f);
                }
            }
            else if (infos.WstRtuLdlSxxAvgSetNew.Op == 5)
            {
                int areaid = infos.WstRtuLdlSxxAvgSetNew.AreaId;
                var dic = new Dictionary<Tuple<int, int, int>, List<RtuSetsNew.RtuLoopSxxSectionInfo.RtuLoopSxx>>();
                foreach (var f in infos.WstRtuLdlSxxAvgSetNew.SxxItemsOneRtu)
                {
                    var tu = new Tuple<int, int, int>(areaid, f.SectionId, f.RtuId);
                    if (dic.ContainsKey(tu) == false)
                        dic.Add(tu, new List<RtuSetsNew.RtuLoopSxxSectionInfo.RtuLoopSxx>());
                    dic[tu].Add(f);
                }

                foreach (var f in dic)
                {
                    int aid = f.Key.Item1;
                    int sid = f.Key.Item2;
                    int rtuid = f.Key.Item3;
                    var tukey = new Tuple<int, int>(aid, sid);
                    if (InfoItems.ContainsKey(tukey) == false) continue;
                    var ntgs = (from t in InfoItems[tukey].Items where t.RtuId == rtuid select t).ToList();
                    foreach (var xf in ntgs)
                    {
                        if (InfoItems[tukey].Items.Contains(xf)) InfoItems[tukey].Items.Remove(xf);
                    }
                    foreach (var l in f.Value) InfoItems[tukey].Items.Add(l);
                }
            }

        }

        private void OnSxxInfoArrive1(string session, MsgWithMobile infos)
        {
            if (infos == null || infos.WstRtuLdlSxxAvgSet == null) return;

            var datax = infos.WstRtuLdlSxxAvgSet;
            if (datax.Op == 1 || datax.Op == 11)
            {
                if (datax.SxxRuleItems.Count == 0) return;
                foreach (var g in datax.SxxRuleItems)
                {
                    if (Rules.ContainsKey(g.AreaId)) Rules[g.AreaId].Clear();
                    else Rules.TryAdd(g.AreaId, new List<RtuSets.LoopSxxRuleItem>());

                    foreach (var f in datax.SxxRuleItems)
                    {
                        this.Rules[g.AreaId].AddRange(f.SxxRuleItem);
                    }
                }
            }
        }

        /// <summary>
        ///  请求比对数据 触发点
        /// </summary>
        public void RequestInfo()
        {
            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_ldl_sxx_avg_set_new;
            info.WstRtuLdlSxxAvgSetNew.Op = 4;
            info.WstRtuLdlSxxAvgSetNew.AreaId = -1;
            SndOrderServer.OrderSnd(info, 10, 6);


            var infox = Wlst.Sr.ProtocolPhone.LxRtu.wst_ldl_sxx_avg_set ;
            infox.WstRtuLdlSxxAvgSet.Op = 1;
            //infox.WstRtuLdlSxxAvgSetNew.AreaId = -1;
            SndOrderServer.OrderSnd(infox, 10, 6);
        }



    }

}
