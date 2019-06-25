using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Wlst.Sr.tmphold
{
    public class FileRead
    {
        public static  List<string> ReadFile(string strPath)
        {
            var rtn = new List<string>();
            try
            {
               
                if (System.IO.File.Exists(strPath) == false) return new List<string>();

                using (System.IO.StreamReader sw = new StreamReader(strPath))
                {
                    while (sw.EndOfStream == false)
                    {
                        var line = sw.ReadLine();
                        rtn.Add(line);
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return rtn;
        }
    }
}
