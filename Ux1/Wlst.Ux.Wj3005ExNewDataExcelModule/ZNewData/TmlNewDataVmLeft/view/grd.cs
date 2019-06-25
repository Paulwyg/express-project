using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Wlst.Ux.Wj3005ExNewDataExcelModule.ZNewData.TmlNewDataVmLeft.view
{
    public class grd
    {
        
        public static bool IsVerticalScrollBarAtButtom(ScrollViewer sc)
        {
            

            bool isAtButtom = false;

            // get the vertical scroll position
            double dVer = sc.VerticalOffset;

            //get the vertical size of the scrollable content area
            double dViewport = sc.ViewportHeight;

            //get the vertical size of the visible content area
            double dExtent = sc.ExtentHeight;
            dExtent = dExtent - 20*12;

            if (dVer > 0.001)
            {
                if (dExtent-dVer - dViewport < 0.001)
                {
                    isAtButtom = true;
                }
                else
                {
                    isAtButtom = false;
                }
            }
            else
            {
                isAtButtom = false;
            }

            if (sc.VerticalScrollBarVisibility == ScrollBarVisibility.Disabled
                || sc.VerticalScrollBarVisibility == ScrollBarVisibility.Hidden)
            {
                isAtButtom = true;
            }

            return isAtButtom;

        }



        public FrameworkElement FindChildByType(DependencyObject relate, Type type)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(relate); i++)
            {
                var el = VisualTreeHelper.GetChild(relate, i) as FrameworkElement;
                if (el == null) continue;
                if (el.GetType() == type)
                {
                    return el;

                }
                else
                {
                    var sp = FindChildByType(el, type);
                    if (sp != null) return sp;
                }
            }
            return null;
        }

        /// <summary>
        /// 获得子控件
        /// </summary>
        /// <typeparam name="T">要获得控件类名</typeparam>
        /// <param name="obj">当前控件名</param>
        /// <param name="name">要查询子控件名</param>
        /// <returns>要获得控件类名</returns>
        public static T GetChildObject<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            DependencyObject child = null;
            T grandChild = null;


            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);


                if (child is T && (((T)child).Name == name | string.IsNullOrEmpty(name)))
                {
                    return (T)child;
                }
                else
                {
                    grandChild = GetChildObject<T>(child, name);
                    if (grandChild != null)
                        return grandChild;
                }
            }


            return null;


        }

        /// <summary>
        /// 获得子控件
        /// </summary>
        /// <typeparam name="T">要获得控件类名</typeparam>
        /// <param name="obj">当前控件名</param>
        /// <param name="name">要查询子控件名</param>
        /// <returns>要获得控件类名</returns>
        public static T GetChildObject<T>(DependencyObject obj, Type name) where T : FrameworkElement
        {
            DependencyObject child = null;
            T grandChild = null;


            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);


                if (child is T)
                {
                    return (T)child;
                }
                else
                {
                    grandChild = GetChildObject<T>(child, name);
                    if (grandChild != null)
                        return grandChild;
                }
            }


            return null;


        }

    }
}
