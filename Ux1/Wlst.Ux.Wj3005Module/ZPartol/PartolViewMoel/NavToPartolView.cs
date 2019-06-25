using System.ComponentModel.Composition;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Ux.WJ3005Module.Services;

namespace Wlst.Ux.WJ3005Module.ZPartol.PartolViewMoel
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToPartolView : MenuItemBase
    {
        public NavToPartolView()
        {
            Id = MenuIdAssgin.NavToPartolViewId;// Infrastructure.IdAssign.MenuIdAssign.NavToPatrolViewId;
            Text = "终端巡测";
            Tag = "巡测界面";
            Description = "巡测界面，ID 为" + MenuIdAssgin.NavToPartolViewId;// Infrastructure.IdAssign.MenuIdAssign.NavToPatrolViewId;
            Tooltips = "巡测界面";
            this.Classic = "主菜单";
            base.IsEnabled = true;
            base.IsCheckable = false;
            base.Command = new RelayCommand(Ex );
        }
        public override bool IsCanBeShowRwx()
        {
            return true;
            //var equipment = this.Argu as Wlst.Sr .EquipmentInfoHolding .Model .WjParaBase ;//.WjEquipmentBaseModels.Interface.IIEquipmentInfo;
            //if (equipment == null || equipment.RtuStateCode  == 0) return false;
            //return  Wlst.Cr.CoreMims.Services.UserInfo.CanR(equipment.AreaId);  
        }

        private void Ex()
        {
            this.ExNavWithArgs( ViewIdAssign .PartolViewId,1 );

        }

    }
}
