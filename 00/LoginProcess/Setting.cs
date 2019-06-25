using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LoginProcess
{

    public partial class Setting
    {

        public const string XmlConfigName = "processlogin";

        /// <summary>
        /// RowHeight LoopNameLength TimeNameLength VaNameLength
        /// </summary>
        /// <returns></returns>
        public static List<Tuple<string, string, string, string>> LoadNewDataLenghtSetConfg()
        {


            CreateFIle(XmlConfigName);

            var rtn = new List<Tuple<string, string, string, string>>();
            var info = Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read(XmlConfigName);

            for (int i = 0; i < 10; i++)
            {
                if (info.ContainsKey("areaxname" + i) && info.ContainsKey("areaxpath" + i))
                {
                    string x1 = info["areaxname" + i].Trim();
                    string x2 = info["areaxpath" + i].Trim();

                    string x3 = info.ContainsKey("areaxusername" + i) ? info["areaxusername" + i].Trim() : "";
                    string x4 = info.ContainsKey("areaxuserpsw" + i) ? info["areaxuserpsw" + i].Trim() : "";

                    rtn.Add(new Tuple<string, string, string, string>(x1, x2, x3, x4));
                }

            }
            return rtn;
        }


        private static void CreateFIle(string xmlFileName)
        {
         
            if (!xmlFileName.EndsWith(".xml"))
            {
                xmlFileName += ".xml";
            }
            string dir = Directory.GetCurrentDirectory() + "\\SystemXmlConfig";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            string path = dir + "\\" + xmlFileName;

            if (File.Exists(path)) return;
            var info = new Dictionary<string, string>();
            info.Add("areaxname0", "程序名字");
            info.Add("areaxpath0", "程序路径，可使用绝对路径");
            info.Add("areaxusername0", "用户名，用户名");
            info.Add("areaxuserpsw0", "密码，密码");
            File.Create(path);
            Wlst.Cr.CoreOne.Services.SystemXmlConfig.Save(info, xmlFileName);

        }

    }

}
