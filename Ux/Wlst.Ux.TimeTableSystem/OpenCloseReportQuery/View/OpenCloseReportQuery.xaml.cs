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
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Ux.TimeTableSystem.OpenCloseReportQuery.Services;
using Wlst.Ux.TimeTableSystem.OpenCloseReportQuery.ViewModel;

namespace Wlst.Ux.TimeTableSystem.OpenCloseReportQuery.View
{
    /// <summary>
    /// OpenCloseReportQuery.xaml 的交互逻辑
    /// </summary>


    [ViewExport(TimeTableSystem.Services.ViewIdAssign.OpenCloseReportQueryViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class OpenCloseReportQuery
    {
        public OpenCloseReportQuery()
        {
            InitializeComponent();

        }

        [Import]
        public IIOpenCloseReportQuery Model
        {
            get { return DataContext as IIOpenCloseReportQuery; }
            set { DataContext = value; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(lv);
            }
            catch (Exception ex)
            {
                
            }
        }

        private void lv_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var sdr = sender as Telerik.Windows.Controls.RadGridView;
            if (sdr == null) return;
            var item = sdr.SelectedItem as OpenCloseReportRtuItem;
            if (item == null) return;
            var args = new PublishEventArgs
            {
                EventType = PublishEventType.Core,
                EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
            };
            args.AddParams(item.RtuId);
            EventPublish.PublishEvent(args);
        }
    }
}
