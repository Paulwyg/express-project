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
using Wlst.Ux.EquipemntLightFault.EquipmentFaultOnTabViewModel.Services;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultOnTabViewModel.ViewModel;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultOnTabViewModel.Views
{
    /// <summary>
    /// EquipmentAllFaultOnTabView.xaml 的交互逻辑
    /// </summary>
    //[ViewExport(EquipemntLightFault.Services.ViewIdAssign.EquipmentAllFaultOnTabViewId,
    //    AttachNow = true, AttachRegion = EquipemntLightFault.Services.ViewIdAssign.EquipmentFaultOnTabViewAttachRegion)]
    //[PartCreationPolicy(CreationPolicy.Shared)]
    public partial class EquipmentAllFaultOnTabView : UserControl
    {
        public EquipmentAllFaultOnTabView()
        {
            InitializeComponent();
        }

        [Import]
        public IIEquipmentAllFaultOnTabViewModel Model
        {
            get { return DataContext as IIEquipmentAllFaultOnTabViewModel; }
            set { DataContext = value; }
        }

        private void RadGridView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(e.RightButton ==MouseButtonState.Pressed  )
            {
                  Model.ClearVoiceReportItems();
            }
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var sdr = sender as Telerik.Windows.Controls.RadGridView;
                if (sdr == null) return;
                var item = sdr.SelectedItem as OneTmlExistFaultViewModel;
                if (item == null) return;
                if (Model != null) Model.OnRequestServerData(item);
            }
          
        }


        //private void RadGridView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    Model.ClearVoiceReportItems();
        //}

    }
}
