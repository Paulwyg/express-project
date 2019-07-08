using System.ComponentModel.Composition;
using System.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.Setting.Services;
using Wlst.Ux.Setting.SettingViewModel.Services;

namespace Wlst.Ux.Setting.SettingViewModel.View
{
    /// <summary>
    /// SettingView.xaml 的交互逻辑    所有参数设置的框架
    /// </summary>
    [ViewExport(ViewIdAssign.SettingViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class SettingView : UserControl
    {
        public SettingView()
        {
            InitializeComponent();
        }

        [Import]
        public IISettingViewModel Model
        {

            get { return DataContext as IISettingViewModel; }
            set { DataContext = value; }
        }
    }
}
