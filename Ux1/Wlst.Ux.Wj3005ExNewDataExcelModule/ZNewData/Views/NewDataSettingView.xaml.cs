using System.ComponentModel.Composition;
using System.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.Wj3005ExNewDataExcelModule.Services;
using Wlst.Ux.Wj3005ExNewDataExcelModule.ZNewData.NewDataSetting;

namespace Wlst.Ux.Wj3005ExNewDataExcelModule.ZNewData.Views
{
    /// <summary>
    /// NewDataSettingView.xaml 的交互逻辑
    /// </summary>
  

    [ViewExport(ViewIdAssign.NewDataSettingViewId)]
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
