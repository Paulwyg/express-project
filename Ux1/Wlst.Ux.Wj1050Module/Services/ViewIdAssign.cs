using Wlst.Cr.CoreOne.Services;

namespace Wlst.Ux.Wj1050Module.Services
{
    public class ViewIdAssign
    {
        /// <summary>
        /// 本模块的视图起始Id，1100000 + 25*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int ViewIdAssignBaseId = 1100000 + 25*100;


        public const int Wj1050InfoSetViewId = ViewIdAssignBaseId + 1;

                
        public const int Wj1050ManageViewId = ViewIdAssignBaseId + 2;

        public const string Wj1050ManageViewAttachRegion =
            RegionNames.LeftViewRegion  ;

        public const int Wj1050MruEventScheduleViewId = ViewIdAssignBaseId + 3;
    
        
            public const int Wj1050ManageSettingViewId = ViewIdAssignBaseId + 4;
 

        public const int Wj1050DataInqueryViewId = ViewIdAssignBaseId + 5;

    }
}
