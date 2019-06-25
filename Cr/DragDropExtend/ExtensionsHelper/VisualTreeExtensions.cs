using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace DragDropExtend.ExtensionsHelper
{
    public static class VisualTreeExtensions
    {
        /// <summary>
        /// 获取指定DependencyObject对象的满足类型为type的父类  搜索到第一个满足条件时  返回
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="d">指定DependencyObject对象</param>
        /// <returns>指定DependencyObject对象的满足类型为type的第一个父类</returns>
        public static T GetVisualAncestor<T>(this DependencyObject d) where T : class
        {
            DependencyObject item = VisualTreeHelper.GetParent(d);

            while (item != null)
            {
                T itemAsT = item as T;
                if (itemAsT != null) return itemAsT;
                item = VisualTreeHelper.GetParent(item);
            }

            return null;
        }


        /// <summary>
        /// 获取指定DependencyObject对象的满足类型为type的父类  搜索到第一个满足条件时  返回
        /// </summary>
        /// <param name="d">指定DependencyObject对象</param>
        /// <param name="type">目标类型</param>
        /// <returns>指定DependencyObject对象的满足类型为type的第一个父类</returns>
        public static DependencyObject GetVisualAncestor(this DependencyObject d, Type type)
        {
            DependencyObject item = VisualTreeHelper.GetParent(d);

            while (item != null)
            {
                if (item.GetType() == type) return item;
                item = VisualTreeHelper.GetParent(item);
            }

            return null;
        }


        /// <summary>
        /// 获取指定DependencyObject中符合类型T的Children集合中的第一个  如果为空则返回默认值
        /// </summary>
        /// <typeparam name="T">指定类型T 目标类型</typeparam>
        /// <param name="d">指定DependencyObject  原始DependencyObject</param>
        /// <returns>符合T类型的Children集合中的第一个  若为空则返回默认值</returns>
        public static T GetVisualDescendent<T>(this DependencyObject d) where T : DependencyObject
        {
            return d.GetVisualDescendents<T>().FirstOrDefault();
        }

        /// <summary>
        /// 获取指定DependencyObject中符合类型T的Children集合
        /// </summary>
        /// <typeparam name="T">指定类型T 目标类型</typeparam>
        /// <param name="d">指定DependencyObject  原始DependencyObject</param>
        /// <returns>符合T类型的Children集合</returns>
        public static IEnumerable<T> GetVisualDescendents<T>(this DependencyObject d) where T : DependencyObject
        {
            int childCount = VisualTreeHelper.GetChildrenCount(d);

            for (int n = 0; n < childCount; n++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(d, n);

                if (child is T)
                {
                    yield return (T)child;
                }

                foreach (T match in GetVisualDescendents<T>(child))
                {
                    yield return match;
                }
            }

            yield break;
        }


        /// <summary>
        /// 获取指定FrameworkElement控件的指定父类型为T的实例控件
        /// </summary>
        /// <typeparam name="T">需要获取的目标类型</typeparam>
        /// <param name="element">子控件</param>
        /// <returns>符合目标类型的父控件 无则返回null</returns>
        public static T GetTemplatedAncestor<T>(FrameworkElement element) where T : FrameworkElement
        {
            if (element is T)
            {
                return element as T;
            }

            FrameworkElement templatedParent = element.TemplatedParent as FrameworkElement;
            if (templatedParent != null)
            {
                return GetTemplatedAncestor<T>(templatedParent);
            }

            return null;
        }
    }
}
