using System.ComponentModel.Composition;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.WjEquipmentBaseModels.Interface;

namespace Wlst.Ux.Wj1090Module.LduInfoSet
{
    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToLduInfoSetViewfor1090 : MenuItemBase
    {
        public NavToLduInfoSetViewfor1090()
        {
            Id = Wj1090Module.Services.MenuIdAssgin.NavToLduInfoSetViewfor1090Id;
                // Infrastructure.IdAssign.MenuIdAssign.NavToWj3005TmlInfoSetId;
            Text = "参数设置";
            Tag = "线路防盗集中器参数设置";
            Classic = "右键菜单-线路防盗集中器-专有";
            Description = "线路防盗集中器参数设置，防盗通用，ID 为" + Wj1090Module.Services.MenuIdAssgin.NavToLduInfoSetViewfor1090Id;
                // Infrastructure.IdAssign.MenuIdAssign.NavToWj3005TmlInfoSetId;
            Tooltips = "设置当前终端终端参数";
            IsCheckable = false;
            IsEnabled = true;
            base.Command = new RelayCommand(Ex,CanEx,true);
            IsPrivilegLeave = true;

        }
        private static bool CanEx()
        {
            return true;
        }
        protected void Ex()
        {
            var equipment = Argu as IIEquipmentInfo;
            if (equipment == null)
                return;
            int rtuId = equipment.RtuId;
            if (rtuId < 1)
                return;
            ExNavWithArgs(
                Wj1090Module.Services.ViewIdAssign.LduInfoSetViewAttachRegion,
                Wj1090Module.Services.ViewIdAssign.LduInfoSetViewId,
                rtuId);
        }

    }
}