using System.ComponentModel.Composition;
using System.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.RadMapJpeg.Setting.SettingViewModel.Services;

namespace Wlst.Ux.RadMapJpeg.Views
{
    /// <summary>
    /// SettingView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(
        AttachNow = false,
        AttachRegion = RadMapJpeg .Services .ViewIdAssign .SettingViewAttachRegion ,
        ID = RadMapJpeg .Services .ViewIdAssign .SettingViewId )]
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