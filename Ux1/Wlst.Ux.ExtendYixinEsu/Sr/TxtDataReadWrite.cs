using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Wlst.Ux.ExtendYixinEsu.Sr
{
    /// <summary>
    /// 读写文件
    /// </summary>
    public class TxtDataReadWrite
    {

        private const string FilePath = "Config\\CitySet.txt";

        private static void WriteFile(List<string> info)
        {
            var path = FilePath;
            try
            {
                if (File.Exists(path)) File.Delete(path);

                using (StreamWriter sw = new StreamWriter(path, false))
                {
                    //一个string[] 是一行  ，一行中以tab键分隔  

                    foreach (var g in info)
                    {
                        if (!string.IsNullOrEmpty(g))
                        {
                            sw.WriteLine(g);
                        }

                    }
                    //sw.WriteLine(txt);
                    sw.Close(); //关闭线程，很重要！  

                }
            }
            catch (Exception ex)
            {

            }
        }



        private static List<string> ReadFileInfo()
        {
            var path = FilePath;
            var rtn = new List<string>();
            try
            {
                if (!File.Exists(path)) return new List<string>();

                //C#读取TXT文件之建立  FileStream 的对象,说白了告诉程序,     
                //文件在那里,对文件如何 处理,对文件内容采取的处理方式     
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                //仅 对文本 执行  读写操作     
                StreamReader sr = new StreamReader(fs);
                //定位操作点,begin 是一个参考点     
                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                //读一下，看看文件内有没有内容，为下一步循环 提供判断依据     
                //sr.ReadLine() 这里是 StreamReader的要领  可不是 console 中的~      
                string str = sr.ReadLine(); //假如  文件有内容      
                while (str != null)
                {
                    if (!string.IsNullOrEmpty(str))
                        rtn.Add(str);
                    //输出字符串，str 在上面已经定义了 读入一行字符      
                    //   Console.Write(".");
                    //这里我的理会是 当输出一行后，指针移动到下一行~     
                    //下面这句话就是 判断 指针所指这行能无法 有内容~     
                    str = sr.ReadLine();

                }
                //C#读取TXT文件之关上文件，留心顺序，先对文件内部执行 关上，然后才是文件~     
                sr.Close();
                fs.Close();
            }
            catch (Exception ex)
            {

            }
            return rtn;
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="info"></param>
        internal static void WriteFile(Dictionary<string, List<int>> info, long  longupdate)
        {
            var lst = new List<string>();
            foreach (var t in info)
            {
                string tmp = t.Key  + "*" + longupdate + "*";
                foreach (var g in t.Value ) tmp += g + "-";
                lst.Add(tmp);
            }
            WriteFile(lst);
        }

        /// <summary>
        /// 读取数据 
        /// </summary>
        /// <returns></returns>
        internal static Dictionary< string, List<int>> ReadFile(out long updatetimex)
        {
            var info = ReadFileInfo();
            //long xt = 0;
            updatetimex = 0;

            var rtn = new Dictionary<string, List<int>>();
            foreach (var f in info)
            {
                var sps = f.Split('*');
                if (sps.Count() > 2)
                {
                    var namt = sps[0].Trim();

                    Int64.TryParse(sps[1], out updatetimex);
                    var rtus = sps[2].Split('-');
                    var rs = new List<int>();
                    foreach (var g in rtus)
                    {
                        int id = 0;
                        if (Int32.TryParse(g, out id))
                        {
                            rs.Add(id);
                        }
                    }
                    if (!rtn.ContainsKey(namt)) rtn.Add(namt, rs);
                    

                }
            }
            return rtn;
        }
    }
}
