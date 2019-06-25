using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Wlst.Ux.TimeTableSystem.TimeTableManagViewModel.Animations
{
    public static class CancelAnimation
    {

        private static UIElement _uIeTableManage;
        private static UIElement _uIeTableAdd;

        public static void CancelAn(UIElement tableManage, UIElement tableAdd)
        {
            _uIeTableManage = tableManage;
            _uIeTableAdd = tableAdd;
            _uIeTableAdd.Visibility=Visibility.Collapsed;
            _uIeTableManage.IsEnabled = true;
            var sbManage = OpacityManageStoryboard();
            var sbAdd = OpacityAddStoryBoard();
            sbManage.Begin();
            sbAdd.Begin();
        }

        private static Storyboard OpacityManageStoryboard()
        {
            var animation = new DoubleAnimation {  To = 1, Duration = TimeSpan.FromSeconds(0.2) };
            var sb = new Storyboard();
            sb.Children.Add(animation);

            Storyboard.SetTarget(animation, _uIeTableManage);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(Grid.Opacity)"));
            return sb;
        }

        private static Storyboard OpacityAddStoryBoard()
        {
            var animation = new DoubleAnimation { To = 0, Duration = TimeSpan.FromSeconds(0.5) };
            var sb = new Storyboard();
            sb.Children.Add(animation);

            Storyboard.SetTarget(animation, _uIeTableAdd);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(Grid.Opacity)"));
            return sb; 
        }
    }
}
