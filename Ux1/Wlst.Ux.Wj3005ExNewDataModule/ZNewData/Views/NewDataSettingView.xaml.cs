using System.ComponentModel.Composition;
using System.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.Wj3005ExNewDataModule.ZNewData.NewDataSetting;

namespace Wlst.Ux.Wj3005ExNewDataModule.ZNewData.Views
{
    /// <summary>
    /// NewDataSettingView.xaml 的交互逻辑
    /// </summary>
  

    [ViewExport(
       AttachNow = false,
       AttachRegion = Services.ViewIdAssign.NewDataSettingViewAttachRegion,
       ID = Services.ViewIdAssign.NewDataSettingViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class NewDataSettingView : UserControl
    {
        public NewDataSettingView()
        {
            InitializeComponent();
        }

        [Import]
        public IINewDataSetting Model
        {

            get { return DataContext as IINewDataSetting; }
            set { DataContext = value; }
        }
    }
}
