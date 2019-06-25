using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Sr.SlusglInfoHold.Services
{
    /// <summary>
    /// 本模块的全局事件发布起始Id，3100000 + 71*100, 每个模块均发放100个Id值。
    /// </summary>
    public class EventIdAssign
    {
        /// <summary>
        /// 本模块的全局事件发布起始Id，3100000 + 71*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int EventIdAssignBaseId = 3100000 + 71*100;

        /// <summary>
        /// 单灯 参数请求完毕  发布事件包含一个数据  为 list int 类型的 ，为当前本地更新了设备列表[不包含原来来的最新的控制器列表]
        /// </summary>
        public const int SluSglEquReqOver = EventIdAssignBaseId + 1;

        /// <summary>
        /// 单灯 参数更新   发布事件包含一个数据  为 list int 类型的 
        /// </summary>
        public const int SluSglEquUpdate = EventIdAssignBaseId + 2;

        /// <summary>
        /// 单灯 参数更新   发布事件包含二个数据：第一个为 list int 类型的 单灯控制器参数，第二个为 域地址。
        /// </summary>
        public const int SluSglEquAdd = EventIdAssignBaseId + 3;


        /// <summary>
        /// 单灯 参数更新   发布事件包含一个数据  为 list int 类型的 
        /// </summary>
        public const int SluSglEquDelete = EventIdAssignBaseId + 4;


        /// <summary>
        /// 单灯域信息 参数更新   无参数
        /// </summary>
        public const int SluSglFieldReqOver = EventIdAssignBaseId + 5;

        /// <summary>
        /// 单灯域信息 参数更新   发布事件包含一个数据  为   int 类型:当前更新的区域地址
        /// </summary>
        public const int SluSglFieldUpdate = EventIdAssignBaseId + 6;


        /// <summary>
        /// 单灯域下的分组信息更新   发布事件包含一个数据  为   list int 类型:当前更新了的域列表
        /// </summary>
        public const int SluSglFieldGrpUpdate = EventIdAssignBaseId + 7;

        /// <summary>
        /// 控制器选测数据   发布事件包含一个数据  为   list 类型
        /// </summary>
        public const int SluSglMeasure = EventIdAssignBaseId + 8;

        /// <summary>
        /// NB单灯方案请求
        /// </summary>
        public const int SluSglTimeInfoRequest = EventIdAssignBaseId + 9;

        /// <summary>
        /// NB单灯方案更新
        /// </summary>
        public const int SluSglTimeInfoUpdate = EventIdAssignBaseId + 10;

        /// <summary>
        /// NB单灯方案删除
        /// </summary>
        public const int SluSglTimeInfoDelete = EventIdAssignBaseId + 11;

        /// <summary>
        /// NB单灯状态更新
        /// </summary>
        public const int SluSglRunningInfoUpdate = EventIdAssignBaseId + 12;
    }
}
