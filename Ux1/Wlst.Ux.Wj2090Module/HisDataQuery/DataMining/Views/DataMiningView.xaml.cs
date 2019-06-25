using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.Wj2090Module.HisDataQuery.DataMining.Services;

namespace Wlst.Ux.Wj2090Module.HisDataQuery.DataMining.Views
{
    /// <summary>
    /// DataMiningView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(Wj2090Module.Services.ViewIdAssign.DataMiningViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class DataMiningView : UserControl
    {
        public DataMiningView()
        {
            InitializeComponent();
        }

        [Import]
        public IIDataMining Model
        {
            get { return DataContext as IIDataMining; }
            set { DataContext = value; }
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (gridview1.Visibility == Visibility.Visible)
            {
                Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(gridview1);
            }
            else if (gridview2.Visibility == Visibility.Visible)
            {
                Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(gridview2);
            }
        }
    }
}
