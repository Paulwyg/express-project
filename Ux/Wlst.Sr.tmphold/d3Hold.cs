using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Sr.tmphold
{
    //节能率
    public class Jnl
    {
        /// <summary> 发生时间  也是记录编号 数据库存在的记录标号</summary>
        public long DateCreate { get; set; }

        /// <summary> 终端序号</summary>
        public int RtuId { get; set; }

        /// <summary> 电量</summary>
        public double Power { get; set; }

        /// <summary> 节能率</summary>
        public double jnl { get; set; }

    }


    public class d3Hold
    {
        public  List<Jnl> Jnls = new List<Jnl>();

        private static d3Hold myself = null;

        public static d3Hold MySelf
        {
            get
            {
                if (myself == null)
                {
                    myself = new d3Hold();
                    myself.Load();
                }
                return myself;
            }
        }

        private void Load()
        {
            //var dt = DateTime.Now.AddDays(-30).Ticks;
            var sp = Environment.CurrentDirectory + "\\tmpdata\\3.txt";
            var tmp = FileRead.ReadFile(sp);
            foreach (var f in tmp)
            {
                var pr = sps(f);
                if (pr == null) continue;
                //if (pr.DateCreate < dt) continue;
                Jnls.Add(pr);
            }
        }

        private static Jnl sps(string data)
        {
            try
            {
                var tm = data.Replace("\"", "");
                var sp = tm.Split(',');
                if (sp.Length < 3) return null;
                var rtn = new Jnl();

                var pss = sp[0].Split('-', ' ');
                int year = Convert.ToInt32(pss[0]);
                int month = Convert.ToInt32(pss[1]);
                int day = Convert.ToInt32(pss[2]);

                rtn.DateCreate = new DateTime(year, month, day, 0, 0, 1).Ticks;
                rtn.RtuId = Convert.ToInt32(sp[1]);
                rtn.jnl = Convert.ToDouble(sp[3]);
                rtn.Power = Convert.ToDouble(sp[2]);
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
