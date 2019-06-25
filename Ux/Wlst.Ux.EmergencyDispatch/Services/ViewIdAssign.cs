using Wlst.Cr.CoreOne.Services;

namespace Wlst.Ux.EmergencyDispatch.Services
{
    public class ViewIdAssign
    {
        /// <summary>
        /// 本模块的视图起始Id，1100000 + 88*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int ViewIdAssignBaseId = 1100000 + 88*100;


        public const int NavToLightEmergencyDispatch = ViewIdAssignBaseId + 1;
      
       
        public const int NavToLightFaultShieldSetViewId = ViewIdAssignBaseId + 2;
        
        public const int NavToLignthEmergencyManagId = ViewIdAssignBaseId + 3;
       
        public const int NavToControlCenterManagId = ViewIdAssignBaseId + 4;
       
        public const int NavToControlCenterManagDemo2Id = ViewIdAssignBaseId + 5;
      

        public const int NavToLdlViewId = ViewIdAssignBaseId + 6;
        
    }
}
