using System.ComponentModel.Composition;
using Infrastructure.IdAssign;
using Wlst.Cr.Core.Commands;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.Core.Models;

namespace Infrastructure.SettingViewModel
{
    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToSettingViewModel : MenuItemBase
    {
        public NavToSettingViewModel()
        {
            Id = Infrastructure.IdAssign.MenuIdAssign.NavToSettingViewModelId;
            Text = "全局参数设置";
            Tag = "全局参数设置，全局参数设置";
            Describle = "全局参数设置，ID 为" + Infrastructure.IdAssign.MenuIdAssign.NavToSettingViewModelId;
            Tooltips = "全局参数设置，全局参数设置";
            base.IsEnabled = true;
            base.IsCheckable = false;
            base.Command = new RelayCommand(Ex);
        }


        protected void Ex()
        {
            this.ExNavWithArgs(ViewIdNameAssign.InfrastructureSettingViewAttachRegion,
                               ViewIdNameAssign.InfrastructureSettingViewId,
                               1);
            // MessageBoxResult result = MessageBox.Show("Sorry for have't finish this function ......", "Sorry", MessageBoxButton.OK);
        }
    }
}