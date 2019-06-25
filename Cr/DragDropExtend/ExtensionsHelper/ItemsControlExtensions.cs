using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Reflection;
using System.Windows.Controls.Primitives;
using System.Windows;
using System.Windows.Media;
using System.Collections;

namespace DragDropExtend.ExtensionsHelper
{
    public static class ItemsControlExtensions
    {
        public static bool CanSelectMultipleItems(this ItemsControl itemsControl)
        {
            if (itemsControl is MultiSelector)
            {
                // The CanSelectMultipleItems property is protected. Use reflection to
                // get it's value anyway.
                return (bool)itemsControl.GetType()
                    .GetProperty("CanSelectMultipleItems", BindingFlags.Instance | BindingFlags.NonPublic)
                    .GetValue(itemsControl, null);
            }
            else if (itemsControl is ListBox)
            {
                return ((ListBox)itemsControl).SelectionMode != SelectionMode.Single;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取本ItemsControl到child中可视化树中满足  ItemsControl的子item的容器类型的 控件
        /// ItemsControl的子item的容器  由末端到父端
        /// </summary>
        /// <param name="itemsControl">根ItemsControl </param>
        /// <param name="child">末端元素</param>
        /// <returns>跟元素到末端元素中  符合根元素子项控件的 元素  </returns>
        public static UIElement GetItemContainer(this ItemsControl itemsControl, UIElement child)
        {
            Type itemType = GetItemContainerType(itemsControl);

            if (itemType != null)
            {
                return (UIElement)child.GetVisualAncestor(itemType);
            }

            return null;
        }

        /// <summary>
        /// 获取本ItemsControl到child中可视化树中满足  ItemsControl的子item的容器类型的 控件
        /// ItemsControl的子item的容器  由末端到父端
        /// </summary>
        /// <param name="itemsControl">根ItemsControl </param>
        /// <param name="position">末端元素点击位置 </param>
        /// <returns>跟元素到末端元素中  符合根元素子项控件的 元素 未找到则null  </returns>
        public static UIElement GetItemContainerAt(this ItemsControl itemsControl, Point position)
        {
            IInputElement inputElement = itemsControl.InputHitTest(position);
            UIElement uiElement = inputElement as UIElement;

            if (uiElement != null)
            {
                return GetItemContainer(itemsControl, uiElement);
            }

            return null;
        }


        /// <summary>
        /// 获取ItemsControl下具体的子项容器  如 ListBox则获取到的为 ListBoxItem
        /// </summary>
        /// <param name="itemsControl">父控件</param>
        /// <returns>承载子项item的容器类型</returns>
        public static Type GetItemContainerType(this ItemsControl itemsControl)
        {
            // There is no safe way to get the item container type for an ItemsControl. The
            // best we can do is to look for the control's ItemsPresenter, get it's child 
            // panel and the first child of that *should* be an item container.
            //
            // If the control currently has no items, we're out of luck.
            if (itemsControl.Items.Count > 0)
            {
                //ItemsControl  -->  panel  -->  item container
                IEnumerable<ItemsPresenter> itemsPresenters = itemsControl.GetVisualDescendents<ItemsPresenter>();

                foreach (ItemsPresenter itemsPresenter in itemsPresenters)
                {
                    DependencyObject panel = VisualTreeHelper.GetChild(itemsPresenter, 0);
                    DependencyObject itemContainer = VisualTreeHelper.GetChild(panel, 0);

                    // Ensure that this actually *is* an item container by checking it with
                    // ItemContainerGenerator.

                    //判断 ItemsControl中确实存在 itemContainer类型的容器
                    if (itemContainer != null &&
                        itemsControl.ItemContainerGenerator.IndexFromContainer(itemContainer) != -1)
                    {
                        return itemContainer.GetType();
                    }
                }
            }

            return null;
        }

        public static Orientation GetItemsPanelOrientation(this ItemsControl itemsControl)
        {
            ItemsPresenter itemsPresenter = itemsControl.GetVisualDescendent<ItemsPresenter>();
            DependencyObject itemsPanel = VisualTreeHelper.GetChild(itemsPresenter, 0);
            PropertyInfo orientationProperty = itemsPanel.GetType().GetProperty("Orientation", typeof(Orientation));

            if (orientationProperty != null)
            {
                return (Orientation)orientationProperty.GetValue(itemsPanel, null);
            }
            else
            {
                // Make a guess!
                return Orientation.Vertical;
            }
        }

        public static IEnumerable GetSelectedItems(this ItemsControl itemsControl)
        {
            if (itemsControl is MultiSelector)
            {
                return ((MultiSelector)itemsControl).SelectedItems;
            }
            else if (itemsControl is ListBox)
            {
                ListBox listBox = (ListBox)itemsControl;

                if (listBox.SelectionMode == SelectionMode.Single)
                {
                    return Enumerable.Repeat(listBox.SelectedItem, 1);
                }
                else
                {
                    return listBox.SelectedItems;
                }
            }
            else if (itemsControl is TreeView)
            {
                return Enumerable.Repeat(((TreeView)itemsControl).SelectedItem, 1);
            }
            else if (itemsControl is Selector)
            {
                return Enumerable.Repeat(((Selector)itemsControl).SelectedItem, 1);
            }
            else
            {
                return Enumerable.Empty<object>();
            }
        }
    }
}
