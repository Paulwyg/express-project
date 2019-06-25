using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Wlst.Ux.PrivilegesManage.UserInfoManageViewModel.Animations
{
    public static class Animation
    {
        private static UIElement _updateElement;
        private static UIElement _addElement;
        private static double _time;

        #region EnterFromBottomWithUpdateHidden
        public static void EnterFromBottom(UIElement element, double time)
        {
            _addElement = element;
            _time = time;
            var sb = EnterFromBottomStoryboard();
            sb.Completed += EnterFromBottomCompleted;
            sb.Begin();

        }
        private static  Storyboard EnterFromBottomStoryboard ()
        {
            var height = new DoubleAnimation {From = 0, To = 230, Duration = TimeSpan.FromSeconds(_time)};

            var sb = new Storyboard();
            sb.Children.Add(height);

            Storyboard.SetTarget(height, _addElement);
            Storyboard.SetTargetProperty(height, new PropertyPath("(Grid.Height)"));
            return sb;
        }
        private static void EnterFromBottomCompleted (object sender, EventArgs e)
        {
            var sb = EnterFromBottomStoryboard();
            sb.Stop();
        }
        #endregion

        #region EnterFromBottomWithUpdateVisible
        public static void EnterFromBottomWithUpdateVisible(UIElement goelement,UIElement enterelement,double time)
        {
            _addElement = enterelement;
            _updateElement = goelement;
            _time = time;

            Storyboard sb = LeaveToLeftStoryBoard();
            sb.Completed += LeaveToLeftCompletedWithUpdateVisible;
            sb.FillBehavior=FillBehavior.HoldEnd;
            sb.Begin();
           
        }
        private static void LeaveToLeftCompletedWithUpdateVisible(object sender, EventArgs e)
        {
            Storyboard sb = EnterFromBottomStoryboard();
            sb.FillBehavior=FillBehavior.HoldEnd;
            sb.Completed += EnterFromBottomCompletedWithUpdateVisible;
            sb.Begin();
            
        }
        private static void EnterFromBottomCompletedWithUpdateVisible(object sender, EventArgs e)
        {
            Storyboard run0 = LeaveToLeftStoryBoard();
            Storyboard run1 = EnterFromBottomStoryboard();
            run1.Stop();
            run0.Stop();
        }

        #endregion

        #region LeaveToBottom
        public static void LeaveToBottom(UIElement element, double time)
        {
            _time = time;
            _addElement = element;
            Storyboard sb = LeaveToBottomStoryBoard();
            sb.Completed += LeaveToBottomCompleted;
            sb.Begin();

        }

        private static Storyboard LeaveToBottomStoryBoard()
        {
            var height = new DoubleAnimation {From = 230, To = 0, Duration = TimeSpan.FromSeconds(_time)};

            var sb = new Storyboard();
            sb.Children.Add(height);

            Storyboard.SetTarget(height, _addElement);
            Storyboard.SetTargetProperty(height, new PropertyPath("(Grid.Height)"));

            return sb;
        }
        private static  void LeaveToBottomCompleted(object sender,EventArgs e)
        {
            var sb = LeaveToBottomStoryBoard();
            sb.Stop();
        }


        #endregion

        #region UpdateEnterFromLeftWhenAddHidden
        public static void UpdateEnterFromLeftWhenAddHidden(UIElement element, double time)
        {

            _updateElement = element;
            _time = time;
            Storyboard xmove = Xmove();
            xmove.Completed += XmoveCompleted;
            xmove.FillBehavior=FillBehavior.HoldEnd;
            xmove.Begin();

        }

        private static Storyboard Xmove()
        {
            var translate = new TranslateTransform();
            _updateElement.RenderTransform = translate;
            var xstartAnimation = new DoubleAnimation {From = 0, To = 2000, Duration = TimeSpan.FromSeconds(0.1*_time)};

            var xmove = new Storyboard();
            xmove.Children.Add(xstartAnimation);
            Storyboard.SetTarget(xstartAnimation, _updateElement);
            Storyboard.SetTargetProperty(xstartAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));

            return xmove;
        }
        private static  Storyboard HStoryboard()
        {
            var height = new DoubleAnimation {From = 0, To = 230, Duration = TimeSpan.FromSeconds(0.3*_time)};

            var sb = new Storyboard();
            sb.Children.Add(height);

            Storyboard.SetTarget(height, _updateElement);
            
            Storyboard.SetTargetProperty(height, new PropertyPath("(Grid.Height)"));
           
            return sb;
        }
        private static Storyboard ThreeStoryboard()
        {

            var translate = new TranslateTransform();
            _updateElement.RenderTransform = translate;
            var xAnimation = new DoubleAnimation {From = 2000, To = 0, Duration = TimeSpan.FromSeconds(0.6*_time)};
            var sb=new Storyboard();
            sb.Children.Add(xAnimation);
            Storyboard.SetTarget(xAnimation, _updateElement);
            Storyboard.SetTargetProperty(xAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
            return sb;

        }
        private static void XmoveCompleted(object sender, EventArgs e)
        {
            Storyboard xx=HStoryboard();
            xx.Completed += HStoryboardCompleted;
            xx.FillBehavior=FillBehavior.HoldEnd;
            xx.Begin();
        }
        private static void HStoryboardCompleted(object sender,EventArgs e)
        {
            Storyboard run3 = ThreeStoryboard(); 
            run3.Completed += ThreeStoryboardCompleted;
            run3.Begin();
        }
        private static  void ThreeStoryboardCompleted(object sender,EventArgs e)
        {
            Storyboard run1 = Xmove();
            Storyboard run2 = HStoryboard();
            Storyboard run3 = ThreeStoryboard();
            run3.Stop();
            run2.Stop();
            run1.Stop();
        }

        #endregion

        #region UpdateEnterFromLeftWhenAddVisible
        public static  void UpdateEnterFromLeftWhenAddVisible(UIElement goelement,UIElement enterelement,double time)
        {
            _addElement = goelement;
            _updateElement = enterelement;
            _time = time;

            Storyboard sb = LeaveToBottomStoryBoard();
            sb.FillBehavior = FillBehavior.HoldEnd;
            sb.Completed += LeaveToBottomCompletedWithAddVisible;
            sb.Begin();

        }
        private static void LeaveToBottomCompletedWithAddVisible(object sender, EventArgs e)
        {
            Storyboard xmove = Xmove();
            xmove.Completed += XmoveCompletedAddVisible;
            xmove.Begin();
        }
        private static  void XmoveCompletedAddVisible(object sender,EventArgs e)
        {
            Storyboard xx = HStoryboard();
            xx.Completed += HStoryboardCompletedAddVisible;
            xx.FillBehavior = FillBehavior.HoldEnd;
            xx.Begin();
        }
        private static  void HStoryboardCompletedAddVisible(object sender,EventArgs e)
        {
            Storyboard run3 = ThreeStoryboard();
            run3.Completed += ThreeStoryboardCompletedWithAddVisible;
            run3.Begin();
        }
        private static void ThreeStoryboardCompletedWithAddVisible(object sender, EventArgs e)
        {
            Storyboard run0 = LeaveToBottomStoryBoard();
            Storyboard run1 = Xmove();
            Storyboard run2 = HStoryboard();
            Storyboard run3 = ThreeStoryboard();
            run3.Stop();
            run2.Stop();
            run1.Stop();
            run0.Stop();
        }
        #endregion

        #region LeaveToLeft
        public static void LeaveToLeft(UIElement element, double time)
        {
            _updateElement = element;
            _time = time;
            Storyboard sb = LeaveToLeftStoryBoard();
            sb.Begin();
            sb.Completed+=LeaveToLeftCompleted;
        }
        private static Storyboard LeaveToLeftStoryBoard()
        {
            var height = new DoubleAnimation {To = 0, From = 230, Duration = TimeSpan.FromSeconds(_time)};
            var sb = new Storyboard();
            sb.Children.Add(height);
            Storyboard.SetTarget(height, _updateElement);
            Storyboard.SetTargetProperty(height, new PropertyPath("(Grid.Height)"));
            return sb;
        }
        private static void  LeaveToLeftCompleted(object sender,EventArgs e)
        {
            Storyboard sb = LeaveToLeftStoryBoard();
            sb.Stop();
        }
        #endregion 
    }
}
