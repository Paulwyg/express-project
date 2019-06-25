using System.ComponentModel.Composition;
using System.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Cr.Core.CoreServices;
using Wlst.Ux.CoreDataEventMonitor.EventMonitorViewModel.Services;
using Wlst.Ux.CoreDataEventMonitor.Services;

namespace Wlst.Ux.CoreDataEventMonitor.EventMonitorViewModel.Views
{
    /// <summary>
    /// EventMonitorView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(
        AttachNow = true ,
        AttachRegion = DocumentRegionName.DocumentRegion,
        ID = ViewIdAssign.EventMonitorViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class EventMonitorView : UserControl
    {
        public EventMonitorView()
        {
            InitializeComponent();
        }

        [Import]
        public IIEventMonitorViewModel Model
        {
            get { return DataContext as IIEventMonitorViewModel; }
            set { DataContext = value; }
        }
    }
}
