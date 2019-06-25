using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Ux.Wj1090Module.NewData.Services
{
    public class DataHolding
    {
        public static ConcurrentDictionary<Tuple<int, int>, Wlst.client.LduLineData> Info =
            new ConcurrentDictionary<Tuple<int, int>, Wlst.client.LduLineData>();

        public static void UpdateLduInfo(Wlst.client.LduLineData info)
        {
            var tmp = new Tuple<int, int>(info.LduId, info.LineId );
            if (Info.ContainsKey(tmp)) Info[tmp] = info;
            else Info.TryAdd(tmp, info);
        }


        public static Wlst.client.LduLineData GetInfo(int lduId, int loopId)
        {
            var tmp = new Tuple<int, int>(lduId, loopId);
            if (Info.ContainsKey(tmp)) return Info[tmp];
            return null;
        }
    }
}
