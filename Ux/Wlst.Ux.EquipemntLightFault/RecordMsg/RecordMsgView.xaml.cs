using System.ComponentModel.Composition;
using System.Globalization;
using System.Threading;
using System.Windows.Controls;
using Wlst.Cr.Core.Behavior;

namespace Wlst.Ux.EquipemntLightFault.RecordMsg
{
    /// <summary>
    /// RecordMsgView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(EquipemntLightFault.Services.ViewIdAssign.RecordMsgViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class RecordMsgView : UserControl
    {
        public RecordMsgView()
        {
            InitializeComponent();


            //Thread.CurrentThread.CurrentCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();

            //Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongDatePattern = "yyyy-MM-dd HH:mm:ss";
            //Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
               //Thread.CurrentThread.CurrentCulture.DateTimeFormat

        }


        [Import]
        public IIRecordMsg Model
        {

            get { return DataContext as IIRecordMsg; }
            set
            {
                DataContext = value;

            }
        }

        private void BtnExportClick(object sender, System.Windows.RoutedEventArgs e)
        {
            Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(exportgridview);
               
        }
    }
}
