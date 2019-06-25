using System.Windows;
using Microsoft.Windows.Shell;

namespace WindowForWlst
{
    internal class MaximizeButton : CaptionButton
    {
        static MaximizeButton()
        {
            
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaximizeButton), new FrameworkPropertyMetadata(typeof(MaximizeButton)));
        }



        protected override void OnClick()
        {
            base.OnClick();
            var w = Window.GetWindow(this);
            if (w != null && w.WindowState == WindowState.Maximized)
                SystemCommands.RestoreWindow(w);
            else
                SystemCommands.MaximizeWindow(w);
        }


    }
}
