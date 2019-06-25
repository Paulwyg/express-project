using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Sr.TimeTableSystem.InfoHold;
using Wlst.client;

namespace Wlst.Sr.TimeTableSystem.Services
{
    public class HolidayTimeandBandingServices
    {
        private static HolidayTimeandBandingServices _myslef;

        public static HolidayTimeandBandingServices Myself
        {
            get
            {
                if (_myslef == null) new HolidayTimeandBandingServices();
                return _myslef;
            }
        }

        private HolidayTimeandBandingServices()
        {
            if (_myslef != null) return;
            _myslef = this;
            info = new HolidayTimeandBanding();

        }

        private HolidayTimeandBanding info; // = new HolidayTimeandBanding();

        /// <summary>
        /// 执行数据初始化并注册事件,系统执行
        /// </summary>
        public void InitService()
        {
            info.InitStart();
        }


        /// <summary>
        /// 节假日调度方案  获取只能读 不允许改
        /// </summary>
        public Dictionary<Tuple<int,int>, HolidayWeekSetInfo.HolidaySchduleTime> InfoHolidaySchduleTimeGet
        {
            get { return info.InfoHolidaySchduleTimeGet; }
        }

        /// <summary>
        /// 终端绑定调度方案  获取只能读 不允许改
        /// </summary>
        public Dictionary<Tuple<int, int>, int> InfoRtuBandingSchduleGet
        {
            get { return info.InfoRtuBandingSchduleGet; }
        }

        /// <summary>
        ///终端是否处于节假日时间
        /// </summary>
        /// <param name="rtuid"></param>
        /// <returns></returns>
        public bool IsRtuInHoliday(int areaid,int rtuid)
        {
            var tu = new Tuple<int, int>(areaid, rtuid);
            if (!info.InfoRtuBandingSchduleGet.ContainsKey(tu)) return false;
            var holidaytimeid = info.InfoRtuBandingSchduleGet[tu];
            if (!InfoHolidaySchduleTimeGet.ContainsKey(tu)) return false;
            foreach (var t in InfoHolidaySchduleTimeGet[tu].Schdules)
            {
                var nowt = DateTime.Now.Ticks;
                var starttime = new DateTime(DateTime.Now.Year, t.MonthStart, t.DayStart, 0, 0, 1).Ticks;
                var endtime = new DateTime(DateTime.Now.Year, t.MonthEnd, t.DayEnd, 23, 59, 59).Ticks;
                if (nowt > starttime && nowt < endtime) return true;
            }
            return false;
        }

        /// <summary>
        /// 获取终端的 节假日 时间信息
        /// </summary>
        /// <param name="rtuid"></param>
        /// <returns></returns>
        public List< string > GetRtuSwitchOutOpenCloseTimeInholiday(int areaid,int rtuid)
        {
            var lst = new List<string>();
            var tu = new Tuple<int, int>(areaid, rtuid);
            if (!info.InfoRtuBandingSchduleGet.ContainsKey(tu)) return null;
            var holidaytimeid = info.InfoRtuBandingSchduleGet[tu];
            var tu1 = new Tuple<int, int>(areaid, holidaytimeid);
            if (!InfoHolidaySchduleTimeGet.ContainsKey(tu1)) return null;
            foreach (var t in InfoHolidaySchduleTimeGet[tu1].Schdules)
            {
                var nowt = DateTime.Now.Ticks;
                var starttime = new DateTime(DateTime.Now.Year, t.MonthStart, t.DayStart, 0, 0, 1).Ticks;
                var endtime = new DateTime(DateTime.Now.Year, t.MonthEnd, t.DayEnd, 23, 59, 59).Ticks;
                if (nowt > starttime && nowt < endtime)
                {
                    var ntg = (from tx in t.SwitchOutTimeItem orderby tx.Kx ascending select tx).ToList();
                    foreach (var ff in ntg )
                    {
                        var str1 = "";
                        if (ff.OpenTime  > 23*60)
                        {
                            str1 = "---  -  ";
                        }
                        else
                        {
                            str1 = string.Format("{0:D2}", ff.OpenTime / 60) + ":" + string.Format("{0:D2}", ff.OpenTime%60) +
                                  " - ";
                        }
                        if (ff.CloseTime  > 23*60)
                        {
                            str1 += "---";
                        }
                        else
                        {
                            str1 += string.Format("{0:D2}", ff.CloseTime / 60) + ":" + string.Format("{0:D2}", ff.CloseTime  % 60);
                        }
                        lst.Add(str1);
                    }
                    //var str1 = "";
                    //if (t.K1HourStart > 23)
                    //{
                    //    str1 = "---  -  ";
                    //}
                    //else
                    //{
                    //    str1 = string.Format("{0:D2}", t.K1HourStart) + ":" + string.Format("{0:D2}", t.K1MinuteStart) +
                    //          " - ";
                    //}
                    //if (t.K1HourEnd > 23)
                    //{
                    //    str1 += "---";
                    //}
                    //else
                    //{
                    //    str1 += string.Format("{0:D2}", t.K1HourEnd) + ":" + string.Format("{0:D2}", t.K1MinuteEnd);
                    //}
                    //lst.Add(str1);

                    //var str2 = "";
                    //if (t.K2HourStart > 23)
                    //{
                    //    str2 = "---  -  ";
                    //}
                    //else
                    //{
                    //    str2 = string.Format("{0:D2}", t.K2HourStart) + ":" + string.Format("{0:D2}", t.K2MinuteStart) +
                    //          " - ";
                    //}
                    //if (t.K2HourEnd > 23)
                    //{
                    //    str2 += "---";
                    //}
                    //else
                    //{
                    //    str2 += string.Format("{0:D2}", t.K2HourEnd) + ":" + string.Format("{0:D2}", t.K2MinuteEnd);
                    //}
                    //lst.Add(str2);


                    //var str3 = "";
                    //if (t.K3HourStart > 23)
                    //{
                    //    str3 = "---  -  ";
                    //}
                    //else
                    //{
                    //    str3 = string.Format("{0:D2}", t.K3HourStart) + ":" + string.Format("{0:D2}", t.K3MinuteStart) +
                    //          " - ";
                    //}
                    //if (t.K3HourEnd > 23)
                    //{
                    //    str3 += "---";
                    //}
                    //else
                    //{
                    //    str3 += string.Format("{0:D2}", t.K3HourEnd) + ":" + string.Format("{0:D2}", t.K3MinuteEnd);
                    //}
                    //lst.Add(str3);


                    //var str4 = "";
                    //if (t.K4HourStart > 23)
                    //{
                    //    str4 = "---  -  ";
                    //}
                    //else
                    //{
                    //    str4 = string.Format("{0:D2}", t.K4HourStart) + ":" + string.Format("{0:D2}", t.K4MinuteStart) +
                    //          " - ";
                    //}
                    //if (t.K4HourEnd > 23)
                    //{
                    //    str4 += "---";
                    //}
                    //else
                    //{
                    //    str4 += string.Format("{0:D2}", t.K4HourEnd) + ":" + string.Format("{0:D2}", t.K4MinuteEnd);
                    //}
                    //lst.Add(str4);

                    //var str5 = "";
                    //if (t.K5HourStart > 23)
                    //{
                    //    str5= "---  -  ";
                    //}
                    //else
                    //{
                    //    str5 = string.Format("{0:D2}", t.K5HourStart) + ":" + string.Format("{0:D2}", t.K5MinuteStart) +
                    //          " - ";
                    //}
                    //if (t.K5HourEnd > 23)
                    //{
                    //    str5+= "---";
                    //}
                    //else
                    //{
                    //    str5 += string.Format("{0:D2}", t.K5HourEnd) + ":" + string.Format("{0:D2}", t.K5MinuteEnd);
                    //}
                    //lst.Add(str5);


                    //var str6 = "";
                    //if (t.K6HourStart > 23)
                    //{
                    //    str6 = "---  -  ";
                    //}
                    //else
                    //{
                    //    str6 = string.Format("{0:D2}", t.K6HourStart) + ":" + string.Format("{0:D2}", t.K6MinuteStart) +
                    //          " - ";
                    //}
                    //if (t.K6HourEnd > 23)
                    //{
                    //    str6 += "---";
                    //}
                    //else
                    //{
                    //    str6 += string.Format("{0:D2}", t.K6HourEnd) + ":" + string.Format("{0:D2}", t.K6MinuteEnd);
                    //}
                    //lst.Add(str6);

                    return lst;
                }
            }
            return null ;
        }


        //private  string ToStringTupe(Tuple<int, int, int, int> xxx)
        //{
        //    return string.Format("{0:D2}", xxx.Item1) + string.Format("{0:D2}", xxx.Item2) + "-" +
        //           string.Format("{0:D2}", xxx.Item3) + string.Format("{0:D2}", xxx.Item4);
        //}
    }
}
