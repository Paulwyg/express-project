using Wlst.Cr.CoreOne.Services;

namespace Wlst.Ux.PrivilegesManage.Services
{
    public class ViewIdAssign
    {
        /// <summary>
        /// 本模块的视图起始Id，1100000 + 57*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int ViewIdAssignBaseId = 1100000 + 57*100;


        public const int SelfInfoChangeViewId = ViewIdAssignBaseId + 1;

        public const int ModflyOtherUserInfoViewId = ViewIdAssignBaseId + 2;

        
        public const int UserInfoManageViewId = ViewIdAssignBaseId + 3;

        
        public const int UserAndPrivilegeManageViewId = ViewIdAssignBaseId + 4;

        public const int PrivilegeManageViewId = ViewIdAssignBaseId + 5;

        public const int AreaManageViewId = ViewIdAssignBaseId + 6;


    }
}
