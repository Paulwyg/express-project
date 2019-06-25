using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Sr.EquipmentGroupInfoHolding.GrpMulitInfoHold;

using Wlst.client;

namespace Wlst.Sr.EquipmentGroupInfoHolding.Services
{
    public class ServicesGrpMultiInfoHold
    {
        public const int GroupStartId = 401000;
        private static readonly GrpMultiInfoHoldExtend GrpMulti = new GrpMultiInfoHoldExtend();

        /// <summary>
        /// <para>任何使用此数务必注意 此处使用的为原始数据 ___不允许修改___</para>
        /// <para>修改请用GroupInfomatioin 类的clone方法进行克隆副本使用</para>
        /// </summary>
        public static Dictionary<int, GroupItemsInfo.GroupItem> GrpInfoDictionary
        {
            get
            {
               
                return GrpMulti.GrpInfoDictionary;
            }
        }


        /// <summary>
        /// <para>获取升序排列的列表</para>
        /// <para>任何使用此数务必注意 此处使用的为原始数据  ___不允许修改___</para>
        /// <para>修改请用GroupInfomatioin 类的clone方法进行克隆副本使用</para>
        /// </summary>
        public static List<GroupItemsInfo.GroupItem> GrpInfoList
        {
            get { return GrpMulti.GrpInfoList; }
        }

        /// <summary>
        /// 递归查阅该组下的所有终端
        /// </summary>
        /// <param name="grpId"></param>
        /// <returns></returns>
        public static  List<int> GetGrpTmlList(int grpId)
        {
            return GrpMulti.GetGrpTmlList(grpId);
        }

        /// <summary>
        /// 实现分组数据更新
        /// </summary>
        /// <param name="lstfromServer">所有分组数据信息，必须包含根节点信息 根节点地址为 0</param>
        public static void UpdateGrpMultiInfo(List<GroupItemsInfo.GroupItem> lstfromServer)
        {
            GrpMulti.UpdateCoreGroupsInfo(lstfromServer);
        }
    }
}
