using System;
using System.Collections.Generic;
using Wlst.Cr.Core.CoreServices;

namespace Wlst.Cr.CoreOne.Services
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class RegionNames
    {
        public const string MenuViewRegion = "MenuViewRegion";

        public const string BottomViewRegion = "BottomViewRegion";
        public const string LeftViewRegion = "LeftViewRegion";
        public const string DocumentRegion = DocumentRegionName.DocumentRegion;
        public const string TopViewRegion = "TopViewRegion";
        public const string RightViewRegion = "RightViewRegion";


        public const string MapRegion = "MapRegion";
        public const string MsgRegion = "MsgRegion";
        public const string OtherRegion = "OtherRegion";
        public const string StateBarRegion = "StateBarRegion";
        public const string StateBarTimeRegion = "StateBarTimeRegion";
        ///// <summary>
        ///// 中控台使用
        ///// </summary>
        public const string DataRegion = "DataRegion";

        public const string TalkRegion = "TalkRegion";
        ///// <summary>
        ///// 需要显示的最新数据界面 使用的导出目标 
        ///// </summary>
        //public const string DataShowRegion = "DataShowRegion";


        private static List<string> regionNamesLst = new List<string>();

        public static List<string> RegionNamesLst
        {
            get
            {
                if (regionNamesLst.Count == 0)
                {
                    regionNamesLst.Add(MenuViewRegion);
                    regionNamesLst.Add(BottomViewRegion);
                    regionNamesLst.Add(LeftViewRegion);

                    regionNamesLst.Add(DocumentRegion);
                    regionNamesLst.Add(OtherRegion);
                    regionNamesLst.Add(StateBarRegion);
                    regionNamesLst.Add(StateBarTimeRegion);
                    regionNamesLst.Add(DataRegion);
                    regionNamesLst.Add(TopViewRegion);
                    regionNamesLst.Add(MapRegion);
                    regionNamesLst.Add(RightViewRegion);
                    regionNamesLst.Add(MsgRegion);
                    regionNamesLst.Add(TalkRegion);
                }
                return regionNamesLst;
            }
        }
    }
}