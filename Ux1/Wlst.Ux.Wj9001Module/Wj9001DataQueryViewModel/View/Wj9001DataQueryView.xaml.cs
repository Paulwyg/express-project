using System.ComponentModel.Composition;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.Wj9001Module.Wj9001DataQueryViewModel.Services;


namespace Wlst.Ux.Wj9001Module.Wj9001DataQueryViewModel.View
{
    /// <summary>
    /// Wj1090LduDataQueryView.xaml 的交互逻辑
    /// </summary>
    [ViewExport( Wj9001Module.Services.ViewIdAssign.Wj9001DataQueryViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Wj9001DataQueryView
    {
        public Wj9001DataQueryView()
        {
            InitializeComponent();
        }

        [Import]
        public IIWj9001LeakDataQueryView Model
        {
            get { return DataContext as IIWj9001LeakDataQueryView; }
            set { DataContext = value; }
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(gridview);
        }
    }
}
