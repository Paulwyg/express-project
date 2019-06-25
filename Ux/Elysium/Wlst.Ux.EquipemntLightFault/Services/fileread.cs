using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Wlst.Ux.EquipemntLightFault.Services
{
    public
        class fileread
    {
        public static string Read(string path)
        {
            try
            {
                StreamReader sr = new StreamReader(path, Encoding.Default);
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    return line;
                }
            }
            catch (Exception ex)
            {

            }
            return string.Empty;
        }


        public static void Write(string path, string data)
        {
            try
            {
                FileStream fs = new FileStream(path, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                //开始写入
                sw.Write(data);
                //清空缓冲区
                sw.Flush();
                //关闭流
                sw.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
