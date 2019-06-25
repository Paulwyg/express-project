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
using Wlst.Ux.Wj1090Module.Partol.Services;

namespace Wlst.Ux.Wj1090Module.Partol.View
{
    /// <summary>
    /// PartolView.xaml 的交互逻辑
    /// </summary>
    [ViewExport( Wj1090Module.Services.ViewIdAssign.PartolViewId)]
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
            try
            {
                Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(rgvdata);
            }
            catch (Exception ex)
            {
                
            }
        }
    }

}
