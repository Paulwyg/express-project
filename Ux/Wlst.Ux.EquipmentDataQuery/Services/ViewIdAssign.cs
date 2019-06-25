using Wlst.Cr.CoreOne.Services;

namespace Wlst.Ux.EquipmentDataQuery.Services
{
    public class ViewIdAssign
    {
        /// <summary>
        /// 本模块的视图起始Id，1100000 + 37*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int ViewIdAssignBaseId = 1100000 + 37*100;


        

             public const int RecordEventQueryViewId = ViewIdAssignBaseId + 3;

        public const string RecordEventQueryViewAttachRegion =
            RegionNames.DocumentRegion;

        
           

        public const int OperatorDataQueryViewId = ViewIdAssignBaseId + 6;
        public const string OperatorDataQueryViewAttachRegion = RegionNames.DocumentRegion;
    }
}
