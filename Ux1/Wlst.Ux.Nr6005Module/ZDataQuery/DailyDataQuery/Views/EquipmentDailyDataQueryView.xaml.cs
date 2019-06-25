using System.ComponentModel.Composition;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls.ChartView;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.Nr6005Module.ZDataQuery.DailyDataQuery.Services;

namespace Wlst.Ux.Nr6005Module.ZDataQuery.DailyDataQuery.Views
{
    /// <summary>
    /// EquipmentDailyDataQueryView.xaml 的交互逻辑 EquipmentDataQueryEquipmentDailyDataQueryView
    /// </summary>
    [ViewExport(Nr6005Module.Services.ViewIdAssign.EquipmentDailyDataQueryViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class EquipmentDailyDataQueryView : UserControl
    {
        public EquipmentDailyDataQueryView()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(WindowsLoaded);
            //Thread.CurrentThread.CurrentCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();

            //Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongDatePattern = "yyyy-MM-dd HH:mm:ss";
        }

        [Import]
        public IIEquipmentDailyDataQueryViewModel Model
        {
            get { return DataContext as IIEquipmentDailyDataQueryViewModel; }
            set { DataContext = value; }
        }

        private void BtnExportClick(object sender, System.Windows.RoutedEventArgs e)
        {
            Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(exportgridview);
        }

        private void exportgridview_ColumnReordered(object sender, Telerik.Windows.Controls.GridViewColumnEventArgs e)
        {
            Wlst.Cr.CoreOne.Services.LoadSaveDisplayIndex.SaveDisplayIndex(exportgridview.Columns, XmlConfigName + ".exportgridview");
        }

        public const string XmlConfigName = "DisplayIndex\\Wlst.Ux.Nr6005Module.ZDataQuery.DailyDataQuery";

        private void WindowsLoaded(object sender, RoutedEventArgs e)
        {
            Wlst.Cr.CoreOne.Services.LoadSaveDisplayIndex.LoadDisplayIndex(exportgridview.Columns, XmlConfigName + ".exportgridview");
        }

        private DataPointInfo tmp = null;
        private void ChartTrackBallBehavior_TrackInfoUpdated(object sender, Telerik.Windows.Controls.ChartView.TrackBallInfoEventArgs e)
        {
            foreach (DataPointInfo info in e.Context.DataPointInfos)
            {
                tmp = info;
                info.DisplayHeader = "Custom data point header";
            }
            if (e.Context.DataPointInfos.Count == 0) tmp = null;
            
        }

        //private void tableview_Checked(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    if (tableview.IsChecked == true)
        //    {
        //        viewloop.Height = 200;
        //    }
        //    else
        //    {
        //        viewloop.Height = 0;
        //    }
        //}
    }
}
