using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.Wj1050Module.Wj1050DataInqueryModel.Services;

namespace Wlst.Ux.Wj1050Module.Wj1050DataInqueryModel.View
{
    /// <summary>
    /// Wj1050DataInqueryView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(Wj1050Module.Services.ViewIdAssign.Wj1050DataInqueryViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Wj1050DataInqueryView
    {
        public Wj1050DataInqueryView()
        {
            InitializeComponent();
        }

        [Import]
        public IIWj1050DataInquery Model
        {
            get { return DataContext as IIWj1050DataInquery; }
            set { DataContext = value; }
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                //Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(rgdgridview);
                
            
            }
            catch (Exception)
            {
                return;
            }
           
        }
    }
}
