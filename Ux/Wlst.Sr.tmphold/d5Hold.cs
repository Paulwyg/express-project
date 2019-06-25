using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Sr.tmphold
{
    //在线率
    public class Zxl
    {
        /// <summary> 发生时间  也是记录编号 数据库存在的记录标号</summary>
        public long DateCreate { get; set; }

        /// <summary> 终端序号</summary>
        public int RtuId { get; set; }

        /// <summary> 总灯头</summary>
        public int Sum { get; set; }

        /// <summary> 在线数</summary>
        public int Zxs { get; set; }

        /// <summary> 在线率</summary>
        public double zxl { get; set; }

    }


    public class d5Hold
    {
        public  List<Zxl> Zxls = new List<Zxl>();

        private static d5Hold myself = null;

        public static d5Hold MySelf
        {
            get
            {
                if (myself == null)
                {
                    myself = new d5Hold();
                    myself.Load();
                }
                return myself;
            }
        }

        private void Load()
        {
            //var dt = DateTime.Now.AddDays(-30).Ticks;
            var sp = Environment.CurrentDirectory + "\\tmpdata\\5.txt";
            var tmp = FileRead.ReadFile(sp);
            foreach (var f in tmp)
            {
                var pr = sps(f);
                if (pr == null) continue;
                //if (pr.DateCreate < dt) continue;
                Zxls.Add(pr);
            }
        }

        private static Zxl sps(string data)
        {
            try
            {
                var tm = data.Replace("\"", "");
                var sp = tm.Split(',');
                if (sp.Length < 3) return null;
                var rtn = new Zxl();

                var pss = sp[0].Split('-', ' ');
                int year = Convert.ToInt32(pss[0]);
                int month = Convert.ToInt32(pss[1]);
                int day = Convert.ToInt32(pss[2]);

                rtn.DateCreate = new DateTime(year, month, day, 0, 0, 1).Ticks;
                rtn.RtuId = Convert.ToInt32(sp[1]);
                rtn.Sum = Convert.ToInt32(sp[2]);
                rtn.Zxs  = Convert.ToInt32(sp[3]);

                rtn.zxl  = Convert.ToDouble(sp[4]);
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
