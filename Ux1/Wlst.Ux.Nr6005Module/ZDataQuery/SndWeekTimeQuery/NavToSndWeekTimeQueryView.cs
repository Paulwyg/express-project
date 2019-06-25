using System.ComponentModel.Composition;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.Nr6005Module.ZDataQuery.SndWeekTimeQuery
{


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToSndWeekTimeQueryView : MenuItemBase
    {
        public NavToSndWeekTimeQueryView()
        {
            Id = Nr6005Module.Services.MenuIdAssgin.NavToSndWeekTimeQueryViewId;// Infrastructure.IdAssign.MenuIdAssign.NavToSndWeekTimeQueryViewId;
            Text = "周设置发送查询";
            Tag = "周设置发送查询";
            this.Classic = "主菜单";
            Description = "周设置发送查询，ID 为" + Nr6005Module.Services.MenuIdAssgin.NavToSndWeekTimeQueryViewId;// Infrastructure.IdAssign.MenuIdAssign.NavToSndWeekTimeQueryViewId;
            Tooltips = "周设置发送查询";
            base.IsEnabled = true;
            base.IsCheckable = false;
            base.Command = new RelayCommand(Ex, CanEx, true);
        }
        public override bool IsCanBeShowRwx()
        {
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanR();
            return true;
        }
        bool CanEx()
        {
            return true;
        }


        protected void Ex()
        {

            this.ExNavWithArgs(
                               Nr6005Module.Services.ViewIdAssign.SndWeekTimeQueryViewId,
                               0);
            // MessageBoxResult result = MessageBox.Show("Sorry for have't finish this function ......", "Sorry", MessageBoxButton.OK);
        }


    }
}
