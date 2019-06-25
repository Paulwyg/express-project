using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Xml.Serialization;
using Telerik.Windows.Controls;
using Wlst.Cr.Core.CoreInterface;


namespace Elysium.Converters
{
    public class TabItemNameConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {


            try
            {
                var radItem = value as RadPane;
                if (radItem != null)
                {
                    return radItem.Header;
                }



                var tb = value as TabItem;
                if (tb == null) return "Header";
                var dc = tb.DataContext;
                if (dc == null) return "Header";
                var mvvm = dc as IITab;
                if (mvvm != null) return mvvm.Title;
                var gggg = tb.Tag;
                if (gggg != null) return gggg;

            }
            catch (Exception ex)
            {
                //Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
            return "Header";
        }

      

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
           {
               return "Header";
           }

        ///// <summary>
        ///// 获取指定DependencyObject对象的满足类型为type的父类  搜索到第一个满足条件时  返回
        ///// </summary>
        ///// <param name="d">指定DependencyObject对象</param>
        ///// <param name="type">目标类型</param>
        ///// <returns>指定DependencyObject对象的满足类型为type的第一个父类</returns>
        //public DependencyObject GetVisualAncestor(DependencyObject d, Type type)
        //{
        //    DependencyObject item = VisualTreeHelper.GetParent(d);

        //    while (item != null)
        //    {
        //        if (item.GetType() == type) return item;
        //        item = VisualTreeHelper.GetParent(item);
        //    }

        //    return null;
        //}
       
    }
}
