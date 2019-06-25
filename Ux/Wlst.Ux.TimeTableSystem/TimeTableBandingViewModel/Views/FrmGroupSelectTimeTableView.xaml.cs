using System;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using Wlst.Cr.WjEquipmentBaseModels.Interface;

using Wlst.Ux.TimeTableSystem.TimeTableBandingViewModel.ViewModel;

namespace Wlst.Ux.TimeTableSystem.TimeTableBandingViewModel.Views
{
    /// <summary>
    /// FrmGroupSelectTimeTableView.xaml 的交互逻辑
    /// </summary>
    public partial class FrmGroupSelectTimeTableView 
    {
        public FrmGroupSelectTimeTableView()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        //public void SetDataContextRtuParaSwitchInViewModel(RtuParaAnalogueAmpViewModel rtuParaAnalogueAmpViewModel )
        //{
        //    DataContext = rtuParaAnalogueAmpViewModel;
        //}

        public void SetDataContext(bool isGroup,int rtuOrGroupId,string rtuOGroupName,int kLoops,int oldSelectTimeTableId,
            int phyId)
        {
            ViewModel.FrmSelectTimeTableViewModel frmSelectTimeTableViewModel = new FrmSelectTimeTableViewModel();
            frmSelectTimeTableViewModel.IsAllChildApplyThisTimeTableVisi = Visibility.Collapsed;
            frmSelectTimeTableViewModel.IsGroup = isGroup;
            frmSelectTimeTableViewModel.SelectRtuOrGroupPhyId = phyId;
            frmSelectTimeTableViewModel.SelectRtuOrGroupId = rtuOrGroupId;
            frmSelectTimeTableViewModel.SelectRtuOrGroupName = rtuOGroupName;
            frmSelectTimeTableViewModel.OldSelectTimeTableId = oldSelectTimeTableId;
            frmSelectTimeTableViewModel.SelectKloop = kLoops;
            frmSelectTimeTableViewModel.ApplyRtusType = 1;
            
            frmSelectTimeTableViewModel.OnBtnOkClick += new EventHandler(frmSelectTimeTableViewModel_OnBtnOkClick);
            //if (isGroup)
            //{
            //    if (!Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoDictionary.ContainsKey(rtuOrGroupId))
            //    {
            //        frmSelectTimeTableViewModel.OldSelectTimeTableName = "无法查阅分组名称";
            //    }

            //    else
            //        frmSelectTimeTableViewModel.OldSelectTimeTableName =
            //            Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoDictionary[rtuOrGroupId].GroupName;
            //}
            //else
            //{
            //    if (
            //        !Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary .
            //             ContainsKey(rtuOrGroupId))
            //    {
            //        frmSelectTimeTableViewModel.OldSelectTimeTableName = "无法查阅终端名称";
            //    }
            //    else
            //    {
            //        var f =
            //            Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary [
            //                rtuOrGroupId];
            //         frmSelectTimeTableViewModel.OldSelectTimeTableName = f.RtuName;

            //    }


            //}
            DataContext = frmSelectTimeTableViewModel;
        }

        void frmSelectTimeTableViewModel_OnBtnOkClick(object sender, EventArgs e)
        {
            if (OnFormBtnOkClick != null)
            {
                OnFormBtnOkClick(this, new EventArgs());
            }
            this.Close();
        }
        public event EventHandler OnFormBtnOkClick;

        private void ListViewItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //Telerik.Windows.Controls.RadGridView gr=new RadGridView() ;
            //gr.CurrentItem;

            var senderControl = sender as Telerik.Windows.Controls.RadGridView ;
            
            if (senderControl == null) return;
            //var gggg = senderControl.SelectedItem as Telerik .Windows .Controls .GridView .GridViewRowItem ;
            //if (gggg == null) return;
            var senderVm = senderControl.SelectedItem as TimeTableViewModel;
            if (senderVm == null) return;

            var tmp = this.DataContext as FrmSelectTimeTableViewModel;
            if (tmp == null) return;
            tmp.CurrentSelectItem = senderVm;


            if (OnFormBtnOkClick != null)
            {
                OnFormBtnOkClick(this, new EventArgs());
            }
            this.Close();
        }


    }
}
