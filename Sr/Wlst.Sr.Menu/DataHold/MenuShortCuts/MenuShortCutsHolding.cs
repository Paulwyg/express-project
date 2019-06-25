using System.Collections.Generic;

namespace Wlst.Sr.Menu.DataHold.MenuShortCuts
{
    /// <summary>
    /// 保存程序主菜单所有快捷键原始信息
    /// 执行程序加载时自动加载数据库中保存的自定义快捷键信息
    /// 
    /// 本类对应的数据库表为 menu_shortcuts
    /// </summary>
   public class MenuShortCutsHolding
   {

       protected static Dictionary<int, string > DicClassic =
           new Dictionary<int, string >();



       /// <summary>
       /// 获取快捷键信息
       /// </summary>
       /// <param name="keyId">菜单Id值 </param>
       /// <returns>快捷键信息  不存在则返回null</returns>
       public static string  GetClassicValue(int keyId)
       {
           if (DicClassic.ContainsKey(keyId)) return DicClassic[keyId];
           return null;
       }

       /// <summary>
       /// 获取整个数据
       /// </summary>
       public static Dictionary<int, string > GetClassicDic
       {
           get { return DicClassic; }
       }
   }
}
