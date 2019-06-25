using System.Windows;

namespace WindowForWlst
{
    internal class CloseButton : CaptionButton
    {
        static CloseButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CloseButton), new FrameworkPropertyMetadata(typeof(CloseButton)));
        }

    }
}
