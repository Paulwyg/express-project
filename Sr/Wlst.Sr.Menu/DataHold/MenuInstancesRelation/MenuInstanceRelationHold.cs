using System;
using System.Collections.Generic;
using System.Linq;
using Wlst.Sr.Menu.Services;

namespace Wlst.Sr.Menu.DataHold.MenuInstancesRelation
{
    public class MenuInstanceRelationHold
    {
        /// <summary>
        /// 菜单实例关系映射表  
        /// 对应数据库表为 menu_instances_relation
        /// </summary>
        protected  Dictionary<int, Dictionary<Tuple<int, int>, Wlst.Sr.Menu.Models.MenuInstancesRelation>> DicInstanceRelation =
            new Dictionary<int, Dictionary<Tuple<int, int>, Wlst.Sr.Menu.Models.MenuInstancesRelation>>();



        /// <summary>
        /// 获取菜单实例关系映射信息,所有的信息
        /// </summary>
        /// <param name="instanceId">菜单实例关系映射Id值 </param>
        /// <returns>菜单实例关系映射信息 不存在则返回null</returns>
        public  Dictionary<Tuple<int, int>, Wlst.Sr.Menu.Models.MenuInstancesRelation> GetInstanceRelation(int instanceId)
        {
            if (DicInstanceRelation.ContainsKey(instanceId)) return DicInstanceRelation[instanceId];
            return null;
        }

        public  Wlst.Sr.Menu.Models.MenuInstancesRelation GetInstanceRelation(int instanceId, int fatherId, int menuId)
        {
            if (!DicInstanceRelation.ContainsKey(instanceId)) return null;
            if (!DicInstanceRelation[instanceId].ContainsKey(new Tuple<int, int>(fatherId, menuId))) return null;
            return DicInstanceRelation[instanceId][new Tuple<int, int>(fatherId, menuId)];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instanceId"></param>
        /// <param name="fatherId"></param>
        /// <returns></returns>
        public  List<Wlst.Sr.Menu.Models.MenuInstancesRelation> GetInstanceRelationsByfatherId(int instanceId, int fatherId)
        {
            List<Wlst.Sr.Menu.Models.MenuInstancesRelation> lstReturn = new List<Wlst.Sr.Menu.Models.MenuInstancesRelation>();
            if (!DicInstanceRelation.ContainsKey(instanceId)) return lstReturn;
            lstReturn.AddRange(from t in DicInstanceRelation[instanceId]
                               where t.Value.FatherId == fatherId
                               orderby t.Value.SortIndex
                               select t.Value);
            return lstReturn;

        }

        /// <summary>
        /// 获取整个数据
        /// </summary>
        public  Dictionary<int, Dictionary<Tuple<int, int>, Wlst.Sr.Menu.Models.MenuInstancesRelation>> GetInstanceRelatioinDic
        {
            get { return DicInstanceRelation; }
        }


        /// <summary>
        /// 获取可用的实例菜单值，返回值即可用的
        /// </summary>
        /// <returns></returns>
        public  int GetMaxAviableMenuFileId()
        {
            int maxId = MenuIdControlAssign.MenuFileGroupIdMin;
            foreach (var t in DicInstanceRelation)
            {
                foreach (var tt in t.Value)
                {
                    if (tt.Key.Item1 >= maxId) maxId = tt.Key.Item1;
                    if (tt.Key.Item2 > MenuIdControlAssign.MenuFileGroupIdMin &&
                        tt.Key.Item2 <= MenuIdControlAssign.MenuFileGroupIdMax)
                    {
                        if (tt.Key.Item2 >= maxId) maxId = tt.Key.Item2;
                    }
                }
            }
            return maxId + 1;
        }
    }
}
