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
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Ux.Wj2090Module.NewData.PartolView.Services;

namespace Wlst.Ux.Wj2090Module.NewData.PartolView.Views
{
    /// <summary>
    /// PartolView.xaml 的交互逻辑
    /// </summary>
    [ViewExport( Wj2090Module.Services.ViewIdAssign.PartolViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class PartolView : UserControl
    {
        public PartolView()
        {
            InitializeComponent();
        }

        [Import]
        public IIPartolView Model
        {
            get { return DataContext as IIPartolView; }
            set { DataContext = value; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (gridview2.Visibility == Visibility.Visible) Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(gridview2);
            else if (gridview4.Visibility == Visibility.Visible) Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(gridview4);
            else if (gridview6.Visibility == Visibility.Visible) Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(gridview6);
            else if (gridview5.Visibility == Visibility.Visible) Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(gridview5);
            else WlstMessageBox.Show("警告", "导出失败！", WlstMessageBoxType.Ok);

        }
    }
}
