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
using Wlst.Ux.Wj2090Module.HisDataQuery.WeekSetQuery.Services;

namespace Wlst.Ux.Wj2090Module.HisDataQuery.WeekSetQuery.Views
{
    /// <summary>
    /// WeekSetQueryView.xaml 的交互逻辑
    /// </summary>
    [ViewExport( Wj2090Module.Services.ViewIdAssign.WeekSetQueryViewViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class WeekSetQueryView : UserControl
    {
        public WeekSetQueryView()
        {
            InitializeComponent();
        }
        [Import]
        public IIWeekSetQuery Model
        {
            get { return DataContext as IIWeekSetQuery; }
            set { DataContext = value; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(exportgridview);
        }
    }
}
