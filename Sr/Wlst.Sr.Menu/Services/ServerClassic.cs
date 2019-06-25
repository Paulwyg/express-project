using System.Collections.Generic;
using Wlst.Sr.Menu.DataHold.Classic;
using Wlst.Sr.Menu.Models;

namespace Wlst.Sr.Menu.Services
{
    public class ServerClassic
    {
        private static ClassicDataHoldExtend classic = new ClassicDataHoldExtend();


        /// <summary>
        /// 更新模板菜单 无则增加,并回写数据库，发布事件
        /// </summary>
        /// <param name="keyId">唯一标示</param>
        /// <param name="name">模板菜单名称</param>
        /// <param name="content">模板菜单包含的菜单集合</param>
        public static void UpdateMneu(int keyId, string name, List<int> content)
        {
            classic.UpdateMneu(keyId, name, content);
        }


        /// <summary>
        /// 删除模板菜单,并删除数据库中保留的模板菜单，发布事件
        /// </summary>
        /// <param name="keyId"></param>
        public static void DeleteMneu(int keyId)
        {
            classic.DeleteMneu(keyId);
        }

        /// <summary>
        /// 获取模板菜单信息
        /// </summary>
        /// <param name="keyId">模板菜单Id值 </param>
        /// <returns>模板菜单信息，包括名称以及控制的菜单集合  不存在则返回null</returns>
        public static MenuClassic GetClassicValue(int keyId)
        {
            return classic.GetClassicValue(keyId);
        }

        /// <summary>
        /// 获取整个数据
        /// </summary>
        public static Dictionary<int, MenuClassic> GetClassicDic
        {
            get { return classic.GetClassicDic; }
        }


        /// <summary>
        /// 获取可用的实例菜单值，返回值即可用的
        /// </summary>
        /// <returns></returns>
        public static int GetMaxAviableClassicId()
        {
            return classic.GetMaxAviableClassicId();
        }
    }
}
