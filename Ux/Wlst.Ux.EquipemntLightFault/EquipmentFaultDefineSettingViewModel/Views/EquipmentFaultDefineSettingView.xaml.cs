using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultDefineSettingViewModel.Services;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultDefineSettingViewModel.Views
{
    /// <summary>
    /// EquipmentFaultDefineSettingView.xaml 的交互逻辑 EquipemntLightFaultEquipmentFaultDefineSettingView
    /// </summary>
    [ViewExport(EquipemntLightFault .Services .ViewIdAssign .EquipmentFaultDefineSettingViewId )]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class EquipmentFaultDefineSettingView : UserControl
    {
        public EquipmentFaultDefineSettingView()
        {
            InitializeComponent();
        }

        [Import]
        public IIEquipmentFaultDefineSettingViewModel Model
        {
            get { return DataContext as IIEquipmentFaultDefineSettingViewModel; }
            set { DataContext = value; }
        }

        private int count = 0;
        private void TextBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            count++;
            if (count >= 3)
            {
                bjgz.Visibility = Visibility.Visible;
            }
        }
    }
}
