namespace Wlst.Sr.Menu.Models
{
    /// <summary>
    /// 菜单实例关系，主要包含父子关系
    /// </summary>
    public class MenuInstancesRelation
    {
        /// <summary>
        /// 本节点的父节点地址
        /// </summary>
        public int FatherId;
        /// <summary>
        /// 本节点地址
        /// </summary>
        public int Id;
        /// <summary>
        /// 本节点在父节点下的子列表的排序序号
        /// </summary>
        public int SortIndex;
        /// <summary>
        /// 本节点的名称，如果为菜单夹则有名称，否则使用默认名称或汉化的名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 本节点所属的菜单实例的根节点地址，即实例菜单的地址
        /// </summary>
        public int InstancesId;
    }
}
