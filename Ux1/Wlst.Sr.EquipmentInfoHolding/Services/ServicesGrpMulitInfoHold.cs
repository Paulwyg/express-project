using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.ModuleServices;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.client;

namespace Wlst.Sr.EquipmentInfoHolding.Services
{
    /// <summary>
    /// 本地分组 管理  分组中的 AreaId 参数永远为 -1
    /// </summary>
    public class ServicesGrpMulitInfoHold 
    {
      

        private static ServicesGrpMulitInfoHold _mySlef;

        public static ServicesGrpMulitInfoHold MySlef
        {
            get
            {
                if (_mySlef == null)_mySlef = new ServicesGrpMulitInfoHold();
                return _mySlef;
            }
        }

        public void InitSvr(){}
        protected ServicesGrpMulitInfoHold()
        {
            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(Loadfromxml,1,DelayEventHappen.EventOne );
            Loadfromxml();
        }
        void Loadfromxml()
        {
               //Todo load data from db or xml txt
            var tmp = FrLocalInst.LoadRtufromTxt();
            int index = 1;
            foreach (var f in tmp)
            {
                if (ItemsMultGrp.ContainsKey(f.Item1)) continue;
                var tmpr = new GroupInformation(new GroupItemsInfo.GroupItem()
                                                    {
                                                        AreaId = -1,
                                                        GroupName = f.Item2,
                                                        GroupId = f.Item1,
                                                        LstTml = f.Item3
                                                    }) {Index = index++};
                ItemsMultGrp.Add(f.Item1, tmpr);
                ;
            }

            var arg = new PublishEventArgs()
            {
                EventId = EventIdAssign.MulityInfoGroupAllNeedUpdate,
                EventType = PublishEventType.Core
            };
            EventPublish.PublishEvent(arg);
        }

        /// <summary>
        /// 所有的本地分组
        /// </summary>
        public static Dictionary<int, Model.GroupInformation> ItemsMultGrp = new Dictionary<int, GroupInformation>();

        /// <summary>
        /// 不存在返回null  获取分组  与全局分组结构相同 但AreaId 为-1以示区分
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static GroupInformation GetGroup(int groupId)
        {
            return ItemsMultGrp.ContainsKey(groupId) ? ItemsMultGrp[groupId] : null;
        }

        public static List<int> GetRtusInGroup(int groupId)
        {
            return ItemsMultGrp.ContainsKey(groupId) ? ItemsMultGrp[groupId].LstTml : new List<int>();
        }

        /// <summary>
        /// 获取所有分组  与全局分组结构相同 但AreaId 为-1以示区分
        /// </summary>
        /// <returns></returns>
        public static List<GroupInformation> GetAllGroup()
        {
            return (from t in ItemsMultGrp orderby t.Value.Index select t.Value).ToList();
        }

        public static void UpdateAllMultGroups(List<GroupInformation> data)
        {
            ItemsMultGrp.Clear();
            int index = 1;
            foreach (var f in data)
            {
                if (f.LstTml.Count == 0) return;
                if (ItemsMultGrp.ContainsKey(f.GroupId)) return;
                f.AreaId = -1;
                f.Index = index++;
                ItemsMultGrp.Add(f.GroupId, f);
            }

            //todo  update to db or xml txt
            var lst = new List<Tuple<int, string, List<int>>>();
            var ornt = (from t in ItemsMultGrp orderby t.Value.Index ascending select t.Value).ToList();
            foreach (var f in ornt) lst.Add(new Tuple<int, string, List<int>>(f.GroupId, f.GroupName, f.LstTml));
            FrLocalInst.WriteFiles(lst);


            var arg = new PublishEventArgs()
                          {
                              EventId = EventIdAssign.MulityInfoGroupAllNeedUpdate,
                              EventType = PublishEventType.Core
                          };
            EventPublish.PublishEvent(arg);
        }


    }


    internal class FrLocalInst
    {
        public static List<Tuple<int, string, List<int>>> LoadRtufromTxt()
        {
            string username = Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.UserName;
            if (string.IsNullOrEmpty(username)) return new List<Tuple<int, string, List<int>>>( );
           // string path = pt + "\\" + username + ".txt";
            string xmlfilepath = Environment.CurrentDirectory + "\\LocalGrp\\" + username + ".txt";
            var rtn = new List<Tuple<int, string, List<int>>>();
            try
            {

                if (!File.Exists(xmlfilepath)) return new List<Tuple<int, string, List<int>>>();

                try
                {
                    using (StreamReader sw = new StreamReader(xmlfilepath, Encoding.GetEncoding("GBK")))
                    {
                        while (sw.EndOfStream == false)
                        {
                            try
                            {
                                var txt = sw.ReadLine();
                                var sps = txt.Split('-');
                                if (sps.Count() < 3) continue;
                                int grp = 0;
                                Int32.TryParse(sps[0], out grp);
                                string gpn = sps[1];
                                var spr = sps[2].Split(',');
                                List<int> rtus = new List<int>();
                                foreach (var f in spr)
                                {
                                    int rtu = 0;
                                    if (Int32.TryParse(f, out rtu))
                                    {
                                        rtus.Add(rtu);
                                    }
                                }
                                if (grp == 0) continue;
                                if (rtus.Count == 0) continue;
                                rtn.Add(new Tuple<int, string, List<int>>(grp, gpn, rtus));
                            }
                            catch (Exception ex ){}
                        }

                        // sw.WriteLine(txt);
                        sw.Close(); //关闭线程，很重要！  

                    }
                }
                catch
                {

                }

            }
            catch (Exception ex)
            {
                //Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("读取本地终端数据异常:" + ex);
            }
            return rtn;
        }


        public static void WriteFiles(List<Tuple<int, string, List<int>>> data)
        {
            // string xmlfilepath = Environment.CurrentDirectory + "\\Config\\local_group.txt";
            var pt = Environment.CurrentDirectory + "\\LocalGrp";
            if (!Directory.Exists(pt)) Directory.CreateDirectory(pt);
            string username = Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.UserName;
            if (string.IsNullOrEmpty(username)) return;
            string path = pt + "\\"+username +".txt";

            if (File.Exists(path)) File.Delete(path);

            using (StreamWriter sw = new StreamWriter(path, false, Encoding.GetEncoding("GBK")))
            {
                //一个string[] 是一行  ，一行中以tab键分隔  

                foreach (var g in data)
                {
                    var txt = "";
                    txt += g.Item1 + "-" + g.Item2 + "-";
                    foreach (var f in g.Item3) txt += f + ",";
                    sw.WriteLine(txt);
                }
                // sw.WriteLine(txt);
                sw.Close(); //关闭线程，很重要！  
            }

        }
    }
}

