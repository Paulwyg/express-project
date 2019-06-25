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
using Wlst.Ux.EmergencyDispatch.ControlCenterManag.Services;

namespace Wlst.Ux.EmergencyDispatch.ControlCenterManag.Views
{
    /// <summary>
    /// ControlCenterView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(AttachNow = false, ID = EmergencyDispatch.Services.ViewIdAssign.NavToControlCenterManagId,
AttachRegion = EmergencyDispatch.Services.ViewIdAssign.NatToControlCenterManagAttachRegion)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class ControlCenterView : UserControl
    {
        public ControlCenterView()
        {
            InitializeComponent();
        }
       [Import]
        private IIControlCenterManag Model
        {
            get { return DataContext as IIControlCenterManag; }
            set { DataContext = value; }
        }
    }
}
