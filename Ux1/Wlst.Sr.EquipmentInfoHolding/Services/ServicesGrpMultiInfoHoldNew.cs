using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.ModuleServices;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.client;
using Wlst.mobile;

namespace Wlst.Sr.EquipmentInfoHolding.Services
{
    public class ServicesGrpMultiInfoHoldNew
    {

        public const int GroupStartId = 601000;
        private static readonly ServicesGrpMultiInfoHoldNewExtend GrpMulti = new ServicesGrpMultiInfoHoldNewExtend();


        /// <summary>
        /// 程序初始化时必须执行一次;由本模块自动执行
        /// </summary>
        public static void InitLoad()
        {
            GrpMulti.InitLoad();
        }


        /// <summary>
        /// <para>任何使用此数务必注意 此处使用的为原始数据 ___不允许修改___</para>
        /// </summary>
        public static Dictionary<Tuple<int, int>, GroupInformation> ItemsMultiGrp
        {
            get { return GrpMulti.ItemsMultiGrp; }
        }

        public static List<int> GetRtuInGroup(int areaId,int groupId)
        {
            return GrpMulti.GetRtuInGroup(areaId,groupId);
        } 

        /// <summary>
        /// 获取分组信息
        /// </summary>
        /// <param name="group"></param>
        /// <returns>不存在返回 null</returns>
        public static GroupItemsInfo.GroupItem GetGroupInfomation(int areaId, int group)
        {
            var tu = new Tuple<int, int>(areaId, group);
            if (ItemsMultiGrp.ContainsKey(tu))
            {
                var rtus = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(areaId);
                foreach (var f in rtus) if (ItemsMultiGrp[tu].LstTml.Contains(f) == false) ItemsMultiGrp[tu].LstTml.Remove(f);
                return ItemsMultiGrp[tu];
            }
            return null;
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
            return GrpMulti.GrpInfoList();
        }

        public static List<GroupInformation> GrpInfoList(int areaId)
        {
            return GrpMulti.GrpInfoList(areaId);
        }

        /// <summary>
        /// 本系统主动提及分组数据到服务器  请求服务器更新数据
        /// </summary>
        public static void UpdateGroupsInfo(List<GroupInformation> groupinfo)
        {
            GrpMulti.UpdateLocalGroupInfo(groupinfo);
        }

        /// <summary>
        /// 与服务器交互数据 请求比对数据 触发点
        /// </summary>
        public static void RequestGroupInfo()
        {
            GrpMulti.RequestLocalGroupInfo();
        }

        
    }

    internal partial class ServicesGrpMultiInfoHoldNewExtend
    {
        /// <summary>
        /// 所有的本地分组
        /// </summary>
        public Dictionary<Tuple<int, int>, GroupInformation> ItemsMultiGrp = new Dictionary<Tuple<int, int>, GroupInformation>();

        

        protected bool BolLoad = false;

        /// <summary>
        /// 是否与服务器同步了数据
        /// </summary>
        protected bool BolGetServerReturn = false;

        /// <summary>
        /// <para>获取列表</para>
        /// </summary>
        public List<GroupInformation> GrpInfoList()
        {
            return (from pair in ItemsMultiGrp orderby pair.Key ascending select pair.Value).ToList();
        }

  

        public List<int> GetRtuInGroup(int areaId,int groupId)
        {
            var value =(from t in ItemsMultiGrp where t.Key.Item1 ==areaId&&t.Key.Item2 == groupId orderby t.Value.Index ascending select t.Value).ToList();
            var RtuInGroup = new List<int>();
            foreach(var t in value)
            {
                RtuInGroup.AddRange(t.LstTml);
            }
            return RtuInGroup;
        }

        /// <summary>
        /// <para>获取升序排列的列表</para>
        /// </summary>
        public List<GroupInformation> GrpInfoList(int areaId)
        {
            return (from t in ItemsMultiGrp where t.Key.Item1 == areaId orderby t.Value.Index ascending select t.Value).ToList();
        }

      
    }

    /// <summary>
    /// 实现对分组信息的事件捕获与更新
    /// </summary>
    internal partial class ServicesGrpMultiInfoHoldNewExtend : EventHandlerHelperExtendNotifyProperyChanged
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
                RequestLocalGroupInfo();
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
            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(RequestLocalGroupInfo, 1);

        }

        private void InitAction()
        {

            ProtocolServer.RegistProtocol(Sr.ProtocolPhone.LxAreaGrp.wls_group_info_local,
                                       OnSvrLocalGroupInfoArrive,
                                       typeof(ServicesGrpMultiInfoHoldNewExtend), this);

        }

        private void OnSvrLocalGroupInfoArrive(string session, MsgWithMobile infos)
        {
            if (infos.WstAreagrpGroupInfoLocal == null) return;

            var grpInfoExchangefromServer = infos.WstAreagrpGroupInfoLocal;

            var lstfromServer = grpInfoExchangefromServer.GroupItems;
            if (lstfromServer == null) return;


            if (grpInfoExchangefromServer.Op == 1)
            {
                //终端与分组的排序序号
                int index = 1;

                //分组信息更新
                ItemsMultiGrp.Clear();
                foreach (var t in grpInfoExchangefromServer.GroupItems)
                {
                    if (t.GroupId < 0) continue;
                    var item = new GroupInformation(t) { Index = index++ };

                    var tu = new Tuple<int, int>(t.AreaId, t.GroupId);
                    if (ItemsMultiGrp.ContainsKey(tu)) ItemsMultiGrp[tu] = item;
                    else ItemsMultiGrp.Add(tu, item);
                }
            }
            else
            {
                //终端与分组的排序序号
                int index = 1;

                if (grpInfoExchangefromServer.GroupItems.Count == 0) return;
                int areaid = grpInfoExchangefromServer.GroupItems[0].AreaId;
                var dlt = (from t in ItemsMultiGrp where t.Key.Item1 == areaid select t.Key).ToList();
                foreach (var f in dlt) if (ItemsMultiGrp.ContainsKey(f)) ItemsMultiGrp.Remove(f);
                foreach (var t in grpInfoExchangefromServer.GroupItems)
                {
                    if (t.GroupId < 0) continue;
                    var item = new GroupInformation(t) { Index = index++ };

                    var tu = new Tuple<int, int>(t.AreaId, t.GroupId);
                    if (ItemsMultiGrp.ContainsKey(tu)) ItemsMultiGrp[tu] = item;
                    else ItemsMultiGrp.Add(tu, item);
                }
            }

            //更新终端归属第一个最大分组的信息
            //rtu-->grp

            //InfoRtuBelong.Clear();
            //foreach (var f in ItemsMultiGrp)
            //{
            //    var tu = new Tuple<int, int>(f.Value.AreaId, f.Value.GroupId);
            //    foreach (var g in f.Value.LstTml)
            //        if (!InfoRtuBelong.ContainsKey(g)) InfoRtuBelong.Add(g, tu);

            //}


            var arg = new PublishEventArgs()
            {
                EventId = EventIdAssign.MulityInfoGroupAllNeedUpdate,
                EventType = PublishEventType.Core
            };
            EventPublish.PublishEvent(arg);
        }

        /// <summary>
        ///  请求比对数据 触发点
        /// </summary>
        public void RequestLocalGroupInfo()
        {
            BolGetServerReturn = false;
            var info = Wlst.Sr.ProtocolPhone.LxAreaGrp.wls_group_info_local;
            info.WstAreagrpGroupInfoLocal.Op = 1;

            SndOrderServer.OrderSnd(info, 10, 6);
        }

        /// <summary>
        ///   请求服务器更新数据
        /// </summary>
        public void UpdateLocalGroupInfo(List<GroupInformation> groupinfo)
        {
            var ntg = (from t in groupinfo orderby t.Index ascending select t).ToList();
            var info = Wlst.Sr.ProtocolPhone.LxAreaGrp.wls_group_info_local;

            foreach (var t in ntg)
            {
                info.WstAreagrpGroupInfoLocal.GroupItems.Add(new GroupItemsInfo.GroupItem()
                {
                    GroupName = t.GroupName,
                    GroupId = t.GroupId,
                    LstTml = t.LstTml,
                    AreaId = t.AreaId

                });
            }
            info.WstAreagrpGroupInfoLocal.Op = 2;


            SndOrderServer.OrderSnd(info, 10, 6);

            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                0, "所有分组", Wlst.Cr.CoreMims.ShowMsgInfo.OperatrType.UserOperator, "更新分组信息");


        }
    }
}
