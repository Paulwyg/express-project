using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Wlst.Ux.EquipmentGroupManage.GrpMulitManageViewModel.Animations
{
    public static class Animation
    {
        public static void EnterFromLeftAndTop(UIElement element, double time)
        {
            element.RenderTransformOrigin = new Point(0.5, 0.5);


            //x轴移动
            DoubleAnimation xAnimation = new DoubleAnimation();
            xAnimation.From = -4000;
            xAnimation.To = 0;
            xAnimation.Duration = TimeSpan.FromSeconds(time);


            Storyboard sb = new Storyboard();
            sb.Children.Add(xAnimation);

            Storyboard.SetTarget(xAnimation, element);
            Storyboard.SetTargetProperty(xAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
            sb.Begin();

        }

        public static void LeaveFromLeftAndBottom(UIElement element, double time)
        {
            element.RenderTransformOrigin = new Point(0.5, 0.5);
            //x轴移动
            DoubleAnimation xAnimation = new DoubleAnimation();
            xAnimation.To = -4000;
            xAnimation.Duration = TimeSpan.FromSeconds(time);
            Storyboard sb = new Storyboard();
            sb.Children.Add(xAnimation);
            Storyboard.SetTarget(xAnimation, element);
            Storyboard.SetTargetProperty(xAnimation,
                                         new PropertyPath(
                                             "(UIElement.RenderTransform).(TranslateTransform.X)"));
            sb.Begin();

        }
    }
}
