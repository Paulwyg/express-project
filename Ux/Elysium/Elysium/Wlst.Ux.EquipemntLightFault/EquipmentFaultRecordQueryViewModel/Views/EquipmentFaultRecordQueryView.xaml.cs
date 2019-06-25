using System;
using System.ComponentModel.Composition;
using System.Drawing.Printing;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultRecordQueryViewModel.Services;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultRecordQueryViewModel.ViewModel;
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

            //Thread.CurrentThread.CurrentCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();

            //Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongDatePattern = "yyyy-MM-dd HH:mm:ss";
        }

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


    }
}
