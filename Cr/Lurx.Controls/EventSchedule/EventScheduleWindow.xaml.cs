using System;
using System.Windows;
using Wlst.Sr.ProtocolPhone;

namespace Lurx.Controls.EventSchedule
{
    /// <summary>
    /// EventScheduleWindow.xaml 的交互逻辑
    /// </summary>
    public partial class EventScheduleWindow 
    {
        public EventScheduleWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        public void SetDataContextEventScheduleViewModel(Wlst .client . EventSchedule eventSchedule)
        {
            if (eventSchedule == null)
                DataContext = new EventScheduleViewModel();
            else DataContext = new EventScheduleViewModel(eventSchedule);
        }

        public Wlst.client.EventSchedule EventSchedule
        {
            get { var xgr= DataContext as EventScheduleViewModel;
                return xgr.GetEventSchedule();
            }
        }
        public event EventHandler OnSaveEventSchedule;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (OnSaveEventSchedule != null)
            {
                OnSaveEventSchedule(this, new EventArgs());
            }
        }

        public event EventHandler OnFormClosed;
        protected override void OnClosed(System.EventArgs e)
        {
            base.OnClosed(e);
            if (OnFormClosed != null)
            {
                OnFormClosed(this, new EventArgs());
            }
        }
    }
}
