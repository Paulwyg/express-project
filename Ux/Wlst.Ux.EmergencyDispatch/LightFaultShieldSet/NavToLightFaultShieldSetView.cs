using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.EmergencyDispatch.LightFaultShieldSet
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToLightFaultShieldSetView : MenuItemBase
    {
        public NavToLightFaultShieldSetView()
        {
            Id = Wlst.Ux.EmergencyDispatch.Services.MenuIdAssgin.NavToLightFaultShieldViewId;
            Text = "设置报警屏蔽";
            Tag = "设置报警屏蔽";
            Classic = "主菜单";
            Description = "设置报警屏蔽，ID 为" + EmergencyDispatch.Services.MenuIdAssgin.NavToLightFaultShieldViewId;
            Tooltips = "设置报警屏蔽";
            IsEnabled = true;
            IsCheckable = false;
            base.Command = new RelayCommand(Ex, CanEx, true);

        }

        bool CanEx()
        {
            return true;
        }


        protected void Ex()
        {
            ExNavWithArgs(EmergencyDispatch.Services.ViewIdAssign.NavToLightFaultShieldSetViewAttachRegion,
                EmergencyDispatch.Services.ViewIdAssign.NavToLightFaultShieldSetViewId,
                               1);
        }
    }
}
