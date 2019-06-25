using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace Elysium.ThemesSet.RadTreeViewSet
{
    /// <summary>
    /// RadTreeViewAttri.xaml 的交互逻辑
    /// </summary>
    public partial class RadTreeViewAttri : UserControl
    {
        public RadTreeViewAttri()
        {
            InitializeComponent();
        }
        public RadTreeView ShowView
        {
            get { return showradtree; }
        }
    }
}
