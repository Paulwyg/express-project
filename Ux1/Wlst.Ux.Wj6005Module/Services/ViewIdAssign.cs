using Wlst.Cr.CoreOne.Services;

namespace Wlst.Ux.Wj6005Module.Services
{
    public class ViewIdAssign
    {
        /// <summary>
        /// 本模块的视图起始Id，1100000 + 30*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int ViewIdAssignBaseId = 1100000 + 30*100;


        public const int Jd601TmlInfoSetViewIdViewId = ViewIdAssignBaseId + 1;

       
        public const int Jd601EventScheduleViewId = ViewIdAssignBaseId + 2;

        public const int Jd601ControlAndDataViewId = ViewIdAssignBaseId + 3;

       
        
             public const int Jd601ManageViewId = ViewIdAssignBaseId + 4;

        public const string Jd601ManageViewAttachRegion =
           RegionNames.LeftViewRegion ;


        public const int Jd601TmlInfoViewId = ViewIdAssignBaseId + 5;

       
        public const int TmlInfoSetZcForjd601ViewId = ViewIdAssignBaseId + 6;
        
        public const int TmlParmetersInfoSetForJd601ViewId = ViewIdAssignBaseId + 7;
        
        public const int Jd601InstantDataViewId = ViewIdAssignBaseId + 8;
       
        public const int Jd601OperatorControlViewId = ViewIdAssignBaseId + 9;
     
     
            public const int Jd601ManageSettingViewId = ViewIdAssignBaseId + 10;
       

    }
}
