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
using Wlst.Ux.EquipemntLightFault.CurrentEquipmentFaultViewModel.Services;
using Wlst.Ux.EquipemntLightFault.CurrentEquipmentFaultViewModel.ViewModel;

namespace Wlst.Ux.EquipemntLightFault.CurrentEquipmentFaultViewModel.Views
{
    /// <summary>
    /// CurrentEquipmentFaultView.xaml 的交互逻辑
    /// </summary>
    //[ViewExport(EquipemntLightFault.Services.ViewIdAssign.CurrentEquipmentFaultViewId)]
    //[PartCreationPolicy(CreationPolicy.Shared)]
    public partial class CurrentEquipmentFaultView:UserControl
    {
        public CurrentEquipmentFaultView()
        {
            InitializeComponent();
        }

        //[Import]
        //public IICurrentEquipmentFaultView Model
        //{
        //    get { return DataContext as IICurrentEquipmentFaultView; }
        //    set { DataContext = value; }
        //}

    }
}
