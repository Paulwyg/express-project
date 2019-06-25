using System.Windows;
using System.Windows.Controls;
using System.Collections;
using System.Windows.Input;
using DragDropExtend.ExtensionsHelper;

namespace DragDropExtend.DragAndDrop
{
    /// <summary>
    /// 获取鼠标所属ItemsControl控件的信息，包括点击的ItemsControl以及被选中的Item
    /// </summary>
    public class DropTargetInfomation
    {
        /// <summary>
        /// 获取鼠标所属ItemsControl控件的信息，包括点击的ItemsControl以及被选中的Item
        /// </summary>
        /// <param name="sender">事件触发控件</param>
        /// <param name="e">拖动鼠标放开时的DragEventArgs参数</param>
        public DropTargetInfomation(object sender, DragEventArgs e)
        {
            VisualTarget = sender as UIElement;
            var itemsControl = sender as ItemsControl;
            if (itemsControl == null) return;
            var item = itemsControl.GetItemContainerAt(e.GetPosition(itemsControl));

            VisualTargetOrientation = itemsControl.GetItemsPanelOrientation();

            if (item != null)
            {
                var itemParent = ItemsControl.ItemsControlFromItemContainer(item);

                InsertIndex = itemParent.ItemContainerGenerator.IndexFromContainer(item);
                TargetCollection = itemParent.ItemsSource ?? itemParent.Items;
                TargetItem = itemParent.ItemContainerGenerator.ItemFromContainer(item);
                VisualTargetItem = item;

                if (VisualTargetOrientation == Orientation.Vertical)
                {
                    if (e.GetPosition(item).Y > item.RenderSize.Height / 2) InsertIndex++;
                }
                else
                {
                    if (e.GetPosition(item).X > item.RenderSize.Width / 2) InsertIndex++;
                }
            }
            else
            {
                TargetCollection = itemsControl.ItemsSource ?? itemsControl.Items;
                InsertIndex = itemsControl.Items.Count;
            }
        }

        /// <summary>
        /// 获取鼠标所属ItemsControl控件的信息，包括点击的ItemsControl以及被选中的Item
        /// </summary>
        /// <param name="sender">事件触发控件</param>
        /// <param name="e">鼠标参数</param>
        public DropTargetInfomation(object sender, MouseEventArgs e)
        {
            VisualTarget = sender as UIElement;
            var itemsControl = sender as ItemsControl;
            if (itemsControl == null) return;
            var item = itemsControl.GetItemContainerAt(e.GetPosition(itemsControl));

            VisualTargetOrientation = itemsControl.GetItemsPanelOrientation();

            if (item != null)
            {
                var itemParent = ItemsControl.ItemsControlFromItemContainer(item);

                InsertIndex = itemParent.ItemContainerGenerator.IndexFromContainer(item);
                TargetCollection = itemParent.ItemsSource ?? itemParent.Items;
                TargetItem = itemParent.ItemContainerGenerator.ItemFromContainer(item);
                VisualTargetItem = item;

                if (VisualTargetOrientation == Orientation.Vertical)
                {
                    if (e.GetPosition(item).Y > item.RenderSize.Height / 2) InsertIndex++;
                }
                else
                {
                    if (e.GetPosition(item).X > item.RenderSize.Width / 2) InsertIndex++;
                }
            }
            else
            {
                TargetCollection = itemsControl.ItemsSource ?? itemsControl.Items;
                InsertIndex = itemsControl.Items.Count;
            }
        }

        /// <summary>
        /// 及时性获取选中项后台viewmodel
        /// </summary>
        /// <param name="sender">事件触发控件 ，必须为继承ItemsControl</param>
        /// <returns>被选中项的VM 无法查阅则为null</returns>
        public static object GetSelectItemByUiElement(object sender)
        {
            object targetItem = null;
            var itemsControl = sender as ItemsControl;
            if (itemsControl == null) return null;
            var item = itemsControl.GetItemContainerAt(Mouse.GetPosition(itemsControl));
            if (item != null)
            {
                var itemParent = ItemsControl.ItemsControlFromItemContainer(item);
                targetItem = itemParent.ItemContainerGenerator.ItemFromContainer(item);
            }
            return targetItem;
        }

        /// <summary>
        /// 及时性获取选中项后台viewmodel
        /// </summary>
        /// <param name="sender">事件触发控件 ，必须为继承ItemsControl</param>
        /// <param name="e">鼠标参数</param>
        /// <returns>被选中项的VM 无法查阅则为null</returns>
        public static object GetSelectItemByUiElement(object sender,MouseEventArgs e)
        {
            object targetItem = null;
            var itemsControl = sender as ItemsControl;
            if (itemsControl == null) return null;
            var item = itemsControl.GetItemContainerAt(e.GetPosition(itemsControl));
            if (item != null)
            {
                var itemParent = ItemsControl.ItemsControlFromItemContainer(item);
                targetItem = itemParent.ItemContainerGenerator.ItemFromContainer(item);
            }
            return targetItem;
        }

        /// <summary>
        /// 及时性获取选中项后台viewmodel
        /// </summary>
        /// <param name="sender">事件触发控件 ，必须为继承ItemsControl</param>
        /// <param name="e">拖动鼠标放开时的DragEventArgs参数</param>
        /// <returns>被选中项的VM 无法查阅则为null</returns>
        public static object GetSelectItemByUiElement(object sender, DragEventArgs e)
        {
            object targetItem = null;
            var itemsControl = sender as ItemsControl;
            if (itemsControl == null) return null;
            var item = itemsControl.GetItemContainerAt(e.GetPosition(itemsControl));
            if (item != null)
            {
                var itemParent = ItemsControl.ItemsControlFromItemContainer(item);
                targetItem = itemParent.ItemContainerGenerator.ItemFromContainer(item);
            }
            return targetItem;
        }

        /// <summary>
        /// 拖动松开时 处于鼠标下需要插入数据的位置
        /// </summary>
        public int InsertIndex { get; private set; }

        /// <summary>
        /// 拖动松开时 处于鼠标处的item集合 VM
        /// </summary>
        public IEnumerable TargetCollection { get; private set; }

        /// <summary>
        /// 拖动松开时 处于鼠标处的item  后台VM
        /// </summary>
        public object TargetItem { get; private set; }

        /// <summary>
        /// 可视化树中发出sender命令的元素
        /// </summary>
        private UIElement VisualTarget { get;  set; }

        /// <summary>
        /// 可视化树中的点击元素
        /// </summary>
        private UIElement VisualTargetItem { get;  set; }


        private Orientation VisualTargetOrientation { get;  set; }
    }
}
