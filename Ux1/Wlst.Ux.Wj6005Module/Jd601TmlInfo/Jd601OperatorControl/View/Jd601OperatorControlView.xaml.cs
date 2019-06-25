using System.ComponentModel.Composition;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.Wj6005Module.Jd601TmlInfo.Jd601OperatorControl.Service;

namespace Wlst.Ux.Wj6005Module.Jd601TmlInfo.Jd601OperatorControl.View
{
    /// <summary>
    /// Jd601OperatorControl.xaml 的交互逻辑
    /// </summary>
    [ViewExport(Services.ViewIdAssign.Jd601OperatorControlViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Jd601OperatorControlView
    {
        public Jd601OperatorControlView()
        {
            InitializeComponent();
        }

        [Import]
        public IIJd601OperatorControl Model
        {
            get { return DataContext as IIJd601OperatorControl; }
            set { DataContext = value; }
        }

        private void button8_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(listView1);
        }
    }
}
