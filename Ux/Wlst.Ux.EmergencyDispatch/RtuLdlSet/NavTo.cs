using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.MefExtensions.Event;
using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.UtilityFunction;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.EmergencyDispatch.RtuLdlSet
{
    //public class NavTo
    //{
    //    internal static void NavToLdl(IEnumerable<int> rtus)
    //    {
    //        RegionManage.ShowViewByIdAttachRegionWithArgu(EmergencyDispatch.Services.ViewIdAssign.NavToLdlViewId,
    //                                                    rtus);



    //    }
    
    
    
    //}
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavTo : MenuItemBase
    {
        public NavTo()
        {
            Id = Wlst.Ux.EmergencyDispatch.Services.MenuIdAssgin.NavToLdlId;
            // Infrastructure.IdAssign.MenuIdAssign.NavToSndWeekTimeQueryViewId;
            Text = "终端亮灯率设置";
            Tag = "终端亮灯率设置";
            this.Classic = "主菜单";
            Description = "终端亮灯率设置，ID 为" + Wlst.Ux.EmergencyDispatch.Services.MenuIdAssgin.NavToLdlId;
            // Infrastructure.IdAssign.MenuIdAssign.NavToSndWeekTimeQueryViewId;
            Tooltips = "终端亮灯率设置";
            base.IsEnabled = true;
            base.IsCheckable = false;
            base.Command = new RelayCommand(Ex, CanEx, true);
        }
        public override bool IsCanBeShowRwx()
        {
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanR();
            return true;
        }
        private bool CanEx()
        {
            return true;
        }


        protected void Ex()
        {

            this.ExNavWithArgs(
                               EmergencyDispatch.Services.ViewIdAssign.NavToLdlViewId,
                               0);
            // MessageBoxResult result = MessageBox.Show("Sorry for have't finish this function ......", "Sorry", MessageBoxButton.OK);
        }


    }


}
