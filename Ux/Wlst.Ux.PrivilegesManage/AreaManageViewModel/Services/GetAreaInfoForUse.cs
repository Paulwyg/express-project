//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Wlst.client;

//namespace Wlst.Ux.PrivilegesManage.AreaManageViewModel.Services
//{
//    /// <summary>
//    /// 保存区域信息副本
//    /// </summary>
//    public class GetAreaInfoForUse
//    {
//        private Dictionary<int, AreaInfo.AreaItem> _dictionary;


//        /// <summary>
//        /// 任何使用此数务必注意 此处使用的为原始数据 ___不允许修改___
//        /// 修改请用GroupInfomatioin 类的clone方法进行克隆副本使用
//        /// </summary>
//        public Dictionary<int, AreaInfo.AreaItem> GrpInfoDictionary
//        {
//            get { return _dictionary; }
//        }


//        /// <summary>
//        /// 获取升序排列的列表
//        /// 任何使用此数务必注意 此处使用的为原始数据  ___不允许修改___
//        /// 修改请用GroupInfomatioin 类的clone方法进行克隆副本使用
//        /// </summary>
//        public List<AreaInfo.AreaItem> GrpInfoList
//        {
//            get
//            {
//                var lstReturn = new List<AreaInfo.AreaItem>();
//                var result = from pair in _dictionary orderby pair.Key select pair;
//                foreach (var p in result)
//                {
//                    lstReturn.Add(p.Value);
//                }
//                return lstReturn;
//            }
//        }

//        /// <summary>
//        /// 获取可用的分组ID值
//        /// </summary>
//        /// <returns></returns>
//        public int GetAvailableId()
//        {
//            int intstartid = 1;
//            for (int i = intstartid; i < intstartid + 9000; i++)
//            {
//                if (!_dictionary.ContainsKey(i))
//                {
//                    return i;
//                }
//            }
//            return -1;
//        }

//        /// <summary>
//        /// 增加分组 本类内
//        /// </summary>
//        /// <param name="groupInfo"></param>
//        public void AddAreaInfo(AreaInfo.AreaItem groupInfo)
//        {
//            UpdateAreaInfo(groupInfo);

//        }

//        /// <summary>
//        /// 删除分组信息 本类内 递归删除
//        /// </summary>
//        /// <param name="groupId">组id</param>
//        /// <param name="fatherGroupId"> </param>
//        public void DeleteGrpInfo(int areaId, int fatherAreaId)
//        {
//            var lstNeedDelete = new List<int>();
//            if (_dictionary.ContainsKey(areaId))
//            {
//                var gi = _dictionary[areaId];
//                foreach (var t in gi.LstTml)
//                {
//                    lstNeedDelete.Add(t);
//                }
//                _dictionary.Remove(areaId);
//                if (_dictionary.ContainsKey(fatherAreaId) && _dictionary[fatherAreaId].LstTml.Contains(areaId))
//                {
//                    _dictionary[fatherAreaId].LstTml.Remove(areaId);
//                }

//                foreach (var t in lstNeedDelete)
//                {
//                    if (_dictionary.ContainsKey(t))
//                    {
//                        DeleteGrpInfo(t, areaId);
//                    }
//                }


//            }
//        }

        
//        /// <summary>
//        /// 更新分组信息 如果分组不存在则增加 本类内
//        /// </summary>
//        /// <param name="areaInfo"></param>
//        public void UpdateAreaInfo(AreaInfo.AreaItem areaInfomatioin)
//        {

//            ////拷贝分组信息 防止原始分组信息再次被修改后形成脏数据           
//            if (_dictionary.ContainsKey(areaInfomatioin.AreaId))
//            {
//                _dictionary[areaInfomatioin.AreaId] = areaInfomatioin;
//            }
//            else
//            {
//                _dictionary.Add(areaInfomatioin.AreaId, areaInfomatioin);
//            }

//        }

        
//        ///// <summary>
//        ///// 更新程序内部信息并回写数据库 
//        ///// </summary>
//        //public void UpdateSystemAreaInfomationForShow(bool exChangeInfoWithServer)
//        //{
//        //    List<AreaInfo.AreaItem> lst = new List<AreaInfo.AreaItem>();
//        //    foreach (var t in _dictionary)
//        //    {
//        //        lst.Add(t.Value);
//        //    }
//        //    Wlst.Sr.EquipmentGroupInfoHolding.Services.ServicesGrpMultiInfoHold.UpdateGrpMultiInfo(lst);
//        //}
//    }
//}
