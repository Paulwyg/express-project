using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Ux.PrivilegesManage.Services;
using Wlst.client;
using Wlst.mobile;

namespace Wlst.Ux.PrivilegesManage.AreaManageViewModel.Services
{
    public partial class AreaInfoHold
    {
        /// <summary>
        /// 所有区域信息
        /// </summary>
        public Dictionary<int, AreaInfo.AreaItem> InfoAreas = new Dictionary<int, AreaInfo.AreaItem>();

        /// <summary>
        /// 终端属于哪一个区域
        /// </summary>
        public Dictionary<int, int> InfoRtuBelong = new Dictionary<int, int>();

        /// <summary>
        /// 区域分组与终端排序  终端、分组地址 --序号Index
        /// </summary>
        public Dictionary<int, int> InfoSortIndex = new Dictionary<int, int>();


        /// <summary>
        /// 数据库中表的最后更新时间
        /// </summary>
        protected long Time = 0;


        protected bool BolLoad = false;

        /// <summary>
        /// 是否与服务器同步了数据
        /// </summary>
        protected bool SyncOrNot = false;

        #region Simple Get informaiton


        /// <summary>
        /// 获取终端归属分组 最大的分组  
        /// </summary>
        /// <param name="rtuId"></param>
        /// <returns>存在返回  -1</returns>
        public int GetRtuGrpBelong(int rtuId)
        {
            if (InfoRtuBelong.ContainsKey(rtuId)) return InfoRtuBelong[rtuId];
            return -1;
        }


        /// <summary>
        /// <para>获取升序排列的列表</para>
        /// </summary>
        public List<AreaInfo.AreaItem> AreaInfoList
        {
            get { return (from pair in InfoAreas orderby pair.Key ascending select pair.Value).ToList(); }
        }


        /// <summary>
        /// 递归查阅该组下的所有终端
        /// </summary>
        /// <param name="grpId"></param>
        /// <returns></returns>
        public List<int> GetGrpTmlList(int areaId)
        {
            List<int> lstReturn = new List<int>();
            if (InfoAreas.ContainsKey(areaId))
            {

                foreach (var t in InfoAreas[areaId].LstTml)
                    lstReturn.AddRange(GetGrpTmlList(t));
            }
            return lstReturn;
        }


        #endregion

    }



    /// <summary>
    /// 实现对区域分组信息的事件捕获与更新
    /// </summary>
    public partial class AreaInfoHold //: GrpMultiInfoHoldingExtend
    {

        private bool _bolareainitload = false;
        /// <summary>
        /// 程序初始化时必须执行一次
        /// </summary>
        public void InitLoadArea()
        {
            if (_bolareainitload) return;
            _bolareainitload = true;

            this.InitActionArea();
            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(RequestAreaInfo, 1);

        }


        private void InitActionArea()
        {

            ProtocolServer.RegistProtocol(Sr.ProtocolPhone.LxAreaGrp.wls_area_info,
                                       OnSvrAreaInfoArrive,
                                       typeof(AreaInfoHold), this, true);

        }

        private void OnSvrAreaInfoArrive(string session, MsgWithMobile infos)
        {
            if (infos.WstAreagrpAreaInfo == null) return;

            var areaInfoExchangefromServer = infos.WstAreagrpAreaInfo;

            var lstfromServer = areaInfoExchangefromServer.AreaItems;
            if (lstfromServer == null) return;

            //终端与分组的排序序号
            //int index = 1;
            //InfoSortIndex.Clear();
            //foreach (var g in areaInfoExchangefromServer.AreaItems)
            //{
            //    if (!InfoSortIndex.ContainsKey(g)) InfoSortIndex.Add(g, index);
            //    index++;
            //}


            //区域分组信息更新
            InfoAreas.Clear();
            foreach (var t in lstfromServer)
            {
                var x = new AreaInfo.AreaItem()
                {
                    AreaName = t.AreaName,
                    AreaId = t.AreaId,
                    LstTml = t.LstTml,
                };
                if (InfoAreas.ContainsKey(x.AreaId)) continue;
                InfoAreas.Add(x.AreaId, x);
            }


            //更新终端归属区域的信息
            //rtu-->grp
            var rtuRef = new Dictionary<int, int>();

            InfoRtuBelong.Clear();
            foreach (var f in InfoAreas)
            {
                foreach (var g in f.Value.LstTml)
                    if (!rtuRef.ContainsKey(g)) 
                        rtuRef.Add(g, f.Key);

            }

            var arg = new PublishEventArgs()
            {
                EventId = EventIdAssign.SingleInfoAreaAllNeedUpdate,
                EventType = PublishEventType.Core
            };
            EventPublish.PublishEvent(arg);
        }


        /// <summary>
        ///   请求服务器更新数据
        /// </summary>
        public void UpdateAreaInfo(List<AreaInfo.AreaItem> areainfo, long timeNew)
        {

            var info = Wlst.Sr.ProtocolPhone.LxAreaGrp.wls_area_info;

            foreach (var t in areainfo)
            {
                info.WstAreagrpAreaInfo.AreaItems.Add(new AreaInfo.AreaItem()
                {
                    AreaName = t.AreaName,
                    AreaId = t.AreaId,
                    LstTml = t.LstTml,
                });
            }

            info.WstAreagrpAreaInfo.Op = 2;
            //info.WstAreaInfo.AddRange(indexx);
            SndOrderServer.OrderSnd(info, 10, 6);

            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                0, "所有分组", Wlst.Cr.CoreMims.ShowMsgInfo.OperatrType.UserOperator, "更新区域分组信息");
        }


        /// <summary>
        ///  请求比对数据 触发点
        /// </summary>
        /// <param name="qz">是否强制请求更新服务器数据</param>
        public void RequestAreaInfo(bool qz)
        {
            if (qz) Time = 0;
            SyncOrNot = false;
            var info = Wlst.Sr.ProtocolPhone.LxAreaGrp.wls_area_info;
            info.WstAreagrpAreaInfo.Op = 1;
            SndOrderServer.OrderSnd(info, 10, 6);
        }

        /// <summary>
        ///  请求比对数据 触发点
        /// </summary>
        /// <param name="qz">是否强制请求更新服务器数据</param>
        public void RequestAreaInfo()
        {
            RequestAreaInfo(false);
        }





    }
}
