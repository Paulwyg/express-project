using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Wlst.Cr.Coreb.Servers
{
    /// <summary>
    /// 
    /// </summary>
    public class WriteFile
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="info"></param>
        public static void WriteFiles(string path, List<string> info)
        {

            if (File.Exists(path)) File.Delete(path);
            // if (!File.Exists(path)) File.Create(path );

            //using (System.IO.StreamWriter sw = System.IO.File.CreateText( path))
            //{
            //    //一个string[] 是一行  ，一行中以tab键分隔  
            //    foreach (var temp in info)
            //    {
            //        if (!string.IsNullOrEmpty(temp))
            //        {
            //            sw.WriteLine(temp);
            //        }

            //    }
            //    // sw.WriteLine(txt);
            //    sw.Close(); //关闭线程，很重要！  

            //}

            FileStream fs = new FileStream(path, FileMode.Create);
            using (System.IO.StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                // Encoding.GetEncoding("GBK")))
            {
                //一个string[] 是一行  ，一行中以tab键分隔  
                foreach (var temp in info)
                {
                    if (!string.IsNullOrEmpty(temp))
                    {
                        sw.WriteLine(temp);
                    }

                }
                // sw.WriteLine(txt);
                sw.Close(); //关闭线程，很重要！  

            }
            fs.Close();

            //StreamWriter sw = new StreamWriter(fs);
            //    //开始写入  
            //sw.Write(String);  
            ////清空缓冲区 sw.Flush(); 
            ////关闭流 sw.Close();
            //fs.Close();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<string> ReadFiles(string path)
        {
            var rtn = new List<string>();
            try
            {
                if (File.Exists(path) == false) return rtn;
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
                    //输出字符串，str 在上面已经定义了 读入一行字符      
                    // Console.WriteLine("{0}", str);
                    //这里我的理会是 当输出一行后，指针移动到下一行~     
                    //下面这句话就是 判断 指针所指这行能无法 有内容~     
                    rtn.Add(str);
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


    }
}
