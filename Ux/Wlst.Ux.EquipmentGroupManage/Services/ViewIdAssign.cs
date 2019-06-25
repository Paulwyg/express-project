using Wlst.Cr.CoreOne.Services;

namespace Wlst.Ux.EquipmentGroupManage.Services
{
    public class ViewIdAssign
    {
        /// <summary>
        /// 本模块的视图起始Id，1100000 + 42*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int ViewIdAssignBaseId = 1100000 + 42*100;


        public const int TreeModuleGrpMulitManageViewId = ViewIdAssignBaseId + 1;
    
        public const int GrpShowModuleGrpSingleManageViewId = ViewIdAssignBaseId + 2;


    }
}
