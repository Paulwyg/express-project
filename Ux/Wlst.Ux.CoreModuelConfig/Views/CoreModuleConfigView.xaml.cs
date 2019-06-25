using System.ComponentModel.Composition;
using System.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Cr.Core.CoreServices;
using Wlst.Ux.CoreModuelConfig.Services;

namespace Wlst.Ux.CoreModuelConfig.Views
{
    /// <summary>
    /// CoreModuleConfigView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(
        AttachNow = true,
        AttachRegion = DocumentRegionName.DocumentRegion,
        ID = Services.ViewIdAssign.CoreModuleConfigViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class CoreModuleConfigView : UserControl
    {
        public CoreModuleConfigView()
        {
            InitializeComponent();
        }

        [Import]
        public IICoreMoudleConfig Model
        {
            get { return DataContext as IICoreMoudleConfig; }
            set { DataContext = value; }
        }
    }
}
