using Wlst.Cr.CoreOne.Services;

namespace Wlst.Ux.TimeTableSystem .Services
{
    public class ViewIdAssign
    {
        /// <summary>
        /// 本模块的视图起始Id，1100000 + 32*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int ViewIdAssignBaseId = 1100000 + 32*100;


        public const int TimeTableSetViewId = ViewIdAssignBaseId + 1;

        public const int TimeTableBandingViewId = ViewIdAssignBaseId + 2;
    
        public const int TimeTableManageViewId = ViewIdAssignBaseId + 3;
       
        
              public const int OpenCloseReportTabViewId = ViewIdAssignBaseId + 4;
        public const string OpenCloseReportTabViewAttachRegion = RegionNames.MsgRegion;

          public const int OpenCloseReportQueryViewId = ViewIdAssignBaseId + 5;
        
        public const int HolidayTimeSetViewId = ViewIdAssignBaseId + 6;
      
        public const int TimeTabletemporaryViewId = ViewIdAssignBaseId + 7;
       

        public const int TimeInfoMnViewId = ViewIdAssignBaseId + 8;

        public const int TunnelInfoSetViewId = ViewIdAssignBaseId + 9;
    }
}
