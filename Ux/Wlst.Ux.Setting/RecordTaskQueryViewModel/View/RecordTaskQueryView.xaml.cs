using System.ComponentModel.Composition;
using System.Globalization;
using System.Threading;
using System.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.Setting.RecordTaskQueryViewModel.Services;

namespace Wlst.Ux.Setting.RecordTaskQueryViewModel.View
{
    /// <summary>
    /// RecordTaskQueryView.xaml 的交互逻辑
    /// </summary>

    [ViewExport(Setting.Services.ViewIdAssign.RecordTaskQueryViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class RecordTaskQueryView : UserControl
    {
        public RecordTaskQueryView()
        {
            InitializeComponent();

            //Thread.CurrentThread.CurrentCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();

            //Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongDatePattern = "yyyy-MM-dd HH:mm:ss";
        }

        [Import]
        public IIRecordTaskQueryViewModel Model
        {
            get { return DataContext as IIRecordTaskQueryViewModel; }
            set { DataContext = value; }
        }

        private void BtnExportClick(object sender, System.Windows.RoutedEventArgs e)
        {
            Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(RadGridView1);
        }
    }
}
