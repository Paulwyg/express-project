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

namespace Wlst.Ux.ExtendYixinEsu.JnDataQuery
{
    /// <summary>
    /// JnQueryView.xaml 的交互逻辑
    /// </summary>
    [ViewExport( ExtendYixinEsu.Services.ViewIdAssign.JnQueryViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class JnQueryView : UserControl
    {
        public JnQueryView()
        {
            InitializeComponent();
        }

        [Import]
        public IIJnQuery Model
        {
            get { return DataContext as IIJnQuery; }
            set { DataContext = value; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(rgv);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            JnRtuSet.NavTo.NavToLdl();
        }

    }
}
