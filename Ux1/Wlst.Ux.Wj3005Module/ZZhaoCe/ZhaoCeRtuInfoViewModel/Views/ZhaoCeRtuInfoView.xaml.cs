using System.ComponentModel.Composition;
using System.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.WJ3005Module.ZZhaoCe.ZhaoCeRtuInfoViewModel.Services;

namespace Wlst.Ux.WJ3005Module.ZZhaoCe.ZhaoCeRtuInfoViewModel.Views
{
    /// <summary>
    /// ZhaoCeRtuInfoView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(WJ3005Module.Services.ViewIdAssign.ZhaoCeRtuInfoViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class ZhaoCeRtuInfoView : UserControl
    {
        public ZhaoCeRtuInfoView()
        {
            InitializeComponent();
        }

        [Import]
        public IIZhaoCeRtuInfoViewModel Model
        {
            get { return DataContext as IIZhaoCeRtuInfoViewModel; }
            set { DataContext = value; }
        }
    }
}
