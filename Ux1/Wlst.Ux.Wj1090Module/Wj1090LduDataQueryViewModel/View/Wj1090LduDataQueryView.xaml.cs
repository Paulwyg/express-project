using System.ComponentModel.Composition;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.Wj1090Module.Wj1090LduDataQueryViewModel.Services;

namespace Wlst.Ux.Wj1090Module.Wj1090LduDataQueryViewModel.View
{
    /// <summary>
    /// Wj1090LduDataQueryView.xaml 的交互逻辑
    /// </summary>
    [ViewExport( Wj1090Module.Services.ViewIdAssign.Wj1090LduDataQueryViewModelId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Wj1090LduDataQueryView
    {
        public Wj1090LduDataQueryView()
        {
            InitializeComponent();
        }

        [Import]
        public IIWj1090LduDataQueryView Model
        {
            get { return DataContext as IIWj1090LduDataQueryView; }
            set { DataContext = value; }
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(gridview);
        }
    }
}
