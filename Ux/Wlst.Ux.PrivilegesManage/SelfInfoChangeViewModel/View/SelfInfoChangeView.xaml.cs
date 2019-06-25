using System.ComponentModel.Composition;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.PrivilegesManage.SelfInfoChangeViewModel.Services;
using Wlst.Ux.PrivilegesManage.Services;

namespace Wlst.Ux.PrivilegesManage.SelfInfoChangeViewModel.View
{
    /// <summary>
    /// SelfInfoChangeView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(ViewIdAssign.SelfInfoChangeViewId)]
    public partial class SelfInfoChangeView
    {
        public SelfInfoChangeView()
        {
            InitializeComponent();
        }

        [Import]
        public IISelfInfoChangeViewModel Model
        {
            get { return DataContext as IISelfInfoChangeViewModel; }
            set { DataContext = value; }
        }
    }
}
