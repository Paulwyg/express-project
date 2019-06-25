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
using Wlst.Ux.Wj2090Module.Services;

namespace Wlst.Ux.Wj2090Module.ZOperatorLarge.Partols
{
    /// <summary>
    /// PartolsView.xaml 的交互逻辑
    /// </summary>
    [ViewExport( ViewIdAssign.PartolsViewId)]
    public partial class PartolsView : UserControl
    {
        public PartolsView()
        {
            InitializeComponent();
        }

        [Import]
        public IIPartols Model
        {
            get { return DataContext as IIPartols; }
            set { DataContext = value; }
        }

        private void BtnExport(object sender, RoutedEventArgs e)
        {
            if (xg1.Visibility == Visibility.Visible)
            {
                Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(gridview1);
                return;
            }
            if (xg2.Visibility == Visibility.Visible)
            {
                Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(gridview2);
                return;
            }
            if (xg3.Visibility == Visibility.Visible)
            {
                Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(gridview3);
                return;
            }
            if (xg4.Visibility == Visibility.Visible)
            {
                Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(gridview4);
                return;
            }
        }
    }
}
