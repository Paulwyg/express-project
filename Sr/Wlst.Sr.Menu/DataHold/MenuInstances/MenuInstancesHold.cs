using System.Collections.Generic;
using Wlst.Sr.Menu.Models;
using Wlst.Sr.Menu.Services;

namespace Wlst.Sr.Menu.DataHold.MenuInstances
{
    public class MenuInstancesHold
    {
        /// <summary>
        /// 实例菜单根信息
        /// 对应数据库表为 menu_instances
        /// </summary>
        protected  Dictionary<int, MenuInstance> DicInstances =
            new Dictionary<int, MenuInstance>();



        /// <summary>
        /// 获取实例菜单信息
        /// </summary>
        /// <param name="keyId">实例菜单Id值 </param>
        /// <returns>实例菜单信息，包括名称以及右键关键字等  不存在则返回null</returns>
        public  MenuInstance GetInstancesValue(int keyId)
        {
            if (DicInstances.ContainsKey(keyId)) return DicInstances[keyId];
            return null;
        }

        /// <summary>
        /// 获取整个数据
        /// </summary>
        public  Dictionary<int, MenuInstance> GetInstancesDic
        {
            get { return DicInstances; }
        }

        /// <summary>
        /// 获取可用的实例菜单值，返回值即可用的
        /// </summary>
        /// <returns></returns>
        public  int GetMaxAviableInstancesId()
        {
            int maxId = MenuIdControlAssign.MenuInstanceKeyIdMin;
            foreach (var t in DicInstances)
            {
                if (t.Value.Id >= maxId)
                {
                    maxId = t.Value.Id;
                }
            }
            return maxId + 1;
        }
    }
}
