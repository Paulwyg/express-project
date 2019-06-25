using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using GMap.NET;
using GMap.NET.WindowsPresentation;
//using Microsoft.Practices.Prism.MefExtensions.Event;
//using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using Telerik.Windows.Controls.Map;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Ux.RadMapJpeg.MapJepg.Services;
using Wlst.Ux.RadMapJpeg.Views;
using WriteLog = Wlst.Cr.Core.UtilityFunction.WriteLog;

namespace Wlst.Ux.RadMapJpeg.MapJepg.ViewModels
{
    [Export(typeof (IIRadMapJpeg))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class RadMapJpegViewModel : ObservableObject, IIRadMapJpeg
    {
        private GMapControl Gmap;

        public void SetGmap(GMapControl map)
        {
            Gmap = map;
            

            Gmap.MinZoom = RadMapJpegViewModelSet.MySelf.MinZoomInConfig;
            Gmap.MaxZoom = RadMapJpegViewModelSet.MySelf.MaxZoomInConfig;

            Gmap.Zoom = RadMapJpegViewModelSet.MySelf.zoomlevel;
            Gmap.Position = new PointLatLng(RadMapJpegViewModelSet.MySelf.mapy, RadMapJpegViewModelSet.MySelf.mapx);
            RadMapJpegViewModelSet.SetGmap(map);

            this.NavOnLoad();
        }

        private ObservableCollection<CustomMarkerDemo> _equipmentList;

        public ObservableCollection<CustomMarkerDemo> EquipmentList
        {
            get
            {
                if (_equipmentList == null) _equipmentList = new ObservableCollection<CustomMarkerDemo>();
                return _equipmentList;
            }
        }



        public RadMapJpegViewModel()
        {

            
            EventPublish.AddEventTokener(
                Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandlers, FundOrderFilters);
            NavOnLoad();
        }


        public void NavOnLoad(params object[] parsObjects)
        {
            LoadMainEquipmentInfo();
        }

        //加载终端节点
        private void LoadMainEquipmentInfo()
        {
            int x = 0;
            EquipmentList.Clear();
            if (Gmap == null) return;
            Gmap.Markers.Clear();


            int index = 1;
            //main equipment
            foreach (
                var f in Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems .Values)
            {
                try
                {
                    if (f.RtuFid  > 0) continue;

                    x++;
                    if (f.RtuMapX .Equals(0))
                    {
                        f.RtuMapX = Gmap.Position.Lng + (DateTime.Now.Ticks % 100) * 0.01;
                    }
                    if (f.RtuMapY .Equals(0))
                    {
                        f.RtuMapY = Gmap.Position.Lat + (DateTime.Now.Ticks % 100) * 0.01;
                    }

                //    var tmx = WGSGCJLatLonHelper.WGS84ToGCJ02(new LatLngPoint(  f.Ymap , f.Xmap));

                   // GMapMarker it = new GMapMarker(new PointLatLng(f.Xmap, f.Ymap));
                    GMapMarker it = new GMapMarker(new PointLatLng(f.RtuMapX > f.RtuMapY ? f.RtuMapY : f.RtuMapX, f.RtuMapX > f.RtuMapY ? f.RtuMapX : f.RtuMapY));

                    var tmp = new CustomMarkerDemo(Gmap, it, f.RtuId)
                                  {
                                      //EquipmentImageState = 2,
                                      Visi = Visibility.Visible,
                                      EquipmentName = f.RtuName
                                  };

                    it.ZIndex = 1000 + index++;
                    it.Shape = tmp;



                    EquipmentList.Add(tmp);
                    tmp.UpdateTmlStateInfomation();
                    Gmap.Markers.Add(it);
                }
                catch (Exception ex)
                {
                    WriteLog.WriteLogError("RadMapJpeg LoadEquipmentInfo Error when conver to IIEquipmentInfo:" + ex);
                }
            }

            return;
            //异步显示 
            Thread th = new Thread(runx);
            th.Start();
        }

        private bool runxrf = false;
        void runx()
        {

            int fx = 1;
            Interlocked.Increment(ref fx);
            Thread.Sleep(5000);
            for (int i = 0; i < 9999; i++)
            {
                int rfx = i/1000;
                Thread .Sleep(rfx*3+8);

                Application.Current.Dispatcher.BeginInvoke(new Action(
                                                               () =>
                                                                   {
                                                                       runxrf = true;
                                                                       int ifx = Interlocked.Increment(ref fx);
                                                                       ;
                                                                       double lat = 31;
                                                                       double lng = 120;

                                                                       int xfr = ifx%60;
                                                                       int yfr = ifx/60;

                                                                       lat += yfr*0.001;
                                                                       lng += xfr*0.001;

                                                                      
                                                                       //GMapMarker it =
                                                                       //    new GMapMarker(new PointLatLng(lat, lng));
                                                                       GMapMarker it =
                                                                           new GMapMarker(new PointLatLng(lat > lng ? lng : lat, lat > lng ? lat  : lng ));

                                                                       var tmp = new CustomMarkerDemo(Gmap, it, ifx)
                                                                                     {
                                                                                         //EquipmentImageState = 2,
                                                                                         Visi = Visibility.Visible,
                                                                                         EquipmentName = "RtuX" + ifx,
                                                                                         EquipmentImageState = 3012
                                                                                     };

                                                                       it.ZIndex = 10000 + ifx++;
                                                                       it.Shape = tmp;



                                                                       Gmap.Markers.Add(it);
                                                                       runxrf = false;
                                                                   }));
            }

        }


        #region IEventAggregator Subscription



        public void FundEventHandlers(PublishEventArgs args)
        {
            try
            {
                if (args.EventType == PublishEventType.SvAv)
                {
                    if (EquipmentList.Count == 0) this.NavOnLoad();
                    return;
                }
                if (args.EventType == PublishEventType.Core)
                {
                    if (args.EventId == EventIdAssign.EquipmentAddEventId)
                    {
                        var lst = args.GetParams()[0] as List<Tuple<int, int>>;
                        if (lst == null) return;
                        foreach (var t in lst)
                        {
                            if (t.Item2 == 0)
                                AddMainEquipment(t.Item1);
                        }
                        return;
                    }
                    if (args.EventId == EventIdAssign.EquipmentDeleteEventId)
                    {
                        var lst = args.GetParams()[0] as List<Tuple<int, int>>;
                        if (lst == null) return;
                        foreach (var t in lst)
                        {
                            if (t.Item2 == 0)
                                DeleteMainEquipment(t.Item1);
                        }
                        return;
                    }

                    if (args.EventId == EventIdAssign.EquipmentUpdateEventId)
                    {
                        var lst = args.GetParams()[0] as List<Tuple<int, int>>;
                        if (lst == null || lst.Count == 0) return;
                        foreach (var t in lst)
                        {
                            if (t.Item2 == 0)
                                UpdateMainEquipment(t.Item1);
                        }

                        return;
                    }
                    if (args.EventId == EventIdAssign.EquipentxyPositonUpdateId)
                    {
                        int rtuid = Convert.ToInt32(args.GetParams()[0]);

                        if (!Wlst .Sr .EquipmentInfoHolding .Services .EquipmentDataInfoHold .InfoItems .ContainsKey(rtuid))
                            return;
                        if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuid].RtuFid  > 0) return;

                        var f =
                           Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuid];
                        foreach (var t in EquipmentList)
                        {
                            if (t.EquipmentRtuId == rtuid)
                            {
                                t.EquipmentName = f.RtuName;

                                
                               // GMapMarker it = new GMapMarker(new PointLatLng(tmx.LatY, tmx.LonX));


                                t.UpdateEquipmentLocationLo(f.RtuMapX  ,f.RtuMapY  );
                            }
                        }

                    }
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                    {
                        int x = Convert.ToInt32(args.GetParams()[0]);
                        //if (x < 1000000 || x > 1100000) return;

                        //OnSelectedShowInfo(x);
                        if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsRtuIsRtuLight(x)) OnSelectNodeChangeByTree(x); 
                      else  if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsSlu(x)) OnSelectNodeChangeByTree(x); 
                       else  return;
                    }
                    if (args.EventId == EventIdAssign.RunningInfoUpdate1 || args.EventId == EventIdAssign.RunningInfoUpdate2)
                    {
                        var lst = args.GetParams()[0] as List<int>;
                        if (lst == null) return;
                        foreach (var t in EquipmentList)
                        {
                            if (lst.Contains(t.EquipmentRtuId)) t.UpdateTmlStateInfomation();
                        }
                    }



                    //if (args.EventId == Sr.EquipmentGroupInfoHolding.Services.EventIdAssign.MainSingleTreeNodeActive)
                    //{
                    //    int x = Convert.ToInt32(args.GetParams()[0]);
                    //    int y = Convert.ToInt32(args.GetParams()[1]);
                    //    if (y == 2)
                    //    {
                    //        OnSelectNodeChangeByTree(x);
                    //    }
                    //}
                    //if (args.EventId == Sr.EquipemntLightFault.Services.EventIdAssign.RtuErrorStateChanged)
                    //    try
                    //    {

                    //        var lst = args.GetParams()[0] as List<Tuple<int, bool>>;
                    //        if (lst == null) return;
                    //        var tmps = (from t in lst select t.Item1).ToList();
                    //        foreach (var t in EquipmentList)
                    //        {
                    //            if (tmps.Contains(t.EquipmentRtuId))
                    //            {
                    //                t.UpdateTmlStateInfomation();
                    //            }
                    //        }

                    //    }
                    //    catch (Exception ex)
                    //    {
                    //    }
                    //if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.TmlRunningInfoChange )
                    //    try
                    //    {
                    //        if (args.GetParams().Count > 1)
                    //        {
                    //            try
                    //            {
                    //                //int x1 = Convert.ToInt32(args.GetParams()[0]);
                    //                //// int x2 = Convert.ToInt32(args.GetParams()[1]);
                    //                ////UpdateTmlStateInfomation(x1, x2);
                    //                //foreach (var t in EquipmentList)
                    //                //    if (t.EquipmentRtuId == x1)
                    //                //    {
                    //                //        t.UpdateTmlStateInfomation();
                    //                //        break;
                    //                //    }


                    //                var lst = args.GetParams()[0] as List<Tuple<int, bool>>;
                    //                if (lst == null) return;
                    //                var tmps = (from t in lst select t.Item1).ToList();
                    //                foreach (var t in EquipmentList)
                    //                {
                    //                    if (tmps.Contains(t.EquipmentRtuId))
                    //                    {
                    //                        t.UpdateTmlStateInfomation();
                    //                    }
                    //                }

                    //            }
                    //            catch (Exception ex)
                    //            {

                    //            }

                    //        }

                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
                    //    }
                    if (args.EventId == Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuGroupSelectdWantedMapUp)
                        try
                        {
                            if(args.GetParams().Count==0)
                            {
                                foreach (var t in EquipmentList)
                                {
                                    t.ImgVis = Visibility.Visible;

                                }
                                return;
                                
                            }
                            var lst = args.GetParams()[0] as List<int>;
                            if (lst == null) return;
                            if (lst.Count == 0)
                            {
                                foreach (var t in EquipmentList)
                                {
                                    t.ImgVis = Visibility.Visible;

                                }
                            }
                            else
                            {
                                foreach (var t in EquipmentList)
                                {
                                    t.ImgVis = lst.Contains(t.EquipmentRtuId)
                                                   ? Visibility.Visible
                                                   : Visibility.Collapsed;
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                        }
                }
                // int indentfyId = Convert.ToInt32(args.GetParams()[0]);
            }
            catch (Exception xe)
            {
                WriteLog.WriteLogError("RadMapJpeg error in FundEventHandlers:ex:" + xe);
            }
        }





        public void AddMainEquipment(int rtuId)
        {
            try
            {
                if (
                    !Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems .
                         ContainsKey(rtuId))
                    return;
                if (
                    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuId].
                        RtuFid  != 0)
                    return;
                foreach (var t in EquipmentList)
                {
                    if (t.EquipmentRtuId == rtuId) return;
                }

                var f =
                    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[
                        rtuId]
                    ;
                // if (f.RtuModel != 1050) return;

                if (f.RtuMapX .Equals(0))
                {
                    f.RtuMapX = Gmap.Position.Lng + (DateTime.Now.Ticks % 100) * 0.01;
                }
                if (f.RtuMapY .Equals(0))
                {
                    f.RtuMapY = Gmap.Position.Lat + (DateTime.Now.Ticks % 100) * 0.01;
                }

                GMapMarker it = new GMapMarker(new PointLatLng(f.RtuMapX > f.RtuMapY ? f.RtuMapY : f.RtuMapX, f.RtuMapX > f.RtuMapY ? f.RtuMapX : f.RtuMapY));

             //   GMapMarker it = new GMapMarker(new PointLatLng(f.Ymap, f.Xmap));

                var tmp = new CustomMarkerDemo(Gmap, it, f.RtuId)
                              {
                                  //EquipmentImageState = 2,
                                  Visi = Visibility.Visible,
                                  EquipmentName = f.RtuName
                              };
                it.ZIndex = 1000 + Gmap.Markers.Count + 1;
                it.Shape = tmp;



                EquipmentList.Add(tmp);
                tmp.UpdateTmlStateInfomation();
                Gmap.Markers.Add(it);


            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError(
                    "RadMapJpeg LoadAttachEquipmentInfo Error when conver to IIEquipmentInfo:" + ex);
            }
        }

        public void DeleteMainEquipment(int rtuId)
        {
            foreach (var t in EquipmentList)
            {
                if (t.EquipmentRtuId == rtuId)
                {
                    foreach (var f in Gmap.Markers)
                    {
                        int rtuid = 0;
                        if (Int32.TryParse(f.Tag.ToString(), out rtuid))
                        {
                            if (rtuid == rtuId)
                            {
                                Gmap.Markers.Remove(f);
                                break;
                            }
                        }
                    }

                    EquipmentList.Remove(t);
                    break;
                }
            }
        }

        public void ReloadMainEquipment()
        {
            LoadMainEquipmentInfo();
        }


        public void UpdateMainEquipment(int rtuId)
        {
            if (
                !Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems .
                     ContainsKey(rtuId))
                return;
            if (EquipmentDataInfoHold.InfoItems[rtuId].RtuFid  > 0) return;

            var f =
                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[
                    rtuId];

            //if (f.RtuModel != 1050) return;
            foreach (var t in EquipmentList)
            {
                if (t.EquipmentRtuId == rtuId)
                {
                    //  if (f.PhyId == 807) continue;
                    t.EquipmentName  = f.RtuName;
                    if (f.RtuMapX < 0.001) f.RtuMapX = 0.001;
                    if (f.RtuMapY < 0.001) f.RtuMapY = 0.001;

                  //  var tmx = WGSGCJLatLonHelper.WGS84ToGCJ02(new LatLngPoint(f.Ymap, f.Xmap));
                    t.UpdateEquipmentLocationLo(f.RtuMapX, f.RtuMapY);
                    t._phyId = f.RtuPhyId;
                   
                    t.UpdateTmlStateInfomation();
                }
            }
        }



        private void OnSelectNodeChangeByTree(int nodeId)
        {
            foreach (var t in this.EquipmentList)
            {
                if (t.EquipmentRtuId == nodeId)
                {
                    t.OnEquipmentSelected(true);
                }
            }
        }


        //void OnSelectedShowInfo(int rtuid)
        //{
        //    if (Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2801, 15, 1) == 1)
        //    {
        //        RtuInfo = "";
        //        return;
        //    }
        //    var rtus = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(rtuid);
        //    if (rtus == null)
        //    {
        //        RtuInfo = "";
        //        return;
        //    }
        //    RtuInfo = rtus.RtuId + " - " + rtus.RtuPhyId.ToString("d2") + " 终端名:" + rtus.RtuName;

        //    string groupName = string.Empty;

        //    var groupidx =
        //        Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuBelongGrp(rtuid);
        //    if (groupidx != null)
        //    {
        //        var infosss =
        //            Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGroupInfomation(
        //                groupidx.Item1, groupidx.Item2);
        //        if (infosss != null) RtuInfo +="  归属分组:"+ infosss.GroupName;
        //    }

        //    if (Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2801, 15, 1) == 2)
        //    {
        //        //将查询出来的第一个故障 显示在 第一列最后
        //        var ntgx = (from t in Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary
        //                    where t.Value.RtuId == rtuid
        //                    select t.Value).FirstOrDefault();
        //        if (ntgx != null)
        //        {
        //            RtuInfo +=   "  当前故障:" + ntgx.FaultName;
        //        }
        //    }
        //    else if (Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2801, 15, 1) == 3)
        //    {
        //        RtuInfo += "  备注:" + rtus.RtuRemark;
        //    }
        //}

        private string  _currentSelectNode;

        public string  RtuInfo
        {
            get
            {

                return _currentSelectNode;
            }
            set
            {
                if (_currentSelectNode != value)
                {
                    _currentSelectNode = value;
                    this.RaisePropertyChanged(() => this.RtuInfo);
                }
            }
        }


        private ConcurrentDictionary<int, Tuple<int, string, string>> _state = new ConcurrentDictionary<int, Tuple<int, string, string>>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool FundOrderFilters(PublishEventArgs args) //接收终端选中变更事件
        {
            try
            {
                if (args.EventType == PublishEventType.SvAv) return true;
                if (args.EventType == PublishEventType.Core)
                {
                    if (args.EventId == EventIdAssign.EquipmentAddEventId)
                        return true;
                    if (args.EventId == EventIdAssign.EquipmentDeleteEventId)
                        return true;

                    if (args.EventId == EventIdAssign.EquipmentUpdateEventId)
                        return true;
                    if (args.EventId == EventIdAssign.EquipentxyPositonUpdateId)
                        return true;

                    if (RadMapJpegViewModelSet.MySelf.CanNavToEquImage &&
                        args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                        return true;

                    //if (args.EventId == Sr.EquipemntLightFault.Services.EventIdAssign.RtuErrorStateChanged)
                    //    return true;
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.RunningInfoUpdate1 || args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.RunningInfoUpdate2)
                    {


                        var lst = args.GetParams()[0] as List<int>;
                        if (lst == null) return false;
                        foreach (var t in EquipmentList)
                        {
                            if (lst.Contains(t.EquipmentRtuId))
                            {
                                if (t.CanUpdateImage()) return true;
                            }
                        }
                        return false;



                    }
                    if (args.EventId == Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuGroupSelectdWantedMapUp)
                        return true;
                }
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
            return false;
        }

        #endregion


        #region tab iinterface

        public int Index
        {
            get { return 1; }
        }

        public string Title
        {
            get
            {
                //  return I36N .Services.I36N .ConvertByCodingOne("11010001", "Map");
                return "地图";
            }
        }

        public bool CanClose
        {
            get { return false; }
        }

        /// <summary>
        /// <c>True</c> if this instance can pin; otherwise, <c>False</c>.
        /// 是否可锁定
        /// </summary>
        public bool CanUserPin
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this pane can float; otherwise, <c>false</c>.
        /// 是否可悬浮
        /// </summary>
        public bool CanFloat
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this instance can dock in the document host; otherwise, <c>false</c>.
        /// 是否可移动至document host
        /// </summary>
        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        #endregion
    }

    public  class RadMapJpegViewModelSet
    {
        private static RadMapJpegViewModelSet _mySelf;

        public static RadMapJpegViewModelSet MySelf
        {
            get
            {
                if (_mySelf == null) _mySelf = new RadMapJpegViewModelSet();
                return _mySelf;
            }
        }

        private bool _loadCOnfig = false;
        private const string XmlConfigName = "JpegSetConfg";

        protected RadMapJpegViewModelSet()
        {
            if (_loadCOnfig) return;
            if (_mySelf == null) _mySelf = this;
            _loadCOnfig = true;

            var info = Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read(XmlConfigName);


            if (info.ContainsKey("CanNavToEquImage"))
            {
                CanNavToEquImage = info["CanNavToEquImage"].Contains("yes");
            }
            else CanNavToEquImage = true;


            if (info.ContainsKey("ImageMinZoom"))
            {
                try
                {
                    MinZoomInConfig = Convert.ToInt32(info["ImageMinZoom"]);
                }
                catch (Exception ex)
                {
                }

            }

            if (info.ContainsKey("MaxZoomInConfig"))
            {
                try
                {
                    MaxZoomInConfig = Convert.ToInt32(info["MaxZoomInConfig"]);
                }
                catch (Exception ex)
                {
                }

            }

            if (info.ContainsKey("IsUserNetMap"))
            {
                IsUserNetMap = info["IsUserNetMap"].Contains("yes");
            }
            else IsUserNetMap = true;

            //lvf 2018年10月12日09:38:42 更改为读取   系统配置文件  3302 ，4-5
            //if (info.ContainsKey("mapx"))
            //{
            //    try
            //    {
            //        mapx = Convert.ToDouble(info["mapx"]);
            //    }
            //    catch (Exception ex)
            //    {
            //    }

            //}
            //if (info.ContainsKey("mapy"))
            //{
            //    try
            //    {
            //        mapy = Convert.ToDouble(info["mapy"]);
            //    }
            //    catch (Exception ex)
            //    {
            //    }

            //}
            if (info.ContainsKey("zoomlevel"))
            {
                try
                {
                    zoomlevel = Convert.ToDouble(info["zoomlevel"]);
                }
                catch (Exception ex)
                {
                }

            }
            if (info.ContainsKey("usebaidu"))
            {
                try
                {
                    usebaidu = Convert.ToInt32(info["usebaidu"]);
                }
                catch (Exception ex)
                {
                }

            }

            mapx =  Convert.ToDouble(Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOption(3302, 4, "0", null).Trim());
            mapy = Convert.ToDouble(Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOption(3302, 5, "0", null).Trim());

        }

        public void SavConfig()
        {
            var info = new Dictionary<string, string>();

            if (Gmap != null)
            {
                mapx = Gmap.Position.Lng; // CustomMarkerDemo.CurrentRtu.Marker.Position.Lng;
                mapy = Gmap.Position.Lat; // CustomMarkerDemo.CurrentRtu.Marker.Position.Lat;
                zoomlevel = Gmap.Zoom;
            }

            if (IsUserNetMap) info.Add("IsUserNetMap", "yes");
            else info.Add("IsUserNetMap", "no");

            if (CanNavToEquImage) info.Add("CanNavToEquImage", "yes");
            else info.Add("CanNavToEquImage", "no");



            info.Add("ImageMinZoom", MinZoomInConfig + "");
            info.Add("MaxZoomInConfig", MaxZoomInConfig + "");
            info.Add("mapx", mapx + "");
            info.Add("mapy", mapy + "");
            info.Add("zoomlevel", zoomlevel + "");
            info.Add("usebaidu", usebaidu + "");
            Wlst.Cr.CoreOne.Services.SystemXmlConfig.Save(info, XmlConfigName);

           
            if (Gmap != null)
            {
                if (IsUserNetMap) Gmap.Manager.Mode = AccessMode.ServerAndCache;
                else Gmap.Manager.Mode = AccessMode.CacheOnly;
            }
        }


        private static GMapControl Gmap;
        public static void SetGmap(GMapControl gmpa)
        {
            Gmap = gmpa;

        }

        /// <summary>
        /// 是否与终端树联动
        /// </summary>
        public bool CanNavToEquImage;

        public int MinZoomInConfig = 4;

        public int MaxZoomInConfig = 19;

        public bool IsUserNetMap = false;

        public double mapx = 0;
        public double mapy = 0;
        public double zoomlevel = 10;

        public int usebaidu = 1;

    }
}