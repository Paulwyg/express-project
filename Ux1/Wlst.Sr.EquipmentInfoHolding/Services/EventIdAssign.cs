namespace Wlst.Sr.EquipmentInfoHolding.Services
{
    /// <summary>
    /// 本模块的全局事件发布起始Id，3100000 + 21*100, 每个模块均发放100个Id值。
    /// </summary>
    public class EventIdAssign
    {
        /// <summary>
        /// 本模块的全局事件发布起始Id，3100000 + 21*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int EventIdAssignBaseId = 3100000 + 21 * 100;
        /// <summary>
        /// 主设备增加;
        /// <para> 对外发布增加事件时需要携带增加的设备逻辑地址与父设备地址列表为第一参数,List《tupe《增加设备地址，父设备地址》》</para>
        /// </summary>
        public const int EquipmentAddEventId = EventIdAssignBaseId+1;

        /// <summary>
        /// 主设备删除;
        /// <para> 对外发布增加事件时需要携带增加的设备逻辑地址与父设备地址列表为第一参数,List《tupe《删除设备地址，父设备地址》》</para>
        /// </summary>
        public const int EquipmentDeleteEventId = EventIdAssignBaseId + 2;


        /// <summary>
        /// 主设备信息更新;
        /// <para>对外发布增加事件时需要携带增加的设备逻辑地址与父设备地址列表为第一参数,List《tupe《更新设备地址，父设备地址》》</para>
        /// </summary>
        public const int EquipmentUpdateEventId = EventIdAssignBaseId + 3;


        /// <summary>
        /// 请求主设备
        /// </summary>
        public const int EquipmentRequest = EventIdAssignBaseId + 4;



        /// <summary>
        /// 请求住设备列表
        /// </summary>
        public const int EquipentInfoListRequest = EventIdAssignBaseId + 10;




        ///// <summary>
        ///// 终端运行状态信息发生变化
        ///// <para>逻辑地址列表 </para>
        ///// </summary>
        //public const int TmlRunningInfoChange = EventIdAssignBaseId + 9;


        /// <summary>
        /// 主设备增加;
        /// <para> 对外发布增加事件时需要携带增加的设备逻辑地址与父设备地址列表为第一参数,List《tupe《增加设备地址，父设备地址》》</para>
        /// </summary>
        public const int EquipmentUserAddEventId = EventIdAssignBaseId + 20;





        /// <summary>
        /// 设备被选中 SelectedInfo
        /// </summary>
        public const int EquipmentSelected = EventIdAssignBaseId + 21;

        /// <summary>
        /// 主界面中多终端树被选中  SelectedInfo
        /// </summary>
        public const int GroupSelected = EventIdAssignBaseId + 22;


        /// <summary>
        /// 运行状态数据变化
        /// <para>终端逻辑地址列表 </para>
        /// </summary>
        public const int RunningInfoUpdate1 = EventIdAssignBaseId + 24;

        /// <summary>
        /// 最新数据变化
        /// <para>终端逻辑地址列表 </para>
        /// </summary>
        public const int RunningInfoUpdate2 = EventIdAssignBaseId + 25;
        ///// <summary>
        ///// 
        ///// </summary>
        //public const int RtuOnLineRequest = EventIdAssignBaseId + 25;


        /// <summary>
        /// 数据查询时需要在tab面板显示 数据 参数只有一个  TmlNewData
        /// </summary>
        public const int RtuDataQueryDataInfoNeedShowInTab = EventIdAssignBaseId + 26;

        ///// <summary>
        ///// 终端是否亮灯状态变化  第一个参数为设备地址，第二个参数是否亮灯 true 亮灯 ，false 未亮灯
        ///// </summary>
        //public const int RtuLightHasElectricStatesChanged = EventIdAssignBaseId + 27;

        /// <summary>
        ///设备地图坐标更新事件
        /// </summary>
        public const int EquipentxyPositonUpdateId = EventIdAssignBaseId + 28;


        ///// <summary>
        ///// 终端在线列表变更 参数为列表 List《int》
        ///// </summary>
        //public const int RtuOnLineInfoChanged = EventIdAssignBaseId + 29;


        /// <summary>
        /// 终端数中分组选中 希望地图联动只显示选中点 参数为列表 List《int》   若列表中为空 则为全部
        /// </summary>
        public const int RtuGroupSelectdWantedMapUp = EventIdAssignBaseId + 30;

        /// <summary>
        /// 当区域信息发生变化的时候
        /// </summary>
        public const int AreaInfoChanged = EventIdAssignBaseId + 31;

        ///// <summary>
        ///// 主界面筛选后发布的事件  一个参数为List《int》
        ///// </summary>
        //public const int TmlFIltes = EventIdAssignBaseId + 31;


        ///// <summary>
        ///// 集中控制器最新数据到达  带有二个参数 1：int 集中器地址，2：list《int》 包含更新的内容指示；列表  1 集中器数据，2 未知控制器，4 控制器物理信息，5 控制器基本数据，6 控制辅助数据
        ///// </summary>
        //public const int OnSluNewDataArrive = EventIdAssignBaseId + 32;

        /// <summary>
        ///  更新分组 无参数
        /// </summary>
        public const int SingleInfoGroupAllNeedUpdate = EventIdAssignBaseId + 33;

        /// <summary>
        ///  更新分组 无参数
        /// </summary>
        public const int MulityInfoGroupAllNeedUpdate = EventIdAssignBaseId + 34;

        /// <summary>
        ///  最新添加的区域中的设备 参数为《新添加终端Id列表，已设置过的终端id列表》
        /// </summary>
        public const int RequestNewRtuInAreas = EventIdAssignBaseId + 35;

        /// <summary>
        ///  更新计划任务
        /// </summary>
        public const int EventTaskAllNeedUpdate = EventIdAssignBaseId + 36;

        /// <summary>
        ///  区域更新导致分组更新，请求刷新
        /// </summary>
        public const int SingleNeedRefresh = EventIdAssignBaseId + 37;

        /// <summary>
        /// 电子地图需更新图标
        /// </summary>
        public const int MapNeedChangeIcon = EventIdAssignBaseId + 38;

        /// <summary>
        /// 用户操作
        /// </summary>
        public const int UserOperateRtu = EventIdAssignBaseId + 39;



        /// <summary>
        /// 设备被选中 专用于多个终端被选择      args.AddParams(rtus)  一个参数 为list《int》 类型
        /// </summary>
        public const int EquipmentMulSelected = EventIdAssignBaseId + 40;



        public const int AreaEmedataUpdate = EventIdAssignBaseId + 41;


        /// <summary>
        /// 单灯状态变化
        /// </summary>
        public const int RunningInfoUpdate3 = EventIdAssignBaseId + 42;


        /// <summary>
        /// 终端状态变化
        /// </summary>
        public const int RunningInfoUpdate4 = EventIdAssignBaseId + 43;



        /// <summary>
        /// 交叉分组中终端需要更新  lvf 2019年5月7日14:40:21
        /// </summary>
        public const int RtuRegionNeedUpdate = EventIdAssignBaseId + 44;


        /// <summary>
        /// 交叉分组结构需要  lvf 2019年5月7日14:40:21
        /// </summary>
        public const int RegionNeedUpdate = EventIdAssignBaseId + 45;

    }
}