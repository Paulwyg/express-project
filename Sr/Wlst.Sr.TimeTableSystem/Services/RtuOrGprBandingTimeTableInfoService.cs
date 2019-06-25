using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Sr.TimeTableSystem.Services
{
    public class RtuOrGprBandingTimeTableInfoService
    {
       // private static TmlLoopBngTimeTableInfo info = new TmlLoopBngTimeTableInfo();




        /// <summary>
        /// 任何使用此数据务必注意 此数据为原始数据，___只允许读不允许修改___  
        /// 任何修改会使原始数据被修改形成脏数据 
        /// </summary>
        public static Dictionary<Tuple<int,int>, Dictionary<int, int>> BandingInfoDictionary
        {
            get
            {
                return WeekTimeTableInfoService.info .BandingInfoDictionary ; //将原始数据返回  数据安全性无法保证
            }
        }

        /// <summary>
        /// 不存在则返回null
        /// </summary>
        /// <param name="tmlOrGrpId"></param>
        /// <returns></returns>
        public static Dictionary<int, int> GetBandingInfo(int areaid,int tmlOrGrpId)
        {

            return WeekTimeTableInfoService.info.GetBandingInfo(areaid, tmlOrGrpId);
        }

        /// <summary>
        /// 不存在则返回-1
        /// </summary>
        /// <param name="tmlOrGrpId"></param>
        /// <param name="switchoutloopid">1 ~6 </param>
        /// <returns>-1</returns>
        public static int GetBandingInfo(int areaid,int tmlOrGrpId, int switchoutloopid)
        {

            return WeekTimeTableInfoService.info.GetBandingInfo(areaid,tmlOrGrpId, switchoutloopid);
        }


        /// <summary>
        /// 不存在则返回-1
        /// </summary>
        /// <param name="tmlOrGrpId"></param>
        /// <param name="switchoutloopid">1 ~6 </param>
        /// <returns>-1</returns>
        public static int GetBandingInfoNew(int areaid, int tmlOrGrpId, int switchoutloopid)
        {

            return WeekTimeTableInfoService.info.GetBandingInfoNew(areaid, tmlOrGrpId, switchoutloopid);
        }



        public static bool IsThisIdIsRtu(int rtuOrGrpId)
        {
            return rtuOrGrpId > 1000000;
        }

        /// <summary>
        /// 升序排列的数据
        /// </summary>
        /// <returns></returns>
        public static List<Dictionary<int, int>> GetBandingInfoList
        {
            get { return WeekTimeTableInfoService.info.GetBandingInfoList; }
        }


        /// <summary>
        /// <para>获取绑定到本时间表的所有终端;</para>
        /// <para>返回为lst 第一个数据为终端或组地址;</para>
        /// <para>第二个数据为终端回路编号;</para>
        /// </summary>
        /// <param name="timetableid">时间表ID</param>
        /// <returns>终端列表 不会为null的 </returns>
        public static List<Tuple<int, int>> GetBangdingToThisTimeTablesTmls(int areaid,int timetableid)
        {
            return WeekTimeTableInfoService.info.GetBangdingToThisTimeTablesTmls(areaid,timetableid);
        }





    }
}
