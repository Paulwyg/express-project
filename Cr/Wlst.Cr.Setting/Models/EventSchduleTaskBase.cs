using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Cr.Setting.Interfaces;

namespace Wlst.Cr.Setting.Models
{
    public class EventSchduleTaskBase : IIEventSchduleTask
    {
        //public EventSchduleTaskBase ()
        //{
        //    MaxAllowEvent = 0;
        //    EventSchduleDescription = "No Description";
        //    EventSchduleClassId = -1;
        //    EventSchduleName = "Not Set";
        //    EventSchduleViewId = -1;
        //}

        public EventSchduleTaskBase(IIEventSchduleTask info)
        {
            this.MaxAllowEvent = info.MaxAllowEvent;
            this.EventSchduleDescription = info.EventSchduleDescription;
            this.EventSchduleClassId = info.EventSchduleClassId;
            this.EventSchduleName = info.EventSchduleName;
            this.EventSchduleViewId = info.EventSchduleViewId;

        }

        public IIEventSchduleTask BackToEventSchduleTask()
        {
            return this;
        }

        /// <summary>
        /// 最多允许新建的任务数
        /// </summary>
        public int MaxAllowEvent { get; set; }

        /// <summary>
        /// 任务唯一标示码
        /// </summary>
        public int EventSchduleClassId { get; set; }

        /// <summary>
        /// 任务类名称
        /// </summary>
        public string EventSchduleName { get; set; }

        /// <summary>
        /// 任务类描述
        /// </summary>
        public string EventSchduleDescription { get; set; }

        /// <summary>
        /// 当前描述的任务 所在的设置界面Id
        /// </summary>
        public int EventSchduleViewId { get; set; }

        public string TitleShow { get; set; }
    }
}
