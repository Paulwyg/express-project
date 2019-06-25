using Wlst.Cr.CoreOne.Services;

namespace Wlst.Ux.Setting.Services
{
    public class ViewIdAssign
    {
        /// <summary>
        /// 本模块的视图起始Id，1100000 + 53*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int ViewIdAssignBaseId = 1100000 + 53*100;


        public const int SettingViewId = ViewIdAssignBaseId + 1;


        public const int EventTaskViewId = ViewIdAssignBaseId + 2;


        public const int RecordTaskQueryViewId = ViewIdAssignBaseId + 3;

       
    public const int EventScheduleViewId = ViewIdAssignBaseId + 4;

    public const int EventSystemInformationViewId = ViewIdAssignBaseId + 7;
        
    }
}
