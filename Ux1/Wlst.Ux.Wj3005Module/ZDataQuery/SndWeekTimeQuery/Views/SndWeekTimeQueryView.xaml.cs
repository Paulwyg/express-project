using System.ComponentModel.Composition;
using System.Globalization;
using System.Threading;
using System.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.WJ3005Module.ZDataQuery.SndWeekTimeQuery.Services;

namespace Wlst.Ux.WJ3005Module.ZDataQuery.SndWeekTimeQuery.Views
{
    /// <summary>
    /// SndWeekTimeQueryView.xaml 的交互逻辑 EquipmentDataQuerySndWeekTimeQueryView
    /// </summary>

    [ViewExport(WJ3005Module.Services.ViewIdAssign.SndWeekTimeQueryViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class SndWeekTimeQueryView : UserControl 
    {
        public SndWeekTimeQueryView()
        {
            InitializeComponent();

            //Thread.CurrentThread.CurrentCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();

            //Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongDatePattern = "yyyy-MM-dd HH:mm:ss";
        }

        [Import]
        public IISndWeekTimeQueryViewModel Model
        {
            get { return DataContext as IISndWeekTimeQueryViewModel; }
            set { DataContext = value; }
        }

        private void BtnExportClick(object sender, System.Windows.RoutedEventArgs e)
        {
            Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(exportgridview);
        }
    }
}
