using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Cr.CoreMims
{

    public class XmlOptionSvr
    {
        /// <summary>
        /// moduleid     27  1 23
        /// </summary>
        internal static Dictionary<int, Dictionary<int, string>> Data = new Dictionary<int, Dictionary<int, string>>();

        ///// <summary>
        ///// 一个模块 仅允许保存一个选项文件 
        ///// </summary>
        ///// <param name="moduleid"></param>
        ///// <param name="infoset">设置信息</param>
        ///// <param name="infodesc">描述信息 </param>
        //public static void SaveXml(int moduleid, Dictionary<int, string> infoset,
        //                           Dictionary<int, string> infodesc = null)
        //{
        //    var dic = new Dictionary<string, string>();

        //    foreach (var f in infoset)
        //    {
        //        if (dic.ContainsKey(f.Key + "")) continue;
        //        dic.Add(f.Key + "", f.Value);
        //    }

        //    XmlReadSave.Save(dic, moduleid + "");
        //    if (Data.ContainsKey(moduleid) == false) Data.Add(moduleid, new Dictionary<int, string>());
        //    Data[moduleid].Clear();
        //    foreach (var f in infoset)
        //    {
        //        if (Data[moduleid].ContainsKey(f.Key) == false)
        //            Data[moduleid].Add(f.Key, f.Value);
        //    }


        //    if (infodesc == null) return;
        //    var dicdesc = new Dictionary<string, string>();
        //    foreach (var f in infodesc)
        //    {
        //        if (dicdesc.ContainsKey(f.Key + "")) continue;
        //        dicdesc.Add(f.Key + "", f.Value);
        //    }
        //    XmlReadSave.Save(dicdesc, moduleid + "_desc");
        //}


        /// <summary>
        /// 一个模块 仅允许保存一个选项文件 
        /// </summary>
        /// <param name="moduleid"></param>
        /// <param name="infoset">设置信息</param>
        /// <param name="infodesc">描述信息 </param>
        public static void SaveXmlDesc(int moduleid, Dictionary<int, string> infodesc)
        {
            //var dic = new Dictionary<string, string>();

            //foreach (var f in infoset)
            //{
            //    if (dic.ContainsKey(f.Key + "")) continue;
            //    dic.Add(f.Key + "", f.Value);
            //}

            //XmlReadSave.Save(dic, moduleid + "");
            //if (Data.ContainsKey(moduleid) == false) Data.Add(moduleid, new Dictionary<int, string>());
            //Data[moduleid].Clear();
            //foreach (var f in infoset)
            //{
            //    if (Data[moduleid].ContainsKey(f.Key) == false)
            //        Data[moduleid].Add(f.Key, f.Value);
            //}


            if (infodesc == null) return;
            var dicdesc = new Dictionary<string, string>();
            foreach (var f in infodesc)
            {
                if (dicdesc.ContainsKey(f.Key + "")) continue;
                dicdesc.Add(f.Key + "", f.Value);
            }
            XmlReadSave.SaveToConfig( dicdesc, moduleid + "_desc");
        }

        private static void LoadSetpri(int mouduleid)
        {
            if (Data.ContainsKey(mouduleid) == false) Data.Add(mouduleid, new Dictionary<int, string>());

            var data = XmlReadSave.ReadFromConfig(mouduleid + "");

            foreach (var f in data)
            {
                int xkey = 0;
                if (Int32.TryParse(f.Key, out xkey) && Data[mouduleid].ContainsKey(xkey) == false)
                {
                    Data[mouduleid].Add(xkey, f.Value);
                }
            }
        }


        public static List<Tuple<int, int>> LoadSetr(int mouduleid)
        {

            var data = XmlReadSave.ReadFromConfig( mouduleid + "");
            var rtn = new List<Tuple<int, int>>();

            foreach (var f in data)
            {
                int xkey = 0;
                int xvalue = 0;
                if (Int32.TryParse(f.Key, out xkey) && Int32.TryParse(f.Value, out xvalue))
                {
                    rtn.Add(new Tuple<int, int>(xkey, xvalue));
                }
            }
            return rtn;
        }


        public static List<Tuple<int, string>> LoadSets(int mouduleid)
        {

            var data = XmlReadSave.ReadFromConfig(mouduleid + "");
            var rtn = new List<Tuple<int, string>>();

            foreach (var f in data)
            {
                int xkey = 0;
                int xvalue = 0;
                if (Int32.TryParse(f.Key, out xkey) && string .IsNullOrEmpty( f.Value )==false )
                {
                   // if (string.IsNullOrEmpty(f.Value) == false && Int32.TryParse(f.Value, out xvalue) == false)
                    {
                        rtn.Add(new Tuple<int, string>(xkey, f.Value.Trim()));
                    }
                }
            }
            return rtn;
        }

        /// <summary>
        /// 获取指定模块的指定序列的  设置选项
        /// </summary>
        /// <param name="moduleid"></param>
        /// <param name="indexkey"></param>
        /// <returns></returns>
        public static string GetOption(int moduleid, int indexkey)
        {
            if (Data.ContainsKey(moduleid) == false) LoadSetpri(moduleid);

            if (Data.ContainsKey(moduleid) == false) return string.Empty;
            if (Data[moduleid].ContainsKey(indexkey) == false) return string.Empty;
            return Data[moduleid][indexkey];
        }

        public static string GetOption(int moduleid, int indexkey, string defaults)
        {
            var nt = GetOption(moduleid, indexkey);
            if (string.IsNullOrEmpty(nt)) return defaults;
            return nt;
        }

        /// <summary>
        /// 获取指定模块的指定序列的  设置选项  无设置则返回null
        /// </summary>
        /// <param name="moduleid"></param>
        /// <param name="indexkey"></param>
        /// <returns>设置了则返回 true or false  否则返回 null</returns>
        public static bool? GetOptionBool(int moduleid, int indexkey)
        {
            if (Data.ContainsKey(moduleid) == false) LoadSetpri(moduleid);

            if (Data.ContainsKey(moduleid) == false) return null;
            if (Data[moduleid].ContainsKey(indexkey) == false) return null;
            return Data[moduleid][indexkey].Contains("1");
        }

        public static bool GetOptionBool(int moduleid, int indexkey, bool defaults)
        {
            var nt = GetOptionBool(moduleid, indexkey);
            if (nt == null) return defaults;
            else return nt.Value;
        }

        /// <summary>
        /// 获取指定模块的指定序列的  设置选项  无设置则返回 -1
        /// </summary>
        /// <param name="moduleid"></param>
        /// <param name="indexkey"></param>
        /// <returns>设置了则返回 设置值  否则返回 -1</returns>
        public static int GetOptionInt(int moduleid, int indexkey)
        {
            if (Data.ContainsKey(moduleid) == false) LoadSetpri(moduleid);

            if (Data.ContainsKey(moduleid) == false) return -1;
            if (Data[moduleid].ContainsKey(indexkey) == false) return -1;
            int rtn = -1;
            Int32.TryParse(Data[moduleid][indexkey], out rtn);
            return rtn;
        }

        public static int GetOptionInt(int moduleid, int indexkey, int defaults)
        {
            var nt = GetOptionInt(moduleid, indexkey);
            if (nt == -1) return defaults;
            else return nt;
        }

    }
}
