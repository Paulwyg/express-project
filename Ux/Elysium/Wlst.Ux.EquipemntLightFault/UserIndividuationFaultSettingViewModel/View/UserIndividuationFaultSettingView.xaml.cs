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
using Wlst.Ux.EquipemntLightFault.UserIndividuationFaultSettingViewModel.Services;

namespace Wlst.Ux.EquipemntLightFault.UserIndividuationFaultSettingViewModel.View
{
    /// <summary>
    /// UserIndividuationFaultSettingView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(EquipemntLightFault.Services.ViewIdAssign.UserIndividuationFaultSettingViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class UserIndividuationFaultSettingView : UserControl
    {
        public UserIndividuationFaultSettingView()
        {
            InitializeComponent();
        }

        [Import]
        public IIUserIndividuationFaultSettingViewModel Model
        {
            get { return DataContext as IIUserIndividuationFaultSettingViewModel; }
            set { DataContext = value; }
        }
    }
}
