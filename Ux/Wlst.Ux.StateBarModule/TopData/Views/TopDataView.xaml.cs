using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Windows.Input;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.StateBarModule.TopData.Services;

namespace Wlst.Ux.StateBarModule.TopData.Views
{
    /// <summary>
    /// TopDataView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(  StateBarModule.Services.ViewIdAssign.TopDataViewId,AttachNow = true,
        AttachRegion = StateBarModule.Services.ViewIdAssign.TopDataViewAttachRegion)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class TopDataView : UserControl
    {
        public TopDataView()
        {
            InitializeComponent();


            var sunlux = Wlst.Cr.CoreOne.Services.ImageSourceHelper.MySelf.GetImageSourceById(10000104);
            if (sunlux != null) sunvalue.Source = sunlux;
        }

        [Import]
        public IITopDataViewModel Model
        {
            get { return DataContext as IITopDataViewModel; }
            set { DataContext = value; }
        }

        private void TextBlock_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var textblock = sender as Label;
            
            if (textblock == null) return;
            MessageShowInfo.Show(textblock.Content.ToString(), textblock.ToolTip.ToString());
        }
    }
}
