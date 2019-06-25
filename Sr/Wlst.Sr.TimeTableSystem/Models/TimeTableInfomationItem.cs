using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.client;

namespace Wlst.Sr.TimeTableSystem.Models
{

    ///// <summary>一张时间表中一个规则的开关灯的信息</summary>
    //public partial class TimeTableOneDayInfomationItem
    //{
    //    /// <summary>
    //    /// 起始月日如 101 表示1月1日
    //    /// </summary>
    //    public int DateStart { get; set; }

    //    /// <summary>
    //    /// 结束月日 如1231 表示12月31日
    //    /// </summary>
    //    public int DateEnd { get; set; }

    //    /// <summary>
    //    /// 时间段序号 1-4
    //    /// </summary>
    //    public int TimetableSectionId { get; set; }

    //    /// <summary>
    //    /// 分段中的规则序号  如第1个规则
    //    /// </summary>
    //    public int RuleId { get; set; }

    //    /// <summary>
    //    /// 规则列表  如第1个规则中的第一选项 可能包含7个，一周每天不一样
    //    /// </summary>
    //    public int RuleSectionId { get; set; }

    //    /// <summary>
    //    /// 1、光控，2、偏移，3、定时
    //    /// </summary>
    //    public int TypeOn { get; set; }

    //    /// <summary>
    //    /// 1、光控，2、偏移，3、定时
    //    /// </summary>
    //    public int TypeOff { get; set; }

    //    /// <summary>
    //    /// 开灯最后时限
    //    /// </summary>
    //    public int TimeOn { get; set; }

    //    /// <summary>
    //    /// 关灯最后时限
    //    /// </summary>
    //    public int TimeOff { get; set; }

    //    /// <summary>
    //    /// 本规则那几天使用 0为周日  1-6 表示星期几  可多选，但同一分段规则中的多段不能包含同一天
    //    /// </summary>
    //    public int DayOfWeekUsed { get; set; }

    //}

    ///// <summary>一张时间表的信息</summary>
    //public partial class TimeTableInfomationItem
    //{
    //    /// <summary>
    //    /// 时间表ID
    //    /// </summary>
    //    public int TimeId { get; set; }

    //    /// <summary>
    //    /// 时间表名称
    //    /// </summary>
    //    public string TimeName { get; set; }

    //    /// <summary>
    //    /// 时间表描述
    //    /// </summary>
    //    public string TimeDesc { get; set; }

    //    /// <summary>
    //    /// 该时间表使用的光控探头ID
    //    /// </summary>
    //    public int LuxId { get; set; }

    //    /// <summary>
    //    /// 该时间表若是使用光控则开灯光照度值
    //    /// </summary>
    //    public int LuxOnValue { get; set; }

    //    /// <summary>
    //    /// 该时间表若是使用光照度关灯 则关灯光照度值
    //    /// </summary>
    //    public int LuxOffValue { get; set; }

    //    /// <summary>
    //    /// 如果该时间表使用偏移  则开灯偏移值 0不启用偏移 定时； 不为0则启用偏移  根据日出日落计算
    //    /// </summary>
    //    public int LightOnOffset { get; set; }

    //    /// <summary>
    //    /// 如果该时间表使用偏移 则关灯偏移值为；0不启用偏移 定时；不为0则启用偏移  根据日出日落计算
    //    /// </summary>
    //    public int LightOffOffset { get; set; }

    //    /// <summary>
    //    /// 光控有效值
    //    /// </summary>
    //    public int LuxEffective { get; set; }

    //    /// <summary>
    //    /// 一周开关灯规则
    //    /// </summary>
    //    public List<TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem.TimeTableSectionRule> RuleItems { get; set; }

    //    /// <summary>所有规则按星期拆分</summary>
    //    public List<TimeTableOneDayInfomationItem> LstRuleItemsForDay { get; set; }


    //}

}
