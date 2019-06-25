using System.Collections.Generic;
using Wlst.Sr.Menu.Models;
using Wlst.Sr.Menu.Services;

namespace Wlst.Sr.Menu.DataHold.Classic
{
    public class ClassicDataHold
    {
        /// <summary>
        /// 模板菜单  控制菜单的总体分类，如主菜单包含的 所有菜单Id集合
        /// 对应数据库表为 menu_classic
        /// </summary>
        protected  Dictionary<int, MenuClassic> DicClassic =new Dictionary<int, MenuClassic>();



        /// <summary>
        /// 获取模板菜单信息
        /// </summary>
        /// <param name="keyId">模板菜单Id值 </param>
        /// <returns>模板菜单信息，包括名称以及控制的菜单集合  不存在则返回null</returns>
        public  MenuClassic GetClassicValue(int keyId)
        {
            if (DicClassic.ContainsKey(keyId)) return DicClassic[keyId];
            return null;
        }

        /// <summary>
        /// 获取整个数据
        /// </summary>
        public  Dictionary<int, MenuClassic> GetClassicDic
        {
            get { return DicClassic; }
        }


        /// <summary>
        /// 获取可用的实例菜单值，返回值即可用的
        /// </summary>
        /// <returns></returns>
        public  int GetMaxAviableClassicId()
        {
            int maxId = MenuIdControlAssign.MenuClassicIdMin;
            foreach (var t in DicClassic)
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
