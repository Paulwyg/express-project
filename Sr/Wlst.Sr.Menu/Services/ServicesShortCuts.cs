using System.Collections.Generic;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Sr.Menu.DataHold.MenuShortCuts;

namespace Wlst.Sr.Menu.Services
{

    /// <summary>
    /// 系统快捷键
    /// </summary>
    public class ServicesShortCuts
    {
        private static MenuShortCutsHoldingExtend shortCutsHolding = new MenuShortCutsHoldingExtend();


        /// <summary>
        /// 更新或增加快捷键信息,立即回写数据库并发布事件
        /// </summary>
        /// <param name="menuId">菜单部件唯一标示</param>
        /// <param name="shortcut">快捷键信息格式为  ModifierKeys+Key 或者 Key</param>
        public static void UpdateShortCut(int menuId, string shortcut)
        {
            shortCutsHolding.UpdateShortCut(menuId, shortcut);
        }

        /// <summary>
        /// 删除快捷键信息，立即回写数据库并发布事件
        /// </summary>
        /// <param name="menuId"></param>
        public static void DeleteShortCut(int menuId)
        {
            shortCutsHolding.DeleteShortCut(menuId);
        }

        /// <summary>
        /// 获取快捷键信息
        /// </summary>
        /// <param name="menuId">菜单Id值 </param>
        /// <returns>快捷键信息  不存在则返回null</returns>
        public static string GetShortCutValue(int menuId)
        {
            return MenuShortCutsHolding.GetClassicValue(menuId);
        }

        /// <summary>
        /// 获取整个数据
        /// </summary>
        public static Dictionary<int, string> GetShortCutDic
        {
            get { return MenuShortCutsHolding.GetClassicDic; }
        }


        /// <summary>
        /// 注册实例菜单信息到运行程序快捷键
        /// </summary>
        /// <param name="menuItem"></param>
        /// <param name="strWantedPressKys">快捷键信息格式为  ModifierKeys+Key 或者 Key</param>
        public static void AddRegisterMenuItem(IIMenuItem menuItem, string strWantedPressKys)
        {
            RegisteShortCuts.AddMenuItem(menuItem, strWantedPressKys);
        }

        /// <summary>
        /// 清除所有注册到程序的快捷键信息
        /// </summary>
        public static void ClearRegisterLst()
        {
            RegisteShortCuts.ClearLst();
        }
    }
}
