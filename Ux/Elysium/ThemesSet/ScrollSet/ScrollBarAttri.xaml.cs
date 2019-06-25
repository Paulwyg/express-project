using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Elysium.ThemesSet.ScrollSet
{
    /// <summary>
    /// ScrollBarAttri.xaml 的交互逻辑
    /// </summary>
    public partial class ScrollBarAttri : UserControl
    {
        public ScrollBarAttri()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 显示效果按钮
        /// </summary>
        public ScrollBar ShowButton
        {
            get { return scrollBar5; }
        }
    }
}
