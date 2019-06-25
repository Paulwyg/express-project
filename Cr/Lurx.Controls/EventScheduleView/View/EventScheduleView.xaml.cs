using System.Windows.Controls;
using Lurx.Controls.EventScheduleView.Services;
using Wlst.Cr.Core.Behavior;

namespace Lurx.Controls.EventScheduleView.View
{
    /// <summary>
    /// Wj1090LduEventScheduleView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(
      AttachNow = false,
      AttachRegion = Wj1090Module.Services.ViewIdAssign.Wj1090LduEventScheduleViewAttachRegion,
      ID = Wj1090Module.Services.ViewIdAssign.Wj1090LduEventScheduleViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class EventScheduleView : UserControl
    {
        public EventScheduleView()
        {
            InitializeComponent();
        }


        [Import]
        public IIWEventScheduleView Model
        {
            get { return DataContext as IIWEventScheduleView; }
            set { DataContext = value; }
        }
    }
}
