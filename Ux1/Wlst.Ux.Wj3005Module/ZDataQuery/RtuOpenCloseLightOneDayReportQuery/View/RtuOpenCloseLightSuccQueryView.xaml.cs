using System.ComponentModel.Composition;
using System.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.WJ3005Module.ZDataQuery.RtuOpenCloseLightOneDayReportQuery.Service;

namespace Wlst.Ux.WJ3005Module.ZDataQuery.RtuOpenCloseLightOneDayReportQuery.View
{
    /// <summary>
    /// RtuOpenCloseLightSuccQueryView.xaml 的交互逻辑
    /// </summary>
    [ViewExport( WJ3005Module.Services.ViewIdAssign.RtuOpenCloseLightSuccQueryViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class RtuOpenCloseLightSuccQueryView : UserControl
    {
        public RtuOpenCloseLightSuccQueryView()
        {
            InitializeComponent();
        }

        [Import]
        public IIRtuOpenCloseLightOneDayReportQuery Model
        {
            get { return DataContext as IIRtuOpenCloseLightOneDayReportQuery; }
            set { DataContext = value; }
        }
    }
}
