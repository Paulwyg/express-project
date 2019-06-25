using System.ComponentModel.Composition;
using System.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.LhEquipemntTree.SettingViewModel.Services;

namespace Wlst.Ux.LhEquipemntTree.Views
{
    /// <summary>
    /// SettingView.xaml 的交互逻辑  ViewModulesTreeModuleViewsSettingView
    /// </summary>
    [ViewExport( Services .ViewIdAssign .SettingViewId )]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class SettingView : UserControl
    {
        public SettingView()
        {
            InitializeComponent();
        }

        [Import]
        public IISettingViewModel  Model
        {

            get { return DataContext as IISettingViewModel; }
            set { DataContext = value; }
        }
    }
}
