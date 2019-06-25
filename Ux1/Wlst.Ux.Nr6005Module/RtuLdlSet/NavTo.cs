using System.ComponentModel.Composition;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.Nr6005Module.RtuLdlSet
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
            Id = Wlst.Ux.Nr6005Module.Services.MenuIdAssgin.NavToLdlId;
            // Infrastructure.IdAssign.MenuIdAssign.NavToSndWeekTimeQueryViewId;
            Text = "终端亮灯率设置";
            Tag = "终端亮灯率设置";
            this.Classic = "主菜单";
            Description = "终端亮灯率设置，ID 为" + Wlst.Ux.Nr6005Module.Services.MenuIdAssgin.NavToLdlId;
            // Infrastructure.IdAssign.MenuIdAssign.NavToSndWeekTimeQueryViewId;
            Tooltips = "终端亮灯率设置";
            base.IsEnabled = true;
            base.IsCheckable = false;
            base.Command = new RelayCommand(Ex, CanEx, true);
        }
        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0;
        }
        private bool CanEx()
        {
            return true;
        }


        protected void Ex()
        {

            this.ExNavWithArgs(
                               Nr6005Module.Services.ViewIdAssign.NavToLdlViewId,
                               0);
            // MessageBoxResult result = MessageBox.Show("Sorry for have't finish this function ......", "Sorry", MessageBoxButton.OK);
        }


    }


}
