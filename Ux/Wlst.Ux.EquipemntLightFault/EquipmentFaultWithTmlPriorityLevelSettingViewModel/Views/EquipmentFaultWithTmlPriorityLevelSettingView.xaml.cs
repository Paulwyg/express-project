using System.ComponentModel.Composition;
using System.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultWithTmlPriorityLevelSettingViewModel.Services;


namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultWithTmlPriorityLevelSettingViewModel.Views
{
    /// <summary>
    /// EquipmentFaultWithTmlPriorityLevelSettingView.xaml 的交互逻辑 EquipemntLightFaultEquipmentFaultWithTmlPriorityLevelSettingView
    /// </summary>
    [ViewExport(EquipemntLightFault.Services.ViewIdAssign.EquipmentFaultWithTmlPriorityLevelSettingViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class EquipmentFaultWithTmlPriorityLevelSettingView : UserControl
    {
        public EquipmentFaultWithTmlPriorityLevelSettingView()
        {
            InitializeComponent();
        }


        [Import]
        public IIEquipmentFaultWithTmlPriorityLevelSettingViewModel Model
        {
            get { return DataContext as IIEquipmentFaultWithTmlPriorityLevelSettingViewModel; }
            set { DataContext = value; }
        }



       

     
    }
}
