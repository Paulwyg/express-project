namespace Wlst.Sr.Menu.Models
{
    /// <summary>
    /// 菜单实例根节点信息
    /// </summary>
    public class MenuInstance
    {
        /// <summary>
        /// 实例菜单Id值
        /// </summary>
        public int Id;

        /// <summary>
        /// 实例菜单名称
        /// </summary>
        public string Name;

        /// <summary>
        /// 菜单关键字
        /// </summary>
        public string Key;

        /// <summary>
        /// 使用的模板菜单Id
        /// </summary>
        public int IdClassic;

        public MenuInstance()
        {
            Id = -1;
            Name = "";
            Key = "-1";
            IdClassic = -1;
        }
    }
}
