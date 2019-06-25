using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Sr.tmphold
{
    //亮灯率
    public class Ldl
    {
        /// <summary> 发生时间  也是记录编号 数据库存在的记录标号</summary>
        public long DateCreate { get; set; }

        /// <summary> 终端序号</summary>
        public int RtuId { get; set; }

        /// <summary> 总灯头</summary>
        public int Sum { get; set; }

        /// <summary> 亮灯数</summary>
        public int Lds { get; set; }

        /// <summary> 亮灯率</summary>
        public double ldl { get; set; }

    }


    public class d4Hold
    {
        public  List<Ldl> Ldls = new List<Ldl>();

        private static d4Hold myself = null;

        public static d4Hold MySelf
        {
            get
            {
                if (myself == null)
                {
                    myself = new d4Hold();
                    myself.Load();
                }
                return myself;
            }
        }

        private void Load()
        {
            //var dt = DateTime.Now.AddDays(-30).Ticks;
            var sp = Environment.CurrentDirectory + "\\tmpdata\\4.txt";
            var tmp = FileRead.ReadFile(sp);
            foreach (var f in tmp)
            {
                var pr = sps(f);
                if (pr == null) continue;
                //if (pr.DateCreate < dt) continue;
                Ldls.Add(pr);
            }
        }

        private static Ldl sps(string data)
        {
            try
            {
                var tm = data.Replace("\"", "");
                var sp = tm.Split(',');
                if (sp.Length < 3) return null;
                var rtn = new Ldl();

                var pss = sp[0].Split('-', ' ');
                int year = Convert.ToInt32(pss[0]);
                int month = Convert.ToInt32(pss[1]);
                int day = Convert.ToInt32(pss[2]);

                rtn.DateCreate = new DateTime(year, month, day, 0, 0, 1).Ticks;
                rtn.RtuId = Convert.ToInt32(sp[1]);
                rtn.Sum = Convert.ToInt32(sp[2]);
                rtn.Lds = Convert.ToInt32(sp[3]);

                rtn.ldl = Convert.ToDouble(sp[4]);
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
