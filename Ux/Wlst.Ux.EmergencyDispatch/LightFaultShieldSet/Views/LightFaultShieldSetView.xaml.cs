using System.ComponentModel.Composition;
using System.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.EmergencyDispatch.LightFaultShieldSet.Services;

namespace Wlst.Ux.EmergencyDispatch.LightFaultShieldSet.Views
{
    /// <summary>
    /// LightFaultShieldSetView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(AttachNow = false, ID = EmergencyDispatch.Services.ViewIdAssign.NavToLightFaultShieldSetViewId,
AttachRegion = EmergencyDispatch.Services.ViewIdAssign.NavToLightFaultShieldSetViewAttachRegion)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class LightFaultShieldSetView
    {
        public LightFaultShieldSetView()
        {
            InitializeComponent();
        }

        [Import]
        public IILightFaultShieldSetViewModel Model
        {
            get { return DataContext as IILightFaultShieldSetViewModel; }
            set { DataContext = value; }
        }
    }
}
