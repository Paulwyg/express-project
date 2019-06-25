using System;

namespace wlst.sr.iif
{
    /// <summary>
    /// 将c#时钟装换为UTC标准时间
    /// </summary>
    public class UtcTime
    {
        private static long _utcTimeBases = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).Ticks;
        private static bool _isUseUtcCvt = true;
        /// <summary>
        /// 将C#时间转换为UTC标准时间
        /// </summary>
        /// <param name="dttime"></param>
        /// <returns></returns>
        public static long GetUtcTime(long dttime)
        {
            try
            {
                if (_isUseUtcCvt == false) return dttime;
                TimeSpan ts =
                    new TimeSpan(
                        new System.DateTime(dttime).ToUniversalTime().Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
                return (long)ts.TotalMilliseconds ;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return 0;
        }

        /// <summary>
        /// 将C#时间转换为UTC标准时间
        /// </summary>
        /// <param name="dttime"></param>
        /// <returns></returns>
        public static long GetUtcTime(DateTime dttime)
        {
            if (_isUseUtcCvt == false) return dttime.Ticks;
            TimeSpan ts =
                new TimeSpan(dttime.ToUniversalTime().Ticks -
                             new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
            return (long)ts.TotalMilliseconds;
        }

        /// <summary>
        /// UTC标准时间装换为本地C#时间
        /// </summary>
        /// <param name="utc"></param>
        /// <returns></returns>
        public static long GetCsharpTime(long utc)
        {
            if (_isUseUtcCvt == false) return utc;
            long dt = utc * 10000 + new DateTime(1970, 1, 1, 0, 0, 0).Ticks;
            return new DateTime(dt).ToLocalTime().Ticks;
        }

        //public static DateTime GetCsharpTime(long utc)
        //{
        //    long dt = utc*10000 + new DateTime(1970, 1, 1, 0, 0, 0).Ticks;
        //    return new DateTime(dt).ToLocalTime();
        //}

    }
}
