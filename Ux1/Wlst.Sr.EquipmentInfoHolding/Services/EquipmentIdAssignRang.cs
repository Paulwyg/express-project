using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Sr.EquipmentInfoHolding.Services
{
    public class EquipmentIdAssignRang
    {
        public const int EsuStart = 1200000;
        public const int EsuEnd = 1299999;

        public const int RtuStart = 1000000;
        public const int RtuEnd = 1099999;


        public const int MruStart = 1300000;
        public const int MruEnd = 1399999;

        public const int LuxStrt = 1400000;
        public const int LuxEnd = 1499999;

        public const int LineConcentratorStart = 1100000;
        public const int LineConcentratorEnd = 1199999;

        public const int SluStart = 1500000;
        public const int SluEnd = 1599999;


        public const int LeakStart = 1600000;
        public const int LeakEnd = 1699999;
        /// <summary>
        /// 是否为节能设备
        /// </summary>
        /// <param name="rtuId"></param>
        /// <returns></returns>
        public static bool IsRtuIsEsu(int rtuId)
        {
            if (rtuId > EsuStart && rtuId < EsuEnd) return true;
            return false;
        }

        /// <summary>
        /// 是否为终端设备
        /// </summary>
        /// <param name="rtuId"></param>
        /// <returns></returns>
        public static bool IsRtuIsRtuLight(int rtuId)
        {

            if (rtuId > RtuStart && rtuId < RtuEnd) return true;
            return false;
        }

        /// <summary>
        /// 是否为抄表设备
        /// </summary>
        /// <param name="rtuId"></param>
        /// <returns></returns>
        public static bool IsRtuIsMru(int rtuId)
        {

            if (rtuId > MruStart && rtuId < MruEnd) return true;
            return false;
        }

        /// <summary>
        /// 是否为光控设备
        /// </summary>
        /// <param name="rtuId"></param>
        /// <returns></returns>
        public static bool IsRtuIsLux(int rtuId)
        {

            if (rtuId > LuxStrt && rtuId < LuxEnd) return true;
            return false;
        }

        /// <summary>
        /// 是否为线路检测设备
        /// </summary>
        /// <param name="rtuId"></param>
        /// <returns></returns>
        public static bool IsRtuIsLine(int rtuId)
        {

            if (rtuId > LineConcentratorStart && rtuId < LineConcentratorEnd) return true;
            return false;
        }


        /// <summary>
        /// 是否为漏电检测设备
        /// </summary>
        /// <param name="rtuId"></param>
        /// <returns></returns>
        public static bool IsRtuIsLeak(int rtuId)
        {

            if (rtuId > LeakStart  && rtuId < LeakEnd ) return true;
            return false;
        }
        /// <summary>
        /// 是否为单灯设备
        /// </summary>
        /// <param name="rtuId"></param>
        /// <returns></returns>
        public static bool IsSlu(int rtuId)
        {
            if (rtuId > SluStart && rtuId < SluEnd) return true;
            return false;
        }

        /// <summary>
        /// 通过终端物理地址获取逻辑地址 
        /// </summary>
        /// <param name="phyId"></param>
        /// <returns>无法查阅返回 0</returns>
        public static int GetRtuIdByPhyId(int phyId)
        {
            var nts =
                (from t in EquipmentDataInfoHold .InfoItems 
                 where t.Value.RtuPhyId  == phyId
                 select t.Value).ToList();

            if (nts.Count == 0) return 0;
            foreach (var g in nts)
            {
                if (g.RtuId > RtuStart && g.RtuId < RtuEnd) return g.RtuId;
            }
            return 0;

        }
    }
}
