using System.Windows.Controls;

namespace Elysium.ThemesSet.TextBlockSet
{
    /// <summary>
    /// TextBoxsAttri.xaml 的交互逻辑
    /// </summary>
    public partial class TextBlockAttri : UserControl
    {
        public TextBlockAttri()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取显示输入框
        /// </summary>
        public TextBlock ShowTextBox
        { get { return textboxshow; } }
    }
}
