using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Wlst.Ux.StateBarModule.OperatorDataQueryViewModel.Animations
{
    public static class Animation
    {
        public static void EnterFromLeftAndTop(UIElement element, double time, bool isrotate)
        {
            var gGroup = new TransformGroup();
            var rotate = new RotateTransform();

            var translate = new TranslateTransform();
            gGroup.Children.Add(rotate);
            gGroup.Children.Add(translate);

            element.RenderTransformOrigin = new Point(0.5, 0.5);
            element.RenderTransform = gGroup;

            //旋转
            var angle = new DoubleAnimation
                            {
                                From = 0,
                                To = 360,
                                RepeatBehavior = new RepeatBehavior(20*time),
                                Duration = TimeSpan.FromSeconds(0.05*time)
                            };

            //x轴移动
            var xAnimation = new DoubleAnimation {From = -1000, To = 0, Duration = TimeSpan.FromSeconds(time)};

            //Y轴移动
            var yAnimation = new DoubleAnimation {From = -1000, To = 0, Duration = TimeSpan.FromSeconds(time)};

            var sb = new Storyboard();
            sb.Children.Add(xAnimation);
            sb.Children.Add(yAnimation);
            sb.Children.Add(angle);

            Storyboard.SetTarget(xAnimation, element);
            Storyboard.SetTarget(yAnimation, element);
            Storyboard.SetTarget(angle, element);


            if (!isrotate)
            {
                gGroup.Children.Remove(rotate);
                sb.Children.Remove(angle);

                Storyboard.SetTargetProperty(xAnimation, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.X)"));
                Storyboard.SetTargetProperty(yAnimation, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.Y)"));
            }
            else
            {
                Storyboard.SetTargetProperty(angle, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(RotateTransform.Angle)"));
                Storyboard.SetTargetProperty(xAnimation, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[1].(TranslateTransform.X)"));
                Storyboard.SetTargetProperty(yAnimation, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[1].(TranslateTransform.Y)"));
            }
            sb.Begin();

        }

        public static void LeaveFromLeftAndBottom(UIElement element, double time, bool isrotate)
        {
            var gGroup = new TransformGroup();
            var rotate = new RotateTransform();

            var translate = new TranslateTransform();
            gGroup.Children.Add(rotate);
            gGroup.Children.Add(translate);

            element.RenderTransformOrigin = new Point(0.5, 0.5);
            element.RenderTransform = gGroup;

            //旋转
            var angle = new DoubleAnimation
                            {
                                From = 0,
                                To = 360,
                                RepeatBehavior = new RepeatBehavior(20*time),
                                Duration = TimeSpan.FromSeconds(0.05*time)
                            };

            //x轴移动
            var xAnimation = new DoubleAnimation {To = -1000, Duration = TimeSpan.FromSeconds(time)};

            //Y轴移动
            var yAnimation = new DoubleAnimation {To = 1000, Duration = TimeSpan.FromSeconds(time)};

            var sb = new Storyboard();
            sb.Children.Add(xAnimation);
            sb.Children.Add(yAnimation);
            sb.Children.Add(angle);

            Storyboard.SetTarget(xAnimation, element);
            Storyboard.SetTarget(yAnimation, element);
            Storyboard.SetTarget(angle, element);


            if (!isrotate)
            {
                gGroup.Children.Remove(rotate);
                sb.Children.Remove(angle);

                Storyboard.SetTargetProperty(xAnimation, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.X)"));
                Storyboard.SetTargetProperty(yAnimation, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.Y)"));
            }
            else
            {
                Storyboard.SetTargetProperty(angle, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(RotateTransform.Angle)"));
                Storyboard.SetTargetProperty(xAnimation, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[1].(TranslateTransform.X)"));
                Storyboard.SetTargetProperty(yAnimation, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[1].(TranslateTransform.Y)"));
            }
            sb.Begin();

        }
    }
}
