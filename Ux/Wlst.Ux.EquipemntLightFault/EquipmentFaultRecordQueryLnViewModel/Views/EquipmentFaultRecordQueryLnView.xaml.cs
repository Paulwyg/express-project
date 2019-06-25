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
using Wlst.Ux.EquipemntLightFault.EquipmentFaultRecordQueryLnViewModel.Services;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultRecordQueryLnViewModel.ViewModel;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultRecordQueryLnViewModel.Views
{
    /// <summary>
    /// EquipmentFaultRecordQueryLnView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(EquipemntLightFault.Services.ViewIdAssign.EquipmentFaultRecordQueryLnViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class EquipmentFaultRecordQueryLnView : UserControl
    {
        public EquipmentFaultRecordQueryLnView()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(WindowsLoaded);
        }

        public const string XmlConfigName = "DisplayIndex\\Wlst.Ux.EquipemntLightFault.EquipmentFaultRecordQueryLnViewModel.Views.EquipmentFaultRecordQueryLnView";

        private void WindowsLoaded(object sender, RoutedEventArgs e)
        {
            Wlst.Cr.CoreOne.Services.LoadSaveDisplayIndex.LoadDisplayIndex(rgv.Columns, XmlConfigName);
            //LoadDisplayIndex();
            //if (_newDataColumnsDisplayIndex == null || _newDataColumnsDisplayIndex.Count == 0) return;
            //foreach (var g in rgv.Columns)
            //{
            //    foreach (var j in _newDataColumnsDisplayIndex)
            //    {
            //        if (g.Header.ToString() == j.Key)
            //        {
            //            g.DisplayIndex = int.Parse(j.Value);
            //            break;
            //        }
            //    }
            //}
        }





        [Import]
        public IIEquipmentFaultRecordQueryLnViewModel Model
        {
            get { return DataContext as IIEquipmentFaultRecordQueryLnViewModel; }
            set { DataContext = value; }
        }


        private void Label_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (Model != null)
                Model.CounterLableDoubleClick += 1;
        }


        private void rgv_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var sdr = sender as Telerik.Windows.Controls.RadGridView;
            if (sdr == null) return;
            var item = sdr.SelectedItem as EquipmentFaultViewModel;
            if (item == null) return;
            // var tmp = item.DataContext as EquipmentFaultViewModel;
            if (Model != null) Model.OnRequestServerData(item);


            // Telerik .Windows .Con
        }


        private void RadGridView_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //选项中没有勾选“双击复制”return   lvf 2018年10月8日09:18:28
            if (Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(3102, 4, false) == false) return;
            try
            {
                var listView = sender as Telerik.Windows.Controls.RadGridView;
                if (listView == null) return;
                var ggg = listView.CurrentCellInfo;
                if (ggg == null) return;
                var mvvm = ggg.Item as EquipmentFaultViewModel;
                if (mvvm == null) return;
                //var cellIndex = ggg.Column.DisplayIndex;
                //if (cellIndex < 0) return;

                //int index = cellIndex + 1;
                //var strdata = string.Empty;
                //if (index == 1) strdata = mvvm.Index + "";
                //if (index == 2) strdata = mvvm.PhyId + "";
                //if (index == 3) strdata = mvvm.RtuName + "";
                //if (index == 4) strdata = mvvm.CQJ + "";
                //if (index == 5) strdata = mvvm.DYGH + "";
                //if (index == 6) strdata = mvvm.RtuLoopName + "";
                //if (index == 7) strdata = mvvm.FaultName + "";
                //if (index == 8) strdata = mvvm.DtCreateTime + "";
                //if (index == 9) strdata = mvvm.DtRemoceTime + "";
                //if (index == 10) strdata = mvvm.Remark + "";

                var sps = mvvm.Index + "\t";
                sps += mvvm.PhyId + "\t";
                sps += mvvm.RtuName + "\t";

                if (fxg.IsVisible) sps += mvvm.CQJ + "\t";
                if (fxg.IsVisible) sps += mvvm.DYGH + "\t";
                sps += mvvm.RtuLoopName + "\t";
                sps += mvvm.FaultName + "\t";

                sps += mvvm.DtCreateTime + "\t";
                if (rbold.IsChecked == true) sps += mvvm.DtRemoceTime;
                sps += mvvm.Remark;

                Clipboard.SetDataObject(sps);
            }
            catch (Exception ex)
            {

            }

        }

        //private Dictionary<string, string> _newDataColumnsDisplayIndex = new Dictionary<string, string>();
        private void rgv_ColumnReordered(object sender, Telerik.Windows.Controls.GridViewColumnEventArgs e)
        {
            //_newDataColumnsDisplayIndex.Clear();
            //foreach (var g in rgv.Columns)
            //{
            //    _newDataColumnsDisplayIndex.Add(g.Header.ToString(), g.DisplayIndex.ToString());
            //}


            //Wlst.Cr.CoreOne.Services.SystemXmlConfig.Save(_newDataColumnsDisplayIndex, XmlConfigName);
            Wlst.Cr.CoreOne.Services.LoadSaveDisplayIndex.SaveDisplayIndex(rgv.Columns, XmlConfigName);
        }

        private void DatePicker_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Wlst.Cr.CoreOne.Services.OtherSvr.ChangeDatePickerToToday(sender, e);
        }



    }


}
