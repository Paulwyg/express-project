using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Sr.EquipemntLightFault.InfoHold;
using Wlst.Sr.EquipemntLightFault.Model;


namespace Wlst.Sr.EquipemntLightFault.Services
{
    /// <summary>
    /// 所有设备存在的故障信息
    /// </summary>
    public partial  class TmlExistFaultsInfoServices
    {
        private static TmlExistFaultsInfo _info = new TmlExistFaultsInfo();

        public TmlExistFaultsInfoServices()
        {
            _info.InitStartSerive();
        }
        internal  static void OnUserInviOrTypeChange()
        {
            _info.OnTypeOrUserIndividuationChange();
        }

        internal static List<int> GetSluLampErrs(int sluId, int ctrlId, int lampId = 0)
        {
            return (from t in Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary
                    where
                        t.Value.IsThisUserShow && t.Value.RtuId == sluId && t.Value.LoopId == ctrlId &&
                        t.Value.LampId > 0
                    select t.Value.FaultId - 54).ToList();
                //55-功率越上限 56功率越下限  57灯具漏电 58光源故障  59补偿电容故障 60意外灭灯  61意外亮灯  62自熄灯 63 控制器断电告警  64 继电器故障
        }

        /// <summary>
        /// <para>任何使用此数据务必注意 此数据为原始数据，___只允许读不允许修改___ </para> 
        /// <para>任何修改会使原始数据被修改形成脏数据 </para>
        /// <para>修改请用SupperEquipmentInstanceContains 中具体数据的clone方法进行克隆副本使用</para>
        /// </summary>
        public static ConcurrentDictionary<int, FaultInfoBase> InfoDictionary
        {
            get { return _info.InfoDictionary; } //将原始数据返回  数据安全性无法保证
        }

        public static List<int> SpeRtus
        {
            get { return _info.SpeRtus; }
        }

        /// <summary>
        /// 根据设备地址获取该设备包含的现存故障
        /// </summary>
        /// <param name="id">终端地址</param>
        /// <returns></returns>
        public static List<FaultInfoBase > GetFaultLstInfoByRtuId(int id)
        {
            return _info.GetFaultLstInfoByRtuId(id);
        }



        /// <summary>
        /// 请求最新故障的最新时间
        /// </summary>
        public static DateTime ReqExistFaultsTime = new DateTime();


        ///// <summary>
        ///// 根据设备逻辑地址获取设备信息；不存在返回null
        ///// </summary>
        ///// <param name="id">终端地址</param>
        ///// <returns></returns>
        //public static List<int> GetLstInfoByRtuIdEffice(int id)
        //{
        //    var tmp = (from t in UserIndividuationFaultInfoService.GetInfoList select t.FaultCode).ToList();
        //    return
        //        (from t in _info.InfoDictionary
        //         where t.Value.RtuId == id  && tmp.Contains(t.Value.FaultCodeId)
        //         select t.Key).ToList();

        //}

        /// <summary>
        /// 根据终端地址 故障编码获取故障信息
        /// </summary>
        /// <param name="id">终端地址</param>
        /// <returns>不存或用户已经过滤 或无法解析在返回null</returns>
        public static FaultInfoBase GetFaultInfoById(int id)
        {

           return    _info.GetInfoById(id);

            //  if (record == null) return null;
            //  var showInfo = EquipmentFaultInfoConverServices.GetInfoByRtuId( record.RtuId ,record .LoopId ,record .LampId );
            //  if (showInfo == null) return null;
            //  var typeInfo = TmlFaultTypeInfoServices.GetInfoById(record.FaultCodeId);
            //  if (typeInfo == null) return null;
            ////  if (!typeInfo.IsEnable) return null;


            //  var userSet = UserIndividuationFaultInfoService.GetInfoById(record.FaultCodeId);
            //  if (userSet != null)
            //  {
            //      int times = 3;
            //      if (!userSet.IsAlarm) return null;
            //      times = userSet.AlarmTimes;

            //      var rtnInfo = new EquipmentFaultShowInfo()
            //                        {
            //                            RtuId   = record.RtuId,
            //                            LoopId =record .LoopId ,
            //                            ShowId  =showInfo .ShowId  ,
            //                            AlarmTimes = times,
            //                            Color = typeInfo.Color,
            //                            CreatTime = record.DateCreate.ToString("yyyy-MM-dd HH:mm:ss"),
            //                            ShowNameOne  = showInfo.ShowNameOne ,
            //                            ShowNameThree  = showInfo.ShowNameThree,
            //                            ShowNameTwo  = showInfo.ShowNameTwo ,
            //                            FaultName = typeInfo.FaultNameByDefine,
            //                            Id = record.Id,
            //                            FaultRemak = typeInfo.FaultRemak,

            //                            Count =record .AlarmCount,
            //                            RemarkFromServer =record .Remark ,
            //                            DateCreateId =record.RecordId  ,

            //                        };
            //      return rtnInfo;
            //  }
            //  return null;
        }
    }


    //public partial class TmlExistFaultsInfoServices
    //{
    //    /// <summary>
    //    /// 终端是否有故障
    //    /// </summary>
    //   // public static Dictionary<int, bool> RtusHasErr = new Dictionary<int, bool>();



    //    /// <summary>
    //    /// 获取终端是否存在故障
    //    /// </summary>
    //    /// <param name="rtuId"></param>
    //    /// <returns></returns>
    //    public static bool IsRtuHasError(int rtuId)
    //    {
    //        var sss = GetFaultLstInfoByRtuId(rtuId);
    //        if (sss == null) return false;
    //        if (sss.Count == 0) return false;
    //        return true;
    //    }


    //    /// <summary>
    //    /// 通过给定的终端地址列表 来更新终端故障状态
    //    /// </summary>
    //    /// <param name="rtusLst"></param>
    //    public static void RtuErrorsChangeAttachShowError(List<int> rtusLst)
    //    {
    //        var lduerror = new List<Tuple<int, int, int, long>>(); // 地址 回路 故障代码 发生时间

    //        var tmplst = new List<int>();
    //        if (rtusLst != null) foreach (var t in rtusLst) if (!tmplst.Contains(t) && t > 1100000 && t < 1200000) tmplst.Add(t);
    //        if (tmplst.Count == 0) return;

    //        var lst = new List<Tuple<int, bool>>();
    //        foreach (var t in tmplst)
    //        {
    //            var errors =(from g in InfoDictionary where g.Value.RtuId == t select g).ToList();
    //            lst.Add(new Tuple<int, bool>(t, errors.Count > 0));

    //            lduerror.AddRange(from ggg in errors select new Tuple<int, int, int, long>
    //                (ggg.Value.RtuId, ggg.Value.LoopId,ggg.Value.FaultId, ggg.Value.DateCreate.Ticks));
    //        }

    //      //  if (lst.Count > 0) UpdateEquipmentError(lst);


    //        //41 线路被盗
    //        //42 线路短路
    //      //  var ldus = (from t in lst where t.Item1 > 1100000 && t.Item1 < 1200000 select t).ToList();
    //        foreach (var t in lst)
    //        {
    //            if (
    //                !Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems .ContainsKey
    //                     (t.Item1))
    //                continue;
    //            var tmpsg =
    //                Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems [t.Item1] as
    //                Wlst.Sr .EquipmentInfoHolding .Model .Wj1090Ldu ;
    //            if (tmpsg == null) return;
    //            var binds =
    //                tmpsg.WjLduLines.Values  .Select(ttt => new Tuple<int, int, bool>(ttt.LduLineId , ttt.LduLoopId , ttt.IsUsed)).
    //                    ToList(); //序号  绑定的终端回路 是否使用
                

    //            if (t.Item2 == false)
    //            {
    //                foreach (var gggg in binds)
    //                {
    //                    string info = "正常";
    //                    Wlst.Sr.EquipmentInfoHolding.Services.RtuNewDataService.UpdateAttachInfo(t.Item1, gggg.Item1,
    //                                                                                             info, false, 0);
    //                    Wlst .Sr .EquipmentInfoHolding .Services .RunningInfoHold .GetRunInfo( 0).e
    //                }
    //            }
    //            else
    //            {

    //                foreach (var gggg in binds)
    //                {
    //                    var tmpsssss =
    //                        (from tmpsssgs in lduerror
    //                         where tmpsssgs.Item1 == t.Item1 && tmpsssgs.Item2 == gggg.Item1
    //                         select tmpsssgs).ToList();
    //                    if (tmpsssss.Count == 0)
    //                    {
    //                        string info = "正常";
    //                        Wlst.Sr.EquipmentInfoHolding.Services.RtuNewDataService.UpdateAttachInfo(t.Item1,
    //                                                                                                 gggg.Item1,
    //                                                                                                 info, false, 0);
    //                    }
    //                    else
    //                    {
    //                        string info = "被盗";

    //                        if (tmpsssss[0].Item3 == 42) info = "短路";
    //                        Wlst.Sr.EquipmentInfoHolding.Services.RtuNewDataService.UpdateAttachInfo(t.Item1,
    //                                                                                                 gggg.Item1,
    //                                                                                                 info, true, tmpsssss[0].Item4);
    //                    }
    //                }
    //            }

    //        }
    //    }

    //}

}
