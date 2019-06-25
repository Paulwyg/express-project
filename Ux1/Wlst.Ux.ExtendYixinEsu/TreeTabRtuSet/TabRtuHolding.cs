using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Ux.ExtendYixinEsu.TreeTabRtuSet
{
    public class TabRtuHolding
    {
        public static Dictionary<int, TabRtuInfo> Info = new Dictionary<int, TabRtuInfo>();



        public static void OnUpdaetAll()
        {
            var dl = new List<int>();
            foreach (var f in Info) if (f.Value.GrpOrRtus.Count == 0) dl.Add(f.Key);
            foreach (var f in dl) if (Info.ContainsKey(f)) Info.Remove(f);
            TreeTabVmx.vms.vms.TreeSingleViewModel.ReloadAllOnAreaDeArri();
        }

        public static List<int> GetRtuLstByIdx(int id)
        {
            var rtn = new List<int>();
            if (!Info.ContainsKey(id)) return rtn;
            var tmp = Info[id];
            foreach (var f in tmp.GrpOrRtus)
            {
                if (f > 1000000) rtn.Add(f);
                rtn.AddRange(Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGrpTmlList(0,f));
                //rtn.AddRange(Wlst.Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GetGrpTmlList(f));
            }
            return rtn;
        }

        public static List<int> GetRtuLstSpecialByIdx(int id)
        {
            var rtn = new List<int>();
            if (!Info.ContainsKey(id)) return rtn;
            var tmp = Info[id];
            foreach (var f in tmp.GrpOrRtus)
            {
                if (f > 1000000) rtn.Add(f);
                // rtn.AddRange(Wlst.Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GetGrpTmlList(f));
            }

            var or = GetAllGrpNormalRtuLst();
            return (from t in rtn where !or.Contains(t) select t).ToList();
            return rtn;
        }

        private static List<int> GetAllGrpNormalRtuLst()
        {
            var rtn = new List<int>();
            //lvf 2018年4月17日14:37:13 默认区域id为0
            var tu = new Tuple<int, int>(0, 0);

            //if (Wlst.Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups.ContainsKey(0))
            //{
            //    foreach (var f in
            //        Wlst.Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups[0].LstGrp)
            //        rtn.AddRange(Wlst.Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GetGrpTmlList(f));
            //}

            foreach (var f in Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups)
            {
                rtn.AddRange(f.Value.LstTml);
            }
            return rtn;
        }


        public static List<int> GetGrpLstByIdx(int id)
        {
            var rtn = new List<int>();
            if (!Info.ContainsKey(id)) return rtn;
            var tmp = Info[id];
            foreach (var f in tmp.GrpOrRtus)
            {
                if (f < 1000000) rtn.Add(f);
                // rtn.AddRange(Wlst.Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GetGrpTmlList(f));
            }
            return rtn;
        }
    }


    public class TabRtuInfo
    {
        public int Id;
        public string Name;
        public List<int> GrpOrRtus;

        public TabRtuInfo()
        {
            GrpOrRtus = new List<int>();
        }
    }
}
