namespace Wlst.Ux.Setting.SettingViewModel.Services
{
    public interface IITreeNode
    {
        /// <summary>
        /// 当前节点是否为系统当前选中节点
        /// 1、需要向外发布终端树当前选中的节点 
        /// 2、如果是则用户可能需要右击终端弹出菜单 
        ///    此时需要刷新菜单
        /// </summary>
        bool IsSelected { get; set; }


        /// <summary>
        /// 当前节点是否展开
        /// </summary>
        bool IsExpanded { get; set; }

        /// <summary>
        /// 需要导航到的视图地址
        /// </summary>
        int ViewId { get; set; }

        /// <summary>
        /// 节点名称  终端名称或是分组名称
        /// </summary>
        string NodeName { get; set; }
    }
}