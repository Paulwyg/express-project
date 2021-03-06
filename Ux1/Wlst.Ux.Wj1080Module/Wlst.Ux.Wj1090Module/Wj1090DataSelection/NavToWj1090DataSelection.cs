﻿using System.ComponentModel.Composition;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.WjEquipmentBaseModels.Interface;

namespace Wlst.Ux.Wj1090Module.Wj1090DataSelection
{
    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToWj1090DataSelection : MenuItemBase
    {
        public NavToWj1090DataSelection()
        {
            Id = Wj1090Module.Services.MenuIdAssgin.NavWj1090DataSelectionMenuId;
                // Infrastructure.IdAssign.MenuIdAssign.NavToWj3005TmlInfoSetId;
            Text = "数据选测";
            Tag = "线路防盗集中器数据选测";
            Classic = "右键菜单-线路防盗集中器-专有";
            Description = "线路防盗集中器数据选测，防盗通用，ID 为" + Wj1090Module.Services.MenuIdAssgin.NavWj1090DataSelectionMenuId;
                // Infrastructure.IdAssign.MenuIdAssign.NavToWj3005TmlInfoSetId;
            Tooltips = "防盗集中器数据选测";
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
                Wj1090Module.Services.ViewIdAssign.Wj1090DataSelectionViewAttachRegion,
                Wj1090Module.Services.ViewIdAssign.Wj1090DataSelectionViewId,
                rtuId);
        }

    }
}