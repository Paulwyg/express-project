using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Ux.Wj2090Module.Services
{

    /// <summary>
    /// 本模块的全局事件发布起始Id，3100000 + 60*100, 每个模块均发放100个Id值。
    /// </summary>
    public class EventIdAssign
    {
        /// <summary>
        /// 本模块的全局事件发布起始Id，3100000 + 60*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int EventIdAssignBaseId = 3100000 + 60*100;

        /// <summary>
        /// 请求的时间表达到;
        /// <para> 无参数</para>
        /// </summary>
        public const int TimeInfoRequestId = EventIdAssignBaseId + 1;

        /// <summary>
        /// 时间表信息进行了修改;
        /// <para> 无参数</para>
        /// </summary>
        public const int TimeInfoUpdateId = EventIdAssignBaseId + 2;



        /// <summary>
        /// 请求的控制器分组到达;
        /// <para> 无参数</para>
        /// </summary>
        public const int GrpConInfoRequestId = EventIdAssignBaseId + 3;

        /// <summary>
        /// 控制器分组更新  一个参数 集中器地址
        /// <para> 一个参数 集中器地址</para>
        /// </summary>
        public const int GrpConInfoUpdateId = EventIdAssignBaseId + 4;

        ///// <summary>
        ///// 集中控制器最新数据到达  带有二个参数 1：int 集中器地址，2：list《int》 包含更新的内容指示；列表  1 集中器数据，2 未知控制器，4 控制器物理信息，5 控制器基本数据，6 控制辅助数据
        ///// </summary>
        //public const int OnSluNewDataArrive = EventIdAssignBaseId + 5;
        ///// <summary>
        ///// 控制器基本数据达到  带有一个参数List《tuple《int，int》》  tuple中第一个地址为集中器地址 第二个地址为控制器地址
        ///// </summary>
        //public const int OnSluCtrlNewDataArrive = EventIdAssignBaseId + 6;

        /// <summary>
        /// 请求的集中器分组到达;
        /// <para> 无参数</para>
        /// </summary>
        public const int GrpSluInfoRequestId = EventIdAssignBaseId +7;

        /// <summary>
        /// 集中器分组更新;
        /// <para> 无参数</para>
        /// </summary>
        public const int GrpSluInfoUpdateId = EventIdAssignBaseId + 8;
    }
}
