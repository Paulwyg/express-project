using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Ux.ExtendYixinEsu.Sr
{
    public class RtuBelongDataHold
    {
        #region

        private static RtuBelongDataHold _mySlef;

        public static RtuBelongDataHold MySlef
        {
            get
            {
                if (_mySlef == null)
                {
                    new RtuBelongDataHold();
                }
                return _mySlef;
            }
        }

        private static object obj = 0;

        protected RtuBelongDataHold()
        {
            lock (obj)
            {
                if (_mySlef != null) return;
                _mySlef = this;


                var tmp = TxtDataReadWrite.ReadFile(out LastUpdateTime);
                Info.Clear();
                foreach (var t in tmp)
                {
                    if (Info.ContainsKey(t.Key)) continue;
                    Info.Add(t.Key, t.Value);
                }

            }
        }


        #endregion

        public long LastUpdateTime = 0;
        public Dictionary<string, List<int>> Info = new Dictionary<string, List<int>>();


        public void UpdateRtus(Dictionary<string, List<int>> allInfo, long updatetime)
        {
            Info.Clear();
            foreach (var f in allInfo) if (!Info.ContainsKey(f.Key))
            {
                Info.Add(f.Key, new List<int>( ));
                foreach (var gg in f.Value) Info[f.Key].Add(gg);
            }
            LastUpdateTime = updatetime;

            TxtDataReadWrite.WriteFile(Info, updatetime);
        }
    }
}
