using System;
using System.ComponentModel.Composition;
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
using Wlst.Ux.StateBarModule.SendFailOperation.Service;

namespace Wlst.Ux.StateBarModule.SendFailOperation.View
{
    /// <summary>
    /// SendFailOperationView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(Services.ViewIdAssign.SendFailOperationViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class SendFailOperationView : UserControl
    {
        public SendFailOperationView()
        {
            InitializeComponent();
        }

        [Import]
        private IISendFailOperationViewModel Model
        {
            get { return DataContext as IISendFailOperationViewModel; }
            set
            {
                DataContext = value;
            }
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(gridview1);
        //}


    }
}
