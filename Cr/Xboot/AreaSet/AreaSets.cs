using System;
using System.Collections.Generic;
using System.Windows.Media;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;


namespace Xboot.AreaSet
{
    public class AreaSets
    {
        private static AreaSets mySelf;

        public static AreaSets MySelf
        {
            get
            {
                if (mySelf == null) new AreaSets();
                return mySelf;
            }
        }

        private AreaSets()
        {
            if (mySelf == null)
            {
                mySelf = this;
                Load();
            }
        }

        public string Color1;
        public string Color2;
        public string Color3;
        public string Color4;
        public string Color5;
        public string ColorBottom;
        public string ColorMenu;
       

        public bool TreeIsHide;
        public bool DataIsHide;
        public bool MsgIsHide;

        /// <summary>
        /// 4 5 45
        /// </summary>
        public int DataArea;

        /// <summary>
        /// 3 4 5 35 45
        /// </summary>
        public int MsgArea;

        /// <summary>
        /// 2 23 2345
        /// </summary>
        public int MainArea;


        public int Area1Wide;
        public int Area45Height;
        public int Area35Wide;


        private const string XmlSetPath = "CETC50_DemoAreaSet";
        private bool _load = false;

        private void Load()
        {
            if (_load) return;
            _load = true;
            var info = Elysium.ThemesSet.Common.ReadSave.Read(XmlSetPath);
            if (info.ContainsKey("Color1")) Color1 = info["Color1"];
            else Color1 = Colors.Transparent.ToString();

            if (info.ContainsKey("Color2")) Color2 = info["Color2"];
            else Color2 = Colors.Transparent.ToString();

            if (info.ContainsKey("Color3")) Color3 = info["Color3"];
            else Color3 = Colors.Transparent.ToString();

            if (info.ContainsKey("Color4")) Color4 = info["Color4"];
            else Color4 = Colors.Transparent.ToString();

            if (info.ContainsKey("Color5")) Color5 = info["Color5"];
            else Color5 = Colors.Transparent.ToString();

            if (info.ContainsKey("ColorBottom")) ColorBottom = info["ColorBottom"];
            else ColorBottom = Colors.Transparent.ToString();

            if (info.ContainsKey("ColorMenu")) ColorMenu = info["ColorMenu"];
            else ColorMenu = Colors.Transparent.ToString();



            if (info.ContainsKey("TreeIsHide")) TreeIsHide = info["TreeIsHide"].Contains("yes");
            else TreeIsHide = false;

            if (info.ContainsKey("DataIsHide")) DataIsHide = info["DataIsHide"].Contains("yes");
            else DataIsHide = false;

            if (info.ContainsKey("MsgIsHide")) MsgIsHide = info["MsgIsHide"].Contains("yes");
            else MsgIsHide = false;

            try
            {
                if (info.ContainsKey("DataArea")) DataArea = Int32.Parse(info["DataArea"]);
                else DataArea = 4;
            }
            catch (Exception ex)
            {
            }


            try
            {
                if (info.ContainsKey("MsgArea")) MsgArea = Int32.Parse(info["MsgArea"]);
                else MsgArea = 5;
            }
            catch (Exception ex)
            {
            }
            //try
            //{
            //    if (info.ContainsKey("DataArea")) DataArea = Int32.Parse(info["DataArea"]);
            //    else DataArea = 4;
            //}
            //catch (Exception ex)
            //{
            //}
            try
            {
                if (info.ContainsKey("MainArea")) MainArea = Int32.Parse(info["MainArea"]);
                else MainArea = 23;
            }
            catch (Exception ex)
            {
            }
            try
            {
                if (info.ContainsKey("Area1Wide")) Area1Wide = Int32.Parse(info["Area1Wide"]);
                else Area1Wide = 200;
            }
            catch (Exception ex)
            {
            }
            try
            {
                if (info.ContainsKey("Area45Height")) Area45Height = Int32.Parse(info["Area45Height"]);
                else Area45Height = 250;
            }
            catch (Exception ex)
            {
            }
            try
            {
                if (info.ContainsKey("Area35Wide")) Area35Wide = Int32.Parse(info["Area35Wide"]);
                else Area35Wide = 580;
            }
            catch (Exception ex)
            {
            }


        }

        public void Save()
        {
            var info = new Dictionary<string, string>();
            info.Add("Color1", Color1);
            info.Add("Color2", Color2);
            info.Add("Color3", Color3);
            info.Add("Color4", Color4);
            info.Add("Color5", Color5);
            info.Add("ColorBottom",ColorBottom);
            info.Add("ColorMenu",ColorMenu);
            
            info.Add("TreeIsHide", TreeIsHide ? "yes" : "no");
            info.Add("DataIsHide", DataIsHide ? "yes" : "no");
            info.Add("MsgIsHide", MsgIsHide ? "yes" : "no");
            info.Add("DataArea", DataArea + "");
            info.Add("MsgArea", MsgArea + "");
            info.Add("MainArea", MainArea + "");
            info.Add("Area1Wide", Area1Wide + "");
            info.Add("Area45Height", Area45Height + "");
            info.Add("Area35Wide", Area35Wide + "");

            Elysium.ThemesSet.Common.ReadSave.Save(info, XmlSetPath);
        }

        public void UpdateCurrentSet()
        {
            if (MainWindow.MySelf != null)
            {
                MainWindow.MySelf.UpdateAreaSet();
                this.Save();

                EventPublish.PublishEvent(new PublishEventArgs()
                                                {
                                                    EventAttachInfo = Area45Height,
                                                    EventId = 0,
                                                    EventType = "newdateheight"
                                                });
            }
        }
    }
}
