using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Sr.EquipemntLightFault.Services
{
    public class FaultClassisDef
    {

        /// <summary>
        /// 故障分类  大于等于 小于等于
        /// </summary>
        public static List<Tuple<int, int, int, string>> FaultClass = new List<Tuple<int, int, int, string>>()
                                                                     {
                                                                         new Tuple<int,int , int, string>(3005,1, 35, "终端故障"),
                                                                         new Tuple<int,int , int, string>(2090,50, 69, "单灯故障"),
                                                                         new Tuple<int, int ,int, string>(1090,41, 49, "其他故障"),
                                                                         new Tuple<int, int, int, string>(0000,81,250,"自定义故障")
                                                                     };
    }
}
