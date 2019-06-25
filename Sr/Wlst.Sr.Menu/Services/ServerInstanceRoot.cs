using System.Collections.Generic;
using Wlst.Sr.Menu.DataHold.MenuInstances;
using Wlst.Sr.Menu.Models;

namespace Wlst.Sr.Menu.Services
{
    public class ServerInstanceRoot
    {
        private static MenuInstancesHoldExtend menuInstances = new MenuInstancesHoldExtend();

        /// <summary>
        /// 更新菜单实例 无则增加，回写数据库
        /// </summary>
        /// <param name="keyId">唯一标示</param>
        /// <param name="name"></param>
        /// <param name="keyMenu"></param>
        /// <param name="idClassic"></param>
        public static  void UpdateMenuInstances(int keyId, string name, string keyMenu, int idClassic)
        {
            menuInstances.UpdateMenuInstances(keyId, name, keyMenu, idClassic);
        }

        /// <summary>
        /// 删除菜单实例，回写数据库
        /// </summary>
        /// <param name="keyId"></param>
        public static  void DeleteMenuInstances(int keyId)
        {
            menuInstances.DeleteMenuInstances(keyId);
        }

        ///// <summary>
        ///// 获取菜单值
        ///// </summary>
        ///// <param name="key"></param>
        ///// <returns>菜单实例id值 不存在则返回-1</returns>
        //public static int GetInstanceIdByKey(string key)
        //{
        //    int instanceId = -1;
        //    foreach (var t in MenuInstancesHolding.GetInstancesDic)
        //    {
        //        if (t.Value.Key.Equals(key))
        //        {
        //            instanceId = t.Key;
        //            break;
        //        }
        //    }
        //    return instanceId;
        //}

        /// <summary>
        /// 获取菜单值
        /// </summary>
        /// <param name="key"></param>
        /// <returns>菜单实例id值 不存在则返回null</returns>
        public static MenuInstance GetInstanceByKey(string key)
        {
            foreach (var t in menuInstances.GetInstancesDic)
            {
                if (t.Value.Key.Equals(key))
                {
                    return t.Value;
                    break;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取实例菜单信息
        /// </summary>
        /// <param name="keyId">实例菜单Id值 </param>
        /// <returns>实例菜单信息，包括名称以及右键关键字等  不存在则返回null</returns>
        public static MenuInstance GetInstancesValue(int keyId)
        {
            return menuInstances.GetInstancesValue(keyId);
        }

        /// <summary>
        /// 获取整个数据
        /// </summary>
        public static Dictionary<int, MenuInstance> GetInstancesDic
        {
            get { return menuInstances.GetInstancesDic; }
        }

        /// <summary>
        /// 获取可用的实例菜单值，返回值即可用的
        /// </summary>
        /// <returns></returns>
        public static int GetMaxAviableInstancesId()
        {
            return menuInstances.GetMaxAviableInstancesId();
        }

    }
}
