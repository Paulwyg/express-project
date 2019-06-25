using System.ComponentModel.Composition;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Ux.Wj2090Module.Services;

namespace Wlst.Ux.Wj2090Module.HisDataQuery.ConcentratorDataQuery
{
    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToConcentratorDataQuery : MenuItemBase
    {
        public NavToConcentratorDataQuery()
        {
            Id = MenuIdAssign.ConcentratorDataQueryViewMenuId;
            Text = "单灯数据";
            Tag = "单灯数据查询";
            this.Classic = "主菜单";
            Description = "单灯数据查询，ID 为" + MenuIdAssign.ConcentratorDataQueryViewMenuId;
            Tooltips = "单灯数据查询";
            base.IsEnabled = true;
            base.IsCheckable = false;
            base.Command = new RelayCommand(Ex); //IsPrivilegLeave = false;
        }
        public override bool IsCanBeShowRwx()
        {
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanR();
            return true;
        }

        protected void Ex()
        {

            ExNavWithArgs(
                          ViewIdAssign.ControlDataQueryViewId,
                          0);
        }

    }
}
