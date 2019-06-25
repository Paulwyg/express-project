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
using Wlst.Ux.WJ3005Module.ZDataQuery.ElectricityQuery.Services;


namespace Wlst.Ux.WJ3005Module.ZDataQuery.ElectricityQuery.Views
{
    /// <summary>
    /// TmlLoopsQueryView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(WJ3005Module.Services.ViewIdAssign.NavToElectricityQueryViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class ElectricityQueryView : UserControl
    {
        public ElectricityQueryView()
        {
            InitializeComponent();
             
        }

        [Import]
        private IIElectricityQueryViewModel Model
        {
            get { return DataContext as IIElectricityQueryViewModel; }
            set
            {
                DataContext = value;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(rgv);
        }


    }
}
