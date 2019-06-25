using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Ux.TimeTableSystem.TimeInfoMn.ViewModel;

namespace Wlst.Ux.TimeTableSystem.TimeInfoMn.Views.BaseView
{
    /// <summary>
    /// FrmGroupSelectTimeTableView.xaml 的交互逻辑
    /// </summary>
    public partial class FrmGroupSelectTimeTableView : CustomChromeWindow
    {
        public FrmGroupSelectTimeTableView()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private int AreaId;
        public void SetDataContext(int rtuOrGroupId, string rtuOGroupName, int kLoops, int oldSelectTimeTableId, string showId, ObservableCollection<TimeTableInfomationItem > items,bool has3005,bool has3006,int areaid)
        {
            var frmSelectTimeTableViewModel = new FrmSelectTimeTableViewModel(items, has3005);
      

            frmSelectTimeTableViewModel.ShowRtuOrGroupId  = showId ;
            frmSelectTimeTableViewModel.SelectRtuOrGroupId = rtuOrGroupId;
            frmSelectTimeTableViewModel.SelectRtuOrGroupName = rtuOGroupName;
            frmSelectTimeTableViewModel.OldSelectTimeTableId = oldSelectTimeTableId;
            frmSelectTimeTableViewModel.SelectKloop = kLoops;
            DataContext = frmSelectTimeTableViewModel;
            AreaId =areaid;
        }


        public event EventHandler<EventArgsFrmSelectTimeTable> OnFormBtnOkClick;

        private void ListViewItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var senderControl = sender as Telerik.Windows.Controls.RadGridView;

            if (senderControl == null) return;
            var senderVm = senderControl.SelectedItem as IdNameDesc;
            if (senderVm == null) return;

            var tmp = this.DataContext as FrmSelectTimeTableViewModel;
            if (tmp == null) return;
            tmp.CurrentSelectItem = senderVm;


            if (OnFormBtnOkClick != null)
            {
                OnFormBtnOkClick(this, new EventArgsFrmSelectTimeTable(tmp));
            }
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var tmp = this.DataContext as FrmSelectTimeTableViewModel;
            if (tmp == null) return;


            if (OnFormBtnOkClick != null)
            {
                if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Contains(AreaId) || Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D)
                {
                    OnFormBtnOkClick(this, new EventArgsFrmSelectTimeTable(tmp));
                    this.Close();
                }
                else
                {
                    var infoss = WlstMessageBox.Show("警告", "您没有权限修改该分组或终端时间表！是否退出？", WlstMessageBoxType.YesNo);
                    if (infoss != WlstMessageBoxResults.Yes)
                    {
                        OnFormBtnOkClick(this, new EventArgsFrmSelectTimeTable(null));
                        this.Close();
                    }
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (OnFormBtnOkClick != null)
            {
                OnFormBtnOkClick(this, new EventArgsFrmSelectTimeTable(null));
            }
            this.Close();
        }
    }

    public class EventArgsFrmSelectTimeTable:EventArgs
    {
        public  FrmSelectTimeTableViewModel Info;
        public EventArgsFrmSelectTimeTable(FrmSelectTimeTableViewModel tmp)
        {
            Info = tmp;
        }
    }
}
