﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Windows;
//using System.Windows.Threading;
//using Microsoft.Practices.Prism.MefExtensions.Event;
//using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
//using Wlst.Cr.Core.AsyncTask;
//using Wlst.Cr.Core.CoreServices;
//using Wlst.Cr.Core.EventHandlerHelper;
//using Wlst.Cr.Core.UtilityFunction;
//using Wlst.Cr.CoreMims.Services;
//using Wlst.Cr.CoreOne.Services;
//using Wlst.Cr.PPProtocolSvrCnt.Common;
//using Wlst.Sr.EquipmentGroupInfoHolding.Models;

//using Wlst.Sr.EquipmentGroupInfoHolding.Services;
//using Wlst.client;
//using Wlst.mobile;


//namespace Wlst.Sr.EquipmentGroupInfoHolding.GrpSingleInfoHold
//{

//    public partial class GrpSingleInfoHoldExtend
//    {
//        /// <summary>
//        /// 所有分组信息
//        /// </summary>
//        public Dictionary<Tuple<int, int>, GroupInformation> InfoGroups = new Dictionary<Tuple<int, int>, GroupInformation>();

//        /// <summary>
//        /// 终端属于哪一个分组 终端-区域-分组
//        /// </summary>
//        public Dictionary<int, Tuple<int, int>> InfoRtuBelong = new Dictionary<int, Tuple<int, int>>();



//        protected bool BolLoad = false;

//        /// <summary>
//        /// 是否与服务器同步了数据
//        /// </summary>
//        protected bool BolGetServerReturn = false;

//        #region Simple Get informaiton


//        /// <summary>
//        /// 获取终端归属分组  区域-分组
//        /// </summary>
//        /// <param name="rtuId"></param>
//        /// <returns>存在返回  null</returns>
//        public Tuple< int ,int > GetRtuGrpBelong(int rtuId)
//        {
//            if (InfoRtuBelong.ContainsKey(rtuId)) return InfoRtuBelong[rtuId];
//            return null ;
//        }


//        /// <summary>
//        /// <para>获取列表</para>
//        /// </summary>
//        public List<GroupInformation> GrpInfoList()
//        {
//            return (from pair in InfoGroups orderby pair.Key ascending select pair.Value).ToList(); 
//        }

//        /// <summary>
//        /// <para>获取升序排列的列表</para>
//        /// </summary>
//        public List<GroupInformation> GrpInfoList(int areaId)
//        {
//            return (from t in InfoGroups where t.Key.Item1 == areaId orderby t.Value.Index ascending select t.Value).ToList();


//        }

//        #endregion




 

//    }



//    /// <summary>
//    /// 实现对分组信息的事件捕获与更新
//    /// </summary>
//    public partial class GrpSingleInfoHoldExtend //: GrpMultiInfoHoldingExtend
//    {

//        private bool _bolinitload = false;
//        /// <summary>
//        /// 程序初始化时必须执行一次
//        /// </summary>
//        internal void InitLoad()
//        {
//            if (_bolinitload) return;
//            _bolinitload = true;

//            this.InitAction();
//            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(RequestGroupInfo, 1);

//        }


//        private void InitAction()
//        {

//            ProtocolServer.RegistProtocol(Sr.ProtocolPhone.LxAreaGrp .wls_group_info ,
//                                       OnSvrGroupInfoArrive,
//                                       typeof(GrpSingleInfoHoldExtend), this);

//        }

//        private void OnSvrGroupInfoArrive(string session, MsgWithMobile infos)
//        {
//            if (infos.WstGroupInfo == null) return;
           
//            var grpInfoExchangefromServer = infos.WstGroupInfo;

//            var lstfromServer = grpInfoExchangefromServer.GroupItems;
//            if (lstfromServer == null) return;

//            //终端与分组的排序序号
//            int index = 1;
            
//            //分组信息更新
//            InfoGroups.Clear();
//            foreach (var t in grpInfoExchangefromServer.GroupItems)
//            {
//                var item = new GroupInformation(t) {Index = index++};

//                var tu = new Tuple<int, int>(t.AreaId, t.GroupId);
//                if (InfoGroups.ContainsKey(tu)) InfoGroups[tu] = item;
//                else InfoGroups.Add(tu, item);
//            }


//            //更新终端归属第一个最大分组的信息
//            //rtu-->grp
 
//            InfoRtuBelong.Clear();
//            foreach (var f in InfoGroups)
//            {
//                var tu = new Tuple<int, int>(f.Value.AreaId, f.Value.GroupId);
//                foreach (var g in f.Value.LstTml)
//                    if (!InfoRtuBelong.ContainsKey(g)) InfoRtuBelong.Add(g, tu);

//            }

 
//            var arg = new PublishEventArgs()
//                          {
//                              EventId = EventIdAssign.SingleInfoGroupAllNeedUpdate,
//                              EventType = PublishEventType.Core
//                          };
//            EventPublisher.EventPublish(arg);
//        }


 


//        /// <summary>
//        ///   请求服务器更新数据
//        /// </summary>
//        public void UpdateGroupsInfo(List<GroupInformation> groupinfo)
//        {
//            var ntg = (from t in groupinfo orderby t.Index ascending select t).ToList();
//            var info = Wlst.Sr.ProtocolPhone.LxAreaGrp .wls_group_info ;

//            foreach (var t in ntg)
//            {
//                info.WstGroupInfo.GroupItems.Add(new GroupItemsInfo.GroupItem()
//                {
//                    GroupName = t.GroupName,
//                    GroupId = t.GroupId,
//                    LstTml = t.LstTml,
//                    AreaId =t.AreaId 
 
//                });
//            }

//            info.WstGroupInfo.Op = 2;
//            SndOrderServer.OrderSnd(info, 10, 6);

//            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
//                0, "所有分组", Wlst.Cr.CoreMims.ShowMsgInfo.OperatrType.UserOperator, "更新分组信息");


//        }


//        /// <summary>
//        ///  请求比对数据 触发点
//        /// </summary>
//        public void RequestGroupInfo( )
//        {
//            BolGetServerReturn = false;
//            var info = Wlst.Sr.ProtocolPhone.LxAreaGrp .wls_group_info ;
//            info.WstGroupInfo.Op = 1;
//            SndOrderServer.OrderSnd(info, 10, 6);
//        }







//    }


//}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               