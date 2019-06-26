using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Cr.Coreb.Servers;

namespace Wlst.Cr.Core.CoreServices
{
    /// <summary>
    /// 提供数字形式的 保存模式
    /// </summary>
    public class SystemOption
    {


        /// <summary>
        /// 获取选项
        /// </summary>
        /// <param name="opid"></param>
        /// <returns>不存在 返回 -1</returns>
        public static int GetOption(int opid)
        {
            return XmlOptionSvr.GetOptionInt(opid);

        }



  


    }


    internal class XmlOptionSvr
    {

        internal static Dictionary<int, int> Data = new Dictionary<int, int>();

      /// <summary>
        /// 系统可供设置的选项
        /// </summary>
        private static void UpdateDesc()
      {
          var dic = new Dictionary<int, string>();
 
          dic.Add(2, "输出服务器事件处理时间，程序内部通信事件时间。1、输出打印");
          dic.Add(3, "系统内部数据，调度函数在UI线程运行 ，默认在UI线程运行 。1、非UI运行");


          SaveXmlDesc(dic);

      }

        /// <summary>
        /// 一个模块 仅允许保存一个选项文件 
        /// </summary>
        /// <param name="infodesc">描述信息 </param>
        public static void SaveXmlDesc(Dictionary<int, string> infodesc)
        {
            if (infodesc == null) return;
            var dicdesc = new Dictionary<string, string>();
            foreach (var f in infodesc)
            {
                if (dicdesc.ContainsKey(f.Key + "")) continue;
                dicdesc.Add(f.Key + "", f.Value);
            }
            XmlReadSave.Save(dicdesc, Environment.CurrentDirectory + "//Config//debug_desc");
        }

        private static bool _load = false;

        private static void LoadSetpri()
        {
            if (_load) return;
            _load = true;
            var data = XmlReadSave.Read(Environment.CurrentDirectory + "//Config//debug");

            foreach (var f in data)
            {
                int xkey = 0;
                int xvalue = 0;
                if (Int32.TryParse(f.Key, out xkey) && Data.ContainsKey(xkey) == false &&
                    Int32.TryParse(f.Value, out xvalue))
                {
                    Data.Add(xkey, xvalue);
                }
            }
            UpdateDesc();

        }


        /// <summary>
        /// 获取指定模块的指定序列的  设置选项  无设置则返回 -1
        /// </summary>
        /// <param name="key"></param>
        /// <returns>设置了则返回 设置值  否则返回 -1</returns>
        public static int GetOptionInt(int key)
        {
            if (Data.ContainsKey(key) == false) LoadSetpri();
            if (Data.ContainsKey(key) == false) return -1;
            return Data[key];
        }

    }
}
