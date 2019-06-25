using System;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Cr.Core.CoreServices;
using Wlst.Ux.MenuManage.MenuClassicViewModel.Services;
using Wlst.Ux.MenuManage.Services;

namespace Wlst.Ux.MenuManage.MenuClassicViewModel.Views
{
    /// <summary>
    /// MenuClassicView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(ViewIdAssign.MenuClassicViewId, AttachNow = true)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class MenuClassicView : UserControl
    {
        public MenuClassicView()
        {
            InitializeComponent();
        }
         
        [Import]
        public IIMenuClassicViewModel Model
        {
            get
            {
                return DataContext as IIMenuClassicViewModel;
            }
            set
            {
                DataContext = value;
            }
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(grd);
            }
            catch (Exception exr)
            {
                
            }
        }
    }
}
