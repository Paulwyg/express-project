using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Windows;
using Microsoft.Practices.Prism.MefExtensions.Event;
using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using Telerik.Windows.Controls.Map;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.UtilityFunction;
using Wlst.Cr.WjEquipmentBaseModels.Interface;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Ux.RadMapJpeg.MapJepg.Services;

namespace Wlst.Ux.RadMapJpeg.MapJepg.ViewModels
{
    [Export(typeof(IIRadMapJpeg))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial  class RadMapJpegViewModel : ObservableObject, IIRadMapJpeg
    {
        private ObservableCollection<MapNodeViewModel> _mianEquipmentList;
        public  const int IconWidths = 18;
        public const int IconWidthsBig = 24;

        public ObservableCollection<MapNodeViewModel> MainEquipmentList
        {
            get
            {
                if (_mianEquipmentList == null) _mianEquipmentList = new ObservableCollection<MapNodeViewModel>();
                return _mianEquipmentList;
            }
        }
        
        private ObservableCollection<MapNodeViewModel> _attachEquipmentList;

        public ObservableCollection<MapNodeViewModel> AttachEquipmentList
        {
            get
            {
                if (_attachEquipmentList == null) _attachEquipmentList = new ObservableCollection<MapNodeViewModel>();
                return _attachEquipmentList;
            }
        }

        public RadMapJpegViewModel()
        {
            //Uri str = new Uri("../../Maps/osm_{zoom}.png");
            //var t = str;
            LoadConfig();
            EventPublisher.AddEventSubScriptionTokener(
                Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandlers, FundOrderFilters);
            NavOnLoad();
        }


        public void NavOnLoad(params object[] parsObjects)
        {
            LoadMainEquipmentInfo();
            //LoadAttachEquipmentInfo();
        }

        //加载终端节点
        private void LoadMainEquipmentInfo()
        {
            int x = 0;
            MainEquipmentList.Clear();
           // ZoomLevel = 16;

            Random rand = new Random();
            int min = 0;
            int max = 999999;

                //int a = rand.Next(min, max);
            
            //main equipment
            foreach (
                var f in Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary .Values)
            {
                try
                {
                    if (f.AttachRtuId > 0) continue;
                  
                    x++;
                    if (f.Xmap  .Equals(0) )
                    {
               
                        f.Xmap =0.0001;
                        //if (f.Xgis > 0.25) f.Xgis = f.Xgis - 0.25;
                    }
                    if (f.Ymap.Equals(0))
                    {
                        
                        f.Ymap =  0.0001;
                        //if (f.Ygis > 0.20) f.Ygis = f.Ygis - 0.20;
                    }
                    

                    var ggg = new MapNodeViewModel()
                                  {
                                      EquipmentRtuId = f.RtuId,
                                      EquipmentLocation = new Location(f.Xmap, f.Ymap),
                                      Visi = Visibility.Visible,
                                      EquipmentName = f.RtuName
                                  };
                   

                    MainEquipmentList.Add(ggg); 
                    ggg .UpdateTmlStateInfomation(); 
                }
                catch (Exception ex)
                {
                    WriteLog.WriteLogError("RadMapJpeg LoadEquipmentInfo Error when conver to IIEquipmentInfo:" + ex);
                }
            }
        }

        //private void LoadAttachEquipmentInfo()
        //{
        //    AttachEquipmentList.Clear();
        //    int x = 0;
        //    //add 勒克司
        //    if (Setting.Setting.IsShowLuxImageOnJpegMap)
        //    {
        //        foreach (var f in
        //            Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary .Values)
        //        {
        //            try
        //            {
        //                //var k = t.Value.Instances as WjEquipmentModels.Interface.IILux;
        //                //if (k == null) continue;
        //                if (f.RtuModel != 1050) continue;
        //                x++;
        //                if (f.Xmap.Equals(0))
        //                {
        //                    f.Xmap = x*0.0002;
        //                    if (f.Xmap > 0.3) f.Xmap = f.Xmap - 0.3;
        //                }
        //                if (f.Ymap.Equals(0))
        //                {
        //                    f.Ymap = x*0.0002;
        //                    if (f.Ymap > 0.3) f.Ymap = f.Ymap - 0.3;
        //                }

        //                var ggg = new MapNodeViewModel()
        //                              {
        //                                  EquipmentRtuId = f.RtuId,
        //                                  EquipmentLocation = new Location(f.Xmap, f.Ymap),
        //                                  EquipmentImageState = 2,
        //                                  Visi = Visibility.Visible,
        //                                  EquipmentName = f.RtuName
        //                              };

        //                AttachEquipmentList.Add(ggg);
        //            }
        //            catch (Exception ex)
        //            {
        //                WriteLog.WriteLogError(
        //                    "RadMapJpeg LoadAttachEquipmentInfo Error when conver to IIEquipmentInfo:" + ex);
        //            }
        //        }
        //    }
        //}

        #region IEventAggregator Subscription


        //delegate void AppendStringCallback(PublishEventArgs text);
        //public void FundEventHandlers(PublishEventArgs args)
        //{
        //    AppendStringCallback appendStringCallback = new AppendStringCallback(FundEventHandlerss);
        //    Application.Current.Dispatcher.Invoke(appendStringCallback, args);
        //}



        public void FundEventHandlers(PublishEventArgs args)
        {
            try
            {
                if (args.EventType == PublishEventType.SvAv)
                {
                    if (MainEquipmentList.Count == 0) this.NavOnLoad();
                    return;
                }
                if (args.EventType == PublishEventType.Core)
                {
                    //if (args.EventId == EventIdAssign.EquipmentAddEventId )
                    //{
                    //    //var lst = args.GetParams()[0] as List <Tuple<int, int>>;
                    //    //if (lst == null) return;
                    //    //foreach (var t in lst )
                    //    //AddAttachEquipment(t.Item1 );
                    //    return;
                    //}
                    //if (args.EventId == EventIdAssign.EquipmentDeleteEventId )
                    //{
                    //    int rtuId = Convert.ToInt32(args.GetParams()[0]);
                    //    DeleteAttachEquipment(rtuId);
                    //    return;
                    //}

                    //if (args.EventId == EventIdAssign.EquipmentUpdateEventId )
                    //{
                    //    ReloadAttachEquipment();
                    //    return;
                    //}
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

                        if (!ServicesEquipemntInfoHold.EquipmentInfoDictionary.ContainsKey(rtuid))
                            return;
                        if (ServicesEquipemntInfoHold.EquipmentInfoDictionary[rtuid].AttachRtuId > 0) return;

                        var f =
                            Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary[rtuid];
                        foreach (var t in MainEquipmentList)
                        {
                            if (t.EquipmentRtuId == rtuid)
                            {
                                t.EquipmentName = f.RtuName;
                                t.EquipmentLocation = new Location(f.Xmap, f.Ymap);
                            }
                        }

                    }
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                    {
                        int x = Convert.ToInt32(args.GetParams()[0]);
                        // if (x < 1000000 || x > 1100000) return;
                        if (!Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsRtuIsRtuLight(x)) return;
                        OnSelectNodeChangeByTree(x);
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
                    if (args.EventId == Sr.EquipemntLightFault.Services.EventIdAssign.RtuErrorStateChanged)
                        try
                        {

                            var lst = args.GetParams()[0] as List<Tuple<int, bool>>;
                            if (lst == null) return;
                            var tmps = (from t in lst select t.Item1).ToList();
                            foreach (var t in MainEquipmentList)
                            {
                                if (tmps.Contains(t.EquipmentRtuId))
                                {
                                    t.UpdateTmlStateInfomation();
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                        }
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuLightHasElectricStatesChanged)
                        try
                        {
                            if (args.GetParams().Count > 1)
                            {
                                try
                                {
                                    int x1 = Convert.ToInt32(args.GetParams()[0]);
                                    // int x2 = Convert.ToInt32(args.GetParams()[1]);
                                    //UpdateTmlStateInfomation(x1, x2);
                                    foreach (var t in MainEquipmentList)
                                        if (t.EquipmentRtuId == x1)
                                        {
                                            t.UpdateTmlStateInfomation();
                                            break;
                                        }

                                }
                                catch (Exception ex)
                                {

                                }

                            }

                        }
                        catch (Exception ex)
                        {
                            Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
                        }
                    if (args.EventId == Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuGroupSelectdWantedMapUp)
                        try
                        {

                            var lst = args.GetParams()[0] as List<int>;
                            if (lst == null) return;
                            if (lst.Count == 0)
                            {
                                foreach (var t in MainEquipmentList)
                                {
                                    t.ImgVis = Visibility.Visible;

                                }
                                foreach (var t in AttachEquipmentList)
                                {
                                    t.ImgVis = Visibility.Visible;
                                }
                            }
                            else
                            {
                                foreach (var t in MainEquipmentList)
                                {
                                    t.ImgVis = lst.Contains(t.EquipmentRtuId)
                                                   ? Visibility.Visible
                                                   : Visibility.Collapsed;
                                }
                                foreach (var t in AttachEquipmentList)
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

        //private void UpdateTmlStateInfomation(int rtuId, int state)
        //{
        //    //if (f.RtuModel != 1050) return;
        //    foreach (var t in MainEquipmentList)
        //    {
        //        if (t.EquipmentRtuId == rtuId)
        //        {
        //            if (
        //                Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.MainEquipmentInfoDictionary.
        //                    ContainsKey(rtuId))
        //            {

        //                {
        //                    //图标更新
        //                    var s =
        //                        Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary[
        //                            rtuId].RtuState;
        //                    if (s == 0)
        //                    {
        //                        t.EquipmentImageState = 1010198;
        //                        return;
        //                    }
        //                    if (s == 1)
        //                    {
        //                        t.EquipmentImageState = 1010199;
        //                        return;
        //                    }
        //                    t.EquipmentImageState = 1010100 + state;
        //                }

        //            }
        //        }
        //    }
        //}

        //public void AddAttachEquipment(int rtuId)
        //{
        //    if (! Setting.Setting.IsShowLuxImageOnJpegMap) return;
        //    try
        //    {
        //        if (
        //            !Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary .
        //                 ContainsKey(rtuId))
        //            return;
        //        if (ServicesEquipemntInfoHold.EquipmentInfoDictionary[rtuId].AttachRtuId == 0) return;
        //        foreach (var t in AttachEquipmentList)
        //        {
        //            if (t.EquipmentRtuId == rtuId) return;
        //        }

        //        var g =
        //            Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.
        //                EquipmentInfoDictionary [
        //                    rtuId];
        //        //var gg = g as WjEquipmentModels.Interface.IILux;
        //        //if (gg == null) return;

        //        var f = g as IIEquipmentInfo;
        //        if (f == null) return;
        //        if (f.RtuModel != 1050) return;

        //        if (f.Xmap.Equals(0))
        //        {
        //            f.Xmap = 0.005;
        //        }
        //        if (f.Ymap.Equals(0))
        //        {
        //            f.Ymap = 0.005;
        //        }

        //        var ggg = new MapNodeViewModel()
        //                      {
        //                          EquipmentRtuId = f.RtuId,
        //                          EquipmentLocation = new Location(f.Xmap, f.Ymap),
        //                          EquipmentImageState = 2,
        //                          Visi = Visibility.Visible,
        //                          EquipmentName = f.RtuName
        //                      };

        //        AttachEquipmentList.Add(ggg);
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog.WriteLogError(
        //            "RadMapJpeg LoadAttachEquipmentInfo Error when conver to IIEquipmentInfo:" + ex);
        //    }
        //}

        public void DeleteAttachEquipment(int rtuId)
        {
            foreach (var t in AttachEquipmentList)
            {
                if (t.EquipmentRtuId == rtuId)
                {
                    AttachEquipmentList.Remove(t);
                    break;
                }
            }
        }

        //public void ReloadAttachEquipment()
        //{
        //    LoadAttachEquipmentInfo();
        //}

        public void AddMainEquipment(int rtuId)
        {
            try
            {
                if (
                    !Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary .
                         ContainsKey(rtuId))
                    return;
                if (Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary[rtuId].AttachRtuId != 0)
                    return;
                foreach (var t in MainEquipmentList)
                {
                    if (t.EquipmentRtuId == rtuId) return;
                }

                var f =
                    Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary [
                        rtuId]
                ;
               // if (f.RtuModel != 1050) return;

                if (f.Xmap.Equals(0))
                {
                    f.Xmap = 0.005;
                }
                if (f.Ymap.Equals(0))
                {
                    f.Ymap = 0.005;
                }

                var ggg = new MapNodeViewModel()
                              {
                                  EquipmentRtuId = f.RtuId,
                                  EquipmentLocation = new Location(f.Xmap, f.Ymap),
                                  EquipmentImageState = 2,
                                  Visi = Visibility.Visible,
                                  EquipmentName = f.RtuName
                              };

                MainEquipmentList.Add(ggg);
                ggg.UpdateTmlStateInfomation();
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError(
                    "RadMapJpeg LoadAttachEquipmentInfo Error when conver to IIEquipmentInfo:" + ex);
            }
        }

        public void DeleteMainEquipment(int rtuId)
        {
            foreach (var t in MainEquipmentList)
            {
                if (t.EquipmentRtuId == rtuId)
                {
                    MainEquipmentList.Remove(t);
                    break;
                }
            }
        }

        public void ReloadMainEquipment()
        {
            LoadMainEquipmentInfo();
        }

        //public void UpdateAttachEquipment(int rtuId)
        //{
        //    if (!Setting.Setting.IsShowLuxImageOnJpegMap) return;
        //    if (
        //        !Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary .
        //             ContainsKey(rtuId))
        //        return;

        //    if (ServicesEquipemntInfoHold.EquipmentInfoDictionary[rtuId].AttachRtuId == 0) return;

        //    var f =
        //        Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary [
        //            rtuId];

        //    if (f.RtuModel != 1050) return;
        //    foreach (var t in AttachEquipmentList)
        //    {
        //        if (t.EquipmentRtuId == rtuId)
        //        {
        //            t.EquipmentName = f.RtuName;
        //            t.EquipmentLocation = new Location(f.Xmap, f.Ymap);
        //        }
        //    }
        //}

        public void UpdateMainEquipment(int rtuId)
        {
            if (
                !Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary.
                     ContainsKey(rtuId))
                return;
            if (ServicesEquipemntInfoHold.EquipmentInfoDictionary[rtuId].AttachRtuId > 0) return;

            var f =
                Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary[
                    rtuId];

            //if (f.RtuModel != 1050) return;
            foreach (var t in MainEquipmentList)
            {
                if (t.EquipmentRtuId == rtuId)
                {
                  //  if (f.PhyId == 807) continue;
                    t.EquipmentName = f.RtuName;
                    if (f.Xmap < 0.001) f.Xmap = 0.001;
                    if (f.Ymap < 0.001) f.Ymap = 0.001;
                    t.EquipmentLocation = new Location(f.Xmap, f.Ymap);

                    t.UpdateTmlStateInfomation();
                }
            }
        }



        private void OnSelectNodeChangeByTree(int nodeId)
        {
            foreach (var t in this.MainEquipmentList)
            {
                if (t.EquipmentRtuId == nodeId)
                {
                    Location x = t.EquipmentLocation;
                    if (OnSelectEquipmentChange != null)
                    {
                        CurrentSelectNode = t;
                        t.ImgVis = Visibility.Visible;
                        OnSelectEquipmentChange(x);
                    }
                }
            }
        }


        private MapNodeViewModel _currentSelectNode;

        public MapNodeViewModel CurrentSelectNode
        {
            get
            {
                if (_currentSelectNode == null) _currentSelectNode = new MapNodeViewModel();
                return _currentSelectNode;
            }
            set
            {
                if (_currentSelectNode != value)
                {
                    if (_currentSelectNode != null)
                    {
                        _currentSelectNode.ImageHeight = RadMapJpeg .MapJepg .ViewModels .RadMapJpegViewModel .IconWidths ;
                        _currentSelectNode.ImageWidth = RadMapJpeg.MapJepg.ViewModels.RadMapJpegViewModel.IconWidths;
                        _currentSelectNode.VisiTooltips = Visibility.Collapsed;
                    }

                    _currentSelectNode = value;
                    _currentSelectNode.ImageHeight = RadMapJpeg.MapJepg.ViewModels.RadMapJpegViewModel.IconWidthsBig;
                    _currentSelectNode.ImageWidth = RadMapJpeg.MapJepg.ViewModels.RadMapJpegViewModel.IconWidthsBig;
                    _currentSelectNode.VisiTooltips = Visibility.Visible;
                    this.RaisePropertyChanged(() => this.CurrentSelectNode);
                }
            }
        }

        public Action<Location> OnSelectEquipmentChange { get; set; }
        //public event EventHandler OnSelectEquipmentChange = delegate { };
        //private void DeleteAnalogueAmpViewModelEx()
        //{
        //    if (OnSelectEquipmentChange == null) return;
        //    OnSelectEquipmentChange(this, EventArgs.Empty);
        //}

   

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
                    if (args.EventId == EventIdAssign.EquipmentAddEventId )
                        return true;
                    if (args.EventId == EventIdAssign.EquipmentDeleteEventId )
                        return true;
                
                    if (args.EventId == EventIdAssign.EquipmentUpdateEventId )
                        return true;
                    if (args.EventId == EventIdAssign.EquipentxyPositonUpdateId)
                        return true;

                    if (CanNavToEquImage &&  args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                        return true;
                  
                    if (args.EventId ==Sr .EquipemntLightFault .Services .EventIdAssign .RtuErrorStateChanged )
                        return true;
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuLightHasElectricStatesChanged)
                        return true;
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

        //public string MapUri
        //{
        //    get { return "E:/lp/Client/CETC50_Demo/ResourceLibrary/MapJpeg/osm_{zoom}.png"; }
        //}

        #region tab iinterface

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
            get { return false ; }
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

    public partial class RadMapJpegViewModel
    {


        private bool _loadCOnfig = false;
        public const string XmlConfigName = "JpegSetConfg";

        private void LoadConfig()
        {
            if (_loadCOnfig) return;
            if (_mySelf == null) _mySelf = this;
            _loadCOnfig = true;

            var info = Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read(XmlConfigName);
            if (info.ContainsKey("CanZoom"))
            {
                CanZoom = info["CanZoom"].Contains("yes");
            }
            else CanZoom = true;

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

        }

        public static void SavConfig()
        {
            var info = new Dictionary<string, string>();
            if (CanZoom) info.Add("CanZoom", "yes");
            else info.Add("CanZoom", "no");

            if (CanNavToEquImage) info.Add("CanNavToEquImage", "yes");
            else info.Add("CanNavToEquImage", "no");

           // if (_mySelf.MinZoom != 16)
            info.Add("ImageMinZoom", MinZoomInConfig + "");
            Wlst.Cr.CoreOne.Services.SystemXmlConfig.Save(info, XmlConfigName);
        }

        private  static RadMapJpegViewModel _mySelf;

        private static bool _canZoom;

        public static bool CanZoom
        {
            get { return _canZoom; }
            set
            {
                _canZoom = value;
                if (_mySelf == null) return;
                _mySelf.MinZoom = value ? MinZoomInConfig : 16;
            }
        }
        /// <summary>
        /// 是否与终端树联动
        /// </summary>
        public static bool CanNavToEquImage;

        public static  int MinZoomInConfig;


        private bool isload = false;
        private int _minZoom;

        public int MinZoom
        {
            get
            {
                if (!isload)
                {
                    LoadConfig();
                    isload = true;
                    if (MinZoomInConfig < 12) MinZoomInConfig = 12;
                }
                if (_minZoom < MinZoomInConfig) _minZoom = MinZoomInConfig;
                
                if (_minZoom > 16) _minZoom = 16;

                return _minZoom;
            }
            set
            {
                if (_minZoom != value)
                {
                    _minZoom = value;
                    if (_minZoom < MinZoomInConfig) _minZoom = MinZoomInConfig;
                    if (_minZoom > 16) _minZoom = 16;
                    this.RaisePropertyChanged(() => this.MinZoom);
                }
            }
        }

    

        //private int _mZoomLevel;
       // private DateTime dt = DateTime.Now.AddHours( -1);

       // public int ZoomLevel
       // {
       //     get { return _mZoomLevel; }
       //     set
       //     {
       //         if (_mZoomLevel != value)
       //         {
       //             if (DateTime.Now.Ticks - dt.Ticks < 20000000) return;

       //             _mZoomLevel = value;
       //             this.RaisePropertyChanged(() => this.ZoomLevel);
       //         }
       //     }
       // }
    }
}