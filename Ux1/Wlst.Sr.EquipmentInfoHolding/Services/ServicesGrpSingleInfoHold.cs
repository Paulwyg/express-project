using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.client;
using Wlst.mobile;

namespace Wlst.Sr.EquipmentInfoHolding.Services
{
    public class ServicesGrpSingleInfoHold : EventHandlerHelperExtendNotifyProperyChanged
    {


        public const int GroupStartId = 501000;
        private static readonly GrpSingleInfoHoldExtend GrpSingle = new GrpSingleInfoHoldExtend();

        public static long GetVersion
        {
            get { return GrpSingleInfoHoldExtend.Version; }
        }

        /// <summary>
        /// 程序初始化时必须执行一次;由本模块自动执行
        /// </summary>
        public static void InitLoad()
        {
            GrpSingle.InitLoad();
        }


        /// <summary>
        /// 终端属于哪一个分组 第一个分组既最大的哪一个分组
        /// </summary>
        public static Dictionary<int, Tuple< int ,int >> InfoRtuBelong
        {
            get { return GrpSingle.InfoRtuBelong; }
        }


        /// <summary>
        /// 获取终端归属分组  区域-分组
        /// </summary>
        /// <param name="rtuId"></param>
        /// <returns>null 不存在 </returns>
        public static Tuple< int ,int > GetRtuBelongGrp(int rtuId)
        {
            if (GrpSingle.InfoRtuBelong.ContainsKey(rtuId)) return GrpSingle.InfoRtuBelong[rtuId];
            return null ;
        }


        /// <summary>
        /// <para>任何使用此数务必注意 此处使用的为原始数据 ___不允许修改___</para>
        /// </summary>
        public static Dictionary<Tuple<int, int>, GroupInformation> InfoGroups
        {
            get { return GrpSingle.InfoGroups; }
        }

        /// <summary>
        /// 获取分组信息
        /// </summary>
        /// <param name="group"></param>
        /// <returns>不存在返回 null</returns>
        public static GroupItemsInfo.GroupItem GetGroupInfomation(int areaId, int group)
        {
            var tu = new Tuple<int, int>(areaId, group);
            if (InfoGroups.ContainsKey(tu))
            {
                //var rtus = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(areaId);
                //foreach (var f in rtus) if (InfoGroups[tu].LstTml.Contains(f) == false) InfoGroups[tu].LstTml.Remove(f);
                return InfoGroups[tu];
            }
            return null;
        }

        /// <summary>
        /// 获取指定区域未划分分组的终端列表
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public static List<int> GetRtuNotInAnyGroup(int areaId)
        {
            var lst = new List<int>();
            lst.AddRange(AreaInfoHold.MySlef.GetRtuInArea(areaId));
            foreach (var f in InfoGroups)
            {
                if (f.Key.Item1 != areaId) continue;
                foreach (var l in f.Value.LstTml) if (lst.Contains(l)) lst.Remove(l);

            }
            return lst;
        }


        /// <summary>
        /// 当终端所属区域发生变化时，将此终端从曾属分组中移除
        /// </summary>
        /// <param name="areaItems"></param>
        /// <returns></returns>
        public static List<GroupInformation> GetLstOutofPreviousArea(List<AreaInfo.AreaItem> areaItems)
        {
            var grouplst = new List<GroupInformation>();
            foreach (var v in GrpInfoList())
            {
                grouplst.Add(v);
            }
            for (int i = areaItems.Count - 1; i >= 0; i--)
            {
                var lst = new List<int>();
                lst.AddRange(AreaInfoHold.MySlef.GetRtuInArea(areaItems[i].AreaId));
                for (int j = grouplst.Count - 1; j >= 0; j--)
                {
                    if (grouplst[j].AreaId != areaItems[i].AreaId)
                        continue;
                    for (int k = grouplst[j].LstTml.Count - 1; k >= 0; k--)
                    {
                        if (!lst.Contains(grouplst[j].LstTml[k]))
                        {
                            grouplst[j].LstTml.RemoveAt(k);
                        }
                    }
                }
            }
            return grouplst;
        }
        /// <summary>
        /// <para>获取升序排列的列表</para>
        /// <para>任何使用此数务必注意 此处使用的为原始数据  ___不允许修改___</para>
        /// <para>修改请用GroupInfomatioin 类的clone方法进行克隆副本使用</para>
        /// </summary>
        public static List<GroupInformation> GrpInfoList()
        {
             return GrpSingle.GrpInfoList(); 
        }


        public static List<GroupInformation> GrpInfoList(int areaId)
        {
            return GrpSingle.GrpInfoList(areaId );
        }

    
        public static Tuple< List< int >,List< int >> GetRtuOrGrpIndex(List<int> data, int areaid , bool isNewInTop)
        {

            List<int> rtusInArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(areaid);
            List<int> newinarea = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.MySlef.RtusNeedTopShow;

            var tmp1 = (from t in newinarea where rtusInArea.Contains(t) select t).ToList();

            var tmp2 = (from t in data where newinarea.Contains(t) == false select t).ToList();

            var item1 = GetRtuOrGrpIndex(tmp1);
            var item2 = GetRtuOrGrpIndex(tmp2);

            return new Tuple<List<int>, List<int>>(item1, item2);
        }

        public static List<int> GetRtuOrGrpIndex(List<int> data)
        {
            int treesortby = UxTreeSetting .TreeSortBy ;


            if (treesortby == 4)
            {
                return (from t in data orderby t ascending select t).ToList();
            }
            else if (treesortby == 1)
            {
                Dictionary<int, int> dic = new Dictionary<int, int>();
                foreach (var f in data)
                {
                    int phyId = 999999999;
                    if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(f))
                    {
                        phyId = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[f].RtuPhyId;
                    }
                    if (dic.ContainsKey(f)) dic[f] = phyId;
                    else dic.Add(f, phyId);
                }
                return (from t in dic orderby t.Value ascending select t.Key).ToList();
            }
            else if (treesortby == 2)
            {
                Dictionary<int, string> dic = new Dictionary<int, string>();
                foreach (var f in data)
                {
                    string rtuName = "ZZZZZZZZ";
                    if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(f))
                    {
                        rtuName = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[f].RtuName;
                    }
                    if (dic.ContainsKey(f)) dic[f] = rtuName;
                    else dic.Add(f, rtuName);
                }
                return (from t in dic orderby t.Value ascending select t.Key).ToList();
            }
            else if (treesortby == 3)
            {
                Dictionary<int, int> dic = new Dictionary<int, int>();


                var nts =
                    (from t in Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups.Values
                     orderby t.Index ascending
                     select t.LstTml).ToList();
                int orderid = 1;
                foreach (var gx in nts)
                {
                    foreach (var fr in gx)
                    {
                        if (dic.ContainsKey(fr)) dic[fr] = orderid;
                        else dic.Add(fr, orderid);
                        orderid++;
                    }
                }



                var dicrtn = new Dictionary<int, int>();
                foreach (var f in data)
                {
                    orderid = 99999999;
                    if (dic.ContainsKey(f)) orderid = dic[f];
                    if (dicrtn.ContainsKey(f)) dicrtn[f] = orderid;
                    else dicrtn.Add(f, orderid);
                }

                return (from t in dicrtn orderby t.Value ascending select t.Key).ToList();
            }
            return data;
        }

        /// <summary>
        /// 本系统主动提及分组数据到服务器  请求服务器更新数据
        /// </summary>
        public static void UpdateGroupsInfo(List<GroupInformation> groupinfo)
        {
            GrpSingle.UpdateGroupsInfo(groupinfo);
        }

        public static List <int > GetGrpTmlList(int areaId,int gprId)
        {
            var tu = new Tuple<int, int>(areaId, gprId);
            if (InfoGroups.ContainsKey(tu)) return InfoGroups[tu].LstTml ;
            return new List<int> ();
        }



        /// <summary>
        /// 与服务器交互数据 请求比对数据 触发点
        /// </summary>
        public static void RequestGroupInfo()
        {
            GrpSingle.RequestGroupInfo();
        }
    }




    internal  partial class GrpSingleInfoHoldExtend
    {
        /// <summary>
        /// 所有分组信息
        /// </summary>
        public Dictionary<Tuple<int, int>, GroupInformation> InfoGroups = new Dictionary<Tuple<int, int>, GroupInformation>();

        /// <summary>
        /// 终端属于哪一个分组 终端-区域-分组
        /// </summary>
        public Dictionary<int, Tuple<int, int>> InfoRtuBelong = new Dictionary<int, Tuple<int, int>>();



        protected bool BolLoad = false;

        /// <summary>
        /// 是否与服务器同步了数据
        /// </summary>
        protected bool BolGetServerReturn = false;

        #region Simple Get informaiton


        /// <summary>
        /// 获取终端归属分组  区域-分组
        /// </summary>
        /// <param name="rtuId"></param>
        /// <returns>存在返回  null</returns>
        public Tuple<int, int> GetRtuGrpBelong(int rtuId)
        {
            if (InfoRtuBelong.ContainsKey(rtuId)) return InfoRtuBelong[rtuId];
            return null;
        }


        /// <summary>
        /// <para>获取列表</para>
        /// </summary>
        public List<GroupInformation> GrpInfoList()
        {
            return (from pair in InfoGroups orderby pair.Key ascending select pair.Value).ToList();
        }

        /// <summary>
        /// <para>获取升序排列的列表</para>
        /// </summary>
        public List<GroupInformation> GrpInfoList(int areaId)
        {
            return (from t in InfoGroups where t.Key.Item1 == areaId orderby t.Value.Index ascending select t.Value).ToList();


        }

        #endregion






    }



    /// <summary>
    /// 实现对分组信息的事件捕获与更新
    /// </summary>
    internal partial class GrpSingleInfoHoldExtend : EventHandlerHelperExtendNotifyProperyChanged //: GrpMultiInfoHoldingExtend
    {



        internal void InitEvent()
        {
            this.AddEventFilterInfo(100, PublishEventType.ReCn);

        }/// <summary>
        /// 事件数据处理
        /// </summary>
        /// <param name="args"></param>
        public override void ExPublishedEvent(PublishEventArgs args)
        {
            if (args.EventType == PublishEventType.ReCn)
            {
                RequestGroupInfo();
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
            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(RequestGroupInfo, 0);

        }


        private void InitAction()
        {

            ProtocolServer.RegistProtocol(Sr.ProtocolPhone.LxAreaGrp.wls_group_info,
                                       OnSvrGroupInfoArrive,
                                       typeof(GrpSingleInfoHoldExtend), this);

        }

        //更新版本 
        public static long Version = 0;
        private void OnSvrGroupInfoArrive(string session, MsgWithMobile infos)
        {
            if (infos.WstAreagrpGroupInfo == null) return;

            var grpInfoExchangefromServer = infos.WstAreagrpGroupInfo;

            var lstfromServer = grpInfoExchangefromServer.GroupItems;
            if (lstfromServer == null) return;

            
            if (grpInfoExchangefromServer.Op == 1)
            {
               
                //终端与分组的排序序号
                int index = 1;

                //分组信息更新
                InfoGroups.Clear();
                foreach (var t in grpInfoExchangefromServer.GroupItems)
                {
                    var item = new GroupInformation(t) {Index = index++};

                    var tu = new Tuple<int, int>(t.AreaId, t.GroupId);
                    if (InfoGroups.ContainsKey(tu)) InfoGroups[tu] = item;
                    else InfoGroups.Add(tu, item);
                }
                var args = new PublishEventArgs()
                {
                    EventId = EventIdAssign.SingleNeedRefresh,
                    EventType = PublishEventType.Core
                };
                EventPublish.PublishEvent(args);
            }
            else
            {
                //终端与分组的排序序号
                int index = 1;

                if (grpInfoExchangefromServer.GroupItems.Count == 0) return;
                int areaid = grpInfoExchangefromServer.GroupItems[0].AreaId;
                var dlt = (from t in InfoGroups where t.Key.Item1 == areaid select t.Key ).ToList();
                foreach (var f in dlt) if (InfoGroups.ContainsKey(f)) InfoGroups.Remove(f);
                foreach (var t in grpInfoExchangefromServer.GroupItems)
                {
                    var item = new GroupInformation(t) { Index = index++ };

                    var tu = new Tuple<int, int>(t.AreaId, t.GroupId);
                    if (InfoGroups.ContainsKey(tu)) InfoGroups[tu] = item;
                    else InfoGroups.Add(tu, item);
                }
            }

            //更新终端归属第一个最大分组的信息
            //rtu-->grp

            InfoRtuBelong.Clear();
            foreach (var f in InfoGroups)
            {
                var tu = new Tuple<int, int>(f.Value.AreaId, f.Value.GroupId);
                foreach (var g in f.Value.LstTml)
                    if (!InfoRtuBelong.ContainsKey(g)) InfoRtuBelong.Add(g, tu);

            }

            Version = DateTime.Now.Ticks;
            var arg = new PublishEventArgs()
            {
                EventId = EventIdAssign.SingleInfoGroupAllNeedUpdate,
                EventType = PublishEventType.Core
            };
            EventPublish.PublishEvent(arg);
        }





        /// <summary>
        ///   请求服务器更新数据
        /// </summary>
        public void UpdateGroupsInfo(List<GroupInformation> groupinfo)
        {
            var ntg = (from t in groupinfo orderby t.Index ascending select t).ToList();
            var info = Wlst.Sr.ProtocolPhone.LxAreaGrp.wls_group_info;

            foreach (var t in ntg)
            {
                info.WstAreagrpGroupInfo.GroupItems.Add(new GroupItemsInfo.GroupItem()
                {
                    GroupName = t.GroupName,
                    GroupId = t.GroupId,
                    LstTml = t.LstTml,
                    AreaId = t.AreaId

                });
            }

            info.WstAreagrpGroupInfo.Op = 2;

            SndOrderServer.OrderSnd(info, 10, 6);

            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                0, "所有分组", Wlst.Cr.CoreMims.ShowMsgInfo.OperatrType.UserOperator, "更新分组信息");


        }


        /// <summary>
        ///  请求比对数据 触发点
        /// </summary>
        public void RequestGroupInfo()
        {
            BolGetServerReturn = false;
            var info = Wlst.Sr.ProtocolPhone.LxAreaGrp.wls_group_info;
            info.WstAreagrpGroupInfo.Op = 1;
            var dhx = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.OrderSndHttp(info);
            if (dhx != null )
            {
                OnSvrGroupInfoArrive(dhx);
            }
           // SndOrderServer.OrderSnd(info, 10, 6);
        }

        private void OnSvrGroupInfoArrive(  MsgWithMobile infos)
        {
            if (infos.WstAreagrpGroupInfo == null) return;

            var grpInfoExchangefromServer = infos.WstAreagrpGroupInfo;

            var lstfromServer = grpInfoExchangefromServer.GroupItems;
            if (lstfromServer == null) return;


            if (grpInfoExchangefromServer.Op == 1)
            {

                //终端与分组的排序序号
                int index = 1;

                //分组信息更新
                InfoGroups.Clear();
                foreach (var t in grpInfoExchangefromServer.GroupItems)
                {
                    var item = new GroupInformation(t) { Index = index++ };

                    var tu = new Tuple<int, int>(t.AreaId, t.GroupId);
                    if (InfoGroups.ContainsKey(tu)) InfoGroups[tu] = item;
                    else InfoGroups.Add(tu, item);
                }
               
            }
            else
            {
                //终端与分组的排序序号
                int index = 1;

                if (grpInfoExchangefromServer.GroupItems.Count == 0) return;
                int areaid = grpInfoExchangefromServer.GroupItems[0].AreaId;
                var dlt = (from t in InfoGroups where t.Key.Item1 == areaid select t.Key).ToList();
                foreach (var f in dlt) if (InfoGroups.ContainsKey(f)) InfoGroups.Remove(f);
                foreach (var t in grpInfoExchangefromServer.GroupItems)
                {
                    var item = new GroupInformation(t) { Index = index++ };

                    var tu = new Tuple<int, int>(t.AreaId, t.GroupId);
                    if (InfoGroups.ContainsKey(tu)) InfoGroups[tu] = item;
                    else InfoGroups.Add(tu, item);
                }
            }

            //更新终端归属第一个最大分组的信息
            //rtu-->grp

            InfoRtuBelong.Clear();
            foreach (var f in InfoGroups)
            {
                var tu = new Tuple<int, int>(f.Value.AreaId, f.Value.GroupId);
                foreach (var g in f.Value.LstTml)
                    if (!InfoRtuBelong.ContainsKey(g)) InfoRtuBelong.Add(g, tu);

            }

            Version = DateTime.Now.Ticks;

            Wlst.Cr.Coreb.Servers.WriteLog.WriteLogError(
                "OnInitLoadAreaSucc:" + InfoGroups.Count);
            if (SrEquipmentInfoHolding.OnInitLoadRtuSucc   && SrEquipmentInfoHolding.OnInitLoadAreaSucc  )
            {
                if (grpInfoExchangefromServer.Op == 1)
                {
                    var args = new PublishEventArgs()
                    {
                        EventId = EventIdAssign.SingleNeedRefresh,
                        EventType = PublishEventType.Core
                    };
                    EventPublish.PublishEvent(args);
                }
                var arg = new PublishEventArgs()
                {
                    EventId = EventIdAssign.SingleInfoGroupAllNeedUpdate,
                    EventType = PublishEventType.Core
                };
                EventPublish.PublishEvent(arg);
            }

            SrEquipmentInfoHolding.OnInitLoadGrpSucc  = true;

        }






    }

}
