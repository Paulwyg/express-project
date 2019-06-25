using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Ux.Wj2090Module.Services
{
   public  class MenuIdAssign
    {
        /// <summary>
        /// 本模块的菜单起始Id，2100000 + 60*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int MenuIdAssignBaseId = 2100000 + 60*100 ;
        public const int MenuIdAssignBaseIdAdd = 2100000 + 560 * 100;

       public const int MeasureControllerForCtrIdBase = MenuIdAssignBaseId + 2;
       public const int MeasureControllerForMenuId = MenuIdAssignBaseId + 1;

       //public const int ReMeasureId = MenuIdAssignBaseId + 8;
       //public const int SetSluWorksId = MenuIdAssignBaseId + 9;
       //public const int SetSluParasId = MenuIdAssignBaseId + 10;
       //public const int ResetNetWorkOneId = MenuIdAssignBaseId + 3;
       //public const int ResetNetWokrkOneId = MenuIdAssignBaseId + 4;
       //public const int ResetNetWorkgOneId = MenuIdAssignBaseId + 5;
       //public const int ResetNetWorksneId = MenuIdAssignBaseId + 6;

       //public const int ZcDomainChangeInfoId = MenuIdAssignBaseId + 11;

       public const int MeasureControllerForCtrIdFz = MenuIdAssignBaseId + 12;
       public const int MeasureControllerForMenuIdNotRgCt = MenuIdAssignBaseId + 13;
       public const int MeasureControllerForCtrPhy = MenuIdAssignBaseId + 14;


       public const int ZcSluTimeInfo = MenuIdAssignBaseId + 15;

       public const int ZcAllConnsInfoId = MenuIdAssignBaseId + 16;

       public const int NavTaskPartolSluEventScheduleViewId = MenuIdAssignBaseId + 20;

    //   public const int Partol2090SluScheduleId = MenuIdAssignBaseId + 20;


       public const int NavToTimeInfoSetViewId = MenuIdAssignBaseId + 21;

       public const int NavToWj2090SluInfoSetMenuId = MenuIdAssignBaseId + 22;

       public const int NavToWeekSetQueryViewId = MenuIdAssignBaseId + 23;

       public const int NavToWj2090GrpManager = MenuIdAssignBaseId + 24;

       public const int NavLineWj2090TreeSetId = MenuIdAssignBaseId + 25;

       public const int ConcentratorDataQueryViewMenuId = MenuIdAssignBaseId + 26;

       public const int ControlDataQueryViewMenuId = MenuIdAssignBaseId + 27;

       public const int NavToConcentratorDataQuerySluCtrlId = MenuIdAssignBaseId + 28;

       public const int NavToConcentratorDataQuerySluId = MenuIdAssignBaseId + 29;

   
    

       public const int NavToPartolsViewId = MenuIdAssignBaseId + 30;

       public const int NavToOrdersViewId = MenuIdAssignBaseId + 31;

       public const int TimeInfoQueryViewId = MenuIdAssignBaseId + 32;

       public const int NavToDataMiningViewId = MenuIdAssignBaseId + 33;


       public const int NavToWj2090GrpManagerDelayId = MenuIdAssignBaseId + 34;
       public const int NavToOrdersGrpViewId = MenuIdAssignBaseId + 35;


       //40到99划归 RightOperatorsBase40 
       public const int RightOperatorsBase40 = MenuIdAssignBaseId + 40;



       //后续21个均为单灯控制器右键菜单 到22
       public const int CtrlRightOperatorZcParasId = MenuIdAssignBaseIdAdd + 1; //~22


       //23到73划归 RightOperatorHunheSluBaseId 下一个为74
       public const int RightOperatorHunheSluBaseId = MenuIdAssignBaseIdAdd + 23;


       //74到89划归 RightOperatorHunheSluBaseId 下一个为90
       public const int RightOperatorHunheCtrlBaseId = MenuIdAssignBaseIdAdd + 74;

       public const int NavToDataMining2ViewId = MenuIdAssignBaseId + 36;
       
    

    }
}
