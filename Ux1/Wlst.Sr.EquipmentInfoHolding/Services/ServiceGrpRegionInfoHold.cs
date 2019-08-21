
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.client;
using Wlst.mobile;


namespace Wlst.Sr.EquipmentInfoHolding.Services
{

    public class ServiceGrpRegionInfoHold : EventHandlerHelperExtendNotifyProperyChanged
    {
        public const int GroupStartId = 502000;
        private static readonly GrpRegionInfoHoldExtend GrpSingle = new GrpRegionInfoHoldExtend();


        /// <summary>
        /// 程序初始化时必须执行一次;由本模块自动执行
        /// </summary>
        public static void InitLoad()
        {
            GrpSingle.InitLoad();
           // Wlst.Sr.EquipmentInfoHolding.Services.Others.SeverHttpPort = Wlst.Cr.CoreMims.SystemOption.GetOptionInt(1001) == -1 ? "8182" : Wlst.Cr.CoreMims.SystemOption.GetOptionInt(1001)+"";
        }


        /// <summary>
        /// <para>分组类型：1-地区  2：普通分组  ， 分组id ， 分组名称</para>
        /// </summary>
        public static Dictionary<Tuple<int,int, int>, string> InfoRegionGroups
        {
            get { return GrpSingle.InfoRegionGroups; }
        }

        /// <summary>
        /// 交叉分组信息，分组组合id，下终端id  终端图标状态
        /// </summary>
        public static Dictionary<Tuple<int,int, int>, List<Tuple<int, int>>> RegionRtuInfo
        {
            get { return GrpSingle.RegionRtuInfo; }
        }



        /// <summary>
        /// 终端信息，终端id  ，grpid ，regionId，imageId
        /// </summary>
        public static Dictionary<int, Tuple<int,int, int,int>> RtuRegionInfo
        {
            get { return GrpSingle.RtuRegionInfo; }
        }


        /// <summary>
        /// 获取相关类型的分组 1是地区  2是普通分组（全局分组）
        /// </summary>
        /// <param name="group"></param>
        /// <returns>不存在返回 null;grpid,grpname</returns>
        public static Dictionary<int, string> GetGrpByType(int areaId, int grpType)
        {
            var regionLst = new Dictionary<int, string>();
            foreach(var g in InfoRegionGroups)
            {
                if (g.Key.Item1 != grpType) continue;
                if (g.Key.Item2 != areaId) continue;
                
                regionLst.Add(g.Key.Item3, g.Value);

               
            }
            return regionLst.OrderBy(o => o.Key).ToDictionary(o => o.Key, p => p.Value);

        }


        /// <summary>
        /// 获取相关类型的分组 1是地区  2是普通分组（全局分组）
        /// </summary>
        /// <param name="group"></param>
        /// <returns>不存在返回 null;grpid,grpname</returns>
        public static string GetGrpName(int type,int areaid,int grpid)
        {
            var tu = new Tuple<int, int, int>(type,areaid, grpid);
            if (InfoRegionGroups.ContainsKey(tu)) return InfoRegionGroups[tu];

            return "地域分组";

        }

        /// <summary>
        /// 获取分组和区域下的终端list  rtuid，imageid
        /// </summary>
        /// <param name="grpId"></param>
        /// <param name="regionId"></param>
        /// <returns></returns>
        public static List<Tuple<int,int>> GetRtulstByGrpRegionId(int areaId,int grpId,int regionId)
        {
            if (grpId > -1 && regionId>-1)
            {
                var tukey = new Tuple<int, int, int>(areaId, grpId, regionId);

                if (RegionRtuInfo.ContainsKey(tukey)) return RegionRtuInfo[tukey];

                return null;
            }
            else if(grpId ==-1 && regionId >-1) //获取该地区下所有终端
            {
                var lst = new List<Tuple<int, int>>();
                foreach (var g in RegionRtuInfo)
                {
                    if (g.Key.Item1 == areaId && g.Key.Item3 == regionId) lst.AddRange(g.Value);

                }
                return lst;
            }

            return null;
        }


/// <summary>
/// 获取地区下的特殊终端
/// </summary>
/// <param name="areaId"></param>
/// <param name="regionGrpId">地区id</param>
/// <param name="isHaveRegion">true：查询没有地区的；false：查询地区下没有分组的</param>
/// <returns>rtuid  imageid</returns>
        public static List<Tuple<int, int>> GetSpRtulstByGrpRegionId(int areaId, int regionGrpId, bool isHaveRegion)
        {
            var rtulst = new List<Tuple<int, int>>();
            foreach (var g in RegionRtuInfo)
            {
                if (g.Key.Item1 != areaId) continue;
                //区域下 没有地区的终端
                if (isHaveRegion)
                {
                    if (g.Key.Item3 == 0)
                    {
                        rtulst.AddRange(g.Value);
                    }
                }
                //区域下  有地区，但是没有分组的终端
                else
                {
                    if (g.Key.Item3 == regionGrpId && g.Key.Item2 == 0)
                    {

                        rtulst.AddRange(g.Value);
                    } 
                }

            }

            return rtulst;

        }

        /// <summary>
        /// 获取终端的分组信息
        /// </summary>
        /// <param name="rtuId"></param>
        /// <returns></returns>
        public static Tuple<int,int,int> GetGrpRegionByRtuid(int rtuId)
        {

            if (RtuRegionInfo.ContainsKey(rtuId))
            {
                var tmp = RtuRegionInfo[rtuId];
                var tu = new Tuple<int, int,int>(tmp.Item1, tmp.Item2,tmp.Item3);

                return tu;
            }
            return null;

        }

        /// <summary>
        /// 获取终端的状态
        /// </summary>
        /// <param name="rtuId"></param>
        /// <returns></returns>
        public static int GetImageIdByRtuid(int rtuId)
        {

            if (RtuRegionInfo.ContainsKey(rtuId))
            {
                var tmp = RtuRegionInfo[rtuId];

                //解析图片id
                int imageIndex = 3005;

                switch (tmp.Item4)
                {
                    case 1:
                        //停运
                        imageIndex = 3002;
                        break;
                    case 2:
                        //不用
                        imageIndex = 3001;
                        break;
                    case 3:
                        //开灯正常
                        imageIndex = 3007;
                        break;
                    case 4:
                        //开灯故障
                        imageIndex = 3008;
                        break;
                    case 5:
                        //关灯正常
                        imageIndex = 3005;
                        break;
                    case 6:
                        //关灯故障
                        imageIndex = 3006;
                        break;
                    case 7:
                        //离线
                        imageIndex = 3003;
                        break;
                    default:
                        //正常关灯
                        imageIndex = 3005;
                        break;
                }

                return imageIndex;
            }
            return 3005;

        }




    }



    internal partial class GrpRegionInfoHoldExtend
    {
        /// <summary>
        /// 所有分组名称   分组类型：1-分组类型  2-区域id  3_分组id ， 分组名称
        /// </summary>
        public Dictionary<Tuple<int,int, int>, string> InfoRegionGroups = new Dictionary<Tuple<int,int, int>, string>();

        /// <summary>
        /// 区域下有哪些终端 areaid  grpid，region，rtuid，imageid
        /// </summary>
        public Dictionary<Tuple<int,int, int>, List<Tuple<int,int>>> RegionRtuInfo = new Dictionary<Tuple<int,int, int>, List<Tuple<int, int>>>();

        /// <summary>
        /// 终端属于哪个区域   rtuid， areaid, grpid，regionid,imageid
        /// </summary>
        public Dictionary<int, Tuple<int,int,int,int>> RtuRegionInfo = new Dictionary<int, Tuple<int,int, int,int>>();




        protected bool BolLoad = false;

        /// <summary>
        /// 是否与服务器同步了数据
        /// </summary>
        protected bool BolGetServerReturn = false;

        #region Simple Get informaiton


        /// <summary>
        /// 获取终端归属分组  区域-分组
        /// </summary>
        /// <param name="rtuId"></param>
        /// <returns>存在返回  null;areaid,grpid ,regionid,imageid</returns>
        public Tuple<int,int, int,int> GetRegionByRtuId(int rtuId)
        {
            if (RtuRegionInfo.ContainsKey(rtuId))
            {

                return RtuRegionInfo[rtuId];
            }
            return null;
        }

        /// <summary>
        /// 获取区域分组下 所有终端列表
        /// </summary>
        /// <param name="rtuId"></param>
        /// <returns>存在返回  null,   rtuid,imageid</returns>
        public List<Tuple<int, int>> GetRtusFromRegion(Tuple<int,int, int> tu)
        {
            if (RegionRtuInfo.ContainsKey(tu)) return RegionRtuInfo[tu];
            return null;
        }


        #endregion






    }



    /// <summary>
    /// 实现对分组信息的事件捕获与更新
    /// </summary>
    internal partial class GrpRegionInfoHoldExtend : EventHandlerHelperExtendNotifyProperyChanged //: GrpMultiInfoHoldingExtend
    {


        internal void InitEvent()
        {
            this.AddEventFilterInfo(100, PublishEventType.ReCn);

        }/// <summary>
         /// 事件数据处理
         /// </summary>
         /// <param name="args"></param>
        public override void ExPublishedEvent(PublishEventArgs args)
        {
            if (args.EventType == PublishEventType.ReCn)
            {
                RequestHttpRegionInfo();
                RequestHttpRtuRegion();
                return;
            }
        }


        private bool _bolinitload = false;
        /// <summary>
        /// 程序初始化时必须执行一次
        /// </summary>
        internal void InitLoad()
        {
            if (_bolinitload) return;
            _bolinitload = true;

            InitEvent();
            this.InitAction();
            //获取区域信息
            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(RequestHttpRegionInfo, 1);
            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(RequestHttpRtuRegion, 1);
            //RequestHttpRegionInfo();

            //RequestHttpRtuRegion();



        }

        private void InitAction()
        {

            ProtocolServer.RegistProtocol(Sr.ProtocolPhone.LxAreaGrp.wls_area_region_id_name_info,
                                       OnSvrRegionInfoArrive,
                                       typeof(GrpRegionInfoHoldExtend), this);


            ProtocolServer.RegistProtocol(Sr.ProtocolPhone.LxAreaGrp.wls_area_region_info,
                                       OnSvrRtuRegionInfoArrive,
                                       typeof(GrpRegionInfoHoldExtend), this);


        }


        /// <summary>
        /// 分组结构变化 lvf 2019年5月10日09:59:08
        /// </summary>
        /// <param name="session"></param>
        /// <param name="infos"></param>
        private void OnSvrRegionInfoArrive(string session, MsgWithMobile infos)
        {
            if (infos.WstAreagrpRegionIdNameInfo == null) return;
            //op:2 推送分组更新
            if (infos.WstAreagrpRegionIdNameInfo.Op != 2) return;

            if (infos.WstAreagrpRegionIdNameInfo.NameItems == null) return;

            var regionlst = infos.WstAreagrpRegionIdNameInfo.NameItems;

            //全部推送，先比对 暂时全刷
            if (regionlst.Count > 0)
            {

                InfoRegionGroups.Clear();

            }
            foreach (var g in regionlst)
            {
                //分组类型 areaid subid
                var tu = new Tuple<int, int,int>(g.Op,g.AreaId, g.SubId);
                if (InfoRegionGroups.ContainsKey(tu)) InfoRegionGroups[tu] = g.SubName;
                else InfoRegionGroups.Add(tu, g.SubName);
            }

            var arg = new PublishEventArgs()
            {
                EventId = EventIdAssign.RegionNeedUpdate,
                EventType = PublishEventType.Core,
                EventAttachInfo = "ChangeStructure",
            };
            EventPublish.PublishEvent(arg);
        }


        private void OnSvrRtuRegionInfoArrive(string session, MsgWithMobile infos)
        {
            if (infos.WstAreagrpRegionInfo == null) return;

          

            if (infos.WstAreagrpRegionInfo.RtuItems == null || infos.WstAreagrpRegionInfo.RtuItems.Count == 0) return;




            var rtulst = new List<int>();
            //
            bool opChangeImage = false;

            foreach (var k in infos.WstAreagrpRegionInfo.RtuItems)
            {
                //分组结构变化 
                if (k.OpOnupdate == 2)
                {
                    opChangeImage = false;
                    //var rtuinfos = infos.WstAreagrpRegionInfo.RtuItems;

                    //foreach (var k in rtuinfos)
                    {
                        var newGrpRegionId = new Tuple<int, int>(k.GroupId, k.RegionId);
                        if (RtuRegionInfo.ContainsKey(k.RtuId) == false)
                        {
                            RtuRegionInfo.Add(k.RtuId, new Tuple<int,int, int, int>(k.AreaId,k.GroupId, k.RegionId, k.ImageType));
                            //rtulst.Add(new Tuple<int,int, int, int>(k.RtuId,k.GroupId, k.RegionId, k.ImageType));
                            rtulst.Add(k.RtuId);
                        }
                        else
                        {
                            //更新终端信息缓存
                            var oldtu = RtuRegionInfo[k.RtuId];
                            var newtu = new Tuple<int,int, int, int>(k.AreaId,k.GroupId, k.RegionId, k.ImageType);
                            RtuRegionInfo[k.RtuId] = newtu;
                            //rtulst.Add(new Tuple<int, int, int, int>(k.RtuId, k.GroupId, k.RegionId, k.ImageType));
                            rtulst.Add(k.RtuId);

                            //更新地区终端缓存
                            var oldGrpRegion = new Tuple<int,int, int>(oldtu.Item1, oldtu.Item2,oldtu.Item3);
                            var oldinfo = new Tuple<int, int>(k.RtuId, oldtu.Item4);
                            var newGrpRegion = new Tuple<int, int,int>(k.AreaId,k.GroupId, k.RegionId);

                            //原有区域字典 删除该终端信息
                            var oldtmp = RegionRtuInfo[oldGrpRegion];
                            if (oldtmp.Contains(oldinfo)) oldtmp.Remove(oldinfo);
                            RegionRtuInfo[oldGrpRegion] = oldtmp;

                            //新区域添加该终端信息
                            if (RegionRtuInfo.ContainsKey(newGrpRegion))
                            {
                                var newtmp = RegionRtuInfo[newGrpRegion];
                                newtmp.Add(new Tuple<int, int>(k.RtuId, k.ImageType));
                                RegionRtuInfo[newGrpRegion] = newtmp;
                                //RegionRtuInfo[newGrpRegion].Add(new Tuple<int, int>(k.RtuId, k.ImageType));
                            }
                            else
                            {
                                var rtulsttmp = new List<Tuple<int, int>>();
                                rtulsttmp.Add(new Tuple<int, int>(k.RtuId, k.ImageType));
                                RegionRtuInfo.Add(newGrpRegion, rtulsttmp);
                            }



                        }

                    }

                    //var arg = new PublishEventArgs()
                    //{
                    //    EventId = EventIdAssign.RtuRegionNeedUpdate,
                    //    EventType = PublishEventType.Core,
                    //    EventAttachInfo = "ChangeStructure"

                    //};
                    //arg.AddParams(rtulst);
                    //EventPublish.PublishEvent(arg);

                }
                //图标变化 
                else if (k.OpOnupdate == 3)
                {
                    opChangeImage = true;
                 //  var rtuinfos = infos.WstAreagrpRegionInfo.RtuItems;

                    //foreach (var k in rtuinfos)
                    {
                        var newGrpRegionId = new Tuple<int, int>(k.GroupId, k.RegionId);
                        if (RtuRegionInfo.ContainsKey(k.RtuId) == false)
                        {
                            RtuRegionInfo.Add(k.RtuId, new Tuple<int,int, int, int>(k.AreaId,k.GroupId, k.RegionId, k.ImageType));
                            //rtulst.Add(new Tuple<int, int, int, int>(k.RtuId, k.GroupId, k.RegionId, k.ImageType));
                            rtulst.Add(k.RtuId);
                        }
                        else
                        {
                            //更新终端信息缓存
                            var oldtu = RtuRegionInfo[k.RtuId];
                            var newtu = new Tuple<int,int, int, int>(k.AreaId,k.GroupId, k.RegionId, k.ImageType);
                            RtuRegionInfo[k.RtuId] = newtu;
                            //rtulst.Add(new Tuple<int, int, int, int>(k.RtuId, k.GroupId, k.RegionId, k.ImageType));
                            rtulst.Add(k.RtuId);
                        }

                    }

                  
                }


            }
            var args = new PublishEventArgs()
            {
                EventId = EventIdAssign.RtuRegionNeedUpdate,
                EventType = PublishEventType.Core,
                EventAttachInfo = opChangeImage ? "ChangeImage" : "ChangeStructure",

            };
            args.AddParams(rtulst);
            EventPublish.PublishEvent(args);





            ////分组结构变化 
            //if (infos.WstAreagrpRegionInfo.Op == 2)
            //{

            //    var rtuinfos = infos.WstAreagrpRegionInfo.RtuItems;

            //    foreach (var k in rtuinfos)
            //    {
            //        var newGrpRegionId = new Tuple<int, int>(k.GroupId, k.RegionId);
            //        if (RtuRegionInfo.ContainsKey(k.RtuId) == false)
            //        {
            //            RtuRegionInfo.Add(k.RtuId, new Tuple<int, int, int>(k.GroupId, k.RegionId, k.ImageType));
            //            //rtulst.Add(new Tuple<int,int, int, int>(k.RtuId,k.GroupId, k.RegionId, k.ImageType));
            //            rtulst.Add(k.RtuId);
            //        }
            //        else
            //        {
            //            //更新终端信息缓存
            //            var oldtu = RtuRegionInfo[k.RtuId];
            //            var newtu = new Tuple<int, int, int>(k.GroupId, k.RegionId, k.ImageType);
            //            RtuRegionInfo[k.RtuId] = newtu;
            //            //rtulst.Add(new Tuple<int, int, int, int>(k.RtuId, k.GroupId, k.RegionId, k.ImageType));
            //            rtulst.Add(k.RtuId);

            //            //更新地区终端缓存
            //            var oldGrpRegion = new Tuple<int, int>(oldtu.Item1, oldtu.Item2);
            //            var oldinfo = new Tuple<int, int>(k.RtuId, oldtu.Item3);
            //            var newGrpRegion = new Tuple<int, int>(k.GroupId, k.RegionId);

            //            RegionRtuInfo[oldGrpRegion].Remove(oldinfo);
            //            RegionRtuInfo[newGrpRegion].Add(new Tuple<int, int>(k.RtuId, k.ImageType));


            //        }

            //    }

            //    var arg = new PublishEventArgs()
            //    {
            //        EventId = EventIdAssign.RtuRegionNeedUpdate,
            //        EventType = PublishEventType.Core,
            //        EventAttachInfo = "ChangeStructure"

            //    };
            //    arg.AddParams(rtulst);
            //    EventPublish.PublishEvent(arg);

            //}
            ////图标变化 
            //else if (infos.WstAreagrpRegionInfo.Op == 3)
            //{

            //    var rtuinfos = infos.WstAreagrpRegionInfo.RtuItems;

            //    foreach (var k in rtuinfos)
            //    {
            //        var newGrpRegionId = new Tuple<int, int>(k.GroupId, k.RegionId);
            //        if (RtuRegionInfo.ContainsKey(k.RtuId) == false)
            //        {
            //            RtuRegionInfo.Add(k.RtuId, new Tuple<int, int, int>(k.GroupId, k.RegionId, k.ImageType));
            //            //rtulst.Add(new Tuple<int, int, int, int>(k.RtuId, k.GroupId, k.RegionId, k.ImageType));
            //            rtulst.Add(k.RtuId);
            //        }
            //        else
            //        {
            //            //更新终端信息缓存
            //            var oldtu = RtuRegionInfo[k.RtuId];
            //            var newtu = new Tuple<int, int, int>(k.GroupId, k.RegionId, k.ImageType);
            //            RtuRegionInfo[k.RtuId] = newtu;
            //            //rtulst.Add(new Tuple<int, int, int, int>(k.RtuId, k.GroupId, k.RegionId, k.ImageType));
            //            rtulst.Add(k.RtuId);
            //        }

            //    }

            //    var arg = new PublishEventArgs()
            //    {
            //        EventId = EventIdAssign.RtuRegionNeedUpdate,
            //        EventType = PublishEventType.Core,
            //        EventAttachInfo = "ChangeImage"

            //    };
            //    arg.AddParams(rtulst);
            //    EventPublish.PublishEvent(arg);

            //}


        }


        long lastdatatime = 0;
        /// <summary>
        /// 请求地区信息 lvf 2019年5月6日16:37:52
        /// </summary>
        public void RequestHttpRegionInfo()
        {
            var info = Wlst.Sr.ProtocolPhone.LxAreaGrp.wls_area_region_id_name_info;
            //1. 请求
            info.WstAreagrpRegionIdNameInfo.Op =1;


            //序列化，请求数据结构
            var base64data = System.Convert.ToBase64String(MsgWithMobile.SerializeToBytes(info));

            //http get
            //var url = "http://" + Wlst.Sr.EquipmentInfoHolding.Services.Others.SeverIpAddr + ":" + Wlst.Sr.EquipmentInfoHolding.Services.Others.SeverHttpPort + "/mims/wls_area_region_id_name_info_http";
            var url = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.HttpUrl + "wls_area_region_id_name_info_http";
            var data = wlst.sr.iif.HttpGetPost.HttpGet(url, "?pb2=" + base64data);
            //var data1 = wlst.sr.iif.HttpGetPost.HttpPost(url , "pb2=" + base64data);

            if (data == null) return;
            // 反序列化get到的数据
            var databk = MsgWithMobile.Deserialize(System.Convert.FromBase64String(data));
            if (databk == null || databk.WstAreagrpRegionIdNameInfo == null) return;

            var regionlst = databk.WstAreagrpRegionIdNameInfo.NameItems;
            if (regionlst.Count > 0) InfoRegionGroups.Clear();
            foreach(var g in regionlst)
            {
                var tu = new Tuple<int,int, int>(g.Op,g.AreaId, g.SubId);
                if (InfoRegionGroups.ContainsKey(tu)) InfoRegionGroups[tu] = g.SubName;
                else InfoRegionGroups.Add(tu, g.SubName);
            }


            var arg = new PublishEventArgs()
            {
                EventId = EventIdAssign.RegionNeedUpdate,
                EventType = PublishEventType.Core,
                EventAttachInfo = "ChangeStructure",
            };
            EventPublish.PublishEvent(arg);



        }


        public void RequestHttpRtuRegion()
        {
            //每5分钟 请求一次终端信息
            Wlst.Cr.Coreb.AsyncTask.Qtz.AddQtz("null", 8888, DateTime.Now.Ticks + 20000000,300, RequestHttpRtuRegionInfo);

        }

        /// <summary>
        /// 请求终端信息  5分钟 请求一次
        /// </summary>
        public void RequestHttpRtuRegionInfo(object obj)
        {
            var info = Wlst.Sr.ProtocolPhone.LxAreaGrp.wls_area_region_info;
            //1. 请求所有设备列表
            info.WstAreagrpRegionInfo.Op = 4;
            info.WstAreagrpRegionInfo.DtUpdate = lastdatatime;

            //序列化，请求数据结构
            var base64data = System.Convert.ToBase64String(MsgWithMobile.SerializeToBytes(info));

            //http get
            //var url = "http://" + Wlst.Sr.EquipmentInfoHolding.Services.Others.SeverIpAddr + ":" + Wlst.Sr.EquipmentInfoHolding.Services.Others.SeverHttpPort + "/mims/wls_area_region_info_http";//"http://10.3.9.8:18080/mims/get10010"
            var url = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.HttpUrl + "wls_area_region_info_http";
            var data = wlst.sr.iif.HttpGetPost.HttpGet(url, "?pb2=" + base64data);
            //var data1 = wlst.sr.iif.HttpGetPost.HttpPost(url , "pb2=" + base64data);

            if (data == null) return;
            // 反序列化get到的数据
            var databk = MsgWithMobile.Deserialize(System.Convert.FromBase64String(data));
            if (databk == null || databk.WstAreagrpRegionInfo == null) return;
            lastdatatime = databk.WstAreagrpRegionInfo.DtUpdate;
            var rtulst = databk.WstAreagrpRegionInfo.RtuItemsMd5;
            var packRtu = new List<List<int>>();
            int index = 0;
            var ptmp = new List<int>();
            //int packageNum = rtulst.Count / 400;
            //if (rtulst.Count % 400 > 0) packageNum++;

            if (rtulst.Count == 0) return;
            //400个终端一包 打包
            foreach (var g in rtulst)
            {
                ptmp.Add(g.RtuId);
                index++;
                if (index == 400)
                {
                    var rtulstt = new List<int>();
                    foreach (var k in ptmp) rtulstt.Add(k);
                    packRtu.Add(rtulstt);
                    ptmp.Clear();
                    index = 0;
                }
            }
            var rtulsttt = new List<int>();
            foreach (var k in ptmp) rtulsttt.Add(k);
            packRtu.Add(rtulsttt);
            
            //分页请求终端数据 400个终端一批
            foreach(var g in packRtu)
            {
                var infos = Wlst.Sr.ProtocolPhone.LxAreaGrp.wls_area_region_info;
                //1. 请求需要更新的终端列表
                infos.WstAreagrpRegionInfo.Op = 1;
                infos.WstAreagrpRegionInfo.DtUpdate = lastdatatime;

                var lst = new List<RtuGroupItemInfoForChangsha.RtuMd5ForChangsha>();
                foreach(var k in g)
                {
                    lst.Add(new RtuGroupItemInfoForChangsha.RtuMd5ForChangsha() { RtuId = k});
                }
                infos.WstAreagrpRegionInfo.RtuItemsMd5 = lst;

                //序列化，请求数据结构
                var base64datas = System.Convert.ToBase64String(MsgWithMobile.SerializeToBytes(infos));

                //http get
                //var urls = "http://" + Wlst.Sr.EquipmentInfoHolding.Services.Others.SeverIpAddr + ":" + Wlst.Sr.EquipmentInfoHolding.Services.Others.SeverHttpPort + "/mims/wls_area_region_info_http";//"http://10.3.9.8:18080/mims/get10010"

                var urls = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.HttpUrl + "wls_area_region_info_http";
                var datas = wlst.sr.iif.HttpGetPost.HttpGet(urls, "?pb2=" + base64datas);
                //var data1 = wlst.sr.iif.HttpGetPost.HttpPost(url , "pb2=" + base64data);

                if (datas == null) return;
                // 反序列化get到的数据
                var databks = MsgWithMobile.Deserialize(System.Convert.FromBase64String(datas));
                if (databks == null || databks.WstAreagrpRegionInfo == null) return;
                lastdatatime = databks.WstAreagrpRegionInfo.DtUpdate;
                var rtuinfos = databks.WstAreagrpRegionInfo.RtuItems;

                foreach (var k in rtuinfos)
                {
                    var grptu = new Tuple<int,int, int>(k.AreaId,k.GroupId,k.RegionId);

                    //lvf 2019年5月7日13:38:49 记录 地区下 有多少终端
                    if (RegionRtuInfo.ContainsKey(grptu)==false)
                    {
                        var rtulsttmp = new List<Tuple<int, int>>();
                        rtulsttmp.Add(new Tuple<int, int>(k.RtuId,k.ImageType));
                        RegionRtuInfo.Add(grptu, rtulsttmp);

                    }else
                    {
                        var rtulsttmp = RegionRtuInfo[grptu];

                        //如果有这终端，先删掉
                        for (int i = rtulsttmp.Count - 1; i >= 0; i--) {

                            if (rtulsttmp[i].Item1 == k.RtuId) rtulsttmp.RemoveAt(i);

                        }

                        //添加新的
                        rtulsttmp.Add(new Tuple<int, int>(k.RtuId, k.ImageType));
                        RegionRtuInfo[grptu] =rtulsttmp;
                    }

                    // 记录终端属于哪个地区

                   if(RtuRegionInfo.ContainsKey(k.RtuId)==false)
                    {
                        RtuRegionInfo.Add(k.RtuId, new Tuple<int,int, int,int>(k.AreaId,k.GroupId, k.RegionId,k.ImageType));
                    }else
                    {
                        RtuRegionInfo[k.RtuId] = new Tuple<int,int, int,int>(k.AreaId,k.GroupId, k.RegionId,k.ImageType);

                    }





                }
            }


            var arg = new PublishEventArgs()
            {
                EventId = EventIdAssign.RtuRegionNeedUpdate,
                EventType = PublishEventType.Core,
                EventAttachInfo = "ChangeImage" ,

            };


            var rt = (from t in rtulst orderby t.RtuId select t.RtuId).ToList();
            arg.AddParams(rt);
            EventPublish.PublishEvent(arg);

        }
    

    }


}
