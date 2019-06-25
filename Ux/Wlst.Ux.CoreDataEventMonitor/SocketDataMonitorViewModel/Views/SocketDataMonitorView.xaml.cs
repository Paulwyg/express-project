using System.ComponentModel.Composition;
using System.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Cr.Core.CoreServices;
using Wlst.Ux.CoreDataEventMonitor.Services;
using Wlst.Ux.CoreDataEventMonitor.SocketDataMonitorViewModel.Services;

namespace Wlst.Ux.CoreDataEventMonitor.SocketDataMonitorViewModel.Views
{
    /// <summary>
    /// SocketDataMonitorView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(
    AttachNow = true ,
    AttachRegion = DocumentRegionName.DocumentRegion,
    ID = ViewIdAssign.SocketDataMonitorId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class SocketDataMonitorView : UserControl
    {
        public SocketDataMonitorView()
        {
            InitializeComponent();
        }

        [Import]
        public IISocketDataMonitorViewModel Model
        {
            get { return DataContext as IISocketDataMonitorViewModel; }
            set { DataContext = value; }
        }
    }
}
