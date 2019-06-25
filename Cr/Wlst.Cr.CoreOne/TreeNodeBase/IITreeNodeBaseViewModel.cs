using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Wlst.Cr.CoreOne.CoreInterface;

namespace Wlst.Cr.CoreOne.TreeNodeBase
{
    public interface IITreeNodeBaseViewModel
    {
        ///// <summary>
        ///// 设置节点前景颜色
        ///// </summary>
        //string ForeGround { get; set; }

        ///// <summary>
        ///// 背景色
        ///// </summary>
        //string BackGround { get; set; }


        //Visibility Visi { get; set; }

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
        object ImagesIcon { get; set; }


        /// <summary>
        /// 节点ID  
        /// </summary>
        int NodeId { get; }


        /// <summary>
        /// 节点名称  终端名称或是分组名称
        /// </summary>
        string NodeName { get; set; }


        /// <summary>
        /// 菜单  右键菜单  在节点被选中的时候显示刷新右键菜单
        /// </summary>
        ObservableCollection<IIMenuItem> CmItems { get; set; }

        /// <summary>
        /// 更新菜单函数
        /// </summary>
        void ResetContextMenu();

        void ReloadChild();

        void OnNodeSelectActive();

        /// <summary>
        /// 当节点取消选中状态时;
        /// 如果节点有子节点则删除所有子节点可在此操作
        /// </summary>
        void OnNodeSelectDisActive();
    }
}