using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Sr.SlusglInfoHold
{
    public class Ioc
    {
        public static Func<int, int, int, List<int>> GetSluLampError;
        public static List<int> GetSluLampErrors(int sluId, int ctrlId, int lampId)
        {
            if (GetSluLampError == null) return new List<int>();
            return GetSluLampError(sluId, ctrlId, lampId);
        }
    }
}
