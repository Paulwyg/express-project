using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Services;
using Wlst.Sr.EquipemntLightFault.Model;
using Wlst.client;
using Wlst.mobile;

namespace Wlst.Sr.EquipemntLightFault.Services
{
    /// <summary>
    /// 请求历史故障
    /// </summary>
    public class PreErrorServices
    {

        /// <summary>
        /// 请求历史故障
        /// </summary>
        /// <param name="dtStartTime">起始时间</param>
        /// <param name="dtEndtime">结束时间</param>
        /// <param name="level">故障等级   0-全部 10-推送显示  11-写数据库但不推送  13-推送并优先显示  </param>
        public static void ReqeustPreExistError(DateTime dtStartTime, DateTime dtEndtime, int level = 0)
        {
            var dts = new DateTime(dtStartTime.Year, dtStartTime.Month, dtStartTime.Day, 0, 0, 1);
            var dte = new DateTime(dtEndtime.Year, dtEndtime.Month, dtEndtime.Day, 23, 59, 59);
            var info = Wlst.Sr.ProtocolPhone.LxFault.wlst_fault_pre;
            info.WstFaultPre.RtuId = 0;
            info.WstFaultPre.DtEndTime = dte.Ticks;
            info.WstFaultPre.DtStartTime = dts.Ticks;

            info.Args.Cid = level;

            SndOrderServer.OrderSnd(info, 10, 6);
        }

        public static MsgWithMobile ReqeustPreExistErrorHttp(DateTime dtStartTime, DateTime dtEndtime, int pageIndex, int pagingFlag, int level = 0)
        {
            var dts = new DateTime(dtStartTime.Year, dtStartTime.Month, dtStartTime.Day, 0, 0, 1);
            var dte = new DateTime(dtEndtime.Year, dtEndtime.Month, dtEndtime.Day, 23, 59, 59);
            var info = Wlst.Sr.ProtocolPhone.LxFaultHttp.wlst_fault_pre_http;
            info.WstFaultPre.RtuId = 0;
            info.WstFaultPre.DtEndTime = dte.Ticks;
            info.WstFaultPre.DtStartTime = dts.Ticks;

            info.Args.Cid = level;
            info.Head.PagingIdx = pageIndex + 1;
            info.Head.PagingFlag = pagingFlag;
            var data = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.OrderSndHttp(info);
            return data;
        }

        /// <summary>
        /// 请求历史故障
        /// </summary>
        /// <param name="rtuId">设备地址</param>
        /// <param name="dtStartTime">起始时间</param>
        /// <param name="dtEndtime">结束时间</param>
        /// <param name="level">故障等级   0-全部 10-推送显示  11-写数据库但不推送  13-推送并优先显示  </param>
        public static void ReqeustPreExistError(List<int> rtuId, DateTime dtStartTime, DateTime dtEndtime, int level = 0)
        {
            if (rtuId.Count == 0) return;
            var dts = new DateTime(dtStartTime.Year, dtStartTime.Month, dtStartTime.Day, 0, 0, 1);
            var dte = new DateTime(dtEndtime.Year, dtEndtime.Month, dtEndtime.Day, 23, 59, 59);
            var info = Wlst.Sr.ProtocolPhone.LxFault .wlst_fault_pre ;
            info.WstFaultPre  .RtuId = rtuId[0];
            info.Args.Addr.AddRange(rtuId);
            info.WstFaultPre.DtEndTime = dte.Ticks;
            info.WstFaultPre.DtStartTime = dts.Ticks;

            info.Args.Cid = level;

            SndOrderServer.OrderSnd(info, 10, 6);
        }

        public static MsgWithMobile ReqeustPreExistErrorHttp(List<int> rtuId, DateTime dtStartTime, DateTime dtEndtime, int pageIndex, int pagingFlag, int level = 0)
        {
            if (rtuId.Count == 0) return null;
            var dts = new DateTime(dtStartTime.Year, dtStartTime.Month, dtStartTime.Day, 0, 0, 1);
            var dte = new DateTime(dtEndtime.Year, dtEndtime.Month, dtEndtime.Day, 23, 59, 59);
            var info = Wlst.Sr.ProtocolPhone.LxFaultHttp.wlst_fault_pre_http;
            info.WstFaultPre.RtuId = rtuId[0];
            info.Args.Addr.AddRange(rtuId);
            info.WstFaultPre.DtEndTime = dte.Ticks;
            info.WstFaultPre.DtStartTime = dts.Ticks;

            info.Args.Cid = level;
            info.Head.PagingIdx = pageIndex + 1;
            info.Head.PagingFlag = pagingFlag;
            var data = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.OrderSndHttp(info);
            return data;
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        /// <param name="dtStartTime">起始时间</param>
        /// <param name="dtEndtime">结束时间</param>
        /// <param name="faultIds">故障代码</param>
        /// <param name="level">故障等级   0-全部 10-推送显示  11-写数据库但不推送  13-推送并优先显示  </param>
        public static void ReqeustPreExistError(DateTime dtStartTime, DateTime dtEndtime, List<int> faultIds, int level = 0)
        {
            var tStartTime = new DateTime(dtStartTime.Year, dtStartTime.Month, dtStartTime.Day, 0, 0, 1);
            var tEndTime = new DateTime(dtEndtime.Year, dtEndtime.Month, dtEndtime.Day, 23, 59, 59);


            var info = Wlst.Sr.ProtocolPhone.LxFault .wlst_fault_pre ;
            info.WstFaultPre.RtuId = 0;
            info.WstFaultPre.DtEndTime = tEndTime.Ticks;
            info.WstFaultPre.DtStartTime = tStartTime.Ticks;
            info.WstFaultPre.FaultIds.AddRange(faultIds);

            info.Args.Cid = level;

            SndOrderServer.OrderSnd(info, 10, 6);
        }

        public static MsgWithMobile ReqeustPreExistErrorHttp(DateTime dtStartTime, DateTime dtEndtime, List<int> faultIds, int pageIndex, int pagingFlag, int level = 0)
        {
            var tStartTime = new DateTime(dtStartTime.Year, dtStartTime.Month, dtStartTime.Day, 0, 0, 1);
            var tEndTime = new DateTime(dtEndtime.Year, dtEndtime.Month, dtEndtime.Day, 23, 59, 59);


            var info = Wlst.Sr.ProtocolPhone.LxFaultHttp.wlst_fault_pre_http;
            info.WstFaultPre.RtuId = 0;
            info.WstFaultPre.DtEndTime = tEndTime.Ticks;
            info.WstFaultPre.DtStartTime = tStartTime.Ticks;
            info.WstFaultPre.FaultIds.AddRange(faultIds);

            info.Args.Cid = level;
            info.Head.PagingIdx = pageIndex + 1;
            info.Head.PagingFlag = pagingFlag;
            var data = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.OrderSndHttp(info);
            return data;
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        /// <param name="rtuId"> 设备地址</param>
        /// <param name="dtStartTime">起始时间</param>
        /// <param name="dtEndtime">结束时间</param>
        /// <param name="faultIds">故障代码</param>
        /// <param name="level">故障等级   0-全部 10-推送显示  11-写数据库但不推送  13-推送并优先显示  </param>
        public static void ReqeustPreExistError(List<int> rtuId, DateTime dtStartTime, DateTime dtEndtime, List<int> faultIds, int level = 0)
        {
            if (rtuId.Count == 0) return;
            var tStartTime = new DateTime(dtStartTime.Year, dtStartTime.Month, dtStartTime.Day, 0, 0, 1);
            var tEndTime = new DateTime(dtEndtime.Year, dtEndtime.Month, dtEndtime.Day, 23, 59, 59);


            var info = Wlst.Sr.ProtocolPhone.LxFault  .wlst_fault_pre ;
            info.WstFaultPre.RtuId = rtuId[0];
            info.Args.Addr.AddRange(rtuId);
            info.WstFaultPre.DtEndTime = tEndTime.Ticks;
            info.WstFaultPre.DtStartTime = tStartTime.Ticks;
            info.WstFaultPre.FaultIds.AddRange(faultIds); // = faultId;

            info.Args.Cid = level;

            SndOrderServer.OrderSnd(info, 10, 6);
        }

        public static MsgWithMobile ReqeustPreExistErrorHttp(List<int> rtuId, DateTime dtStartTime, DateTime dtEndtime, List<int> faultIds, int pageIndex, int pagingFlag, int level = 0)
        {
            if (rtuId.Count == 0) return null;
            var tStartTime = new DateTime(dtStartTime.Year, dtStartTime.Month, dtStartTime.Day, 0, 0, 1);
            var tEndTime = new DateTime(dtEndtime.Year, dtEndtime.Month, dtEndtime.Day, 23, 59, 59);


            var info = Wlst.Sr.ProtocolPhone.LxFaultHttp.wlst_fault_pre_http;
            //info.WstFaultPre.RtuId = rtuId[0];
            info.WstFaultPre.RtuIds.Add(rtuId[0]);
            info.Args.Addr.AddRange(rtuId);
            info.WstFaultPre.DtEndTime = tEndTime.Ticks;
            info.WstFaultPre.DtStartTime = tStartTime.Ticks;
            info.WstFaultPre.FaultIds.AddRange(faultIds); // = faultId;

            info.Args.Cid = level;
            info.Head.PagingIdx = pageIndex + 1;
            info.Head.PagingFlag = pagingFlag;
            var data = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.OrderSndHttp(info);
            return data;
        }
       
        /// <summary>
        /// 请求故障发生时候的数据
        /// </summary>
        /// <param name="rtuId"></param>
        /// <param name="loopId"></param>
        /// <param name="dataCreateId"></param>
        public static  void RequestDataWhenErrorHappen(int rtuId, int loopId, long dataCreateId)
        {
            var nt = Wlst.Sr.ProtocolPhone.LxFault .wlst_fault_occu_data ;
            nt.WstFaultOccuData  .LoopId  = loopId;
            nt.WstFaultOccuData.DataRecordId = dataCreateId;
            nt.WstFaultOccuData.RtuId = rtuId;
            SndOrderServer.OrderSnd(nt, 10, 2);
        }

        /// <summary>
        /// 请求在时间时刻之前最近的一条故障 为苏州园区特殊定制
        /// </summary>
        /// <param name="rtuId"></param>
        /// <param name="dtTime"></param>
        /// <param name="faultIds"></param>
        public static void RequestErrAtSomeTime(int rtuId, DateTime dtTime,  List<int> faultIds ,bool isPre)
        {
            var tTime = new DateTime(dtTime.Year, dtTime.Month, dtTime.Day, 0, 0, 1);
           

            var info = Wlst.Sr.ProtocolPhone.LxFault.wlst_fault_pre_for_single;
            info.WstFaultPreForSingle.RtuId = rtuId;
            info.WstFaultPreForSingle.IsPre = isPre;
            info.WstFaultPreForSingle.FaultId.AddRange(faultIds);
            info.WstFaultPreForSingle.DtTime = tTime.Ticks;
            SndOrderServer.OrderSnd(info, 10, 6);
        }
        /// <summary>
        /// 根据自定义时间间隔   统计现存故障 为苏州园区特殊定制
        /// </summary>
        /// <param name="hourDiff"></param>

        public static void RequestErrCountBetweenSomeTime(int hourDiff, List<EquipmentFaultCurr.OneFaultItem> lstTemp )
        {
            var info = Wlst.Sr.ProtocolPhone.LxFault.wlst_fault_curr_time_cal;
            info.WstFaultCurrForTimeCal.HourDiff = hourDiff;
            info.WstFaultCurrForTimeCal.FaultItems.AddRange(lstTemp);
            SndOrderServer.OrderSnd(info, 10, 6);
        }
    }
}
