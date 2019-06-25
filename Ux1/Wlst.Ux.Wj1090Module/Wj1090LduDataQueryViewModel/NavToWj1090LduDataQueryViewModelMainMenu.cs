using System.ComponentModel.Composition;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Sr.EquipmentInfoHolding.Model;

namespace Wlst.Ux.Wj1090Module.Wj1090LduDataQueryViewModel
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToWj1090LduDataQueryViewModelMainMenu : MenuItemBase
    {
        public NavToWj1090LduDataQueryViewModelMainMenu()
        {
            Id = Wj1090Module.Services.MenuIdAssgin.NavToWj1090LduDataQueryViewModelMainId;
            Text = "线路数据查询";
            Tag = "线路检测集中器数据查询";
            Classic = "主菜单";
            Description = "线路检测集中器数据查询，防盗通用，ID 为"
                + Wj1090Module.Services.MenuIdAssgin.NavToWj1090LduDataQueryViewModelMainId;
            Tooltips = "线路检测数据查询";
            IsCheckable = false;
            IsEnabled = true;
            base.Command = new RelayCommand(Ex, CanEx, true);
            //IsPrivilegLeave = true;

        }
        public override bool IsCanBeShowRwx()
        {
            return true;
        }
        bool CanEx()
        {
            return true;
        }
        protected void Ex()
        {



            ExNavWithArgs(

                Wj1090Module.Services.ViewIdAssign.Wj1090LduDataQueryViewModelId,
                0);
        }
    }
}
