using System;
using System.Collections.Generic;
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
using System.ComponentModel.Composition;
using Wlst.Ux.Wj2090Module.ZcInfo.ZcConnParas.Services;

namespace Wlst.Ux.Wj2090Module.ZcInfo.ZcConnParas.Views
{
    /// <summary>
    /// ZcConnParasView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(Wj2090Module.Services.ViewIdAssign.ZcConnParasViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class ZcConnParasView : UserControl
    {
        public ZcConnParasView()
        {
            InitializeComponent();
        }

        [Import]
        public IIZcConnParas Model
        {
            get { return DataContext as IIZcConnParas; }
            set { DataContext = value; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(gridview);
            }
            catch (Exception ex){}
        }
    }
}
