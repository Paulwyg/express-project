using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Sr.tmphold
{


    public class Elec
    {
        /// <summary> 发生时间  也是记录编号 数据库存在的记录标号</summary>
        public long DateCreate { get; set; }

        /// <summary> 终端序号</summary>
        public int RtuId { get; set; }

        /// <summary> 回路序号</summary>
        public int LoopId { get; set; }

        /// <summary> 电量</summary>
        public double  Power { get; set; }

    }


   public class d2Hold
   {
       public   List<Elec> Elecs = new List<Elec>();

       private static d2Hold myself = null;

       public static d2Hold MySelf
       {
           get
           {
               if (myself == null)
               {
                   myself = new d2Hold();
                   myself.Load();
               }
               return myself;
           }
       }

       private void Load()
       {
           //var dt = DateTime.Now.AddDays(-30).Ticks;
           var sp = Environment.CurrentDirectory + "\\tmpdata\\2.txt";
           var tmp = FileRead.ReadFile(sp);
           foreach (var f in tmp)
           {
               var pr = sps(f);
               if (pr == null) continue;
               //if (pr.DateCreate < dt) continue;
               Elecs.Add(pr);
           }
       }

       private static Elec sps(string data)
       {
           try
           {
               var tm = data.Replace("\"", "");
               var sp = tm.Split(',');
               if (sp.Length < 5) return null;
               var rtn = new Elec();
               rtn.DateCreate = Convert.ToInt64(sp[0]);
               rtn.RtuId = Convert.ToInt32(sp[1]);
               rtn.LoopId = Convert.ToInt32(sp[2]);
               rtn.Power  = Convert.ToDouble( sp[5]);
               return rtn;
           }
           catch (Exception ex)
           {
               return null;
           }
           return null;
       }
    }
}
