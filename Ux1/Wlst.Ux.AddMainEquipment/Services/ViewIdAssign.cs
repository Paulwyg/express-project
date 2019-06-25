using Wlst.Cr.CoreOne.Services;

namespace Wlst.Ux.AddMainEquipment.Services
{
    public class ViewIdAssign
    {
        /// <summary>
        /// 本模块的视图起始Id，1100000 + 52*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int ViewIdAssignBaseId = 1100000 + 52*100;


        public const int AddMainEquipmentViewId = ViewIdAssignBaseId + 1;


    }
}
