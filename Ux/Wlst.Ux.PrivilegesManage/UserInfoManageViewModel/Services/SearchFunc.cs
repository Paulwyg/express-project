using System.Collections.Generic;
using System.Linq;

namespace Wlst.Ux.PrivilegesManage.UserInfoManageViewModel.Services
{
    public static class SearchFunc
    {
        private static bool _theFirstLetter;
        public static List<string> GetSearchResult(string source, List<string> target)
        {
            var res = new List<string>();
            foreach (var item in target)
            {
                _theFirstLetter = true;
                if (IsMarch(source, item))
                {
                    res.Add(item);
                }

            }
            return res;
        }

        private static bool IsMarch(string source, string target)
        {
            var res = false;
            if (source.Length == 0) return true;
            if (target.Length == source.Length)
            {
                return target == source;
            }
            if (!_theFirstLetter)
            {
                return source == target.Substring(0, source.Length);
            }
            if (target.Length > source.Length && source.Length > 0)
            {
                var listSource = source.ToList();
                var listTarget = target.ToList();
                for (var i = 0; i < listTarget.Count; i++)
                {
                    if (listSource[0] != listTarget[i]) continue;
                    _theFirstLetter = false;
                    res = IsMarch(source.Substring(1, source.Length - 1), target.Substring(i + 1, target.Length - i - 1));
                    if (!res) break;
                }
            }
            else
            {
                return false;
            }
            return res;
        }
    }
}
