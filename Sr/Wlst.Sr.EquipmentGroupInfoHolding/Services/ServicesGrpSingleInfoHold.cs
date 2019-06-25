//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Wlst.Sr.EquipmentGroupInfoHolding.GrpSingleInfoHold;
//using Wlst.Sr.EquipmentGroupInfoHolding.Models;
//using Wlst.client;

//namespace Wlst.Sr.EquipmentGroupInfoHolding.Services
//{
//    public class ServicesGrpSingleInfoHold
//    {
//        public const int GroupStartId = 501000;
//        private static readonly GrpSingleInfoHoldExtend GrpSingle = new GrpSingleInfoHoldExtend();


//        /// <summary>
//        /// 程序初始化时必须执行一次;由本模块自动执行
//        /// </summary>
//        public static void InitLoad()
//        {
//            GrpSingle.InitLoad();
//        }


//        /// <summary>
//        /// 终端属于哪一个分组 第一个分组既最大的哪一个分组
//        /// </summary>
//        public static Dictionary<int, Tuple< int ,int >> InfoRtuBelong
//        {
//            get { return GrpSingle.InfoRtuBelong; }
//        }


//        /// <summary>
//        /// 获取终端归属分组  区域-分组
//        /// </summary>
//        /// <param name="rtuId"></param>
//        /// <returns>null 不存在 </returns>
//        public static Tuple< int ,int > GetRtuBelongGrp(int rtuId)
//        {
//            if (GrpSingle.InfoRtuBelong.ContainsKey(rtuId)) return GrpSingle.InfoRtuBelong[rtuId];
//            return null ;
//        }


//        /// <summary>
//        /// <para>任何使用此数务必注意 此处使用的为原始数据 ___不允许修改___</para>
//        /// </summary>
//        public static Dictionary<Tuple<int, int>, GroupInformation> InfoGroups
//        {
//            get { return GrpSingle.InfoGroups; }
//        }

//        /// <summary>
//        /// 获取分组信息
//        /// </summary>
//        /// <param name="group"></param>
//        /// <returns>不存在返回 null</returns>
//        public static GroupItemsInfo.GroupItem GetGroupInfomation(int areaId, int group)
//        {
//            var tu = new Tuple<int, int>(areaId, group);
//            if (InfoGroups.ContainsKey(tu)) return InfoGroups[tu];
//            return null;
//        }


//        /// <summary>
//        /// <para>获取升序排列的列表</para>
//        /// <para>任何使用此数务必注意 此处使用的为原始数据  ___不允许修改___</para>
//        /// <para>修改请用GroupInfomatioin 类的clone方法进行克隆副本使用</para>
//        /// </summary>
//        public static List<GroupInformation> GrpInfoList()
//        {
//             return GrpSingle.GrpInfoList(); 
//        }

//        public static List<int> GetRtuOrGrpIndex(List<int> data)
//        {
//            return data;
//        }



//        /// <summary>
//        /// 本系统主动提及分组数据到服务器  请求服务器更新数据
//        /// </summary>
//        public static void UpdateGroupsInfo(List<GroupInformation> groupinfo)
//        {
//            GrpSingle.UpdateGroupsInfo(groupinfo);
//        }

//        public static List <int > GetGrpTmlList(int areaId,int gprId)
//        {
//            var tu = new Tuple<int, int>(areaId, gprId);
//            if (InfoGroups.ContainsKey(tu)) return InfoGroups[tu].LstTml ;
//            return new List<int> ();
//        }



//        /// <summary>
//        /// 与服务器交互数据 请求比对数据 触发点
//        /// </summary>
//        public static void RequestGroupInfo()
//        {
//            GrpSingle.RequestGroupInfo();
//        }
//    }
//}
