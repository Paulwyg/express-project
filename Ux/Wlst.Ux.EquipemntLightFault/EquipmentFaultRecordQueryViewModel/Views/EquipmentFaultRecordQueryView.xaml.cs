using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing.Printing;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultRecordQueryViewModel.Services;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultRecordQueryViewModel.ViewModel;
using Clipboard = System.Windows.Clipboard;
using PrintDialog = System.Windows.Controls.PrintDialog;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultRecordQueryViewModel.Views
{
    /// <summary>
    /// EquipmentFaultRecordQueryView.xaml 的交互逻辑 EquipmentDataQueryEquipmentFaultRecordQueryView
    /// </summary>
    [ViewExport( EquipemntLightFault.Services.ViewIdAssign.EquipmentFaultRecordQueryViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class EquipmentFaultRecordQueryView
    {
        public EquipmentFaultRecordQueryView()
        {
            InitializeComponent();
            this.Loaded +=new RoutedEventHandler(WindowsLoaded);
            //Thread.CurrentThread.CurrentCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();

            //Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongDatePattern = "yyyy-MM-dd HH:mm:ss";
        }

        public const string XmlConfigName = "DisplayIndex\\Wlst.Ux.EquipemntLightFault.EquipmentFaultRecordQueryViewModel.Views.EquipmentFaultRecordQueryView";

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
        //private void LoadDisplayIndex()
        //{
        //    _newDataColumnsDisplayIndex.Clear();

        //    var info = Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read(XmlConfigName);
        //    foreach (var g in info)
        //    {
        //        _newDataColumnsDisplayIndex.Add(g.Key, g.Value);
        //    }
        //}
        

        [Import]
        public IIEquipmentFaultRecordQueryViewModel Model
        {
            get { return DataContext as IIEquipmentFaultRecordQueryViewModel; }
            set { DataContext = value; }
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(this.rgv);


            //PrintPreviewDialog ppd = new PrintPreviewDialog();
            //PrintDocument docToPrint =
            //                 new System.Drawing.Printing.PrintDocument();//创建一个PrintDocument的实例 
            //docToPrint.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(docToPrint_PrintPage);
            

            ////打印预览的打印文档设置为被打印文档
            //ppd.Document = docToPrint;
            //ppd.ShowDialog();

         //   HappyPrint .PrintHelper .PrintControl(rgv );


            //PrintDialog dialog = new PrintDialog();

            //if (dialog.ShowDialog() == true)
            //{
            //    dialog.PrintVisual(rgv, "Print Test");
            //}

        }


        ////设置打印机开始打印的事件处理函数
        //private void docToPrint_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        //{
        //   // e.Graphics.DrawString("Hello, world!", new System.Drawing.Font("Arial", 16, System.Drawing.FontStyle.Regular), System.Drawing.Brushes.Black, 100, 100);
        //    var se = sender as PrintDocument;
        //    if (se == null) return;
        //    se .Print();

            
        //}



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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                HappyPrint.PrintHelper.PrintControl(rgv);
            }
            catch (Exception ex)
            {

            }

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



    }
}
