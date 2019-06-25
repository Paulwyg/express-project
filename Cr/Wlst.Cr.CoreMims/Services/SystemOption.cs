using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wlst.Cr.CoreMims
{
    public class SystemOption
    {

        private static bool load = false;

        internal static ConcurrentDictionary<int, string> OptionString = new ConcurrentDictionary<int, string>();
 
 
        /// <summary>
        /// 获取 string类型的  设置值
        /// </summary>
        /// <param name="opid"></param>
        /// <returns></returns>
        public static string GetOption(int opid)
        {
            if (OptionString.Count == 0 && load == false)
            {
                UpdateDesc();
                load = true;
                LoadSet();
            }
            if (OptionString.ContainsKey(opid)) return OptionString[opid];
            return string.Empty;
        }

        /// <summary>
        /// 获取 string类型的  设置值
        /// </summary>
        /// <param name="opid"></param>
        /// <returns></returns>
        public static int GetOptionInt(int opid)
        {
            var x = GetOption(opid);
            if (string.IsNullOrEmpty(x)) return -1;
            int y = 0;
            if (Int32.TryParse(x, out y))
            {
                return y;
            }
            return -1;
        }


        /// <summary>
        /// 获取设置是否为默认值  未设置也为default
        /// </summary>
        /// <param name="opid"></param>
        /// <param name="opdef"></param>
        /// <returns></returns>
        public static bool GetOptionIsDefaults(int opid, int opdef)
        {
            var x = GetOption(opid);
            if (string.IsNullOrEmpty(x)) return true;
            int y = 0;
            if (Int32.TryParse(x, out y))
            {
                if (y != opdef) return false;
            }
            return true;

        }

        /// <summary>
        /// 获取值是否为指定值
        /// </summary>
        /// <param name="opid"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool GetOptionIsThisValue(int opid, int value)
        {
            var x = GetOption(opid);
            if (string.IsNullOrEmpty(x)) return false;
            int y = 0;
            if (Int32.TryParse(x, out y))
            {
                if (y == value) return true;
            }
            return false;
        }

        private static void LoadSet()
        {

            var dt1 = XmlOptionSvr.LoadSets(292);
            foreach (var f in dt1)
            {
                if (OptionString.ContainsKey(f.Item1) == false) OptionString.TryAdd(f.Item1, f.Item2);
                else OptionString[f.Item1] = f.Item2;
            }

        }


        /// <summary>
        /// 系统可供设置的选项
        /// </summary>
        private static void UpdateDesc()
        {
            var dic = new Dictionary<int, string>();
            dic.Add(0, @"文件名标记为292.xml的文件为配置文件，本文件为配置文件的说明文件，本说明文件包含所有可供设置的选项。");
            dic.Add(1, @"选项设置方式为：查看本文件的数字与对应的说明，然后手动在292.xml文件里面设置对应的数字与值。");
            dic.Add(2, @"采用默认值的选项请不要加入到100.xml文件内。");
            dic.Add(99, @"...........................下方为可供设置的选项，修改请慎重................................");
            dic.Add(1001, @"HTTP服务端口 默认8281 ，此端口应与：中间层IpConfig配置文件中WebSocketPort端口号一致 ");
            dic.Add(1002, @"单灯有功功率修正，不设置为默认为1, 有功功率原始数据 乘以 倍数,");
            dic.Add(1003, @"终端右击菜单是否呈现单灯集中器菜单。默认为0，不呈现；1为呈现");
            dic.Add(2001, @"海门将外箱门设置成线控。默认为0，不呈现；1为呈现");

            XmlOptionSvr.SaveXmlDesc(292, dic);

        }




    }
}
