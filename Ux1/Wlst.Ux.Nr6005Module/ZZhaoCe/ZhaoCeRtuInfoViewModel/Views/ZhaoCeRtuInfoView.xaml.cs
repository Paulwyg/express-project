using System.ComponentModel.Composition;
using System.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.Nr6005Module.ZZhaoCe.ZhaoCeRtuInfoViewModel.Services;

namespace Wlst.Ux.Nr6005Module.ZZhaoCe.ZhaoCeRtuInfoViewModel.Views
{
    /// <summary>
    /// ZhaoCeRtuInfoView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(Nr6005Module.Services.ViewIdAssign.ZhaoCeRtuInfoViewId)]
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
