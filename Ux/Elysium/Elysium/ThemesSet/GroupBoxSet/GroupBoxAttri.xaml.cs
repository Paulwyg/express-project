using System.Windows.Controls;

namespace Elysium.ThemesSet.GroupBoxSet
{
    /// <summary>
    /// TextBoxsAttri.xaml 的交互逻辑
    /// </summary>
    public partial class GroupBoxAttri : UserControl
    {
        public GroupBoxAttri()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取显示输入框
        /// </summary>
        public GroupBox ShowTextBox
        { get { return textboxshow; } }
    }
}
