using System.ComponentModel.Composition;
using System.Globalization;
using System.Threading;
using System.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.EmergencyDispatch.ControlCenterManagDemo2.Services;

namespace Wlst.Ux.EmergencyDispatch.ControlCenterManagDemo2.Views
{
    /// <summary>
    /// ControlCenterView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(EmergencyDispatch.Services.ViewIdAssign.NavToControlCenterManagDemo2Id)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class ControlCenterView : UserControl
    {
        public ControlCenterView()
        {
            InitializeComponent();



            Thread.CurrentThread.CurrentCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();

            Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongDatePattern = "yyyy-MM-dd HH:mm:ss";
            Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
        }
       [Import]
        private IIControlCenterManagDemo2 Model
        {
            get { return DataContext as IIControlCenterManagDemo2; }
            set { DataContext = value; }
        }
    }
}
