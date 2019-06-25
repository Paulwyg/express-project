namespace Wlst.Ux.Wj6005Module.Services
{
    public class MenuIdAssgin
    {
        /// <summary>
        /// 本模块的菜单起始Id，2100000 + 30*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int MenuIdAssignBaseId = 2100000 + 30*100;


        public const int NavToJd601TmlInfoSetId = MenuIdAssignBaseId + 1;

        //public const int NavToWj1080ManageViewId = MenuIdAssignBaseId + 2;

        //public const int NavToEquipmentFaultWithTmlSettingViewforMainMenuId = MenuIdAssignBaseId + 3;

        public const int EventSchduleNavTaskJd601EventTaskInstanceInfoViewModelId = MenuIdAssignBaseId + 2;

        public const  int NavToJd601ControlAndDataId = MenuIdAssignBaseId + 3;

        public const int NavToJd601ManageViewId = MenuIdAssignBaseId + 4;


        public const int NavLineJd601ManageSettingView = MenuIdAssignBaseId + 5;
    }
}
