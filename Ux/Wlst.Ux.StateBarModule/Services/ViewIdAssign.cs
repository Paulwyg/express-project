using Wlst.Cr.CoreOne.Services;

namespace Wlst.Ux.StateBarModule.Services
{
    public class ViewIdAssign
    {
        /// <summary>
        /// 本模块的视图起始Id，1100000 + 33*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int ViewIdAssignBaseId = 1100000 + 33*100;


        public const int StateBarViewId = ViewIdAssignBaseId + 1;

        public const string StateBarViewAttachRegion =
            RegionNames.StateBarRegion;



        public const int TopDataViewId = ViewIdAssignBaseId + 2;

        public const string TopDataViewAttachRegion = RegionNames.TopViewRegion;


        public const int OperatorDataQueryViewId = ViewIdAssignBaseId + 3;
       
        public const int NewSvrMsgViewId = ViewIdAssignBaseId + 4;  

        public const string NewSvrMsgViewAttachRegion =
            RegionNames.MsgRegion;

          public const int NewMsgViewBottomViewId = ViewIdAssignBaseId + 14;

          public const int UserOperateMsgViewId = ViewIdAssignBaseId + 15;


        //lvf 2018年5月9日14:32:41 用户开关灯操作
         
          public const int UserOcInfoViewId = ViewIdAssignBaseId + 16;



        public const int ElysiumColorFontViewId = ViewIdAssignBaseId + 5;

        public const int EventMoniterViewId = ViewIdAssignBaseId + 6;

        public const int StateBarTimeViewId = ViewIdAssignBaseId + 7;

        public const string StateBarTimeViewAttachRegion =
            RegionNames.StateBarTimeRegion;


        public const int CommonSettingViewId = ViewIdAssignBaseId + 8;
        
        public const int AreaSetViewId = ViewIdAssignBaseId + 9;
        
        public const int DataAreaControllerViewId = ViewIdAssignBaseId + 10;
        public const string DataAreaControllerAttachRegion = RegionNames.DataRegion;


        public const int TalkViewId = ViewIdAssignBaseId + 11;
        public const string TalkAttachRegion = RegionNames.TalkRegion;


        public const int FlashPlayerViewId = ViewIdAssignBaseId + 12;
       
        public const int VideoViewId = ViewIdAssignBaseId + 13;


        // lvf 2019年5月30日11:13:00 下发失败查询界面
        public const int SendFailOperationViewId = ViewIdAssignBaseId + 17;

    }
}
