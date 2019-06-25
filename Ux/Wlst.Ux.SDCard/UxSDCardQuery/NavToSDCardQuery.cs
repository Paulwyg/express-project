using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.Models;
using System.ComponentModel.Composition;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Sr.EquipmentInfoHolding.Model;

namespace Wlst.Ux.SDCard.UxSDCardQuery
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToSDCardQuery : MenuItemBase
    {
        public NavToSDCardQuery()
        {
            Id = Wlst.Ux.SDCard.Services.MenuIdAssgin.NavToUxSDCardQueryViewModelMainId;
            Text = "SD卡查询";
            Tag = "SD卡查询";
            Classic = "主菜单";
            Description = "SD卡查询，ID 为" + Wlst.Ux.SDCard.Services.MenuIdAssgin.NavToUxSDCardQueryViewModelMainId;
            Tooltips = "SD卡查询";
            IsCheckable = false;
            IsEnabled = true;
            base.Command = new RelayCommand(Ex,CanEx,true);

        }
        public override bool IsCanBeShowRwx()
        {
            return true;
        }

        private static bool CanEx()
        {
            return true;
        }

        protected void Ex()
        {
            ExNavWithArgs(SDCard.Services.ViewIdAssign.UxSDCardQueryViewId, 0);
        }
    }
}
