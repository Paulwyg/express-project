using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Sr.TimeTableSystem.Models
{
    public class TodayOpenCloseTime
    {
        public int  TimeTableId;
        public  string TimeTableName;

        //public  bool IsOpenLightUseLux;
        //public  bool IsCloseLightUserLux;

        ///// <summary>
        ///// 开灯时间  如 16:30  则为 16*60+30
        ///// </summary>
        //public List<int> OpenLightTime;

        ///// <summary>
        ///// 关灯时间  如 5:30  则为 5*60+30
        ///// </summary>
        //public List<int> CloseLightTime;

        public TodayOpenCloseTime()
        {
                TimeTableId  = -1;
                TimeTableName  = "";
        }

        public List<Tuple<int, int>> TimeOnOff;
        

    }

    
}
