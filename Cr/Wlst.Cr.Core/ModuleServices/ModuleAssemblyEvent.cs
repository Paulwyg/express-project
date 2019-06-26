using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Cr.Core.ModuleServices
{


    /// <summary>
    /// 模块加载卸载发布事件
    /// 本模块的全局事件发布起始Id，3100000 + 1*100, 每个模块均发放100个Id值。
    /// </summary>
    public class ModuleAssemblyEvent
    {
        /// <summary>
        /// 本模块的全局事件发布起始Id，3100000 + 1*100, 每个模块均发放100个Id值。
        /// </summary>
        private  const int EventIdAssignBaseId = 3100000 + 1*100;

        /// <summary>
        /// 加载模块事件 PublishEventType.Core 发布参数为 第一个： AssemblyConfig 第二个：加载成功与否 bool
        /// </summary>
        public const int ModuleLoadedEvent = EventIdAssignBaseId + 1;


        /// <summary>
        /// 卸载模块事件 PublishEventType.Core 发布参数为 第一个： AssemblyConfig 第二个：卸载成功与否 bool
        /// </summary>
        public const int ModuleUnLoadedEvent = EventIdAssignBaseId + 2;


        /// <summary>
        /// 加载模块事件 PublishEventType.Core 发布参数为 AssemblyConfig 
        /// </summary>
        public const int ModulePreLoadEvent = EventIdAssignBaseId + 3;


        /// <summary>
        /// 卸载模块事件 PublishEventType.Core 发布参数为 AssemblyConfig 
        /// </summary>
        public const int ModulePreUnLoadEvent = EventIdAssignBaseId + 4;
    }
}
