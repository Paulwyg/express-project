//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Wlst.client;

//namespace Wlst.Ux.PrivilegesManage.AreaManageViewModel.Services
//{
//    public class ServicesAreaInfoHold
//    { 
//            public const int AreaStartId = 601000;
//            private static readonly AreaInfoHold GrpArea = new AreaInfoHold();

//            /// <summary>
//            /// 程序初始化时必须执行一次;由本模块自动执行
//            /// </summary>
//            public static void InitLoad()
//            {
//                GrpArea.InitLoadArea();
//            }

//            /// <summary>
//            /// 终端属于哪一个分组 第一个分组既最大的哪一个分组
//            /// </summary>
//            public static Dictionary<int, int> InfoRtuBelong
//            {
//                get { return GrpArea.InfoRtuBelong; }
//            }

//            public static List<int> GetRtuOrAreaIndex(List<int> data)
//            {
//                return data;
//            }

//            /// <summary>
//            /// 获取终端归属分组 第一个最大的分组
//            /// </summary>
//            /// <param name="rtuId"></param>
//            /// <returns>-1 不存在 其他分组地址</returns>
//            public static int GetRtuBelongGrp(int rtuId)
//            {
//                if (GrpArea.InfoRtuBelong.ContainsKey(rtuId)) return GrpArea.InfoRtuBelong[rtuId];
//                return -1;
//            }

//            /// <summary>
//            /// <para>任何使用此数务必注意 此处使用的为原始数据 ___不允许修改___</para>
//            /// </summary>
//            public static Dictionary<int, AreaInfo.AreaItem> InfoAreas
//            {
//                get { return GrpArea.InfoAreas; }
//            }

//            /// <summary>
//            /// 获取区域分组信息
//            /// </summary>
//            /// <param name="group"></param>
//            /// <returns>不存在返回 null</returns>
//            public static AreaInfo.AreaItem GetAreaInfomation(int area)
//            {
//                if (InfoAreas.ContainsKey(area)) return InfoAreas[area];
//                return null;
//            }

//            /// <summary>
//            /// <para>获取升序排列的列表</para>
//            /// <para>任何使用此数务必注意 此处使用的为原始数据  ___不允许修改___</para>
//            /// <para>修改请用GroupInfomatioin 类的clone方法进行克隆副本使用</para>
//            /// </summary>
//            public static List<AreaInfo.AreaItem> GrpInfoList
//            {
//                get { return GrpArea.AreaInfoList; }
//            }

//            /// <summary>
//            /// 递归查阅该组下的所有终端
//            /// </summary>
//            /// <param name="grpId"></param>
//            /// <returns></returns>
//            public static List<int> GetGrpTmlList(int grpId)
//            {
//                return GrpArea.GetGrpTmlList(grpId);

//            }

//            /// <summary>
//            /// 本系统主动提交区域分组数据到服务器  请求服务器更新数据
//            /// </summary>
//            public static void UpdateAreasInfo(List<AreaInfo.AreaItem> areainfo, long timeNew)
//            {
//                GrpArea.UpdateAreaInfo(areainfo, timeNew);
//            }

//            /// <summary>
//            /// 与服务器交互数据 请求比对数据 触发点
//            /// </summary>
//            /// <param name="qz">是否强制请求更新服务器数据</param>
//            public static void RequestAreaInfo(bool qz = false)
//            {
//                GrpArea.RequestAreaInfo(qz);
//            }
//        }
    
//}
