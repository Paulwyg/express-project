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
using Wlst.Cr.Core.Behavior;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Ux.TimeTableSystem.TimeInfoMn.Services;
using Wlst.Ux.TimeTableSystem.TimeInfoMn.ViewModel;
using Wlst.Ux.TimeTableSystem.TimeInfoMn.Views.BaseView;

namespace Wlst.Ux.TimeTableSystem.TimeInfoMn.Views
{
    /// <summary>
    /// TimeInfoMnView.xaml 的交互逻辑
    /// </summary>
    //[ViewExport(TimeTableSystem.Services.ViewIdAssign.TimeInfoMnViewId)]
    //[PartCreationPolicy(CreationPolicy.Shared)]
    public partial class TimeInfoMnView : UserControl
    {
        public TimeInfoMnView()
        {
            InitializeComponent();

        }

        [Import]
        public IITimeInfoMn Model
        {
            get { return DataContext as IITimeInfoMn; }
            set
            {
                DataContext = value;
                value.OnUserWantSetGroupWeekSet += new EventHandler<EventArgsEx>(value_OnUserWantSetGroupWeekSet);
            }
        }

        void value_OnUserWantSetGroupWeekSet(object sender, EventArgsEx e)
        {
            func(e.Info);
        }
        

        private void treeListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var listView = sender as Telerik.Windows.Controls.RadGridView;
            if (listView == null) return;
            var ggg = listView.CurrentCellInfo;
            var mvvm = ggg.Item as OneGrpOrRtuLoopsSet;
            if (mvvm == null) return;
            var cellIndex = ggg.Column.DisplayIndex;
            var nodeId = mvvm.RtuOrGrpId;
            if (nodeId == 0) return; // special terminal
            if (mvvm.Items.Count + 1 < cellIndex) return;

            int typeloop = 6;
            if (mvvm.Has3006 || (mvvm.Has3005 == false && mvvm.Has3006 == false)) typeloop = 8;


            if (cellIndex < 2) //2 is K1;
            {
                func(mvvm);
            }
            else if (cellIndex < typeloop + 2)
            {
                int timetable = mvvm.Items[cellIndex - 2].TimeTalbe;

                FrmGroupSelectTimeTableView frmGroupSelect = new FrmGroupSelectTimeTableView();
                frmGroupSelect.Background = this.Background;
                frmGroupSelect.OnFormBtnOkClick +=
                    new EventHandler<EventArgsFrmSelectTimeTable>(frmGroupSelect_OnFormBtnOkClick);
                frmGroupSelect.SetDataContext(mvvm.RtuOrGrpId, mvvm.RtuOrGrpName, cellIndex - 1, timetable, mvvm.PhyId,
                                              Model.Items, mvvm.Has3005, mvvm.Has3006, mvvm.AreaId);
                frmGroupSelect.ShowDialog();
            }
            else
            {
                WlstMessageBox.Show("警告", "该终端或分组不支持此回路设置！", WlstMessageBoxType.Ok);
            }



        }

        void func(OneGrpOrRtuLoopsSet mvvm)
        {
            int timetable = -1;
            List<int> lsttimetable = new List<int>();
            for (int i = 0; i < 8; i++)
            {
                lsttimetable.Add(mvvm.Items[i].TimeTalbe);
            }

            GroupWatchTimeTableView _GroupWatchTimeTableView = new GroupWatchTimeTableView();
            _GroupWatchTimeTableView.Background = this.Background;
            _GroupWatchTimeTableView.Topmost = true;
            _GroupWatchTimeTableView.OnFormBtnOkClick +=
                new EventHandler<EventArgsFrmSelectTimeTable>(GroupWatchTimeTableView_OnFormBtnOkClick);
            _GroupWatchTimeTableView.SetDataContext(mvvm.RtuOrGrpId, mvvm.RtuOrGrpName, mvvm.PhyId, lsttimetable,
                                                    Model.Items, mvvm.Has3005, mvvm.Has3006, mvvm.AreaId);
            _GroupWatchTimeTableView.ShowDialog();
        }

        void frmGroupSelect_OnFormBtnOkClick(object sender, EventArgsFrmSelectTimeTable args)
        {
            var tmp = args.Info;
            if (tmp == null) return;

            if (this.Model != null && tmp.CurrentSelectItem != null)
            {
                Model.UpdateNodeTimeTable(tmp.SelectRtuOrGroupId, tmp.SelectKloop, tmp.CurrentSelectItem.Id,
                                          tmp.CurrentSelectItem.Name, tmp.CurrentSelectItem.NameDesc);
            }

        }

        void GroupWatchTimeTableView_OnFormBtnOkClick(object sender, EventArgsFrmSelectTimeTable args)
        {
            var tmp = args.Info;
            if (tmp == null) return;

            for (int i = 0; i < tmp.TimeTableComboBoxSelected.Count;i++ )
            {
                if (this.Model != null && tmp.TimeTableComboBoxSelected[i] != null)
                {
                    Model.UpdateNodeTimeTable(tmp.SelectRtuOrGroupId, i+1, tmp.TimeTableComboBoxSelected[i].Id,
                                                  tmp.TimeTableComboBoxSelected[i].Name, tmp.TimeTableComboBoxSelected[i].NameDesc);
                }
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(this.treeview);
            //}
            //catch (Exception ex)
            //{

            //}



        }

    }
}
