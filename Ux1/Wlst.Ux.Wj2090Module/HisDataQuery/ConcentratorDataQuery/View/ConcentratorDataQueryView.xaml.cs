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
using Telerik.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.Wj2090Module.Services;
using Wlst.Ux.Wj2090Module.HisDataQuery.ConcentratorDataQuery.Service;

namespace Wlst.Ux.Wj2090Module.HisDataQuery.ConcentratorDataQuery.View
{
    /// <summary>
    /// ConcentratorDataQueryView.xaml 的交互逻辑
    /// </summary>
    [ViewExport( ViewIdAssign.ControlDataQueryViewId)]
    public partial class ConcentratorDataQueryView : UserControl
    {
        public ConcentratorDataQueryView()
        {
            InitializeComponent();

           // RadGridView rg;
            //rg.IsBusy = false;

          //  gridview3.
        }
        [Import]
        public IIConcentratorDataQuery Model
        {
            get { return DataContext as IIConcentratorDataQuery; }
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
        }

        private void CheckBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Model != null) Model.IsShowAllLampData = true;
        }
    }
}
