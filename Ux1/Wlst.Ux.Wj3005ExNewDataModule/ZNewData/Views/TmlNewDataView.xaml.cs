using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Windows.Input;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.Wj3005ExNewDataModule.Services;
using Wlst.Ux.Wj3005ExNewDataModule.ZNewData.TmlNewDataViewModel.Services;

namespace Wlst.Ux.Wj3005ExNewDataModule.ZNewData.Views
{
    /// <summary>
    /// TmlNewDataView.xaml 的交互逻辑
    /// </summary>

    //[ViewExport(
    //    AttachNow = true,
    //    AttachRegion = Services .ViewIdAssign .TmlNewDataViewAttachRegion  ,
    //    ID = Services .ViewIdAssign .TmlNewDataViewId )]
    //[PartCreationPolicy(CreationPolicy.Shared)]

    [ViewExport(
        AttachNow = false ,
        AttachRegion = ViewIdAssign.TmlNewDataViewAttachRegion,
        ID = ViewIdAssign.TmlNewDataViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class TmlNewDataView : UserControl
    {
        public TmlNewDataView()
        {
            InitializeComponent();
        }

        [Import]
        public IINewDataViewModel Model
        {
            get { return DataContext as IINewDataViewModel; }
            set { DataContext = value; }
        }

        private void TextBlock_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed || e.LeftButton == MouseButtonState.Released)
            {
                Model.MeasureRtu();
            }
        }

        private void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed || e.LeftButton == MouseButtonState.Released)
            {
                Model.RequestNearData();
            }
        }
    }
}
