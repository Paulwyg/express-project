using Wlst.Cr.CoreOne.Services;

namespace Wlst.Ux.EquipemntLightFault.Services
{
    public class ViewIdAssign
    {
        /// <summary>
        /// 本模块的视图起始Id，1100000 + 36*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int ViewIdAssignBaseId = 1100000 + 36*100;


        public const int EquipmentFaultDefineSettingViewId = ViewIdAssignBaseId + 1;

       
        public const int EquipmentFaultOnTabViewId = ViewIdAssignBaseId + 2;

        public const string EquipmentFaultOnTabViewAttachRegion =
            RegionNames.MsgRegion;

        public const int EquipmentFaultRecordQueryViewId = ViewIdAssignBaseId + 3;

       
        public const int EquipmentFaultWithTmlSettingViewId = ViewIdAssignBaseId + 4;

      
        public const int UserIndividuationFaultSettingViewId = ViewIdAssignBaseId + 5;

     

        public const int FaultDefineSettingManagViewId = ViewIdAssignBaseId + 6;
       
        public const int EquipmentAllFaultOnTabViewId = ViewIdAssignBaseId + 7;

        

        public const int EquipmentFaultWithTmlAlarmMsgViewId = ViewIdAssignBaseId + 8;

      
        public const int RecordMsgViewId = ViewIdAssignBaseId + 9;



        public const int EquipmentTmlMobileViewId = ViewIdAssignBaseId + 10;
        
        public const int RtuAmpSxxViewId = ViewIdAssignBaseId + 11;

        public const int RapidSetRtuAmpViewId = ViewIdAssignBaseId + 12;

        public const int CurrentEquipmentFaultViewId = ViewIdAssignBaseId + 15;

        public const int UserFaultSettingByAdminViewId = ViewIdAssignBaseId +16;


        public const int EquipmentFaultRecordQuerySzViewId = ViewIdAssignBaseId + 17;

        public const int RtuAmpSxxViewNewId = ViewIdAssignBaseId + 18;

        public const int NewRuleSectionViewId = ViewIdAssignBaseId + 19;

        public const int EquipmentFaultManageSettingViewId = ViewIdAssignBaseId +21;

        public const int EquipmentFaultWithTmlPriorityLevelSettingViewId = ViewIdAssignBaseId + 22;

        public const int SendOrderViewId = ViewIdAssignBaseId + 25;

        //武汉故障查询
        public const int EquipmentFaultRecordQueryLevelLowViewId = ViewIdAssignBaseId + 26;

        //武汉火零不平衡查询
        public const int EquipmentFaultRecordQueryLnViewId = ViewIdAssignBaseId + 27;

        //sb武汉火零不平衡故障设置界面  lvf 2018年6月28日10:46:21
        public const int EquipmentFaultManageSettingLnViewId = ViewIdAssignBaseId + 28;


        

    }
}
