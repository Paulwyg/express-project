using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.ShowMsgInfo;
using Wlst.Sr.TimeTableSystem.InfoHold;
using Wlst.Sr.TimeTableSystem.Models;
using Wlst.client;

namespace Wlst.Sr.TimeTableSystem.Services
{
    public class WeekTimeTableInfoService
    {
        internal static TimeTableInfosHold info = new TimeTableInfosHold();

        /// <summary>
        /// 执行数据初始化并注册事件,系统执行
        /// </summary>
        public static void InitService()
        {
            info.InitStart();
        }

        /// <summary>
        /// <para>任何使用此数据务必注意 此数据为原始数据，___只允许读不允许修改___ </para> 
        /// <para>任何修改会使原始数据被修改形成脏数据 </para>
        /// </summary>
        public static Dictionary<Tuple<int, int>, TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem> WeekTimeTableInfoDictionary
        {
            get { return info.InfoTimeTableDictionary ; } //将原始数据返回  数据安全性无法保证
        }

        /// <summary>
        /// 不存在返回null
        /// </summary>
        /// <param name="id"></param>
        /// <returns>不存在返回null</returns>
        public static TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem GeteekTimeTableInfo(int area, int id)
        {
            return info.GetInfoTimeTableByIdNew(area, id);
            return info.GetInfoTimeTableById(area,id);
        }

        /// <summary>
        /// <para>获取升序排列的列表</para>
        /// <para>任何使用此数据务必注意 此数据为原始数据，___只允许读不允许修改___  </para>
        /// <para>任何修改会使原始数据被修改形成脏数据 </para>
        /// </summary>
        public static List<TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem> GeteekTimeTableInfoList(int areaid)
        {

            return info.GetInfoTimeTableList(areaid);
        }




                /// <summary>
        /// 
        /// </summary>
        /// <param name="lstTimeTables"></param>
        /// <param name="lstUpdateRtuOrGrpBanding">第一个参数为终端或组地址，第二个参数为终端或组回路，第三个参数为时间表地址</param>
        public static  void UpdateTimeTable(int areaid,
            List<TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem> lstTimeTables,
            List<Tuple<int, int, int>> lstUpdateRtuOrGrpBanding)
                {
                 info .UpdateTimeTableInfos(areaid,lstTimeTables ,lstUpdateRtuOrGrpBanding );   
                }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstTimeTables"></param>
        /// <param name="lstUpdateRtuOrGrpBanding">第一个参数为终端或组地址，第二个参数为终端或组回路，第三个参数为时间表地址</param>
        public static void UpdateTimeTableNew(int areaid,
            List<TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem> lstTimeTables)
        {
            info.UpdateTimeTableInfosNew(areaid, lstTimeTables);
        }


        /// <summary>
        /// 获取终端某个回路上绑定的时间表名称
        /// </summary>
        /// <param name="rtuIdorGrpId">特殊终端地址或分组地址</param>
        /// <param name="loopId">回路地址 1~6</param>
        /// <returns>绑定到此回路的时间表名称  无绑定则为 ""</returns>
        public static string GetTmlLoopBandTimeTableNamex(int areaid,int rtuIdorGrpId, int loopId)
        {
            int timetableid = RtuOrGprBandingTimeTableInfoService.GetBandingInfo(areaid,rtuIdorGrpId, loopId);
            if (timetableid == -1) return "";
            //int areaid = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoRtuBelong[rtuIdorGrpId].Item1;
            var timetable = WeekTimeTableInfoService.GeteekTimeTableInfo(areaid, timetableid);
            if (timetable == null) return "";
            return timetable.TimeName;
        }

        /// <summary>
        /// 获取绑定到此时间表今天的开关灯时间
        /// </summary>
        /// <param name="rtuIdorGrpId">特殊终端地址或分组地址</param>
        /// <param name="switchoutloopid"></param>
        /// <param name="yesterday">昨天开关灯时间 </param>
        public static TodayOpenCloseTime GetTmlLoopBandTimeTableTodayOpenCloseTimex(int areaid, int rtuIdorGrpId, int switchoutloopid)
        {
            int RtuGrpId = 0;
            if (Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoRtuBelong.ContainsKey(rtuIdorGrpId))
            {
                RtuGrpId =
                    Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoRtuBelong[rtuIdorGrpId].Item2;
            }
            else
            {
                RtuGrpId = rtuIdorGrpId;
            }
            //采用新协议 细化到终端
            int timetableid = RtuOrGprBandingTimeTableInfoService.GetBandingInfoNew(areaid, RtuGrpId, switchoutloopid);
            //int timetableid = RtuOrGprBandingTimeTableInfoService.GetBandingInfo(areaid,RtuGrpId, switchoutloopid);
            if (timetableid == -1) return null;

            //绑定该时间表的临时方案 2018/4/20
            var plans = (from tt in TimeTabletemporaryHold.Myself.Info
                         where tt.Key.Item1 == areaid
                         orderby tt.Key.Item2 ascending
                         select tt.Value).ToList();
            bool istemporary=false;
            var temporarytable=new TempTimePlanWithTimeTableBandingInfo.TimeTablePlan();
            foreach (var t in plans)
            {
                foreach (var tt in t.TimetablesUseThisPlan)
                {
                    foreach (var ttt in t.ItemsPlan)
                    {
                        if (tt == timetableid && ttt.Date.ToString().Substring(6, 2) == DateTime.Now.Day.ToString().PadLeft(2, '0') && ttt.Date.ToString().Substring(4, 2) == DateTime.Now.Month.ToString().PadLeft(2, '0'))
                        {
                            temporarytable = t;
                            istemporary = true;
                        }
                    }                  
                }
            }


            var timetable = WeekTimeTableInfoService.GeteekTimeTableInfo(areaid, timetableid);
            if (timetable == null) return null;

            int todayweek = (int)DateTime.Now.DayOfWeek;
            //int yesterdayweek = todayweek - 1;

            //if (yesterdayweek == -1) yesterdayweek = 6;

            //int yesterdaylasttimeoff = 1500;

           // List<Tuple<int, int>> todaytime = new List<Tuple<int, int>>();
            List<Tuple<int, int>> returntime = new List<Tuple<int, int>>();
            if(istemporary)
                foreach (var t in temporarytable.ItemsPlan)
                {
                    var timelight = SunRiseSetInfoServices.GetSunRiseItemInfo(DateTime.Now.Month, DateTime.Now.Day);
                    if (t.Date.ToString().Substring(6, 2) == DateTime.Now.Day.ToString().PadLeft(2, '0') && t.Date.ToString().Substring(4, 2) == DateTime.Now.Month.ToString().PadLeft(2, '0'))
                    {
                        Tuple<int, int> tu = new Tuple<int, int>(t.TimeOn, t.TimeOff);
                        if (t.TypeOn == 3 && t.TypeOff == 3)
                        {
                            tu = new Tuple<int, int>(t.TimeOn, t.TimeOff);
                        }
                        else if (t.TypeOn == 3 && t.TypeOff != 3)
                        {
                            tu = new Tuple<int, int>(t.TimeOn, timelight.time_sunrise + temporarytable.LightOffOffset);
                        }
                        else if (t.TypeOn != 3 && t.TypeOff == 3)
                        {
                            tu = new Tuple<int, int>(timelight.time_sunset + temporarytable.LightOnOffset, t.TimeOff);
                        }
                        else
                        {
                            tu = new Tuple<int, int>(timelight.time_sunset + temporarytable.LightOnOffset,
                                                     timelight.time_sunrise + temporarytable.LightOffOffset);
                        }

                        returntime.Add(tu);
                    }
                }
            else
                foreach (var t in timetable.RuleItems)
                {
                    //if (t.DayOfWeekUsed.Contains(yesterdayweek))
                    //{
                    //    if (t.TimeOn>t.TimeOff)
                    //    {
                    //        if (t.TypeOff == 3) yesterdaylasttimeoff = t.TimeOff;
                    //        else{
                    //            var timelightoff = SunRiseSetInfoServices.GetSunRiseItemInfo(DateTime.Now.AddDays(-1).Month, DateTime.Now.AddDays(-1).Day).time_sunrise;
                    //            yesterdaylasttimeoff = timelightoff + timetable.LightOffOffset;
                    //        }

                    //    }
                    //}

                    //if (t.DayOfWeekUsed.Contains(todayweek))
                    //{
                    //    var timelight = SunRiseSetInfoServices.GetSunRiseItemInfo(DateTime.Now.Month, DateTime.Now.Day);
                    //if (t.TimeOn < t.TimeOff)
                    //{
                    //    Tuple<int, int> tu = new Tuple<int, int>(t.TimeOn, t.TimeOff);
                    //    if (t.TypeOn == 3 && t.TypeOff == 3)
                    //    {
                    //        tu = new Tuple<int, int>(t.TimeOn, t.TimeOff);
                    //    }
                    //    else if (t.TypeOn == 3 && t.TypeOff != 3)
                    //    {
                    //        tu = new Tuple<int, int>(t.TimeOn, timelight.time_sunrise + timetable.LightOffOffset);
                    //    }
                    //    else if (t.TypeOn != 3 && t.TypeOff == 3)
                    //    {
                    //        tu = new Tuple<int, int>(timelight.time_sunset + timetable.LightOnOffset, t.TimeOff);
                    //    }
                    //    else
                    //    {
                    //        tu = new Tuple<int, int>(timelight.time_sunset + timetable.LightOnOffset, timelight.time_sunrise + timetable.LightOffOffset);
                    //    }

                    //    todaytime.Add(tu);
                    //}
                    //else
                    //{
                    //    Tuple<int, int> tu = new Tuple<int, int>(t.TimeOn, t.TimeOff);
                    //    if (t.TypeOn == 3)
                    //    {
                    //        tu = new Tuple<int, int>(t.TimeOn, 1500);
                    //    }
                    //    else
                    //    {
                    //        tu = new Tuple<int, int>(timelight.time_sunset + timetable.LightOnOffset, 1500);
                    //    }
                    //    todaytime.Add(tu);
                    //}


                    //   }

                    var timelight = SunRiseSetInfoServices.GetSunRiseItemInfo(DateTime.Now.Month, DateTime.Now.Day);

                    if (t.DayOfWeekUsed.Contains(todayweek))
                    {
                        Tuple<int, int> tu = new Tuple<int, int>(t.TimeOn, t.TimeOff);
                        if (t.TypeOn == 3 && t.TypeOff == 3)
                        {
                            tu = new Tuple<int, int>(t.TimeOn, t.TimeOff);
                        }
                        else if (t.TypeOn == 3 && t.TypeOff != 3)
                        {
                            tu = new Tuple<int, int>(t.TimeOn, timelight.time_sunrise + timetable.LightOffOffset);
                        }
                        else if (t.TypeOn != 3 && t.TypeOff == 3)
                        {
                            tu = new Tuple<int, int>(timelight.time_sunset + timetable.LightOnOffset, t.TimeOff);
                        }
                        else
                        {
                            tu = new Tuple<int, int>(timelight.time_sunset + timetable.LightOnOffset,
                                                     timelight.time_sunrise + timetable.LightOffOffset);
                        }

                        returntime.Add(tu);
                    }
                }


            //var todaytimeitem = (from t in todaytime orderby t.Item1 select t).ToList();

            //if (yesterdaylasttimeoff != 1500)
            //{
            //    if (todaytimeitem.Count > 0)
            //    {
            //        if (todaytimeitem.Last().Item2 == 1500)
            //        {
            //            Tuple<int, int> tu = new Tuple<int, int>(todaytimeitem.Last().Item1, yesterdaylasttimeoff);
            //            todaytimeitem.Remove(todaytimeitem.Last());
            //            todaytimeitem.Add(tu);
            //        }
            //        else
            //        {
            //            Tuple<int, int> tu = new Tuple<int, int>(1500, yesterdaylasttimeoff);
            //            returntime.Add(tu);
            //        }
            //    }

            //}

            //if (todaytimeitem.Count > 0)
            //{
            //    foreach (var tu in todaytimeitem)
            //    {
            //        returntime.Add(tu);
            //    }
            //}

            var today = new TodayOpenCloseTime()
                            {
                                TimeTableId = timetable.TimeId,
                                TimeTableName = timetable.TimeName,
                                TimeOnOff = returntime
                            };
            return today;

            #region
        //    public static TodayOpenCloseTime GetTmlLoopBandTimeTableTodayOpenCloseTimex(int areaid, int rtuIdorGrpId,int switchoutloopid)
        //{
            //yesterday = null;
            //int timetableid = RtuOrGprBandingTimeTableInfoService.GetBandingInfo(rtuIdorGrpId, switchoutloopid);
            //if (timetableid == -1) return null;
            //var timetable = WeekTimeTableInfoService.GeteekTimeTableInfo(areaid, timetableid);
            //if (timetable == null) return null;

            //var today = new TodayOpenCloseTime()
            //                {
            //                    TimeTableId = timetable.TimeId,
            //                    TimeTableName = timetable.TimeName
            //                };

            //yesterday = new TodayOpenCloseTime()
            //                {
            //                    TimeTableId = timetable.TimeId,
            //                    TimeTableName = timetable.TimeName,
            //                    CloseLightTime = 0,
            //                    OpenLightTime = 0,
            //                };

            //bool find = false;
            //if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday )
            //{


            //    yesterday = new TodayOpenCloseTime()
            //    {
            //        TimeTableId = timetable.TimeId,
            //        TimeTableName = timetable.TimeName,
            //        CloseLightTime = timetable.LastSaturdayOpenCloseControl.TimeOff,
            //        OpenLightTime = timetable.LastSaturdayOpenCloseControl.TimeOn,
            //    };
            //}

            //foreach (var t in timetable.LstOneWeekOpenCloseControl)
            //{
            //    if (t.DateDay == DateTime.Now.Day && t.DateMonth == DateTime.Now.Month)
            //    {
            //        today.CloseLightTime = t.TimeOff;
            //        today.OpenLightTime = t.TimeOn;
            //        find = true;
            //        //  break;
            //    }
            //    if (t.DateMonth == DateTime.Now.AddDays(-1).Month && t.DateDay == DateTime.Now.AddDays(-1).Day)
            //    {


            //        yesterday = new TodayOpenCloseTime()
            //                        {
            //                            TimeTableId = timetable.TimeId,
            //                            TimeTableName = timetable.TimeName,
            //                            CloseLightTime = t.TimeOff,
            //                            OpenLightTime = t.TimeOn,
            //                        };
            //    }
            //}
            //if (find)
            //    return today;
            //CantfindTodayOpenCloseTime(areaid, timetable.TimeId);
            #endregion


            return null;
        }

        private static DateTime _lastsndrequest = DateTime.Now.AddHours(-1);

        //private static void CantfindTodayOpenCloseTime(int area,int timeid)
        //{
        //    var timetable = WeekTimeTableInfoService.GeteekTimeTableInfo(area,timeid);
        //    if (timetable == null) return;

        //    bool maxall = true;
        //    var today = DateTime.Now.Month*50 + DateTime.Now.Day;


        //    foreach (var t in timetable.LstOneWeekOpenCloseControl)
        //    {
        //        if (t.DateMonth == 12 && DateTime.Now.Month == 1) continue;

        //        var dayof = t.DateMonth*50 + t.DateDay;
        //        if (dayof >= today)
        //        {
        //            maxall = false;
        //            break;
        //        }
        //    }
        //    if (maxall)
        //    {
        //        if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
        //        {
        //            if (DateTime.Now.Minute > 3)
        //            {
        //                if (DateTime.Now.Ticks - _lastsndrequest.Ticks < 50000000) return;
        //                info.RequestWeekTimeTableInfo();
        //                Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(0, "系统", OperatrType.SystemInfo,
        //                                                                      "系统请求本周周设置时间...");
        //                _lastsndrequest = DateTime.Now;
        //            }

        //        }
        //        else
        //        {
        //            if (DateTime.Now.Ticks - _lastsndrequest.Ticks < 50000000) return;
        //            info.RequestWeekTimeTableInfo();
        //            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(0, "系统", OperatrType.SystemInfo,
        //                                                                  "系统请求本周周设置时间...");
        //            _lastsndrequest = DateTime.Now;
        //        }
        //    }
        //}


    }
}
