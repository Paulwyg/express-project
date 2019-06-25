using System;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Windows.Input;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.TimeTableSystem.TimeTableBandingViewModel.Services;

namespace Wlst.Ux.TimeTableSystem.TimeTableBandingViewModel.Views
{
    /// <summary>
    /// TimeTableBandingView.xaml 的交互逻辑 TimeTableSystemModuleTimeTableBandingView
    /// </summary>
    [ViewExport(
        AttachNow = false,
        AttachRegion =TimeTableSystem .Services .ViewIdAssign .TimeTableBandingViewAttachRegion, 
        ID = TimeTableSystem .Services .ViewIdAssign .TimeTableBandingViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class TimeTableBandingView1 : UserControl
    {
        public TimeTableBandingView1()
        {
            InitializeComponent();
        }


        [Import]
        public IITimeTableBandingViewModel Model
        {
            get { return DataContext as IITimeTableBandingViewModel; }
            set { DataContext = value; }
        }

        private void treeListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var listView = sender as Telerik.Windows.Controls.RadTreeListView;
            if (listView == null) return;
            var ggg = listView.CurrentCellInfo;
            var mvvm = ggg.Item as ListTreeNodeBase;
            if (mvvm == null) return;
            var cellIndex = ggg.Column.DisplayIndex;
            var nodeId = mvvm.NodeId;
            if (nodeId == 0) return;// special terminal
            if (cellIndex < 3) return; //3 is K1;
            if (!mvvm.IsListTreeNodeGroup && !mvvm.IsThisTmlSpecialTerminal) return;
            int timetable = 0;
            if (cellIndex == 3) timetable = mvvm.K1TimeTalbe;
            if (cellIndex == 4) timetable = mvvm.K2TimeTalbe;
            if (cellIndex == 5) timetable = mvvm.K3TimeTalbe;
            if (cellIndex == 6) timetable = mvvm.K4TimeTalbe;
            if (cellIndex == 7) timetable = mvvm.K5TimeTalbe;
            if (cellIndex == 8) timetable = mvvm.K6TimeTalbe;


            FrmGroupSelectTimeTableView frmGroupSelect = new FrmGroupSelectTimeTableView();
            frmGroupSelect.OnFormBtnOkClick += new EventHandler(frmGroupSelect_OnFormBtnOkClick);
            frmGroupSelect.SetDataContext(mvvm.IsListTreeNodeGroup, mvvm.NodeId,mvvm .NodeName , cellIndex - 2, timetable,mvvm .PhyId );
            frmGroupSelect.Show();
        }

        void frmGroupSelect_OnFormBtnOkClick(object sender, EventArgs args)
        {
            var f = sender as FrmGroupSelectTimeTableView;
            if (f == null) return;
            var g = f.DataContext as ViewModel.FrmSelectTimeTableViewModel;
            if (g == null) return;
            int rtuIdOrGourpId = g.SelectRtuOrGroupId;
            bool isGroup = g.IsGroup;

            if (g.ApplyRtusType == 0) g.ApplyRtusType = 1;
            int oldTable = g.OldSelectTimeTableId;
            int newTable = g.CurrentSelectItem.TimeTableId;
            int kLoop = g.SelectKloop;
            if (this.Model != null)
            {
                Model.UpdatRtuTimeTable(isGroup, rtuIdOrGourpId, newTable, kLoop, g.ApplyRtusType );
            }

        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(this.treeview);
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
