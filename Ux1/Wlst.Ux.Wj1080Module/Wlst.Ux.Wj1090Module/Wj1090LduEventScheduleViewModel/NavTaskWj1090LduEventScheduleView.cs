using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.Setting.Interfaces;

namespace Wlst.Ux.Wj1090Module.Wj1090LduEventScheduleViewModel
{
    [Export(typeof(IIEventSchduleTask))]
    [PartCreationPolicy(CreationPolicy.Shared)]
   public  class NavTaskWj1090LduEventScheduleView:Wlst .Cr .Setting .Interfaces .IIEventSchduleTask 
    {
        public NavTaskWj1090LduEventScheduleView()
       {
           this.MaxAllowEvent = 2;
           this.EventSchduleId = Wj1090Module.Services.MenuIdAssgin.EventSchduleNavTaskWj1090LduEventScheduleViewId; ;
           this.EventSchduleName = "线路检测巡测任务";
           this.EventSchduleDescription = "设置系统定时巡测线路检测数据";
           this.EventSchduleViewId = Wj1090Module.Services.ViewIdAssign.Wj1090LduEventScheduleViewId;
       }

        /// <summary>
        /// 最多允许新建的任务数
        /// </summary>
        public int MaxAllowEvent { get; set; }

        /// <summary>
        /// 任务唯一标示码
        /// </summary>
        public int EventSchduleId { get; set; }

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
    }
}
