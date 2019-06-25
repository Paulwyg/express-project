using System.ComponentModel.Composition;
using Infrastructure.IdAssign;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.TimeTableSystem.TimeTableSetViewModel
{
    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToTimeTableSet : MenuItemBase
    {
        public NavToTimeTableSet()
        {
            Id = TimeTableSystem.Services.MenuIdAssgin.NavToTimeTableSetId;
            //Infrastructure.IdAssign.MenuIdAssign.NavToTimeTableSetId;
            Text = "时间表设置";
            Tag = "时间表设置";
            this.Classic = "主菜单";
            Description = "时间表设置，ID 为" + TimeTableSystem.Services.MenuIdAssgin.NavToTimeTableSetId;
            //Infrastructure.IdAssign.MenuIdAssign.NavToTimeTableSetId;
            Tooltips = "时间表设置";
            base.Command = new RelayCommand(Ex );
            this.IsPrivilegLeave = true;
        }

        protected void Ex()
        {
            this.ExNavWithArgs(
                //ViewIdNameAssign.TimeTableSystemModuleTimeTableSetViewAttachRegion,
                //ViewIdNameAssign.TimeTableSystemModuleTimeTableSetViewId,
                TimeTableSystem .Services .ViewIdAssign .TimeTableSetViewAttachRegion ,
                TimeTableSystem .Services .ViewIdAssign .TimeTableSetViewId ,
                1);
        }
    }
}