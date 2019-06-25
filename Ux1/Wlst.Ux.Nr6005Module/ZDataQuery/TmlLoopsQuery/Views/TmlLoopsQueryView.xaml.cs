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
using Wlst.Ux.Nr6005Module.ZDataQuery.TmlLoopsQuery.Services;

namespace Wlst.Ux.Nr6005Module.ZDataQuery.TmlLoopsQuery.Views
{
    /// <summary>
    /// TmlLoopsQueryView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(Nr6005Module.Services.ViewIdAssign.NavToTmlLoopsQueryViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class TmlLoopsQueryView : UserControl
    {
        public TmlLoopsQueryView()
        {
            InitializeComponent();
        }

        [Import]
        private IITmlLoopsQueryViewModel Model
        {
            get { return DataContext as IITmlLoopsQueryViewModel; }
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
