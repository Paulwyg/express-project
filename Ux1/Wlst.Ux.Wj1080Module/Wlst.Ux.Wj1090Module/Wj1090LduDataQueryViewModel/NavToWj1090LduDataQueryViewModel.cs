using System.ComponentModel.Composition;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.WjEquipmentBaseModels.Interface;

namespace Wlst.Ux.Wj1090Module.Wj1090LduDataQueryViewModel
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToWj1090LduDataQueryViewModel : MenuItemBase
    {
        public NavToWj1090LduDataQueryViewModel()
        {
            Id = Wj1090Module.Services.MenuIdAssgin.NavToWj1090LduDataQueryViewModelId;
            Text = "数据查询";
            Tag = "线路防盗集中器数据查询";
            Classic = "右键菜单-线路防盗集中器-专有";
            Description = "线路防盗集中器数据查询，防盗通用，ID 为" 
                + Wj1090Module.Services.MenuIdAssgin.NavToWj1090LduDataQueryViewModelId;
            Tooltips = "防盗数据查询";
            IsCheckable = false;
            IsEnabled = true;
            base.Command = new RelayCommand(Ex, CanEx, true);
            IsPrivilegLeave = true;

        }
        bool CanEx()
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
                Wj1090Module.Services.ViewIdAssign.Wj1090LduDataQueryViewModelAttachRegion,
                Wj1090Module.Services.ViewIdAssign.Wj1090LduDataQueryViewModelId,
                rtuId);
        }
    }
}
