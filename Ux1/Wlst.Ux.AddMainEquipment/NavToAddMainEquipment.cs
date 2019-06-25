using System.ComponentModel.Composition;
using CoreRun.CoreService;

using Infrastructure.IdAssign;
using Wlst.Cr.Core.Commands;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.Core.Models;

namespace WjUniversallyFunctionalModule.AddMainEquipmentViewModel
{
    /// <summary>
    /// 导航到增加主设备
    /// </summary>

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToAddMainEquipment : MenuItemBase
    {
        public NavToAddMainEquipment()
        {
            Id = Infrastructure.IdAssign.MenuIdAssign.NavToAddMainEquipmentId;
            Text = "增加主设备";
            Tag = "增加主设备";
            Describle = "增加主设备，ID 为" + Infrastructure.IdAssign.MenuIdAssign.NavToAddMainEquipmentId;
            Tooltips = "增加主设备";
            base.IsEnabled = SysRunInfo.CanWrite;
            base.IsCheckable = false;
            base.Command = new RelayCommand(Ex);
        }


        protected void Ex()
        {
            this.ExNavWithArgs(ViewIdNameAssign.EquipmentInfoHoldingAddMainEquipmentViewAttachRegion,
                               ViewIdNameAssign.EquipmentInfoHoldingAddMainEquipmentViewId,
                               1);
        }
    }
}
