using System.ComponentModel.Composition;
using System.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultWithTmlSettingViewModel.Services;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultWithTmlSettingViewModel.Views
{
    /// <summary>
    /// EquipmentFaultWithTmlSettingView.xaml 的交互逻辑 EquipemntLightFaultEquipmentFaultWithTmlSettingView
    /// </summary>
    [ViewExport( EquipemntLightFault .Services .ViewIdAssign .EquipmentFaultWithTmlSettingViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class EquipmentFaultWithTmlSettingView : UserControl
    {
        public EquipmentFaultWithTmlSettingView()
        {
            InitializeComponent();
        }


        [Import]
        public IIEquipmentFaultWithTmlSettingViewModel Model
        {
            get { return DataContext as IIEquipmentFaultWithTmlSettingViewModel; }
            set { DataContext = value; }
        }

     
    }
}
