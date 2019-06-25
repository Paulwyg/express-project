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

namespace Elysium.ThemesSet.ScrollViewSet
{
    /// <summary>
    /// ScrollVeiwAttri.xaml 的交互逻辑
    /// </summary>
    public partial class ScrollVeiwAttri : UserControl
    {
        public ScrollVeiwAttri()
        {
            InitializeComponent();
        }

        public ScrollViewer ShowView
        {
            get { return showview; }
        }
    }
}
