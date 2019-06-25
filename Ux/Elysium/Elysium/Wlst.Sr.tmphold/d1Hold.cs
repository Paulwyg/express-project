using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Sr.tmphold
{

    public class OnePreFaultItem
    {
        /// <summary> 发生时间  也是记录编号 数据库存在的记录标号</summary>
        public long DateCreate { get; set; }

        /// <summary> 终端序号</summary>
        public int RtuId { get; set; }

        /// <summary> 回路序号</summary>
        public int LoopId { get; set; }

        public int LampId { get; set; }

        /// <summary> 故障序号</summary>
        public int FaultId { get; set; }

        /// <summary> 备注</summary>
        public string Remark { get; set; }

        /// <summary> 发生时间 记录编号 数据库存在的记录标号</summary>
        public long DateRemove { get; set; }

        /// <summary>当前电压</summary>
        public double V { get; set; }

        /// <summary>当前电流</summary>
        public double A { get; set; }

        /// <summary>上限设置值</summary>
        public double AUpper { get; set; }

        /// <summary>下限设置值</summary>
        public double ALower { get; set; }

        /// <summary>额定设置值</summary>
        public double Aeding { get; set; }

    }

    public class d1Hold
    {
        public static List<OnePreFaultItem> Faults = new List<OnePreFaultItem>();

        private static d1Hold myself = null;

        public static d1Hold MySelf
        {
            get
            {
                if (myself == null)
                {
                    myself = new d1Hold();
                    myself.Load();
                }
                return myself;
            }
        }

        private void Load()
        {
            var dt = DateTime.Now.AddDays(-30).Ticks;
            var sp = Environment.CurrentDirectory + "\\data\\1.txt";
            var tmp = FileRead.ReadFile(sp);
            foreach (var f in tmp)
            {
                var pr = sps(f);
                if (pr == null) continue;
                if (pr.DateCreate < dt) continue;
                Faults.Add(pr);
            }
        }

        private static OnePreFaultItem sps(string data)
        {
            try
            {
                var tm = data.Replace("\"", "");
                var sp = tm.Split(',');
                if (sp.Length < 6) return null;
                var rtn = new OnePreFaultItem();
                rtn.DateCreate = Convert.ToInt64(sp[0]);
                rtn.FaultId = Convert.ToInt32(sp[1]);
                rtn.RtuId = Convert.ToInt32(sp[2]);
                rtn.LoopId = Convert.ToInt32(sp[3]);
                rtn.DateRemove = Convert.ToInt64(sp[4]);
                rtn.LampId = Convert.ToInt32(sp[5]);
                rtn.Remark = "";
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
