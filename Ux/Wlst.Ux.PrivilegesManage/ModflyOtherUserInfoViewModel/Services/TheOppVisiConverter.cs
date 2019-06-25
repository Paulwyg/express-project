using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Wlst.Ux.PrivilegesManage.ModflyOtherUserInfoViewModel.Services
{
    public class TheOppVisiConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {


            try
            {
                var visi = (Visibility) value;
                if (visi == Visibility.Visible) return Visibility.Collapsed;
                return visi == Visibility.Collapsed ? Visibility.Visible : visi;
            }
            catch (Exception ex)
            {
                Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
            return Visibility.Visible;
        }



        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
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
