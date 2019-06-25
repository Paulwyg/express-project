using System.ComponentModel.Composition;
using System.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultOnTabViewModel.Services;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultOnTabViewModel.ViewModel;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultOnTabViewModel.Views
{
    /// <summary>
    /// EquipmentFaultOnTabView.xaml 的交互逻辑 EquipemntLightFaultEquipmentFaultOnTabView
    /// </summary>
    [ViewExport(    EquipemntLightFault .Services .ViewIdAssign .EquipmentFaultOnTabViewId,
      AttachNow = true,
      AttachRegion = EquipemntLightFault .Services .ViewIdAssign .EquipmentFaultOnTabViewAttachRegion
   )]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class EquipmentFaultOnTabView : UserControl
    {
        public EquipmentFaultOnTabView()
        {
            InitializeComponent();
        }

        [Import]
        public IIEquipmentFaultOnTabViewModel Model
        {
            get { return DataContext as IIEquipmentFaultOnTabViewModel; }
            set { DataContext = value; }
        }

        private void RadGridView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var sdr = sender as Telerik.Windows.Controls.RadGridView;
            if (sdr == null) return;
            var item = sdr.SelectedItem as OneTmlExistFaultViewModel;
            if (item == null) return;
            if (Model != null) Model.OnRequestServerData(item);
        }
    }
}
