using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
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
using WindowForWlst;
using Wlst.Cr.Core.Behavior;
using Wlst.Cr.CoreMims.Services;
using Wlst.Ux.TimeTableSystem.TimeInfoMn.Services;
using Wlst.Ux.TimeTableSystem.TimeInfoMn.ViewModel;
using Wlst.Ux.TimeTableSystem.TimeTabletemporaryView.Services;
using Wlst.Ux.TimeTableSystem.TimeTabletemporaryView.ViewModels;

namespace Wlst.Ux.TimeTableSystem.TimeTabletemporaryView.Views
{
    /// <summary>
    /// TimeTabletemporaryView.xaml 的交互逻辑
    /// </summary>
       
    public partial class TimeTabletemporaryView : CustomChromeWindow
    {
        public TimeTabletemporaryView()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Title = "开关灯特设方案";

            this.DataContext = new TimeTabletemporaryViewModel();
        }

       

        //private TimeTableInfomationItem dt;
        //private int areaid;
        //public void SetContext(TimeTableInfomationItem oit, int area, int tableid)
        //{
        //    dt = oit;
        //    DataContext = oit;
        //    areaid = area;
        //}

        //public event EventHandler<EventArgsTemporaryTimeTable> OnFormBtnOkClick;

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var info = Sr.ProtocolPhone.LxRtuTime.wst_timetable_set;
            info.WstRtutimeTimetableSet.Op = 1;
            SndOrderServer.OrderSnd(info, 10, 2);
            this.Close();
        }
    }
        //public class EventArgsTemporaryTimeTable : EventArgs
        //{
        //    public TimeTableInfomationItem TemporaryTimeTableInfo;

        //    public EventArgsTemporaryTimeTable(TimeTableInfomationItem info)
        //    {
        //        TemporaryTimeTableInfo = info;
        //    }
        //}
}
