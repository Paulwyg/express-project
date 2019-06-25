using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Elysium.ThemesSet.TextBoxsSet
{
    /// <summary>
    /// TextBoxsAttri.xaml 的交互逻辑
    /// </summary>
    public partial class TextBoxsAttri : UserControl
    {
        public TextBoxsAttri()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取显示输入框
        /// </summary>
        public TextBox ShowTextBox
        { get { return textboxshow; } }
    }
}
