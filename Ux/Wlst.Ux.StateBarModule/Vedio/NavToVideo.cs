using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.StateBarModule.Vedio
{
   

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToVideo : MenuItemBase
    {
        public NavToVideo()
        {
            Id = Wlst.Ux.StateBarModule.Services.MenuIdAssgin.NavToVideoViewId;
            // Infrastructure.IdAssign.MenuIdAssign.NavToEquipmentDailyDataQueryViewId;
            Text = "播放视频";
            Tag = "播放视频";
            Classic = "播放视频";
            Description = "播放视频，ID 为" + Wlst.Ux.StateBarModule.Services.MenuIdAssgin.NavToVideoViewId;
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

                VideoView vd = new VideoView();
                vd.Show();
            }
            catch (Exception ex)
            {

            }
        }

    }
}
