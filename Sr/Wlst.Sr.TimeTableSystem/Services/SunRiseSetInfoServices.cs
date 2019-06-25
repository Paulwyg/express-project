using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Sr.TimeTableSystem.InfoHold;
using Wlst.Sr.TimeTableSystem.Models;

namespace Wlst.Sr.TimeTableSystem.Services
{
    public class SunRiseSetInfoServices
    {
        private static SunRiseInfoHold info = new SunRiseInfoHold();

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
        public static Dictionary<string, SunRiseItemInfomation> InfoDictionary
        {
            get { return info.InfoDictionary; } //将原始数据返回  数据安全性无法保证
        }


        /// <summary>
        /// 获取日出日落信息
        /// </summary>
        /// <param name="month">月 </param>
        /// <param name="day"> 日</param>
        /// <returns>日出日落信息 无则null</returns>
        public static SunRiseItemInfomation GetSunRiseItemInfo(int month, int day)
        {
            return info.GetSunRiseItemInfo(month, day);
        }
    }
}
