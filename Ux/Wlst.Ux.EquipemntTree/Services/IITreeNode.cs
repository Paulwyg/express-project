using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Ux.EquipemntTree.Models;

namespace Wlst.Ux.EquipemntTree.Services
{
    public interface IITreeNode
    {
        /// <summary>
        /// 目标设备类型
        /// </summary>
        TreeNodeModel NodeModel { get; set; }


        /// <summary>
        /// 节点类型 是组节点 还是终端节点
        /// 或是其他节点
        /// </summary>
        TreeNodeType NodeType { get; set; }



        /// <summary>
        /// 设置节点前景颜色
        /// </summary>
        Color ForeGround { get; set; }

        /// <summary>
        /// 背景色
        /// </summary>
        Color BackGround { get; set; }


        /// <summary>
        /// 设置节点是否可见
        /// </summary>
        Visibility Visi { get; set; }


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
        /// 前台界面绑定的图标
        /// </summary>
        object  ImagesIcon { get; set; }

        /// <summary>
        /// 终端地址或分组地址4为地址化
        /// </summary>
        string FormatNodeName { get; }

        /// <summary>
        /// 节点ID  终端地址或分组地址  
        /// 如果为设备类型地址则为终端地址1000000+终端地址  一百万+终端地址
        /// </summary>
        int NodeId { get; }


        /// <summary>
        /// 节点名称  终端名称或是分组名称
        /// </summary>
        string NodeName { get; set; }


        /// <summary>
        /// 菜单  右键菜单  在节点被选中的时候显示刷新右键菜单
        /// </summary>
        ObservableCollection<IIMenuItem>  CmItems { get; }

    }
}
