using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Cr.Setting.Interfaces
{
    /// <summary>
    /// 任务泛型类定义  总体的
    /// </summary>
    public interface IIEventSchduleTask
    {
        /// <summary>
        /// 最多允许新建的任务数
        /// </summary>
        int MaxAllowEvent { get; set; }

        /// <summary>
        /// 任务唯一标示码
        /// </summary>
        int  EventSchduleClassId { get; set; }

        /// <summary>
        /// 任务类名称
        /// </summary>
        string EventSchduleName { get; set; }

        /// <summary>
        /// 任务类描述
        /// </summary>
        string EventSchduleDescription { get; set; }

        /// <summary>
        /// 当前描述的任务 所在的设置界面Id
        /// </summary>
        int EventSchduleViewId { get; set; }

        /// <summary>
        /// 用于显示的页面标题
        /// </summary>
        string TitleShow { get; set; }
    }
}
