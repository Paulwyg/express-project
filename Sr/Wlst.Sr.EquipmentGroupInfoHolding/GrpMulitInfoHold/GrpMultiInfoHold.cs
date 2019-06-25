using System.Collections.Generic;
using System.Linq;

using Wlst.client;


namespace Wlst.Sr.EquipmentGroupInfoHolding.GrpMulitInfoHold
{
    public class GrpMultiInfoHold
    {
        protected Dictionary<int, GroupItemsInfo.GroupItem> DicGrpInfo = new Dictionary<int, GroupItemsInfo.GroupItem>();
        protected bool BolLoad = false;


        /// <summary>
        /// <para>任何使用此数务必注意 此处使用的为原始数据 ___不允许修改___</para>
        /// <para>修改请用GroupInfomatioin 类的clone方法进行克隆副本使用</para>
        /// </summary>
        public Dictionary<int, GroupItemsInfo.GroupItem> GrpInfoDictionary
        {
            get { return DicGrpInfo; }
        }


        /// <summary>
        /// <para>获取升序排列的列表</para>
        /// <para>任何使用此数务必注意 此处使用的为原始数据  ___不允许修改___</para>
        /// <para>修改请用GroupInfomatioin 类的clone方法进行克隆副本使用</para>
        /// </summary>
        public List<GroupItemsInfo.GroupItem> GrpInfoList
        {
            get
            {
                var lstReturn = new List<GroupItemsInfo.GroupItem>();
                var result = from pair in DicGrpInfo orderby pair.Key select pair;
                foreach (var p in result)
                {
                    lstReturn.Add(p.Value);
                }
                return lstReturn;
            }
        }


        /// <summary>
        /// 递归查阅该组下的所有终端
        /// </summary>
        /// <param name="grpId"></param>
        /// <returns></returns>
        public List<int> GetGrpTmlList(int grpId)
        {
            List<int> lstReturn = new List<int>();
            if (DicGrpInfo.ContainsKey(grpId))
            {
                foreach (var t in DicGrpInfo[grpId].LstTml)
                {
                    lstReturn.Add(t);
                }
                //foreach (var t in DicGrpInfo[grpId].LstGrp)
                //{
                //    var tt = GetGrpTmlList(t);
                //    foreach (var ttt in tt)
                //    {
                //        lstReturn.Add(ttt);
                //    }
                //}
            }
            return lstReturn;
        }
    }
}
