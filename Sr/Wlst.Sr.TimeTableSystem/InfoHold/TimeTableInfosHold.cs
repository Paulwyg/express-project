using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.UtilityFunction;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreMims.TopDataInfo;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.TimeTableSystem.Models;
using Wlst.Sr.TimeTableSystem.Services.IdServices;
using Wlst.client;
using Wlst.mobile;

namespace Wlst.Sr.TimeTableSystem.InfoHold
{

    /// <summary>
    /// 为数据持有提供基础服务;key为时间表Id   数据为一周时间表数据 仅保持一周数据  
    /// </summary>
    public partial class TimeTableInfosHold : Wlst.Cr.Core.EventHandlerHelper.EventHandlerHelperExtendNotifyProperyChanged
    {
        /// <summary>
        /// 提供数据持有的数据结构 area_id -  rtuOrGrpId - timetable
        /// </summary>
        protected Dictionary<Tuple<int, int>, TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem> InfoTimeItes;


        /// <summary>
        /// 提供数据持有的数据结构 area_id -  rtuOrGrpId - timetable  新协议  lvf 2019年6月19日10:01:12
        /// </summary>
        protected Dictionary<Tuple<int, int>, TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem> InfoTimeItemsNew;


        #region 提供外部对数据的操作Get Set

        /// <summary>
        /// <para>任何使用此数据务必注意 此数据为原始数据，___只允许读不允许修改___ </para> 
        /// <para>任何修改会使原始数据被修改形成脏数据 </para>
        /// </summary>
        public Dictionary<Tuple<int, int>, TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem>
            InfoTimeTableDictionary
        {
            get { return InfoTimeItes; } //将原始数据返回  数据安全性无法保证
        }

        /// <summary>
        /// 不存在返回null
        /// </summary>
        /// <param name="id"></param>
        /// <returns>不存在返回null</returns>
        public TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem GetInfoTimeTableById(int area, int id)
        {
            var tu = new Tuple<int, int>(area,id);
            if (InfoTimeItes.ContainsKey(tu))
            {
                return InfoTimeItes[tu];
            }
            return null;
        }

        /// <summary>
        /// <para>获取升序排列的列表</para>
        /// <para>任何使用此数据务必注意 此数据为原始数据，___只允许读不允许修改___  </para>
        /// <para>任何修改会使原始数据被修改形成脏数据 </para>
        /// </summary>
        public List<TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem> GetInfoTimeTableList(int AreaId)
        {
                var lstReturn = new List<TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem>();
                var result = from pair in InfoTimeItes where pair.Key.Item1 == AreaId  orderby pair.Key select pair;
                foreach (var p in result)
                {
                    //将原始数据的地址赋给返回list 共享原始数据   数据安全性无法保证
                    lstReturn.Add(p.Value);
                }
                return lstReturn;
           
        }

        #endregion


        /// <summary>
        /// 提供数据持有的数据结构  
        /// </summary>
        protected Dictionary<Tuple< int ,int >, Dictionary<int, int>> InfoBanding;


        /// <summary>
        /// 提供数据持有的数据结构  新协议  lvf 2019年6月19日08:39:52
        /// </summary>
        protected Dictionary<Tuple<int, int>, Dictionary<int, int>> InfoBandingNew;

        #region TmlWeekTimeTaleBelongInfomation

        /// <summary>
        /// 任何使用此数据务必注意 此数据为原始数据，___只允许读不允许修改___  
        /// 任何修改会使原始数据被修改形成脏数据 areaId - rtuOrGrpId ----  loop - timetable
        /// </summary>
        public Dictionary<Tuple<int, int>, Dictionary<int, int>> BandingInfoDictionary
        {
            get { return InfoBanding; //将原始数据返回  数据安全性无法保证
            }
        }

        /// <summary>
        /// 不存在则返回null
        /// </summary>
        /// <param name="tmlorgrpId"></param>
        /// <returns></returns>
        public Dictionary<int, int> GetBandingInfo(int areaId, int tmlorgrpId)
        {
            var tu = new Tuple<int, int>(areaId, tmlorgrpId);
            if (InfoBanding.ContainsKey(tu))
            {
                return InfoBanding[tu];
            }
            return null;
        }

        /// <summary>
        /// 不存在则返回-1
        /// </summary>
        /// <param name="tmlorgrpId"></param>
        /// <param name="loop">1 ~6 </param>
        /// <returns>-1</returns>
        public int GetBandingInfo(int areaId, int tmlorgrpId, int loop)
        {
            var tu = new Tuple<int, int>(areaId, tmlorgrpId);
            if (InfoBanding.ContainsKey(tu ) && InfoBanding[tu ].ContainsKey(loop))
            {
                return InfoBanding[tu ][loop];
            }
            return -1;
        }

        /// <summary>
        /// 不存在则返回-1
        /// </summary>
        /// <param name="tmlorgrpId"></param>
        /// <param name="loop">1 ~6 </param>
        /// <returns>-1</returns>
        public int GetBandingInfoNew(int areaId, int tmlorgrpId, int loop)
        {
            var tu = new Tuple<int, int>(areaId, tmlorgrpId);
            if (InfoBandingNew.ContainsKey(tu) && InfoBandingNew[tu].ContainsKey(loop))
            {
                return InfoBandingNew[tu][loop];
            }
            return -1;
        }


        /// <summary>
        /// 升序排列的数据
        /// </summary>
        /// <returns></returns>
        public List<Dictionary<int, int>> GetBandingInfoList
        {
            get
            {
                var lstReturn = new List<Dictionary<int, int>>();
                var result = from pair in InfoBanding orderby pair.Key select pair;
                foreach (var p in result)
                {
                    //将原始数据的地址赋给返回list 共享原始数据   数据安全性无法保证
                    lstReturn.Add(p.Value);
                }
                return lstReturn;
            }
        }

        #endregion

        /// <summary>
        /// <para>获取绑定到本时间表的所有终端;</para>
        /// <para>返回为lst 第一个数据为终端或组地址;</para>
        /// <para>第二个数据为终端回路编号;</para>
        /// </summary>
        /// <param name="timetableid">时间表ID</param>
        /// <returns>终端列表 不会为null的 </returns>
        public List<Tuple<int, int>> GetBangdingToThisTimeTablesTmls(int areaId, int timetableid)
        {
            List<Tuple<int, int>> lstReturn = new List<Tuple<int, int>>();
 
            foreach (var t in InfoBanding)
            {
                foreach (var tt in t.Value)
                {
                    if (tt.Value == timetableid && t.Key.Item1  ==areaId )
                    {
                        Tuple<int, int> tu = new Tuple<int, int>(t.Key.Item2 , tt.Key);
                        lstReturn.Add(tu);
                    }
                }
            }
            return lstReturn;
        }
    }

    /// <summary>
    /// 为数据持有提供基础服务
    /// </summary>
    public partial class TimeTableInfosHold
    {
        public void InitStart()
        {
            InitAction();

            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(RequestWeekTimeTableInfo, 1);
            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(RequestEventTaskInfo, 1);
        }



        public TimeTableInfosHold()
        {
            this.AddEventFilterInfo(100, PublishEventType.ReCn);
            InfoTimeItes =
                new Dictionary<Tuple<int, int>, TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem>();
            InfoBanding = new Dictionary<Tuple< int ,int >, Dictionary<int, int>>();

            InfoBandingNew = new Dictionary<Tuple<int, int>, Dictionary<int, int>>();
            InfoTimeItemsNew = new Dictionary<Tuple<int, int>, TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem>();
        }

        public override void ExPublishedEvent(PublishEventArgs args)
        {
            if (args.EventType == PublishEventType.ReCn)
            {
                this.RequestWeekTimeTableInfo();
                this.RequestEventTaskInfo();
            }
        }


        /// <summary>
        /// 请求时间时间表;
        /// </summary>
        public void RequestWeekTimeTableInfo()
        {
            var info = Wlst.Sr.ProtocolPhone.LxRtuTime .wst_timetable_set ;//.wlst_cnt_request_timetable_info;
            info.WstRtutimeTimetableSet.Op = 1;
            SndOrderServer.OrderSnd(info, 10, 6);


            //新协议 cs  lvf 2019年6月18日18:41:22   请求时间表
            var infos = Wlst.Sr.ProtocolPhone.LxRtuTime.wst_timetable_set_new;//.wlst_cnt_request_timetable_info;
            infos.WstRtutimeTimetableSetnew.Op = 1;
            SndOrderServer.OrderSnd(infos, 10, 6);

            //新协议 cs  lvf 2019年6月18日18:41:22 请求绑定信息
            var infoss = Wlst.Sr.ProtocolPhone.LxRtuTime.wst_timetable_set_bandingnew;//.wlst_cnt_request_timetable_info;
            infoss.WstRtutimeTimetableSetbandingnew.Op = 1;
            SndOrderServer.OrderSnd(infoss, 10, 6);

        }


        /// <summary>
        /// 请求时间时间表;
        /// </summary>
        public void RequestEventTaskInfo()
        {
            var info = Wlst.Sr.ProtocolPhone.LxRtuTime .wst_timetable_next_execute_info ;//.wlst_cnt_request_timetable_next_execute_info;//.ServerPart.wlst_TimeTable_clinet_request_timetableevent;
       
            SndOrderServer.OrderSnd(info, 10, 6);




        }

    }



    /// <summary>
    /// Event
    /// </summary>
    public partial class TimeTableInfosHold
    {


        public void InitAction()
        {
            ProtocolServer.RegistProtocol(Sr.ProtocolPhone.LxRtuTime .wst_timetable_set ,
                                          OnRequestTimeTableInfo,
                                          typeof (TimeTableInfosHold), this);

            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone .LxRtuTime .wst_timetable_next_execute_info ,// .wlst_svr_ans_cnt_request_timetable_next_execute_info ,//.ClientPart.wlst_TimeTable_server_ans_clinet_request_timetableevent,
                timetableNextExecuteInfo,
                typeof(TimeTableInfosHold), this);


            ProtocolServer.RegistProtocol(Sr.ProtocolPhone.LxRtuTime.wst_timetable_set_new,
                              OnRequestTimeTableInfoNew,
                              typeof(TimeTableInfosHold), this);


            ProtocolServer.RegistProtocol(Sr.ProtocolPhone.LxRtuTime.wst_timetable_set_bandingnew,
                              OnRequestTimeTableBandingInfo,
                              typeof(TimeTableInfosHold), this);


        }


        private void OnRequestTimeTableInfo(string session, MsgWithMobile infos)
        {
            var info = infos.WstRtutimeTimetableSet;
            if (info == null) return;

            var x = AddOrUpdateInfo(info);
            if (x == false) return;

            if (info.Op == 1)
            {
                var args = new PublishEventArgs()
                               {
                                   EventType = PublishEventType.Core,
                                   EventId = EventIdAssign.TimeTimeRequest
                               };
                EventPublish.PublishEvent(args);
            }
            if (info.Op == 2)
            {
                var args = new PublishEventArgs()
                {
                    EventType = PublishEventType.Core,
                    EventId = EventIdAssign.TimeTimeUpdate
                };
                EventPublish.PublishEvent(args);
            }
        }


        private bool AddOrUpdateInfo(TimeTableInfoWithRtuOrGrpBandingInfo info)
        {
            if (info == null) return false;
            if (info.RtuOrGrpTimeTableAndBandingItems == null) return false;
            if (info.Op == 1)
            {
                InfoTimeItes.Clear();
                InfoBanding.Clear();
            }

            foreach (var fg in info.RtuOrGrpTimeTableAndBandingItems)
            {
                int areaId = fg.AreaId;
                var removedItem = (from t in InfoTimeItes where t.Key.Item1 == areaId select t.Key).ToList();
                foreach (var f in removedItem) if (InfoTimeItes.ContainsKey(f)) InfoTimeItes.Remove(f);


                //if (fg.RtuOrGrpBndTimeTableItems.Count == 0) continue;
                if (fg.TimeTableItems.Count == 0) continue;


                foreach (var f in fg.TimeTableItems)
                {
                    var tu = new Tuple<int, int>(fg.AreaId, f.TimeId);
                    if (InfoTimeItes.ContainsKey(tu)) InfoTimeItes[tu] = f;
                    else InfoTimeItes.Add(tu, f);
                }

                //InfoBanding.Clear();
                var keysClean = (from t in InfoBanding where t.Key.Item1 == areaId select t.Key).ToList();
                foreach (var f in keysClean)
                {
                    if (InfoBanding.ContainsKey(f)) InfoBanding.Remove(f);
                }

                foreach (var f in fg.RtuOrGrpBndTimeTableItems)
                {
                    var tu = new Tuple<int, int>(areaId, f.RtuOrGrpId);
                    if (!InfoBanding.ContainsKey(tu))
                        InfoBanding.Add(tu, new Dictionary<int, int>());
                    if (InfoBanding[tu].ContainsKey(f.RtuOrGrpLoopId))
                        InfoBanding[tu][f.RtuOrGrpLoopId] = f.TimeTableId;
                    else InfoBanding[tu][f.RtuOrGrpLoopId] = f.TimeTableId;
                }
            }
            return true;
        }


        /// <summary>
        /// 新协议，获取时间表绑定信息
        /// </summary>
        /// <param name="session"></param>
        /// <param name="infos"></param>
        private void OnRequestTimeTableBandingInfo(string session, MsgWithMobile infos)
        {
            var info = infos.WstRtutimeTimetableSetbandingnew;
            if (info == null) return;
            //todo
            var x = AddOrUpdateBandingInfoNew(info);
            if (x == false) return;

            if (info.Op == 1)
            {
                var args = new PublishEventArgs()
                {
                    EventType = PublishEventType.Core,
                    EventId = EventIdAssign.TimeTimeRequest
                };
                EventPublish.PublishEvent(args);
            }
            if (info.Op == 2)
            {
                var args = new PublishEventArgs()
                {
                    EventType = PublishEventType.Core,
                    EventId = EventIdAssign.TimeTimeUpdate
                };
                EventPublish.PublishEvent(args);
            }
        }

        /// <summary>
        /// 处理新协议绑定关系 lvf 2019年6月19日09:39:09
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private bool AddOrUpdateBandingInfoNew(TimeTableBandingRtuInfoNew info)
        {
            if (info == null) return false;
            if (info.RtuOrGrpTimeTableAndBandingItems == null) return false;
            //if (info.Op == 1)
            //{
                InfoBandingNew.Clear();
            //}
            int areaId = info.AreaId;
            int rtuOrGrpid = 0;

            //遍历 绑定关系
            foreach (var fg in info.RtuOrGrpTimeTableAndBandingItems)
            {
                rtuOrGrpid = fg.RtuOrGrpId;
                var tu = new Tuple<int, int>(areaId, fg.RtuOrGrpId);

                //if (InfoBandingNew.ContainsKey(tu)) InfoBandingNew.Remove(tu);

                foreach (var f in fg.SwitchOutBandingTimeTableInfo)
                {

                    if (!InfoBandingNew.ContainsKey(tu))
                        InfoBandingNew.Add(tu, new Dictionary<int, int>());

                    if (InfoBandingNew[tu].ContainsKey(f.LoopId))
                        InfoBandingNew[tu][f.LoopId] = f.TimeTableId;
                    else InfoBandingNew[tu][f.LoopId] = f.TimeTableId;
                }
            }
            return true;
        }

        /// <summary>
        /// 新协议，获取时间表信息
        /// </summary>
        /// <param name="session"></param>
        /// <param name="infos"></param>
        private void OnRequestTimeTableInfoNew(string session, MsgWithMobile infos)
        {
            var info = infos.WstRtutimeTimetableSetnew;
            if (info == null) return;
            //todo
            var x = AddOrUpdateTimeInfoNew(info);
            if (x == false) return;

            if (info.Op == 1)
            {
                var args = new PublishEventArgs()
                {
                    EventType = PublishEventType.Core,
                    EventId = EventIdAssign.TimeTimeRequest
                };
                EventPublish.PublishEvent(args);
            }
            if (info.Op == 2)
            {
                var args = new PublishEventArgs()
                {
                    EventType = PublishEventType.Core,
                    EventId = EventIdAssign.TimeTimeUpdate
                };
                EventPublish.PublishEvent(args);
            }
        }

        /// <summary>
        /// 处理新协议时间表信息 lvf 2019年6月19日09:39:09 暂时用原数据结构，如果不通用则用独立的
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private bool AddOrUpdateTimeInfoNew(TimeTableInfoNew info)
        {
            if (info == null) return false;
            if (info.TimeTableItems == null) return false;
            if (info.Op == 1)
            {
                InfoTimeItes.Clear();
            }
            int areaId = info.AreaId;
            
            foreach (var f in info.TimeTableItems)
            {
                var tu = new Tuple<int, int>(areaId, f.TimeId);
                if (InfoTimeItes.ContainsKey(tu)) InfoTimeItes[tu] = f;
                else InfoTimeItes.Add(tu, f);
            }


            return true;
        }




        #region  timetable event

        public void timetableNextExecuteInfo(string session,Wlst .mobile .MsgWithMobile  infos)
        {
            var info = infos.WstRtutimeTimetableNextExecuteInfo;
            if (info == null) return;
      

            string mainInfo = null;
            string tooltips = "" + Environment.NewLine;
            List<TimeTableEventRequest.TimeTableNextOperator> orderinfo = (from t in info.Info orderby t.OperatorTime select t).ToList();
            if (orderinfo.Count < 1)
            {
                TopDataInfoServers.MySelf.UpdateDataInfo(null, null, 3);
                return;
            }

            var dtone = new DateTime(orderinfo[0].OperatorTime);
            mainInfo = GetTimeTableName(orderinfo[0].AreaId, orderinfo[0].TimeTableId) + "-最后时限：" + string.Format("{0:D2}", dtone.Hour) + ":" +
                       string.Format("{0:D2}", dtone.Minute) + ":" + string.Format("{0:D2}", dtone.Second);

            foreach (var t in orderinfo)
            {
                DateTime nt = new DateTime(t.OperatorTime);
                tooltips += "下次最晚执行时间: ---- " + string.Format("{0:D2}", nt.Month) + "-" +
                            string.Format("{0:D2}", nt.Day) +
                            " " + string.Format("{0:D2}", nt.Hour) + ":" + string.Format("{0:D2}", nt.Minute) + ":" +
                            string.Format("{0:D2}", nt.Second) + " ----" +
                            Environment.NewLine;
                tooltips += "任务生成时间: " + new DateTime(t.CreateTime).ToString("yyyy-MM-dd HH:mm:ss") +
                            Environment.NewLine;

                tooltips += "时间表地址: " + t.TimeTableId + Environment.NewLine;
                //  tooltips += "时间表名称: " + GetTimeTableName(t.TimeTableId) + Environment.NewLine;
                var ffff = Services.WeekTimeTableInfoService.GeteekTimeTableInfo(t.AreaId,t.TimeTableId);
                if (ffff != null)
                {
                    tooltips += "时间表名称: " + ffff.TimeName + Environment.NewLine;
                    tooltips += "时间表描述: " + ffff.TimeDesc + Environment.NewLine;
                }

                tooltips += "执行的操作: " + (t.IsOpenLight ? "开灯" : "关灯") + Environment.NewLine;
                tooltips += "是否为光控: " + (t.IsLightControl ? "是" : "否") + Environment.NewLine;
                if (t.IsLightControl)
                {
                    tooltips += "使用的光控地址: " + t.LuxId + Environment.NewLine;
                    tooltips += "执行操作光控值: " + t.LightControlValue + Environment.NewLine;

                    var intinfox = nt.Hour*60 + nt.Minute - t.LightControlRange;
                    var starthour = intinfox/60;
                    var startminute = intinfox%60;
                    tooltips += "光控起作用的时间段: " + string.Format("{0:D2}", starthour) + ":" +
                                string.Format("{0:D2}", startminute) + " -- " + string.Format("{0:D2}", nt.Hour) +
                                ":" + string.Format("{0:D2}", nt.Minute) + Environment.NewLine;
                }
                tooltips += Environment.NewLine;
            }

            TopDataInfoServers.MySelf.UpdateDataInfo(mainInfo, tooltips, 3);
        }


        private string GetTimeTableName(int area,int timetableid)
        {
            var tmp = this.GetInfoTimeTableById(area,timetableid);
            return tmp == null ? "时间表" : tmp.TimeName;
        }

        #endregion

    }

    /// <summary>
    /// Socket to Server
    /// </summary>
    public partial class TimeTableInfosHold
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstTimeTables"></param>
        /// <param name="lstUpdateRtuOrGrpBanding">第一个参数为终端或组地址，第二个参数为终端或组回路，第三个参数为时间表地址</param>
        public void UpdateTimeTableInfos(int areaId,
            List<TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem> lstTimeTables,
            List<Tuple<int, int, int>> lstUpdateRtuOrGrpBanding)
        {
            if (lstTimeTables.Count == 0 || lstUpdateRtuOrGrpBanding.Count == 0) return;
            var info = Wlst.Sr.ProtocolPhone.LxRtuTime .wst_timetable_set ;
            info.WstRtutimeTimetableSet.Op = 2;
            var tmp = new TimeTableInfoWithRtuOrGrpBandingInfo.RtuOrGrpTimeTableAndBandingItem()
                          {
                              RtuOrGrpBndTimeTableItems =new List<TimeTableInfoWithRtuOrGrpBandingInfo.RtuOrGrpBandingTimeTableItem>( ),
                              TimeTableItems =new List<TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem>( )
                          };

           tmp.AreaId  = areaId;
            foreach (var f in lstTimeTables)
            {
               tmp .TimeTableItems.Add(f);
            }
            foreach (var f in lstUpdateRtuOrGrpBanding)
            {
              tmp .RtuOrGrpBndTimeTableItems.Add(new TimeTableInfoWithRtuOrGrpBandingInfo.RtuOrGrpBandingTimeTableItem() 
                                                                                 {
                                                                                     RtuOrGrpId = f.Item1,
                                                                                     RtuOrGrpLoopId  = f.Item2,
                                                                                     TimeTableId = f.Item3
                                                                                 });
            }
            info.WstRtutimeTimetableSet.RtuOrGrpTimeTableAndBandingItems.Add(tmp);
            SndOrderServer.OrderSnd(info, 10, 6);



            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                0, "周设置修改", Wlst.Cr.CoreMims.ShowMsgInfo.OperatrType.UserOperator, "更新周设置");
        }


        //只保存  lvf 2019年6月20日10:11:04
        public void UpdateTimeTableInfosNew(int areaId,
    List<TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem> lstTimeTables)
        {
            if (lstTimeTables.Count == 0 ) return;
            var info = Wlst.Sr.ProtocolPhone.LxRtuTime.wst_timetable_set_new;
            info.WstRtutimeTimetableSetnew.Op = 2;
            var TimeTableItems = new List<TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem>();
            info.WstRtutimeTimetableSetnew.AreaId = areaId;
            foreach (var f in lstTimeTables)
            {
                TimeTableItems.Add(f);
            }
        
            info.WstRtutimeTimetableSetnew.TimeTableItems =TimeTableItems;
            SndOrderServer.OrderSnd(info, 10, 6);



            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                0, "周设置修改", Wlst.Cr.CoreMims.ShowMsgInfo.OperatrType.UserOperator, "更新周设置");
        }


        //private Wlst.client.TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem  GetInfoBy(
        //    TimeTableInfomationItem info)
        //{
        //    var rtn = new Wlst.client.TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem() 
        //                  {
        //                      //LastSaturdayOpenCloseControl =new TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem.TimeTableOneDayItem() 

        //                      //        {
        //                      //            DateDay = info.LastSaturdayOpenCloseControl.DateDay,
        //                      //            DateMonth = info.LastSaturdayOpenCloseControl.DateMonth,
        //                      //            DayOfWeek = info.LastSaturdayOpenCloseControl.DateDay,
        //                      //            IsLightOffOffSetOn = info.LastSaturdayOpenCloseControl.IsLightOffOffSetOn,
        //                      //            IsLightOnOffSetOn = info.LastSaturdayOpenCloseControl.IsLightOnOffSetOn,
        //                      //            IsLuxOff = info.LastSaturdayOpenCloseControl.IsLuxOff,
        //                      //            IsLuxOn = info.LastSaturdayOpenCloseControl.IsLuxOn,
        //                      //            TimeOff = info.LastSaturdayOpenCloseControl.TimeOff,
        //                      //            TimeOn = info.LastSaturdayOpenCloseControl.TimeOn
        //                      //        },
        //                      LightOffOffset = info.LightOffOffset,
        //                      LightOnOffset = info.LightOnOffset,
        //                   //   LstOneWeekOpenCloseControl =new List<TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem.TimeTableOneDayItem>( ),
        //                      RuleItems  = new List<TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem.TimeTableSectionRule>( ),
        //                   LuxEffective = info.LuxEffective,
        //                      LuxId = info.LuxId,
        //                      LuxOffValue = info.LuxOffValue,
        //                      LuxOnValue = info.LuxOnValue,
        //                      TimeDesc = info.TimeDesc,
        //                      TimeId = info.TimeId,
        //                      TimeName = info.TimeName
        //                  };
        //    foreach (var f in info.LstOneWeekOpenCloseControl)
        //    {
        //        rtn.RuleItems.Add(new TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem.TimeTableSectionRule() 
        //                                               {
        //                                                   //DateDay = f.DateDay,
        //                                                   //DateMonth = f.DateMonth,
        //                                                   //DayOfWeek = f.DayOfWeek,
        //                                                   //IsLightOffOffSetOn = f.IsLightOffOffSetOn,
        //                                                   //IsLightOnOffSetOn = f.IsLightOnOffSetOn,
        //                                                   //IsLuxOff = f.IsLuxOff,
        //                                                   //IsLuxOn = f.IsLuxOn,
        //                                                   TimeOff = f.TimeOff,
        //                                                   TimeOn = f.TimeOn,
        //                                                   DateEnd =1231,
        //                                                   DateStart =101,
        //                                                   RuleId =1,
        //                                                   RuleSectionId =1,
        //                                                   TimetableSectionId =1,
        //                                                   TypeOff =f.IsLuxOff  ?1:f.IsLightOffOffSetOn ?2:3,
        //                                                   TypeOn = f.IsLuxOn  ? 1 : f.IsLightOnOffSetOn ? 2 : 3,

        //                                               });
        //    }
        //    return rtn;
        //}

        //private TimeTableInfomationItem GetInfoBy(
        //    Wlst.client.TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem  info)
        //{
        //    var rtn = new TimeTableInfomationItem()
        //                  {
        //                      //LastSaturdayOpenCloseControl =
        //                      //    new TimeTableOneDayInfomationItem()
        //                      //        {
        //                      //            DateDay = info.LastSaturdayOpenCloseControl.DateDay,
        //                      //            DateMonth = info.LastSaturdayOpenCloseControl.DateMonth,
        //                      //            DayOfWeek = info.LastSaturdayOpenCloseControl.DateDay,
        //                      //            IsLightOffOffSetOn = info.LastSaturdayOpenCloseControl.IsLightOffOffSetOn,
        //                      //            IsLightOnOffSetOn = info.LastSaturdayOpenCloseControl.IsLightOnOffSetOn,
        //                      //            IsLuxOff = info.LastSaturdayOpenCloseControl.IsLuxOff,
        //                      //            IsLuxOn = info.LastSaturdayOpenCloseControl.IsLuxOn,
        //                      //            TimeOff = info.LastSaturdayOpenCloseControl.TimeOff,
        //                      //            TimeOn = info.LastSaturdayOpenCloseControl.TimeOn
        //                      //        },
        //                      LightOffOffset = info.LightOffOffset,
        //                      LightOnOffset = info.LightOnOffset,
        //                      LstOneWeekOpenCloseControl =
        //                          new List<TimeTableOneDayInfomationItem>(),
        //                      LuxEffective = info.LuxEffective,
        //                      LuxId = info.LuxId,
        //                      LuxOffValue = info.LuxOffValue,
        //                      LuxOnValue = info.LuxOnValue,
        //                      TimeDesc = info.TimeDesc,
        //                      TimeId = info.TimeId,
        //                      TimeName = info.TimeName
        //                  };
        //    foreach (var f in info.RuleItems )
        //    {
        //        rtn.LstOneWeekOpenCloseControl.Add(new TimeTableOneDayInfomationItem()
        //                                               {
        //                                                       //DateDay = f.DateDay,
        //                                                       //DateMonth = f.DateMonth,
        //                                                       //DayOfWeek = f.DayOfWeek,
        //                                                       //IsLightOffOffSetOn = f.IsLightOffOffSetOn,
        //                                                       //IsLightOnOffSetOn = f.IsLightOnOffSetOn,
        //                                                       //IsLuxOff = f.IsLuxOff,
        //                                                       //IsLuxOn = f.IsLuxOn,
        //                                                   TimeOff = f.TimeOff,
        //                                                   TimeOn = f.TimeOn
        //                                               });
        //    }
        //    return rtn;
        //}
    }
}
