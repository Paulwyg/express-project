using Wlst.Cr.CoreOne.Services;

namespace Wlst.Ux.Wj9001Module.Services
{
    public class ViewIdAssign
    {
        /// <summary>
        /// 本模块的视图起始Id，1100000 + 66*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int ViewIdAssignBaseId = 1100000 + 66*100;


        public const int Wj9001TreeViewId = ViewIdAssignBaseId + 1;

        public const int Wj9001ManageSettingViewId = ViewIdAssignBaseId + 2;

        public const int Wj9001ParaSetViewId = ViewIdAssignBaseId + 3;
        public const string Wj9001ParaSetViewAttachRegion =
       RegionNames.LeftViewRegion;


        public const int NewDataViewId = ViewIdAssignBaseId + 4;
        public const string NewDataViewAttachRegion =
            RegionNames.DataRegion;

        public const int Wj9001DataQueryViewId = ViewIdAssignBaseId + 5;
    }
}
