using System.ComponentModel.Composition;
using System.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultManageSettingLnViewModel.Services;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultManageSettingLnViewModel.Views
{
    /// <summary>
    /// EquipmentFaultManageSettingView.xaml 的交互逻辑
    /// </summary>

    [ViewExport(Wlst.Ux.EquipemntLightFault.Services.ViewIdAssign.EquipmentFaultManageSettingLnViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class EquipmentFaultManageSettingLnView : UserControl
    {
        public EquipmentFaultManageSettingLnView()
        {
            InitializeComponent();
        }

        [Import]
        public IIEquipmentFaultManageSettingLnViewModel Model
        {

            get { return DataContext as IIEquipmentFaultManageSettingLnViewModel; }
            set { DataContext = value; }
        }
    }
}
