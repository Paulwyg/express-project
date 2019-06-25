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
using Wlst.Ux.WJ3005Module.EmergencyOperationQuery.Services;

namespace Wlst.Ux.WJ3005Module.EmergencyOperationQuery.Views
{
    /// <summary>
    /// EmergencyOperationQueryView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(WJ3005Module.Services.ViewIdAssign.NavToEmergencyOperationQueryViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class EmergencyOperationQueryView : UserControl
    {
        public EmergencyOperationQueryView()
        {
            InitializeComponent();
        }

        [Import]
        private IIEmergencyOperationQuery Model
        {
            get { return DataContext as IIEmergencyOperationQuery; }
            set { DataContext = value; }
        }

        private void BtnExportClick(object sender, RoutedEventArgs e)
        {
            Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(gridview);
        }
    }
}
