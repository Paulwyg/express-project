using System.Collections.Generic;

namespace Wlst.Sr.Menu.Models
{
    /// <summary>
    /// 菜单类别控制；把程序中所有的菜单分门别类出来；
    /// </summary>
    public class MenuClassic
    {
        /// <summary>
        /// 菜单类别Id
        /// </summary>
        public int Id;
        /// <summary>
        /// 菜单类别名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 该菜单类别可控制的所有菜单Id列表
        /// </summary>
        public List<int> Items;

        public MenuClassic()
        {
            Id = 0;
            Name = "None";
            Items = new List<int>();
        }
    }
}
