using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Wlst.Ux.TimeTableSystem.TimeTableManagViewModel.Animations
{
    public static class SaveOrCancelAnimation
    {

        private static UIElement _uIeTableManage;
        private static UIElement _uIeTableAdd;

        public static void SaveOrCancelAn(UIElement tableManage,UIElement tableAdd)
        {
            _uIeTableManage = tableManage;
            _uIeTableAdd = tableAdd;

            var sb = TranslateTransformStoryboard();
            sb.Begin();
        }

        private static Storyboard OpacityStoryboard()
        {
            var animation = new DoubleAnimation { From = 0, To = 1, Duration = TimeSpan.FromSeconds(0.7) };
            var sb = new Storyboard();
            sb.Children.Add(animation);
            //sb.Completed += OpacityStoryboardCompleted;
            Storyboard.SetTarget(animation, _uIeTableManage);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(Grid.Opacity)"));
            return sb;
        }
        private static void OpacityStoryboardCompleted(object sender, EventArgs e)
        {
            var sbopacity = OpacityStoryboard();
            var sbtrans = TranslateTransformStoryboard();
            
            sbtrans.Stop();
            sbopacity.Stop();
        }

        private static Storyboard TranslateTransformStoryboard()
        {
            var translate = new TranslateTransform();
            _uIeTableAdd.RenderTransform = translate;
            var xstartAnimation = new DoubleAnimation { From = 0, To = -2000, Duration = TimeSpan.FromSeconds(0.3) };

            var xmove = new Storyboard();
            xmove.Completed += TranslateTransformCompleted;
            xmove.Children.Add(xstartAnimation);
            Storyboard.SetTarget(xstartAnimation, _uIeTableAdd);
            Storyboard.SetTargetProperty(xstartAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
            return xmove;
        }
        private static void TranslateTransformCompleted(object sender, EventArgs e)
        {
            var sb = OpacityStoryboard();
            sb.Begin();

        }
    }
}
