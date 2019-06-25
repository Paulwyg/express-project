using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Ux.Wj6005Module.Models
{
    public class Jd601DataTwoAdjustVollevel
    {
        private static Dictionary<int, string> _dictionary;

        public static Dictionary<int, string> AdjustVolLevel
        {
            get
            {
                if (_dictionary == null)
                {
                    _dictionary = new Dictionary<int, string>();
                    _dictionary.Add(0, "旁路");
                    _dictionary.Add(1, "1档节能");
                    _dictionary.Add(2, "2档节能");
                    _dictionary.Add(3, "3档节能");
                    _dictionary.Add(4, "4档节能");
                    _dictionary.Add(5, "5档节能");
                }
                return _dictionary;
            }
        }
    }
}
