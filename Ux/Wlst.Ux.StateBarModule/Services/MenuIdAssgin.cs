namespace Wlst.Ux.StateBarModule.Services
{
    public class MenuIdAssgin
    {
        /// <summary>
        /// 本模块的菜单起始Id，2100000 + 33*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int MenuIdAssignBaseId = 2100000 + 33*100;



        public const int NavToOperatorDataQueryViewId = MenuIdAssignBaseId + 1;

        public const int NavLineElysiumColorFontViewId = MenuIdAssignBaseId + 2;

        public const  int NavToEventMoniterViewId = MenuIdAssignBaseId + 3;
        public const int NavLineCommonSettingViewId = MenuIdAssignBaseId + 4;
        public const int NavLineAreaSetId = MenuIdAssignBaseId + 5;

        public const int NavToFlashPlayerViewId = MenuIdAssignBaseId + 6;
        public const int NavToVideoViewId = MenuIdAssignBaseId + 7;

        //lvf 2018年7月4日16:10:51 应急操作中心
        public const int NavToEmergencyOperationCenterViewId = MenuIdAssignBaseId + 8;

        //lvf 2019年5月30日11:17:47 下发失败查询界面
        public const int NavToSendFailOperationViewId = MenuIdAssignBaseId +9;

    }
}
