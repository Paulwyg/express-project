using System.ComponentModel.Composition;
using System.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultManageSettingViewModel.Services;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultManageSettingViewModel.Views
{
    /// <summary>
    /// EquipmentFaultManageSettingView.xaml 的交互逻辑
    /// </summary>

    [ViewExport(Wlst.Ux.EquipemntLightFault.Services.ViewIdAssign.EquipmentFaultManageSettingViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class EquipmentFaultManageSettingView : UserControl
    {
        public EquipmentFaultManageSettingView()
        {
            InitializeComponent();
        }

        [Import]
        public IIEquipmentFaultManageSettingViewModel Model
        {

            get { return DataContext as IIEquipmentFaultManageSettingViewModel; }
            set { DataContext = value; }
        }
    }
}
