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
using Wlst.Ux.TimeTableSystem.ExecuteRecordQuery.Services;

namespace Wlst.Ux.TimeTableSystem.ExecuteRecordQuery.View
{
    /// <summary>
    /// ExecuteRecordView.xaml 的交互逻辑
    /// </summary>

    [ViewExport(AttachNow = false, ID = TimeTableSystem.Services.ViewIdAssign.ExecuteRecordViewId,
        AttachRegion = TimeTableSystem.Services.ViewIdAssign.ExecuteRecordViewAttachRegion)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class ExecuteRecordView
    {
        public ExecuteRecordView()
        {
            InitializeComponent();

        }

        [Import]
        public IIExecuteRecordView Model
        {
            get { return DataContext as IIExecuteRecordView; }
            set { DataContext = value; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(executerecord);

            }
            catch (Exception ex)
            {
            }
        }
    }
}
