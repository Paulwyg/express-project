using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.WjEquipmentBaseModels.Interface;

namespace Wlst.Ux.StateBarModule.FlashPlayer
{

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToFlashPlayerView : MenuItemBase
    {
        public NavToFlashPlayerView()
        {
            Id = Wlst.Ux.StateBarModule.Services.MenuIdAssgin.NavToFlashPlayerViewId;
            // Infrastructure.IdAssign.MenuIdAssign.NavToEquipmentDailyDataQueryViewId;
            Text = "实景三维";
            Tag = "实景三维";
            Classic = "右键菜单";
            Description = "实景三维，ID 为" + Wlst.Ux.StateBarModule.Services.MenuIdAssgin.NavToFlashPlayerViewId;
            // Infrastructure.IdAssign.MenuIdAssign.NavToEquipmentDailyDataQueryViewId;
            Tooltips = "右键菜单";
            IsEnabled = true;
            IsCheckable = false;
            IsPrivilegLeave = false;
            base.Command = new RelayCommand(Ex, CanEx, true);

        }

        private bool CanEx()
        {
            return true;
        }

       // private static Fls fs =null ;
        protected void Ex()
        {
            //var equipment = this.Argu as IIEquipmentInfo;
            //if (equipment == null) return;
            //int rtuId = equipment.RtuId;
            //if (rtuId < 1) return;

            //ExNavWithArgs(Wlst.Ux.StateBarModule.Services.ViewIdAssign.FlashPlayerViewAttachRegion,
            //              Wlst.Ux.StateBarModule.Services.ViewIdAssign.FlashPlayerViewId,
            //              rtuId);
            try
            {

                Fls fs = new Fls();
                fs.Show();
            }
            catch (Exception ex)
            {
                
            }
        }

    }
}
