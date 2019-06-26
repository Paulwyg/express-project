using System.ComponentModel.Composition;
using System.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.RadMapJpeg.Setting.SettingViewModel.Services;

namespace Wlst.Ux.RadMapJpeg.Views
{
    /// <summary>
    /// SettingView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(  RadMapJpeg .Services .ViewIdAssign .SettingViewId ,RadMapJpeg .Services .ViewIdAssign .SettingViewAttachRegion ,false )]
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