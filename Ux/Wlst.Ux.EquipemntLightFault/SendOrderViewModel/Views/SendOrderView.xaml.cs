using System.ComponentModel.Composition;
using System.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.EquipemntLightFault.SendOrderViewModel.Services;

namespace Wlst.Ux.EquipemntLightFault.SendOrderViewModel.Views
{
    /// <summary>
    /// SendOrderView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(EquipemntLightFault.Services.ViewIdAssign.SendOrderViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class SendOrderView : UserControl
    {
        public SendOrderView()
        {
            InitializeComponent();
        }

        [Import]
        private IISendOrderView Model
        {
            get { return DataContext as IISendOrderView; }
            set
            {
                DataContext = value;
            }
        }
    }


}
