using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Geometry;
using ESRI.ArcGIS.Client.Local;
using ESRI.ArcGIS.Client.Tasks;
//using Microsoft.Practices.Prism.MefExtensions.Event;
//using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using Wlst.Cr.Core.Behavior;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.ModuleServices;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Sr.Menu.Services;
using Wlst.Ux.MapGisLocalSP.MapGis.Services;
using Wlst.Ux.MapGisLocalSP.MapGis.ViewModel;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Client.Runtime;
using System.IO;
using System.Timers;
using System.Threading;
using WriteLog = Wlst.Cr.Core.UtilityFunction.WriteLog;


namespace Wlst.Ux.MapGisLocalSP.MapGis.View
{
    /// <summary>
    /// MapGis.xaml 的交互逻辑
    /// </summary>
    [ViewExport(Ux.MapGisLocalSP.Services.ViewIdAssign.MapGisViewId, AttachNow = true,
        AttachRegion = Ux.MapGisLocalSP.Services.ViewIdAssign.MapGisViewAttachRegion)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class MapGis : UserControl
    {
        public static ConcurrentDictionary<int, int> CtrlIcon = new ConcurrentDictionary<int, int>();
        private LocalMapService _mapService;
        private static int isUselocate;
        private const string XmlConfigName = "MapGis";
        private static bool needOnErrChanged = false;
        private string bsField = null;
        private bool needCopyServices = false;
        private bool IsIconFollowTheRtu;
        private FeatureLayer Circuit = new FeatureLayer();
        private FeatureLayer Concentrator = new FeatureLayer();
        private FeatureLayer Controller = new FeatureLayer();
        //private ArcGISLocalFeatureLayer Tml = new ArcGISLocalFeatureLayer();
        private double tmlMaxRes;
        private Dictionary<int, string> FeatureDic = new Dictionary<int, string>();
        private Dictionary<string, int> FeatureDicD = new Dictionary<string, int>();
        private bool isShowTools;
        private bool isShowNoUse;
        private  List<int> rtuLst = new List<int>();

        public MapGis()
        {
            
            ArcGISRuntime.InstallPath = @".";
            ArcGISRuntime.SetLicense(
                "runtimestandard,101,rud105202449,none,2KYRMD1AJ48H2T8AG200,25472A32C2C6D077D75848B17A231B715EDFA8A3869919484DE72955AF62B012466BF973A2BF7B4BB6BC4E619BE2F183A42C532CD0A21784E122580892396B4E439755764A218EDFF6F2442299AB05500E823886D5D0AED32AD956D83FFA50AFE1A05C7D441F89E35FC1A9BCD48ACD68316E2D606D996376099B917A274E5719,FID_27b78480_14b65041ba9__6edd");
            InitializeComponent();
            LoadXml();


            


           
            //        #region load Local Layers

            //        string path = @".\Map\arcgis.mpk";
            //        LocalFeatureService lfs = LocalFeatureService.GetService(path);

            //        Tml.Service = lfs;//.\Map\arcgis.mpk
            //        Tml.LayerName = "终端";
            //        Tml.ID = "Tml";
            //        Tml.DisplayName = "终端";
            //        Tml.MouseLeftButtonUp += new GraphicsLayer.MouseButtonEventHandler(FeatureLayer_MouseLeftButtonUp);
            //        Tml.MouseLeftButtonDown += new GraphicsLayer.MouseButtonEventHandler(FeatureLayer_MouseLeftButtonDown);
            //        Tml.MouseRightButtonUp += new GraphicsLayer.MouseButtonEventHandler(FeatureLayer_MouseRightButtonUp);
            //        Tml.MouseRightButtonDown +=
            //            new GraphicsLayer.MouseButtonEventHandler(FeatureLayer_PreviewMouseDown);
            //        Tml.MouseRightButtonDown +=
            //new GraphicsLayer.MouseButtonEventHandler(FeatureLayer_PreviewMouseDown);
            //        var temp = new OutFields() { };
            //        temp.Add("*");
            //        Tml.OutFields = temp;
            //        Tml.MaximumResolution = tmlMaxRes;
            //        Tml.Mode = FeatureLayer.QueryMode.Snapshot;
            //        Tml.DisableClientCaching = true;
            //        //Tml.AutoSave = true;
            //        Tml.Editable = true;
            //        Tml.MouseRightButtonDown +=
            //            new GraphicsLayer.MouseButtonEventHandler(FeatureLayer_PreviewMouseDown);
            //        try
            //        {
            //            MyMap.Layers.Add(Tml);
            //            Tml.UpdateCompleted += new EventHandler(Tml_UpdateCompleted);

            //        }
            //        catch (Exception ex)
            //        {
            //            Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            //        }
            //        MyMap.Layers.Add(MyGraphicsLayer);
            //        #endregion

            //INIClass iniC = new INIClass(@".\MapGis.ini");
            //isUselocate = double.Parse(iniC.IniReadValue("MapGis", "location"));
            //double xmin  = double.Parse(iniC.IniReadValue("MapGis", "xmin"));
            //double ymin = double.Parse(iniC.IniReadValue("MapGis", "ymin"));
            //double xmax = double.Parse(iniC.IniReadValue("MapGis", "xmax"));
            //double ymax = double.Parse(iniC.IniReadValue("MapGis", "ymax"));
            //double maxRes = double.Parse(iniC.IniReadValue("MapGis","maxResolution"));
            //double tmlMR = double.Parse(iniC.IniReadValue("MapGis", "tmlMR"));
            //double ctrlMR = double.Parse(iniC.IniReadValue("MapGis", "ctrlMR"));
            //double conMR = double.Parse(iniC.IniReadValue("MapGis", "conMR"));
            //double gMR = double.Parse(iniC.IniReadValue("MapGis", "gMR"));

            //////////double typeNum = double.Parse(iniC.IniReadValue("MapGis", "maxResolution"));

            //////////Dictionary<int, string> mydic = new Dictionary<int, string>() 
            //////////{ { 1, "a" }, { 2, "b" }, { 3, "c" } };
            //////////AddCtrllerType.SelectedValuePath = "Key"; 
            //////////AddCtrllerType.DisplayMemberPath = "Value";
            //string  type = iniC.IniReadValue("MapGis", "type");
            //if (type == "")
            //{
            //    Tml.Where = "";
            //}
            //else
            //{
            //    string temp = "type='" + type + "'";
            //    Tml.Where = temp;
            //}
            //Tml.Update();
            //this.MyMap.Extent = new Envelope(new MapPoint(xmin, ymin), new MapPoint(xmax, ymax));
            //this.MyMap.MaximumResolution = maxRes ;
            //Tml.MaximumResolution = tmlMR;
            //Controller.MaximumResolution = ctrlMR;
            //Concentrator.MaximumResolution = conMR;
            //MyGraphicsLayer.MaximumResolution = gMR;




            //Tml.Where="status>1";
            //this.MyMap.Extent = new Envelope(new MapPoint(13272337.431, 3627494.452), new MapPoint(13392352.671, 3719398.186));


            //model = new MapGisViewModel();
            //DataContext = model;


            editGeometry = LayoutRoot.Resources["MyEditGeometry"] as EditGeometry;
            LocalMapService.GetServiceAsync(@".\Map\arcgis.mpk", (localMapService) =>
                                                                     {
                                                                         _mapService = localMapService;
                                                                         SearchButton.IsEnabled = true;
                                                                     });

            MyMap.Layers.LayersInitialized += (o, evt) =>
                                                  {
                                                      //model.IsBusy = false;
                                                      ESRI.ArcGIS.Client.Editor myEditor =
                                                          LayoutRoot.Resources["MyEditor"] as ESRI.ArcGIS.Client.Editor;
                                                      myEditor.Map = MyMap;
                                                      myEditor.LayerIDs = new string[] {"Tml"};
                                                      // MyMap.ZoomToResolution(3 ,Tml.FullExtent.GetCenter());
                                                      //Wlst.Cr.Coreb.AsyncTask.Qtz.AddQtz("nuu", 8888, DateTime.Now.Ticks, intervalTime, Funcx);   //todo

                                                      try
                                                      {

                                                          //double x = double.Parse(iniC.IniReadValue("MapGis", "x"));
                                                          //double y = double.Parse(iniC.IniReadValue("MapGis", "y"));
                                                          //double rresolution = double.Parse(iniC.IniReadValue("MapGis", "Resolution"));
                                                          //double mresolution = double.Parse(iniC.IniReadValue("MapGis", "maxResolution"));
                                                          //Graphic gr = new Graphic();
                                                          //gr.Geometry = ChangeCoodinateFrom84(x, y);
                                                          //MapPoint mp=new MapPoint ();
                                                          //mp=gr.Geometry as MapPoint ;
                                                          //mp.SpatialReference = MyMap.SpatialReference;
                                                          //MyMap.ZoomToResolution(rresolution,mp );
                                                          //MyMap.MaximumResolution = mresolution;

                                                          InitEvent();
                                                      }
                                                      catch
                                                      {
                                                      }

                                                  };
            //Tml.Initialized += new EventHandler<EventArgs>(Tml_Initialized);
            Tml.UpdateCompleted += new EventHandler(Tml_UpdateCompleted);
            //Tml.Initialized +=new EventHandler(Tml_Initialized);

            //////List<string> list1 = new List<string>();
            //////list1.Add("AAA");
            //////list1.Add("BBB");
            //////list1.Add("CCC");
            //////AddCtrllerType.ItemsSource=null ;
            //////AddCtrllerType.ItemsSource = list1;
            ////BitmapImage imagetemp = new BitmapImage(new Uri(@".\Image\addTml0", UriKind.Relative));

        }

        //void Tml_Initialized(object sender, EventArgs e)
        //{
        //    //CopyFeatureLayers();
        //    //FilterFeatures();
        //}

        [Import]
        public IIMapGis Model
        {
            get { return DataContext as IIMapGis; }
            set
            {
                DataContext = value;
            }
        }
        
        private void Tml_UpdateCompleted(object sender, EventArgs e)
        {
            //needCopyServices = true;
            if (needCopyServices) //苏州园区 为true 需要将唯一标识转换成Lid 标识符为Code 要赋值为Lid
            {
                CopyFeatureLayers();
                //FilterFeatures();
            }

            if (needOnErrChanged == true)
            {
                //throw new NotImplementedException();
                var rtus = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.Keys;
                //.EquipmentInfoDictionary.Keys;
                foreach (int id in rtus)
                {
                    OnErrChanged(id);

                }
                ModifyFeaturesAttribute3("Status");

                needOnErrChanged = false;
            }
        }

        protected bool IsLoadOnlyOneArea;

        private void FilterFeatures()
        {
            GetRtus();
            if (rtuLst.Count == 0) return;
            if (!isShowNoUse)

            {
                string idt = "";
                //var rtuLst =
                //    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoList(WjParaBase.EquType.Rtu);
                foreach (var a in rtuLst)
                {

                    var pb = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(a);
                    if (pb == null) continue;

                    if (pb.Idf == null) continue;
                    if (string.IsNullOrEmpty(pb.Idf)) continue;
                    string tmp = "\'" + pb.Idf + "\'";
                    idt = idt + tmp + ",";
                    
                }
                if (idt.Length == 0) return;
                idt = idt.Substring(0, idt.Length - 1);
                string temp = "CODE in (" + idt + ")";
                //string temp = "CODE in (\'100J\',\'101J\')";
                Tml.Where = temp;
                Tml.Update();
                needOnErrChanged = true;

            }
        }
        private void GetRtus()
        {
            List<int> areaLst = new List<int>();
            rtuLst .Clear();
            var userProperty = UserInfo.UserLoginInfo;
            areaLst.AddRange(userProperty.AreaX);
            foreach (var t in userProperty.AreaW)
            {
                if (!areaLst.Contains(t))
                {
                    areaLst.Add(t);
                }
            }
            foreach (var f in userProperty.AreaR)
            {
                if (!areaLst.Contains(f))
                {
                    areaLst.Add(f);
                }
            }
            IsLoadOnlyOneArea = areaLst.Count < 2;
            if (userProperty.D == true)
            {
                foreach (var f in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys)
                {

                    var lstInArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(f);

                    foreach (var a in lstInArea)
                    {
                        var pb = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(a);
                        if (pb == null) continue;
                        if (pb.EquipmentType == WjParaBase.EquType.Rtu)
                        {
                            rtuLst.Add(pb.RtuId);
                        }
                    }
                }
            }
            else
            {
                foreach (var f in areaLst)
                {
                    var lstInArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(f);

                    foreach (var a in lstInArea)
                    {
                        var pb = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(a);
                        if (pb == null) continue;
                        if (pb.EquipmentType == WjParaBase.EquType.Rtu)
                        {
                            rtuLst.Add(pb.RtuId);
                        }
                    }
                }
            }
        }
        private void LoadXml()
        {

            double xmin = 0;
            double ymin = 0;
            double xmax = 0;
            double ymax = 0;
            var info = Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read(XmlConfigName);
            foreach (var g in info)
            {
                if (g.Key.Contains("CopyFeatureLayers"))
                {
                    needCopyServices = int.Parse(g.Value) == 1;
                }
                break;
            }
            foreach (var f in info)
            {
                if (f.Key.Contains("xmin"))
                {
                    xmin = double.Parse(info["xmin"]);
                }
                else if (f.Key.Contains("ymin"))
                {
                    ymin = double.Parse(info["ymin"]);
                }
                else if (f.Key.Contains("xmax"))
                {
                    xmax = double.Parse(info["xmax"]);
                }
                else if (f.Key.Contains("ymax"))
                {
                    ymax = double.Parse(info["ymax"]);
                }
                else if (f.Key.Contains("MapMaxRes"))
                {
                    double mapMaxRes = double.Parse(info["MapMaxRes"]);
                    if (mapMaxRes > 1) this.MyMap.MaximumResolution = mapMaxRes;

                }
                else if (f.Key.Contains("TmlMaxRes"))
                {
                    tmlMaxRes = double.Parse(info["TmlMaxRes"]);
                    if (tmlMaxRes > 1) Tml.MaximumResolution = tmlMaxRes;
                }
                else if (f.Key.Contains("SluMaxRes"))
                {
                    double sluMaxRes = double.Parse(info["SluMaxRes"]);
                    Concentrator.MaximumResolution = sluMaxRes;
                }
                else if (f.Key.Contains("CtrlMaxRes"))
                {
                    double ctrlMaxRes = double.Parse(info["CtrlMaxRes"]);
                    Controller.MaximumResolution = ctrlMaxRes;
                }
                else if (f.Key.Contains("GraphicMaxRes"))
                {
                    double graphicMaxRes = double.Parse(info["GraphicMaxRes"]);
                    Controller.MaximumResolution = graphicMaxRes;
                }
                else if (f.Key.Contains("error1"))
                {
                    CtrlIcon.TryAdd(1, int.Parse(info["error1"]));
                }
                else if (f.Key.Contains("error2"))
                {
                    CtrlIcon.TryAdd(2, int.Parse(info["error2"]));
                }
                else if (f.Key.Contains("error3"))
                {
                    CtrlIcon.TryAdd(3, int.Parse(info["error3"]));
                }
                else if (f.Key.Contains("error4"))
                {
                    CtrlIcon.TryAdd(4, int.Parse(info["error4"]));
                }
                else if (f.Key.Contains("error5"))
                {
                    CtrlIcon.TryAdd(5, int.Parse(info["error5"]));
                }
                else if (f.Key.Contains("error6"))
                {
                    CtrlIcon.TryAdd(6, int.Parse(info["error6"]));
                }
                else if (f.Key.Contains("error7"))
                {
                    CtrlIcon.TryAdd(7, int.Parse(info["error7"]));
                }
                else if (f.Key.Contains("ljdz"))
                {
                    if (string.IsNullOrEmpty(f.Value)) continue;
                    bsField = info["ljdz"];
                }
                else if (f.Key.Contains("IsShowTools"))
                {
                    isShowTools = int.Parse(info["IsShowTools"]) == 1;
                    sp.Height = 0;
                    sp.Visibility = isShowTools ? Visibility.Visible : Visibility.Collapsed;
                    hideB.Visibility = isShowTools ? Visibility.Visible : Visibility.Collapsed;
                }
                else if (f.Key.Contains("IsShowNoUse"))
                {
                    isShowNoUse = int.Parse(info["IsShowNoUse"]) == 1;
                }
                else if (f.Key.Contains("isUselocate"))
                {
                    isUselocate = int.Parse(info["isUselocate"]) ;
                }
            }

            var infos = Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read("Wj2090SetConfg");
            if (infos.ContainsKey("IsIconFollowTheRtu"))
            {
                IsIconFollowTheRtu = infos["IsIconFollowTheRtu"].Contains("yes");
            }
            else IsIconFollowTheRtu = true;



            if (xmin > 0 && ymin > 0 && xmax > 0 && ymax > 0)
            {
                this.MyMap.Extent = new Envelope(new MapPoint(xmin, ymin), new MapPoint(xmax, ymax));
            }
        }

        void CopyFeatureLayers()
        {
            int lid =0;
            string wybs = "";
            try
            {
                if (bsField == null) bsField = "Lid";
                var rtuLst = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoList(WjParaBase.EquType.Rtu);

                for (int i = Tml.Graphics.Count-1 ; i > 0; i--)
                {
                    //1000131 -190L
                    //foreach (var g in Tml.Graphics)
                    //{
                    if (!Tml.Graphics[i].Attributes.ContainsKey(bsField) ||
                        !Tml.Graphics[i].Attributes.ContainsKey("Lid")) continue;
                    if (Tml.Graphics[i].Attributes[bsField] == null) continue;
                    if (!string.IsNullOrEmpty(Tml.Graphics[i].Attributes[bsField].ToString()))
                    {
                        wybs = Tml.Graphics[i].Attributes[bsField].ToString().Trim();
                        
                        if (wybs == "" ) continue;

                        int os = Convert .ToInt32(Tml.Graphics[i].Attributes["Status"]) ;
                        //int os = Convert.ToInt32(tmpStatus);   //todo
                         if (wybs.Length >1)
                         {
                             var sps = wybs.Substring(wybs.Length - 1, 1);
                             //var sps = wybs[wybs.Length - 1].ToString(); // wybs.Split('_');
                             if (sps == "J")  //景观灯  三角形  10~16
                             {
                                 if ( os <10 || os >16)
                                 {
                                     Tml.Graphics[i].Attributes["Status"] =Convert .ToInt16(10) ;
                                 }

                             }else if (sps =="Y") //楼宇  正方形 20~26
                             {
                                if(os <20 || os >26)
                                {
                                    Tml.Graphics[i].Attributes["Status"] = Convert.ToInt16(20);
                    
                                }
                             }else //if(sps == "L") 路灯及其他
                             {
                                if( os >6 )
                                {
                                    Tml.Graphics[i].Attributes["Status"] = Convert.ToInt16(0);
                                }
                             }

                             //Tml.Graphics[i].Attributes["TYPE"] == "20";
                         }

                        foreach (var a in rtuLst)
                        {
                   
                            var pb = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(a.RtuId);
                            if (pb == null) continue;
                            if ( pb.Idf.Trim() == wybs)
                            {
                                
                                lid = pb.RtuId;
                                if (FeatureDic.ContainsKey(lid))
                                {
                                    FeatureDic[lid] = wybs;
                                }
                                else
                                {
                                    FeatureDic.Add(lid, wybs);
                                }
                                if (FeatureDicD.ContainsKey(wybs))
                                {
                                    FeatureDicD[wybs] = lid;
                                }
                                else
                                {
                                    FeatureDicD.Add(wybs, lid);
                                }
                                break;
                            }
                        }
                        //Tml.Graphics[i].Attributes["Lid"] = lid;
                        //Tml.SaveEdits();
                        //Tml.Update();
                    }
                }
                Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(FilterFeatures, 2, DelayEventHappen.EventOne);
                //FilterFeatures();
                needCopyServices = false;
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error: " + ex);
            }

        }

        #region Ahri
        /*
         * 地图鼠标处理事件
         * 2014.12  Lvf
         * 嗯？你知道Ahri是谁？灵动起来~有缘人~加油
         */
        //画画，方式很多，目前只用到polyline&point，用于批量添加tml|controller|concentrator
        private Draw _drawObject;
        public static  int TreeSelectId =0;
        private static bool onDraw;
        //图层点击
        //先清空各图层的选中图元
        //生成右上数据表格
        //如果选中的是集中器，则渲染其下控制器
        private void FeatureLayer_MouseLeftButtonUp(object sender, GraphicMouseButtonEventArgs args)
        {
            ArcGISLocalFeatureLayer featureLayer = sender as ArcGISLocalFeatureLayer;
            chkErr.IsChecked = false;
            //////////////////MyGraphicsLayer.Graphics.Clear();
            FindDetailsDataGrid.Visibility = Visibility.Hidden;
            if (Keyboard.Modifiers != ModifierKeys.Control)
            {
                try
                {
                    ESRI.ArcGIS.Client.Editor myEditorC = LayoutRoot.Resources["MyEditorC"] as ESRI.ArcGIS.Client.Editor;
                    myEditorC.Map = MyMap;
                    myEditorC.LayerIDs = new string[] { "Tml" };
                    myEditorC.ClearSelection.Execute(null);
                }
                catch { }
            }
            if (chkEdit.IsChecked == true)
            {
                //MyFeatureDataForm.Visibility = Visibility.Visible;
                MyFeatureDataForm.IsReadOnly = false;

               
            }
            else
            {
                //MyFeatureDataForm.Visibility = Visibility.Hidden;
                MyFeatureDataForm.IsReadOnly = true;
                
            }
            if (chkShowAttr.IsChecked == true)
            {
                MyFeatureDataForm.Visibility  = Visibility.Visible;
                //showPic.Visibility = Visibility.Visible;
                //ShowPic(Convert.ToInt32(args.Graphic.Attributes["Lid"]));
            }
            else 
            {
                MyFeatureDataForm.Visibility = Visibility.Hidden ;
                //showPic.Visibility = Visibility.Hidden;
            }
            ShowPic(Convert.ToInt32(args.Graphic.Attributes["Lid"]));
            MyFeatureDataForm.FeatureLayer = featureLayer;
            MyFeatureDataForm.GraphicSource = args.Graphic;
            FeatureDataFormBorder.Visibility = Visibility.Visible;
            GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
            graphicsLayer.Graphics.Clear();
            if (featureLayer == Concentrator)
            {
                //graphicsLayer.Graphics.Clear();
                foreach (Graphic g in Controller.Graphics)
                {
                    if (Convert.ToInt32(g.Attributes["Bid"]) == Convert.ToInt32(args.Graphic.Attributes["Lid"]))
                    {
                        Graphic gr = new Graphic
                        {
                            Geometry = g.Geometry,
                            Symbol = LayoutRoot.Resources["DefaultMarkerSymbol"] as ESRI.ArcGIS.Client.Symbols.Symbol, // newSymbol is a SimpleMarkerSymbol (point)
                        };
                        graphicsLayer.Graphics.Add(gr);
                    }
                }
            }
            args.Graphic.Select();
            //发布事件  选中当前节点
            var argss = new PublishEventArgs
            {
                EventType = PublishEventType.Core,
                EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                EventAttachInfo ="gis"
            };
            if (featureLayer == Controller) 
            {
                argss.AddParams(args.Graphic.Attributes["Bid"]);
                argss.AddParams(args.Graphic.Attributes["Lid"]);
                bq.Visibility = Visibility.Collapsed;
                

            }
            else
            {
                var lidtmp = "";
                string  wybs = "";
                if (args.Graphic.Attributes[bsField] != null) wybs = args.Graphic.Attributes[bsField].ToString();
                if (FeatureDic.Count == 0) return;
                foreach (var g in FeatureDic.Keys)
                {
                    if (FeatureDic[g] == wybs) lidtmp = g + "";
                }
                argss.AddParams(lidtmp);
                
                int lid = int.Parse(lidtmp);
                if (!Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(lid)) return;
                var info = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[lid];
                var groupidx =
                     Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuBelongGrp(lid);//.Item2;
                 //Wlst.Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuBelongGrp(RtuId);
                 if (groupidx != null )
                 {
                     var infosss =
                         Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGroupInfomation(
                             groupidx.Item1, groupidx.Item2);
                     if (infosss != null)
                     {
                         Model.GroupName = infosss.GroupName;
                     }
                     else
                     {
                         Model.GroupName = "无分组信息";
                     }

                 }
                Model.RtuIdf = info.Idf;
                Model.RtuName = info.RtuName;
                Model.RtuPhyId = info.RtuPhyId + "";
                Model.Remark = info.RtuRemark;
                Model.InstallAddr = info.RtuInstallAddr;
                Model.DataCreate = new DateTime(info.DateCreate).ToString("yyyy-MM-dd");
                bq.Visibility = Visibility.Visible;
            }
            EventPublish.PublishEvent( argss);
        }
        //图层点击
        //给拖动图元做铺垫
        private void FeatureLayer_MouseLeftButtonDown(object sender, GraphicMouseButtonEventArgs e)
        {
            ArcGISLocalFeatureLayer featureLayer = sender as ArcGISLocalFeatureLayer;
            if (featureLayer == Circuit) return;

            Model.SetCmUnVisi();
            if (chkEdit.IsChecked == false)
                return;
            e.Handled = true;

            if (e.Graphic.Geometry is MapPoint)
            {
                e.Graphic.Selected = true;
                selectedPointGraphic = e.Graphic;
            }
            else
            {
                editGeometry.StartEdit(e.Graphic);
            }
        }
        //图层右击，类似左击功能
        private void FeatureLayer_MouseRightButtonUp(object sender, GraphicMouseButtonEventArgs args)
        {
            ArcGISLocalFeatureLayer featureLayer = sender as ArcGISLocalFeatureLayer;
            chkErr.IsChecked = false;
            GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
            graphicsLayer.Graphics.Clear();
            ////////////////////////MyGraphicsLayer.Graphics.Clear();   
            try
            {
                ESRI.ArcGIS.Client.Editor myEditorC = LayoutRoot.Resources["MyEditorC"] as ESRI.ArcGIS.Client.Editor;
                myEditorC.Map = MyMap;
                myEditorC.LayerIDs = new string[] {"Tml" };
                myEditorC.ClearSelection.Execute(null);
            }
            catch { }
            args.Graphic.Select();
            //发布事件  选中当前节点
            var argss = new PublishEventArgs
            {
                EventType = PublishEventType.Core,
                EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                EventAttachInfo ="gis",
            };
            if (featureLayer == Controller)
            {
                argss.AddParams(args.Graphic.Attributes["Bid"]);
                argss.AddParams(args.Graphic.Attributes["Lid"]);
                bq.Visibility = Visibility.Collapsed;
            }
            else
            {
                var lidtmp = "";
                string wybs = "";
                if (args.Graphic.Attributes[bsField] != null) wybs = args.Graphic.Attributes[bsField].ToString();
                if (FeatureDic.Count == 0) return;
                foreach (var g in FeatureDic.Keys)
                {
                    if (FeatureDic[g] == wybs) lidtmp = g + "";
                }
                argss.AddParams(lidtmp);
                int lid = int.Parse(lidtmp);
                if (!Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(lid)) return;
                var info = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[lid];
                Model.RtuIdf = info.Idf;
                Model.RtuName = info.RtuName;
                Model.RtuPhyId = info.RtuPhyId + "";
                Model.InstallAddr = info.RtuInstallAddr;
                Model.DataCreate = new DateTime(info.DateCreate).ToString("yyyy-MM-dd"); ;
                bq.Visibility = Visibility.Visible;


                //argss.AddParams(args.Graphic.Attributes["Lid"]);
            }
            EventPublish.PublishEvent( argss);
        }
        //呈现右击菜单
        private void FeatureLayer_PreviewMouseDown(object sender, GraphicMouseButtonEventArgs e)        
        {
            ArcGISLocalFeatureLayer featureLayer = sender as ArcGISLocalFeatureLayer;
            e.Graphic.Select();
            if (featureLayer == Circuit) return;//线路右击暂时屏蔽
            if (featureLayer == Controller)
            {
                if (Convert.ToInt32(e.Graphic.Attributes["Bid"]) < 1500000 || Convert.ToInt32(e.Graphic.Attributes["Bid"]) > 1600000)
                    return;
                int a = Convert.ToInt32(e.Graphic.Attributes["Bid"]);
                int b = Convert.ToInt32(e.Graphic.Attributes["Lid"]);
                int rtuId = (a * 1000) + b;
                Model.CurrentRtuId = rtuId;
                Model.Cm.IsOpen = true;
            }
            else
            {
                var lidtmp = "";
                string wybs = "";
                if (e.Graphic.Attributes[bsField] != null) wybs = e.Graphic.Attributes[bsField].ToString();
                foreach (var g in FeatureDic.Keys)
                {
                    if (FeatureDic[g] == wybs) lidtmp = g + "";
                }
                if (lidtmp == "") return;
                if (Convert.ToInt32(lidtmp) < 1000000 || Convert.ToInt32(lidtmp) > 2000000) return;
                //if (Convert.ToInt32(e.Graphic.Attributes["Lid"]) < 1000000 || Convert.ToInt32(e.Graphic.Attributes["Lid"]) > 2000000)
                    //return;
                int rtuId = Convert.ToInt32(lidtmp);
                Model.CurrentRtuId = rtuId;
                Model.Cm.IsOpen = true;

                Model.RtuName = "";
            }
            //int rtuId = Convert.ToInt32(e.Graphic.Attributes["Lid"]);
            //model.CurrentRtuId = rtuId;
            //model.Cm.IsOpen = true;
        }
        //点击搜索按钮
        int i = 0;
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            //////////ModifyFeaturesAttribute(1000001, "Status", "10");
            //////var rtus = Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary.Keys;
            //////////////////////test  lvffffffffffffffffffffffffffffffffffffffffff

            //////if (i < 4)
            //////{
            //////    i++;
            //////}
            //////else { i = 0; }
            //////string t = i.ToString();
            //////foreach (int id in rtus)
            //////{
            //////    ModifyFeaturesAttribute(id, "Status", t);
            //////}
            ////////////////232313213123213test
            if (FindText.Text.Trim() == "")
                return;
            isManuele = true;
            FindDetailsDataGrid.Visibility = Visibility.Visible;
            GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
            graphicsLayer.Graphics.Clear();
            FindTask findTask = new FindTask();
            findTask.Url = _mapService.UrlMapService;
            findTask.Failed += FindTask_Failed;
            FindParameters findParameters = new FindParameters();
            findParameters.LayerIds.AddRange(new int[] { 0,1,2 });
            findParameters.SearchFields.AddRange(new string[] { "Lid", "Bid", "type", "address","Name" });
            Binding resultFeaturesBinding = new Binding("LastResult");
            resultFeaturesBinding.Source = findTask;
            FindDetailsDataGrid.SetBinding(DataGrid.ItemsSourceProperty, resultFeaturesBinding);
            findParameters.SearchText = FindText.Text;
            findTask.ExecuteAsync(findParameters);
            isManuele = false;
        }
        private void FindText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (FindText.Text.Trim() == "")
                    return;
                isManuele = true;
                FindDetailsDataGrid.Visibility = Visibility.Visible;
                GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
                graphicsLayer.Graphics.Clear();
                FindTask findTask = new FindTask();
                findTask.Url = _mapService.UrlMapService;
                findTask.Failed += FindTask_Failed;
                FindParameters findParameters = new FindParameters();
                findParameters.LayerIds.AddRange(new int[] { 0, 1, 2 });
                findParameters.SearchFields.AddRange(new string[] { "Lid", "Bid", "type", "address", "Name" });
                Binding resultFeaturesBinding = new Binding("LastResult");
                resultFeaturesBinding.Source = findTask;
                FindDetailsDataGrid.SetBinding(DataGrid.ItemsSourceProperty, resultFeaturesBinding);
                findParameters.SearchText = FindText.Text;
                findTask.ExecuteAsync(findParameters);
                isManuele = false;
            }
        }
        bool isManuele = false;
        //选择搜索结果
        private void FindDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isManuele) return;
            FindDetailsDataGrid.Visibility = Visibility.Collapsed;
            DataGrid dataGrid = sender as DataGrid;
            int selectedIndex = dataGrid.SelectedIndex;
            if (selectedIndex > -1)
            {
                FindResult findResult = (FindResult)FindDetailsDataGrid.SelectedItem;
                Graphic graphic = findResult.Feature;

                graphic.Geometry.SpatialReference = MyMap.SpatialReference;
                switch (graphic.Attributes["SHAPE"].ToString())
                {
                    case "Polygon":
                        graphic.Symbol = LayoutRoot.Resources["DefaultFillSymbol"] as ESRI.ArcGIS.Client.Symbols.Symbol;
                        break;
                    case "Polyline":
                        graphic.Symbol = LayoutRoot.Resources["DefaultLineSymbol"] as ESRI.ArcGIS.Client.Symbols.Symbol;
                        break;
                    case "Point":

                        graphic.Symbol = LayoutRoot.Resources["DefaultMarkerSymbol"] as ESRI.ArcGIS.Client.Symbols.Symbol;
                        break;
                }
                GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
                graphicsLayer.Graphics.Clear();
                graphicsLayer.Graphics.Add(graphic);
                graphic.Select();
                MyMap.PanTo(graphic.Geometry);
            }
        }
        private void FindTask_Failed(object sender, TaskFailedEventArgs args)
        {
            MessageBox.Show("Find failed: " + args.Error);
        }
        //地图编辑checkbox
        private void chkEdit_Click(object sender, RoutedEventArgs e)
        {
            if (chkEdit.IsChecked == false)
            {
                //MyFeatureDataForm.Visibility = Visibility.Hidden;
                MyFeatureDataForm.IsReadOnly = true;
                //showPic.Visibility = Visibility.Hidden;
                DelSelectionButton.IsEnabled = false;
                AddMultiController.IsEnabled = false;
                AddTml.IsEnabled = false;
                AddConcentrator.IsEnabled = false;
                AddLine.IsEnabled = false;
                UpdateMapButton.IsEnabled = false;
                ShowNullButton.IsEnabled = false;
                //DelNullButton.IsEnabled = false;
                BindButton.IsEnabled = false;
                AddCtrllerNum.Text = "";
                //SelectButton.IsEnabled = true ;
                AddMultiController.Style = LayoutRoot.Resources["AddController0"] as Style;
                AddTml.Style = LayoutRoot.Resources["AddTml0"] as Style;
                AddConcentrator.Style = LayoutRoot.Resources["AddConcentrator0"] as Style;
                AddLine.Style = LayoutRoot.Resources["AddLine0"] as Style;
                ShowNullButton.Style =LayoutRoot.Resources["ShowNull0"] as Style ;
            }
            else
            {
                
                MyFeatureDataForm.IsReadOnly = false ;
                //showPic.Visibility = Visibility.Visible ;
                DelSelectionButton.IsEnabled = true ;
                AddMultiController.IsEnabled = true ;
                AddTml.IsEnabled = true;
                AddConcentrator.IsEnabled = true;
                AddLine.IsEnabled = true;
                UpdateMapButton.IsEnabled = true;
                ShowNullButton.IsEnabled = true ;
                //DelNullButton.IsEnabled = true ;
                BindButton.IsEnabled = true;
                //SelectButton.IsEnabled = false ;
                try
                {
                    ESRI.ArcGIS.Client.Editor myEditorC = LayoutRoot.Resources["MyEditorC"] as ESRI.ArcGIS.Client.Editor;
                    myEditorC.Map = MyMap;
                    myEditorC.LayerIDs = new string[] {"Tml" };
                    myEditorC.ClearSelection.Execute(null);
                }
                catch { }
            }
        }

        //显示故障设备
        private void chkErr_Click(object sender, RoutedEventArgs e)
        {
            GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
            graphicsLayer.Graphics.Clear();
             if (chkErr.IsChecked == true )
            {
                if (Tml.Visible == true)
                {
                    foreach (Graphic g in Tml.Graphics)
                    {
                        if (Convert.ToInt32(g.Attributes["Status"]) > 2)
                        {
                            Graphic gr = new Graphic
                            {
                                Geometry = g.Geometry,
                                Symbol = LayoutRoot.Resources["ErrTml"] as ESRI.ArcGIS.Client.Symbols.Symbol, // newSymbol is a SimpleMarkerSymbol (point)
                            };
                            graphicsLayer.Graphics.Add(gr);
                        }
                    }
                }
                if (Concentrator.Visible == true)
                {
                    foreach (Graphic g in Concentrator.Graphics)
                    {
                        if (Convert.ToInt32(g.Attributes["Status"]) > 3)
                        {
                            Graphic gr = new Graphic
                            {
                                Geometry = g.Geometry,
                                Symbol = LayoutRoot.Resources["ErrConcentrator"] as ESRI.ArcGIS.Client.Symbols.Symbol, // newSymbol is a SimpleMarkerSymbol (point)
                            };
                            graphicsLayer.Graphics.Add(gr);
                        }
                    }
                }
                if (Controller.Visible == true)
                {
                    foreach (Graphic g in Controller.Graphics)
                    {
                        if (Convert.ToInt32(g.Attributes["Status"]) > 3)
                        {
                            Graphic gr = new Graphic
                            {
                                Geometry = g.Geometry,
                                Symbol = LayoutRoot.Resources["ErrController"] as ESRI.ArcGIS.Client.Symbols.Symbol, // newSymbol is a SimpleMarkerSymbol (point)
                            };
                            graphicsLayer.Graphics.Add(gr);
                        }
                    }
                }
            }
        }      
        //隐藏工具栏
        private void hideB_Click(object sender, RoutedEventArgs e)
        {
            //if (model.IsEditMapGisEnable == false)
            //{
            //    hideB.Visibility = Visibility.Hidden;
            //}
            //else
            //{
            //    hideB.Visibility = Visibility.Visible;
            //} 
            Storyboard storysh = (Storyboard)LayoutRoot.FindResource("OnClickingSh");
            Storyboard storyop = (Storyboard)LayoutRoot.FindResource("OnClickingOp");
            if (sp.Height < 70)
            {
                storyop.Begin();
            }
            else
            {
                storysh.Begin();
                chkEdit.IsChecked = false;
                chkErr.IsChecked = false;
                DelSelectionButton.IsEnabled = false;
                AddMultiController.IsEnabled = false;
                AddConcentrator.IsEnabled = false;
                AddLine.IsEnabled =false ;
                AddTml.IsEnabled = false;
                ShowNullButton.IsEnabled = false;
                //DelNullButton.IsEnabled = false;
                BindButton.IsEnabled = false;
                UpdateMapButton.IsEnabled = false;
                //MyFeatureDataForm.Visibility = Visibility.Hidden;
                FindDetailsDataGrid.Visibility = Visibility.Hidden;
            }
        }
        
        //private void chkRender_Click(object sender, RoutedEventArgs e)
        //{

        //    if (chkRender.IsChecked == true)
        //    {
        //        QueryTask queryTask = new QueryTask();
        //        queryTask.Url = _mapService.UrlMapService + "/0";
        //        queryTask.ExecuteCompleted += queryTask_ExecuteCompleted;
        //        Query query = new ESRI.ArcGIS.Client.Tasks.Query();
        //        query.OutSpatialReference = MyMap.SpatialReference;
        //        query.ReturnGeometry = true;
        //        query.Where = "1=1";
        //        queryTask.ExecuteAsync(query);
        //    }
        //    else
        //    {
        //        MyGraphicsLayer.Graphics.Clear();
        //    }
        //}               //UseAcceleratedDisplay必须置为false ，影响选中效果
        //void queryTask_ExecuteCompleted(object sender, QueryEventArgs args)
        //{
        //    FeatureSet featureSet = args.FeatureSet;

        //    if (featureSet == null || featureSet.Features.Count < 1)
        //    {
        //        MessageBox.Show("No features retured from query");
        //        return;
        //    }

        //    GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;

        //    foreach (Graphic graphic in featureSet.Features)
        //    {
        //        graphic.Symbol = LayoutRoot.Resources["MediumMarkerSymbol"] as ESRI.ArcGIS.Client.Symbols.SimpleMarkerSymbol;
        //        graphicsLayer.Graphics.Add(graphic);
        //    }
        //}

        //move features~~~~~~~
        EditGeometry editGeometry;
        Graphic selectedPointGraphic;
        //移动图元
        private void MyMap_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (selectedPointGraphic != null)
            {
                selectedPointGraphic.Selected = true ;
                selectedPointGraphic = null;
            }
            INIClass iniC = new INIClass(@".\MapGis.ini");
            try
            {
                iniC.IniWriteValue("MapGis", "Resolution", MyMap.Resolution.ToString());
            }
            catch { }
        }
        private void MyMap_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (chkEdit.IsChecked == false)
                return;
            if (selectedPointGraphic != null)
            {
                //selectedPointGraphic.Geometry = MyMap.ScreenToMap(e.GetPosition(MyMap));
                var p = selectedPointGraphic.Geometry as MapPoint;
                selectedPointGraphic.Geometry = MyMap.ScreenToMap(e.GetPosition(MyMap));
                var g = selectedPointGraphic.Geometry as MapPoint;
                double xT = g.X - p.X;
                double yT = g.Y - p.Y;
                foreach (Graphic gr in Tml.SelectedGraphics)
                {
                    if (gr != selectedPointGraphic)
                    {
                        var t = gr.Geometry as MapPoint;
                        double x = t.X + xT;
                        double y = t.Y + yT;
                        gr.Geometry = new ESRI.ArcGIS.Client.Geometry.MapPoint(x, y);
                        gr.Geometry.SpatialReference = MyMap.SpatialReference;
                    }
                    var k = gr.Geometry as MapPoint;
                    Graphic gt = new Graphic();
                    gt.Geometry = ChangeCoodinateTo84(k.X, k.Y);
                    MapPoint mp = new MapPoint();
                    mp = gt.Geometry as MapPoint;
                    double xt = mp.X;
                    double yt = mp.Y;
                    int rtid = Convert.ToInt32(gr.Attributes["Lid"]);
                    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .OnEquipmentMentMapLocationChangeByMap(
                    rtid, xt, yt);
                      
                }               
                foreach (Graphic gr in Controller.SelectedGraphics)
                {
                    if (gr != selectedPointGraphic)
                    {
                        var t = gr.Geometry as MapPoint;
                        double x = t.X + xT;
                        double y = t.Y + yT;
                        gr.Geometry = new ESRI.ArcGIS.Client.Geometry.MapPoint(x, y);
                        gr.Geometry.SpatialReference = MyMap.SpatialReference;
                    }
                }
                foreach (Graphic gr in Concentrator.SelectedGraphics)
                {
                    if (gr != selectedPointGraphic)
                    {
                        var t = gr.Geometry as MapPoint;
                        double x = t.X + xT;
                        double y = t.Y + yT;
                        gr.Geometry = new ESRI.ArcGIS.Client.Geometry.MapPoint(x, y);
                        gr.Geometry.SpatialReference = MyMap.SpatialReference;
                    }
                }
            }
        }
        //批量右击菜单
        List<int> listRtu = new List<int>();
        List<int> listConcentrator = new List<int>();
        Dictionary<int,List<int>> listController =new  Dictionary<int,List<int>>();
        private void MyMap_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            int count = 0;

            listRtu.Clear();
            listConcentrator.Clear();
            listController.Clear();
            if (Tml.SelectedGraphics.Count() > 0)
                foreach (Graphic g in Tml.SelectedGraphics)
                {

                    //遍历终端图层思密达
                    if ((Convert.ToInt32(g.Attributes["Lid"]) > 1000000) &&
                        (Convert.ToInt32(g.Attributes["Lid"]) < 1100000))
                    {
                        listRtu.Add(Convert.ToInt32(g.Attributes["Lid"]));
                        count++;
                    }
            }
            if (Concentrator.SelectedGraphics.Count() >0)
            {
                foreach (Graphic g in Concentrator.SelectedGraphics)          //遍历终端图层思密达
                    if ((Convert.ToInt32(g.Attributes["Lid"]) > 1500000) && (Convert.ToInt32(g.Attributes["Lid"]) < 1600000))
                    {
                        listConcentrator.Add(Convert.ToInt32(g.Attributes["Lid"]));
                        count++;
                    }
            }
            if (Controller.SelectedGraphics.Count() > 0)
            {
                foreach (Graphic g in Controller.SelectedGraphics)          //遍历终端图层思密达
                    if ((Convert.ToInt32(g.Attributes["Lid"]) > 0) && (Convert.ToInt32(g.Attributes["Bid"]) > 1500000) && (Convert.ToInt32(g.Attributes["Bid"]) < 1600000))
                    {
                        int sluid = Convert.ToInt32(g.Attributes["Bid"]);
                        int ctrlid =Convert.ToInt32(g.Attributes["Lid"]);

                        if (!listController.ContainsKey(sluid)) listController.Add(sluid, new List<int>());
                        if (!listController[sluid].Contains(ctrlid)) listController[sluid].Add(ctrlid);
                        count++;
                    }
            }
            if (count <= 1) return;
            Model.GetCm(listRtu, listConcentrator, listController);
            Model.Cm.IsOpen = true;

        } //批量右击
        //批量增加控制器 先画线  然后 输入增加的数量
        private void AddMultiController_Click(object sender, RoutedEventArgs e)
        {
            //lvf need
            if (Concentrator.SelectionCount > 1)
            {
                var infos = WlstMessageBox.Show("提示", "只能选中一个集中器", WlstMessageBoxType.Close);
                return;
            }
            else if (Concentrator.SelectionCount == 0)
            {
                var infos = WlstMessageBox.Show("提示", "请选中一个集中器", WlstMessageBoxType.Close);
                return;
            }
            else if (Concentrator.SelectionCount == 1)
            {
                if (Concentrator.SelectedGraphics.ToList()[0].Attributes["Lid"] == null)
                {
                    var infos = WlstMessageBox.Show("提示", "您选中的集中器没有绑定", WlstMessageBoxType.Close);
                    return;

                }
                else
                {
                    AddMultiController.Style = LayoutRoot.Resources["AddController1"] as Style;
                    AddTml.Style = LayoutRoot.Resources["AddTml0"] as Style;
                    AddConcentrator.Style = LayoutRoot.Resources["AddConcentrator0"] as Style;
                    AddLine.Style = LayoutRoot.Resources["AddLine0"] as Style;
                    _drawObject = new Draw(MyMap)
                    {
                        DrawMode = DrawMode.Polyline,
                        IsEnabled = true,
                        LineSymbol = LayoutRoot.Resources["DefaultLineSymbol"] as ESRI.ArcGIS.Client.Symbols.LineSymbol
                    };
                    _drawObject.DrawComplete += DrawObject_DrawComplete;
                    _drawObject.DrawBegin += DrawObject_DrawBegin;
                    AddMultiController.Content = "请在地图上画线";
                    onDraw = true;
                }

            }
         
        }
        private void DrawObject_DrawBegin(object sender, EventArgs args)
        {
            GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
            graphicsLayer.Graphics.Clear();
        }
        double x1;
        double x2;
        double y1;
        double y2;
        int num ;
        double xT;
        double yT;
        ESRI.ArcGIS.Client.Geometry.Polyline polylineT;
        private void DrawObject_DrawComplete(object sender, DrawEventArgs args)
        {
            AddMultiController.Style = LayoutRoot.Resources["AddController0"] as Style;
            ESRI.ArcGIS.Client.Geometry.Polyline polyline = args.Geometry as ESRI.ArcGIS.Client.Geometry.Polyline;
            polylineT = polyline;
            polyline.SpatialReference = MyMap.SpatialReference;
            Graphic graphic = new Graphic()
            {
                Symbol = LayoutRoot.Resources["DefaultLineSymbol"] as ESRI.ArcGIS.Client.Symbols.Symbol,
                Geometry = polyline
            };

            GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
            graphicsLayer.Graphics.Add(graphic);
            AddMultiController.Content = "请输入控制器数量并安回车";
            
        }
        private void AddCtrllerNum_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!isNumberic(e.Text))
            {
                e.Handled = true;
            }
            else
                e.Handled = false;
        }
        public static bool isNumberic(string _string)
        {
            if (string.IsNullOrEmpty(_string))
                return false;
            foreach (char c in _string)
            {
                if (!char.IsDigit(c))
                    //if(c<'0' c="">'9')//最好的方法,在下面测试数据中再加一个0，然后这种方法效率会搞10毫秒左右
                    return false;
            }
            return true;
        }
        private void AddCtrllerNum_KeyDown(object sender, KeyEventArgs e)
        {

            if (onDraw == false) return;
            if (AddCtrllerNum.Text == null) return;
            if (e.Key == Key.Enter)
            {
                  
                    int index = this.AddCtrllerType.SelectedIndex;
                    string indexx = index.ToString();
                    int nMax=0;
                    //switch (xfxfx)
                    //{
                    //    case 0:
                    //        index = "0"; break;
                    //    case 1:
                    //        index = "7"; break;               //电子地图图标 宜兴用
                    //    case 2:
                    //        index = "0"; break;
                    //}
                    num = int.Parse(AddCtrllerNum.Text.Trim());
                    if (num > 0 & num < 255)
                    {
                        if (!Concentrator.SelectedGraphics.ToList()[0].Attributes.ContainsKey("Lid")) return;
                        int sluid = (int)Concentrator.SelectedGraphics.ToList()[0].Attributes["Lid"];
                        if(sluid ==0)
                        {
                            var infos = WlstMessageBox.Show("提示", "您选中的集中器没有绑定", WlstMessageBoxType.Close);
                            return;
                        }
                        int ctrlid = 0;
                        foreach (ESRI.ArcGIS.Client.Geometry.PointCollection path in polylineT.Paths)
                        {
                            int intTmp = 0;
                            foreach (MapPoint mp in path)
                            {
                                intTmp = intTmp + 1;
                                if (intTmp == 1)
                                {
                                    x1 = mp.X;
                                    y1 = mp.Y;
                                }
                                else if (intTmp == 2)
                                {
                                    x2 = mp.X;
                                    y2 = mp.Y;
                                }
                            }
                        }
                        xT = (x2 - x1) / num;
                        yT = (y2 - y1) / num;
                        GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
                        graphicsLayer.Graphics.Clear();
                        var temp = new List<Graphic>();
                        for (int i = 1; i < num + 1; i++)
                        {
                            MapPoint mp = new MapPoint();
                            mp.X = x1;
                            mp.Y = y1;
                            Graphic gr = new Graphic();
                            gr.Geometry = mp;
                            gr.Symbol = LayoutRoot.Resources["DefaultMarkerSymbol"] as ESRI.ArcGIS.Client.Symbols.Symbol;
                            gr.Geometry.SpatialReference = MyMap.SpatialReference;
                            if (index == 0)
                            {
                                //gr.Attributes.Add("Type", "10");
                                gr.Attributes["TYPE"] = "100";
                            }
                            else
                            {
                                //gr.Attributes.Add("Type", "20");
                                gr.Attributes["TYPE"] = "200";
                            }
                            //gr.Attributes["Type"] = indexx;
                            foreach (Graphic g in Controller.Graphics)          //遍历终端图层思密达
                                if (Convert.ToInt32(g.Attributes["Bid"]) ==Convert.ToInt32( Concentrator.SelectedGraphics.ToList()[0].Attributes["Lid"]))
                                {
                                    if (nMax < Convert.ToInt32(g.Attributes["Lid"]))
                                        nMax = Convert.ToInt32(g.Attributes["Lid"]);
                                }
                            var sluinfo =
               Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(sluid);
                            var cons = sluinfo as Wlst.Sr.EquipmentInfoHolding.Model.Wj2090Slu;
                            if (cons == null) return;
                            foreach (var g in cons.WjSluCtrls)
                            {
                                if (g.Value.CtrlPhyId == nMax + i)
                                {
                                    // NodeIds = string.Format("{0:D3}", cons.SluRegulators.DicRtuParaSluRegulator[NodeId].PhyId);

                                    ctrlid = g.Value.CtrlId; //转换为逻辑地址
                                    break;
                                }
                            }
                            if (Concentrator.SelectionCount == 1)
                            {
                                gr.Attributes["Bid"] = sluid;//Concentrator.SelectedGraphics.ToList()[0].Attributes["Lid"];
                                gr.Attributes.Add("Lid", ctrlid);
                                //gr.Attributes.Add("Lid", i+nMax );
                            }
                 
                            x1 = x1 + xT;
                            y1 = y1 + yT;
                            temp.Add(gr);
                        }
                        Controller.Graphics.AddRange(temp);

                    }
                    //xT = 0;
                    //yT = 0;
                    Controller.SaveEdits();
                    Controller.Update();
                    AddMultiController.Content = "批量添加控制器";
                    _drawObject.IsEnabled = false;
                    onDraw = false;
            }
        }
        private void AddTml_Click(object sender, RoutedEventArgs e)
        {
            AddTml.Style = LayoutRoot.Resources["AddTml1"] as Style;
            AddConcentrator.Style =LayoutRoot.Resources["AddConcentrator0"] as Style ;
            AddMultiController.Style = LayoutRoot.Resources["AddController0"] as Style;
            AddLine.Style = LayoutRoot.Resources["AddLine0"] as Style;
            _drawObject = new Draw(MyMap)
            {
                DrawMode = DrawMode.Point,
                IsEnabled = true,
            };
            _drawObject.DrawComplete += DrawObjectTml_DrawComplete;
            _drawObject.DrawBegin += DrawObject_DrawBegin;
   
        }
        //增加集中器
        private void AddConcentrator_Click(object sender, RoutedEventArgs e)
        {
            AddConcentrator.Style = LayoutRoot.Resources["AddConcentrator1"] as Style;
            AddTml.Style = LayoutRoot.Resources["AddTml0"] as Style;
            AddMultiController.Style = LayoutRoot.Resources["AddController0"] as Style;
            AddLine.Style = LayoutRoot.Resources["AddLine0"] as Style;
            _drawObject = new Draw(MyMap)
            {
                DrawMode = DrawMode.Point,
                IsEnabled = true,
            };
            _drawObject.DrawComplete += DrawObjectConcentrator_DrawComplete;
            _drawObject.DrawBegin += DrawObject_DrawBegin;
        }
        private void DrawObjectTml_DrawComplete(object sender, DrawEventArgs args)
        {
            AddTml.Style = LayoutRoot.Resources["AddTml0"] as Style;
            ESRI.ArcGIS.Client.Geometry.MapPoint point = args.Geometry as ESRI.ArcGIS.Client.Geometry.MapPoint;
            point.SpatialReference = MyMap.SpatialReference;

            Graphic graphic = new Graphic()
            {
                Symbol = LayoutRoot.Resources["DefaultMarkerSymbol"] as ESRI.ArcGIS.Client.Symbols.Symbol,
                Geometry = point,

            };
            graphic.Attributes.Add("Lid", 0);

            graphic.Geometry.SpatialReference = MyMap.SpatialReference;
            Tml.Graphics.Add(graphic);  
            System.Threading.Thread.Sleep(28);
            _drawObject.IsEnabled = false;
            //Tml.Update();
 
            Tml.SaveEdits();
            Tml.Update();
            
        }
        private void DrawObjectConcentrator_DrawComplete(object sender, DrawEventArgs args)
        {
            AddConcentrator.Style = LayoutRoot.Resources["AddConcentrator0"] as Style;
            ESRI.ArcGIS.Client.Geometry.MapPoint point = args.Geometry as ESRI.ArcGIS.Client.Geometry.MapPoint;
            point.SpatialReference = MyMap.SpatialReference;
            Graphic graphic = new Graphic()
            {
                Symbol = LayoutRoot.Resources["DefaultMarkerSymbol"] as ESRI.ArcGIS.Client.Symbols.Symbol,
                Geometry = point
            };
            graphic.Geometry.SpatialReference = MyMap.SpatialReference;
            Concentrator.Graphics.Add(graphic);
            //System.Threading.Thread.Sleep(28);
            _drawObject.IsEnabled = false;
            Concentrator.SaveEdits();
            Concentrator.Update();
        }
        //清空选中
        private void DelSelectionButton_Click(object sender, RoutedEventArgs e)
        {
            DelSelectionButton.Style = LayoutRoot.Resources["DelSelected1"] as Style;

            AddConcentrator.Style = LayoutRoot.Resources["AddConcentrator0"] as Style;
            AddTml.Style = LayoutRoot.Resources["AddTml0"] as Style;
            AddMultiController.Style = LayoutRoot.Resources["AddController0"] as Style;
            AddLine.Style = LayoutRoot.Resources["AddLine0"] as Style;
            try
            {
                var infoss = WlstMessageBox.Show("确认删除", "是否删除所选设备？", WlstMessageBoxType.YesNo);
                DelSelectionButton.Style = LayoutRoot.Resources["DelSelected0"] as Style;
                if (infoss != WlstMessageBoxResults.Yes) return;
                

                ESRI.ArcGIS.Client.Editor myEditor = LayoutRoot.Resources["MyEditor"] as ESRI.ArcGIS.Client.Editor;
                myEditor.Map = MyMap;
                myEditor.LayerIDs = new string[] { "终端", "控制器", "集中器","线路" };
                myEditor.DeleteSelected.Execute(null);
            }
            catch
            {
            }
        }
        //删除无效图元，但是前台已屏蔽改按钮，原因是可能这些无效图元是采集上来的但还没有绑定的图元，不应该被手动删除
        private void DelNullButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ESRI.ArcGIS.Client.Editor myEditor = LayoutRoot.Resources["MyEditor"] as ESRI.ArcGIS.Client.Editor;
                myEditor.Map = MyMap;
                myEditor.LayerIDs = new string[] { "终端", "控制器", "集中器","线路" };
                myEditor.ClearSelection.Execute(null);
            }
            catch { }
            foreach (Graphic g in Controller.Graphics)
            {
                if (g.Attributes["Lid"] == null)
                {
                    g.Select();  
                }
            }
            foreach (Graphic g in Tml.Graphics)
            {
                if (g.Attributes["Lid"] == null)
                {
                    g.Select();
                }
            }
            foreach (Graphic g in Concentrator.Graphics)
            {
                if (g.Attributes["Lid"] == null)
                {
                    g.Select();
                }
            }
            try
            {
                var infoss = WlstMessageBox.Show("确认删除", "是否删除所选设备？", WlstMessageBoxType.YesNo);
                if (infoss != WlstMessageBoxResults.Yes) return;

                ESRI.ArcGIS.Client.Editor myEditor = LayoutRoot.Resources["MyEditor"] as ESRI.ArcGIS.Client.Editor;
                myEditor.Map = MyMap;
                myEditor.LayerIDs = new string[] { "Tml" };
                myEditor.DeleteSelected.Execute(null);
            }
            catch
            {

            }

        }
        //显示无效设备
        private void ShowNullButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
              
                ESRI.ArcGIS.Client.Editor myEditor = LayoutRoot.Resources["MyEditor"] as ESRI.ArcGIS.Client.Editor;
                myEditor.Map = MyMap;
                myEditor.LayerIDs = new string[] { "Tml" };
                myEditor.ClearSelection.Execute(null);
            }
            catch { }
            ShowNullButton.Style = LayoutRoot.Resources["ShowNull1"] as Style ;
            GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
            graphicsLayer.Graphics.Clear();
            foreach (Graphic g in Controller.Graphics)
            {
                if (g.Attributes["Lid"] == null)
                {
                    Graphic gr = new Graphic
                    {
                        Geometry = g.Geometry,
                        Symbol = LayoutRoot.Resources["DefaultMarkerSymbol"] as ESRI.ArcGIS.Client.Symbols.Symbol, // newSymbol is a SimpleMarkerSymbol (point)
                    };
                    graphicsLayer.Graphics.Add(gr);
                }
            }
            foreach (Graphic g in Tml.Graphics)
            {
                if (g.Attributes["Lid"] == null)
                {
                    Graphic gr = new Graphic
                    {
                        Geometry = g.Geometry,
                        Symbol = LayoutRoot.Resources["DefaultMarkerSymbol"] as ESRI.ArcGIS.Client.Symbols.Symbol, // newSymbol is a SimpleMarkerSymbol (point)
                    };
                    graphicsLayer.Graphics.Add(gr);
                }
            }
            foreach (Graphic g in Concentrator.Graphics)
            {
                if (g.Attributes["Lid"] == null)
                {
                    Graphic gr = new Graphic
                    {
                        Geometry = g.Geometry,
                        Symbol = LayoutRoot.Resources["DefaultMarkerSymbol"] as ESRI.ArcGIS.Client.Symbols.Symbol, // newSymbol is a SimpleMarkerSymbol (point)
                    };
                    graphicsLayer.Graphics.Add(gr);
                }
            }
            ShowNullButton.Style = LayoutRoot.Resources["ShowNull0"] as Style;
        }  
        //绑定图元，不管先后，只要树和地图都有选中的设备，然后点击绑定按钮，会做些许判断，确认无误后成功绑定
        private void BindButton_Click(object sender, RoutedEventArgs e)
        {
            BindButton.Style = LayoutRoot.Resources["Bind1"] as Style;
            AddConcentrator.Style = LayoutRoot.Resources["AddConcentrator0"] as Style;
            AddTml.Style = LayoutRoot.Resources["AddTml0"] as Style;
            AddMultiController.Style = LayoutRoot.Resources["AddController0"] as Style;
            AddLine.Style = LayoutRoot.Resources["AddLine0"] as Style;
            FeatureLayer BingFL= new FeatureLayer();
            double snum=0;
            if (TreeSelectId < 1000000) return;
            if (Tml.SelectionCount > 0)
            {
                snum = snum + Tml.SelectionCount;
                BingFL = Tml;
            }
            if (Controller.SelectionCount > 0)
            {
                snum = snum + Controller.SelectionCount;
                BingFL = Controller;
            }
            if (Concentrator.SelectionCount > 0)
            {
                snum = snum + Concentrator.SelectionCount;
                BingFL = Concentrator;
            }
            if (snum == 1 )
            {
                if (BingFL.SelectedGraphics.ToList()[0].Attributes["Lid"] != null)
                {
                    var infos = WlstMessageBox.Show("绑定提示", "选中图元已绑定，是否覆盖绑定", WlstMessageBoxType.YesNo);
                    if (infos != WlstMessageBoxResults.Yes) return;
                    
                }
                    if (TreeSelectId < 1100000 && TreeSelectId > 1000000 )//终端
                    {
                        if (BingFL == Tml)
                        {
                            BingFL.SelectedGraphics.ToList()[0].Attributes["Lid"] = TreeSelectId;
                            var infoss = WlstMessageBox.Show("确认绑定", "是否绑定", WlstMessageBoxType.YesNo);
                            if (infoss != WlstMessageBoxResults.Yes) return;
                            BingFL.SaveEdits();
                            BingFL.Update();

                            var f =
                            Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems [TreeSelectId];//.EquipmentInfoDictionary[TreeSelectId];
                            
                            if (f.RtuMapX.Equals(0) || f.RtuMapY.Equals(0))
                            {
                                if (isUselocate == 0) return ;
                                var t = BingFL.SelectedGraphics.ToList()[0].Geometry as MapPoint;
                                Graphic gt = new Graphic();
                                gt.Geometry = ChangeCoodinateTo84(t.X, t.Y);
                                MapPoint mp = new MapPoint();
                                mp = gt.Geometry as MapPoint;
                                double xt = mp.X;
                                double yt = mp.Y;
                                Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .OnEquipmentMentMapLocationChangeByMap(
                                TreeSelectId, xt, yt);
                            }
                            else
                            {
                                if (isUselocate == 0) return;
                                BingFL.SelectedGraphics.ToList()[0].Geometry = ChangeCoodinateFrom84(f.RtuMapX, f.RtuMapY);
                                BingFL.SelectedGraphics.ToList()[0].Geometry.SpatialReference = MyMap.SpatialReference;
                            }
                        }
                        else
                        {
                            var infoss = WlstMessageBox.Show("错误", "列表设备与地图设备不匹配");
                        }
                        //foreach (Graphic g in Tml.SelectedGraphics)          
                        //{
                        //    g.Attributes["Lid"] = TreeSelectId;
                        //}
                    }

                    if (TreeSelectId < 1600000 && TreeSelectId > 1500000 && BingFL == Concentrator) //集中器
                    {
                        if (BingFL == Concentrator)
                        {
                            BingFL.SelectedGraphics.ToList()[0].Attributes["Lid"] = TreeSelectId;
                            var infoss = WlstMessageBox.Show("确认绑定", "是否绑定", WlstMessageBoxType.YesNo);
                            if (infoss != WlstMessageBoxResults.Yes) return;
                            BingFL.SaveEdits();
                            BingFL.Update();
                        }
                        else
                        {
                            var infoss = WlstMessageBox.Show("错误", "列表设备与地图设备不匹配");
                        }

                    }   

                    if (TreeSelectId < 1600000000 && TreeSelectId > 1500000000 ) //控制器
                    {
                        if (BingFL == Controller)
                        {
                            int ctrId;
                            int concentratorId;
                            ctrId = TreeSelectId % 1000;
                            concentratorId = Convert.ToInt32(TreeSelectId / 1000);
                            BingFL.SelectedGraphics.ToList()[0].Attributes["Lid"] = ctrId;
                            BingFL.SelectedGraphics.ToList()[0].Attributes["Bid"] = concentratorId;
                            var infoss = WlstMessageBox.Show("确认绑定", "是否绑定", WlstMessageBoxType.YesNo);
                            if (infoss != WlstMessageBoxResults.Yes) return;
                            BingFL.SaveEdits();
                            BingFL.Update();
                        }
                        else 
                        {
                            var infoss = WlstMessageBox.Show("错误", "列表设备与地图设备不匹配");
                        }
                        //foreach (Graphic g in Controller.SelectedGraphics)         
                        //{
                        //    g.Attributes["Lid"] = ctrId;
                        //    g.Attributes["Bid"] = concentratorId;
                        //}
                    }
                    BindButton.Style = LayoutRoot.Resources["Bind0"] as Style;
            }
            else
            {
                var infoss = WlstMessageBox.Show("绑定出错", "选中了0个或多个设备，不能执行绑定，请清空地图选择项，并重新选择需要绑定的设备");
                BindButton.Style = LayoutRoot.Resources["Bind0"] as Style;
            }
        } 
        //更新地图，读取数据库中数据，跟地图对比后，将没有的设备添加到地图上
        private void UpdateMapButton_Click(object sender, RoutedEventArgs e)//手机app数据 更新地图
        {
            UpdateMapButton.Style = LayoutRoot.Resources["UpdateMap1"] as Style;
            AddConcentrator.Style = LayoutRoot.Resources["AddConcentrator0"] as Style;
            AddTml.Style = LayoutRoot.Resources["AddTml0"] as Style;
            AddMultiController.Style = LayoutRoot.Resources["AddController0"] as Style;
            AddLine.Style = LayoutRoot.Resources["AddLine0"] as Style;
            foreach (var f in Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems )//.EquipmentInfoDictionary)//终端
            {
                Boolean needU;
                var bs = f.Value as Sr.EquipmentInfoHolding.Model.WjParaBase;// Wlst.Cr.WjEquipmentBaseModels.Models.EquipmentInfomation;
                if (bs == null) continue;
                if(bs.RtuGisX==0 || bs.RtuGisY==0 ) continue;
                needU = true;
                if (f.Key > 1000001 && f.Key < 1100000)//终端  自己筛选自己需要的设备 
                {
                    foreach (Graphic g in Tml.Graphics)          //遍历终端图层思密达
                        if (Convert.ToInt32(g.Attributes["Lid"]) ==f.Key  )
                        {
                            needU = false;
                            break;       
                        }
                    if (needU == true) 
                    {
                        var gisx = bs.RtuGisX;
                        var gisy = bs.RtuGisY;
                        Graphic graphic = new Graphic()
                        {
                            Symbol = LayoutRoot.Resources["DefaultMarkerSymbol"] as ESRI.ArcGIS.Client.Symbols.Symbol,
                            Geometry = ChangeCoodinateFrom84(gisx, gisy)
                        };
                        graphic.Attributes["Lid"]=f.Key;
                        graphic.Geometry.SpatialReference = MyMap.SpatialReference;
                        Tml.Graphics.Add(graphic);
                        System.Threading.Thread.Sleep(28);
                        Tml.Update();
                    } 
                }
            }
            foreach (var f in Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems )//.EquipmentInfoDictionary)
            {
                
                if (f.Key > 1500000 && f.Key < 1600000) //单灯
                {
                    var bs = f.Value as Sr.EquipmentInfoHolding.Model.Wj2090Slu;//Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface.IISluRegulators;
                    if (bs == null) continue;
                    foreach (var ctrl in bs.WjSluCtrls)//SluRegulators.DicRtuParaSluRegulator)
                    {
                        Boolean needU = true;
                        foreach (Graphic g in Controller.Graphics)          //遍历单灯图层思密达
                        {
                            if (Convert.ToInt32(g.Attributes["Bid"]) == f.Key && Convert.ToInt32(g.Attributes["Lid"])==ctrl.Value.CtrlId)
                            {
                                needU = false;
                                break;       
                            }
                        }
                        if (needU == true)
                        {
                            var sluId = f.Key;
                            var ctrlId = ctrl.Value.CtrlId;
                            var gisx = ctrl.Value.CtrlGisX;
                            var gisy = ctrl.Value.CtrlGisY;
                            Graphic graphic = new Graphic()
                            {
                                Symbol = LayoutRoot.Resources["DefaultMarkerSymbol"] as ESRI.ArcGIS.Client.Symbols.Symbol,
                                Geometry = ChangeCoodinateFrom84(gisx, gisy)
                            };
                            graphic.Attributes["Bid"] = sluId;
                            graphic.Attributes["Lid"] = ctrlId;
                            graphic.Geometry.SpatialReference = MyMap.SpatialReference;
                            Controller.Graphics.Add(graphic);
                            System.Threading.Thread.Sleep(28);
                            Controller.Update();
                        }
                    }
                }
            }
            UpdateMapButton.Style = LayoutRoot.Resources["UpdateMap0"] as Style;
        }
        private void AddLine_Click(object sender, RoutedEventArgs e)
        {
            AddLine.Style = LayoutRoot.Resources["AddLine1"] as Style;
            AddConcentrator.Style = LayoutRoot.Resources["AddConcentrator0"] as Style;
            AddTml.Style = LayoutRoot.Resources["AddTml0"] as Style;
            AddMultiController.Style = LayoutRoot.Resources["AddController0"] as Style;
            _drawObject = new Draw(MyMap)
            {
                DrawMode = DrawMode.Polyline,
                IsEnabled = true,
                LineSymbol = LayoutRoot.Resources["DefaultLineSymbol"] as ESRI.ArcGIS.Client.Symbols.LineSymbol
            };
            _drawObject.DrawComplete += DrawObjectLine_DrawComplete;
            _drawObject.DrawBegin += DrawObject_DrawBegin;
        }
        private void DrawObjectLine_DrawComplete(object sender, DrawEventArgs args)
        {
            AddLine.Style = LayoutRoot.Resources["AddLine0"] as Style;
            ESRI.ArcGIS.Client.Geometry.Polyline polyline = args.Geometry as ESRI.ArcGIS.Client.Geometry.Polyline;
            polylineT = polyline;
            polyline.SpatialReference = MyMap.SpatialReference;
            Graphic graphic = new Graphic()
            {
                Symbol = LayoutRoot.Resources["DefaultLineSymbol"] as ESRI.ArcGIS.Client.Symbols.Symbol,
                Geometry = polyline
            };

            GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
            Circuit.Graphics.Add(graphic);
            System.Threading.Thread.Sleep(28);
            _drawObject.IsEnabled = false;
            Circuit.Update();

        }

        #endregion

        #region Jinx
        /* 
         * 后台处理事件
         *  获取到广播，处理做出相应的反映
         *  2014.12   Lvf
         *  你知道Jinx是谁？有缘人~就像她一样解决问题吧~加油
         */
        private static ESRI.ArcGIS.Client.Projection.WebMercator _mercator =
           new ESRI.ArcGIS.Client.Projection.WebMercator();   //wgs 84 web 
        
        public void LocateSth(string  sid)                     //设备定位
        {
                try
                {
                    ESRI.ArcGIS.Client.Editor myEditor = LayoutRoot.Resources["MyEditor"] as ESRI.ArcGIS.Client.Editor;
                    myEditor.Map = MyMap;
                    myEditor.LayerIDs = new string[] { "Tml" };
                    myEditor.ClearSelection.Execute(null);
                    //INIClass iniC = new INIClass(@".\MapGis.ini");
                    //iniC.IniWriteValue("MapGis", "lastTml", sid.ToString());
                    //iniC.IniWriteValue("MapGis", "Resolution", MyMap.Resolution.ToString());

                }
                catch { }

            //if (sid < 1100000 && sid > 0)
            //{

                foreach (Graphic g in Tml.Graphics)
                {
                    if (g.Attributes[bsField] == null) continue;
                    //遍历终端图层思密达
                    if (g.Attributes[bsField].ToString() == sid)
                    {

                        g.Select();
                        MyMap.PanTo(g.Geometry);
                        MyFeatureDataForm.FeatureLayer = Tml;
                        MyFeatureDataForm.GraphicSource = g;
                        //MyMap.PanTo(g.Geometry);
                        //ESRI.ArcGIS.Client.Geometry.MapPoint mapCenter = MyMap.Extent.GetCenter();
                        //if (MyMap.Resolution == 1)
                        //{
                        //    MyMap.PanTo(g.Geometry);
                        //}
                        //else
                        //{
                        //    double X = g.Geometry.Extent.XMin;// + (g.Geometry.Extent.XMax - g.Geometry.Extent.XMin) / 2;
                        //    double Y = g.Geometry.Extent.YMin;// + (g.Geometry.Extent.YMax - g.Geometry.Extent.YMin) / 2;
                        //    MyMap.ZoomToResolution(3, new ESRI.ArcGIS.Client.Geometry.MapPoint(X, Y));
                        //}
                    }
                }
            //}
            //else if (sid < 1600000 && sid > 1500000)
            //{
               
            //    foreach (Graphic g in Concentrator.Graphics)          //遍历终端图层思密达
            //        if (Convert.ToInt32(g.Attributes["Lid"]) == sid)
            //        {
            //            g.Select();
            //            MyMap.PanTo(g.Geometry);
            //            MyFeatureDataForm.FeatureLayer = Concentrator;
            //            MyFeatureDataForm.GraphicSource = g;
            //            //MyMap.PanTo(g.Geometry);
            //            //ESRI.ArcGIS.Client.Geometry.MapPoint mapCenter = MyMap.Extent.GetCenter();
            //            //if (MyMap.Resolution == 1)
            //            //{
            //            //    MyMap.PanTo(g.Geometry);
            //            //}
            //            //else
            //            //{
            //            //    double X = g.Geometry.Extent.XMin;// + (g.Geometry.Extent.XMax - g.Geometry.Extent.XMin) / 2;
            //            //    double Y = g.Geometry.Extent.YMin;// + (g.Geometry.Extent.YMax - g.Geometry.Extent.YMin) / 2;
            //            //    MyMap.ZoomToResolution(3, new ESRI.ArcGIS.Client.Geometry.MapPoint(X, Y));
            //            //}
            //        }
            //}
            //else if (sid < 1600000000 && sid > 1500000000)
            //{
            //    int ctrId;
            //    int concentratorId;
            //    ctrId = sid % 1000;
            //    concentratorId = Convert.ToInt32(sid / 1000);
            //    foreach (Graphic g in Controller.Graphics)          //遍历终端图层思密达
            //        if (Convert.ToInt32(g.Attributes["Bid"]) == concentratorId && Convert.ToInt32(g.Attributes["Lid"]) == ctrId)
            //        {
            //            g.Select();
            //            MyMap.PanTo(g.Geometry);
            //            MyFeatureDataForm.FeatureLayer = Controller;
            //            MyFeatureDataForm.GraphicSource = g;
            //            //MyMap.PanTo(g.Geometry);
            //            //ESRI.ArcGIS.Client.Geometry.MapPoint mapCenter = MyMap.Extent.GetCenter();
            //            //if (MyMap.Resolution == 1)
            //            //{
            //            //    MyMap.PanTo(g.Geometry);
            //            //}
            //            //else
            //            //{
            //            //    double X = g.Geometry.Extent.XMin;// + (g.Geometry.Extent.XMax - g.Geometry.Extent.XMin) / 2;
            //            //    double Y = g.Geometry.Extent.YMin;// + (g.Geometry.Extent.YMax - g.Geometry.Extent.YMin) / 2;
            //            //    MyMap.ZoomToResolution(3, new ESRI.ArcGIS.Client.Geometry.MapPoint(X, Y));
            //            //}
            //        }
            //}
        }
        public void AddFeatures(int sid)                     //添加设备
        {
            if (sid < 1100000 && sid > 1000000)
            {
                Graphic graphic = new Graphic();
                GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
                
                graphic.Attributes.Add("NAME", "123");
                graphic.Attributes.Add("CODE", "123");
                //double Longitude = Convert.ToDouble(MyMap.Extent.GetCenter());
                //double Latitude = Convert.ToDouble(MyMap.Extent.GetCenter ());
                graphic.Geometry = Tml.FullExtent.GetCenter();//new MapPoint(Longitude, Latitude);
                graphic.Geometry.SpatialReference = MyMap.SpatialReference;
                graphicsLayer.Graphics.Clear();
                Tml.Graphics.Add(graphic);
                MyMap.ZoomToResolution(1, Tml.FullExtent.GetCenter());
                Tml.Update();
                //MyMap.Zoom(2500);
                //MyMap.PanTo(graphic.Geometry);
            }
            else if (sid < 1600000)
            {
                for (int i = 0; i < Concentrator.SelectionCount; i++)
                    Concentrator.SelectedGraphics.ToList()[i].UnSelect();
                foreach (Graphic g in Concentrator.Graphics)         //遍历集中器图层思密达     
                    if (Convert.ToInt32(g.Attributes["LID"]) == sid)
                    {
                        g.Select();
                        MyMap.PanTo(g.Geometry);
                    }
            }
            else if (sid < 130000)
            {
                //for (int i = 0; i < ZD.SelectionCount; i++)
                //    ZD.SelectedGraphics.ToList()[i].UnSelect();
                //foreach (Graphic g in ZD.Graphics)        遍历灯杆图层思密达
                //    if (Convert.ToInt32(g.Attributes["LID"]) == sid)
                //    {
                //        g.Select();
                //        MyMap.PanTo(g.Geometry);
                //    }
            }
        }

       
        public void ModifyFeaturesAttribute1(int sid, string attributeName, string aValue) //修改图元属性呀
        {

            try
            {

                string tmpid = "";
                if (sid < 1100000 && sid > 1000000)
                {
                    if (FeatureDic.ContainsKey(sid))//苏州园区 将lid地址转换成 唯一识别码
                    {
                        tmpid = FeatureDic[sid];
                    }
                    foreach (Graphic g in Tml.Graphics) //修改终端图层思密达
                    {
                        if (g.Attributes[bsField] == null) continue;
                        if (g.Attributes[bsField].ToString().Trim() == tmpid)
                        {
                            //if (attributeName == "Status")
                            //{
                            //    aValue = g.Attributes["Type"] + aValue;
                            //}
                            //if (g.Attributes[attributeName] == aValue) return;
                            int os = Convert.ToInt16(g.Attributes[attributeName]);
                            if (os <= 6 || os > 26)
                            {
                                g.Attributes[attributeName] = Convert.ToInt16(aValue);
                            }
                            else
                            {
                                g.Attributes[attributeName] = os/10*10 + Convert.ToInt16(aValue);
                            }

                            //Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogInfo("arcMap:变图标 设备逻辑地址：" + sid + "改变状态为" +
                                                                               //aValue);
                            break;
                        }
                        
                        //Tml.SaveEdits();
                    }
                }
                else if (sid < 160000 && sid > 1500000)
                {
                    if (FeatureDic.ContainsKey(sid))
                    {
                        tmpid = FeatureDic[sid];
                    }
                    foreach (Graphic g in Concentrator.Graphics)
                    {
                        if (g.Attributes[bsField] == null) continue;
                        //修改集中器图层思密达
                        if (g.Attributes[bsField].ToString() == tmpid)
                        {
                            //if (attributeName == "Status")
                            //{
                            //    aValue = g.Attributes["Type"] + aValue;

                            if (g.Attributes[attributeName] == aValue) return;
                            g.Attributes[attributeName] = Convert.ToInt16(aValue);
                        }
                        Concentrator.SaveEdits();
                    }
                }
            }
            catch
            {
            }

        }

        /// <summary>
        /// sluid    , 2、ctrlid  name-value
        /// </summary>
        /// <param name="sluId"></param>
        /// <param name="namevalue"></param>
        public void ModifyFeaturesAttribute2(int sluId,Dictionary< int ,Tuple< string ,string >>  namevalue) //修改图元属性呀
        {

     
                if (namevalue == null  || namevalue.Count == 0) return;
                    //int ctrId;
                    //int concentratorId;
                    //ctrId = sid%1000;
                    //concentratorId = Convert.ToInt32(sid/1000);
                    foreach (Graphic g in Controller.Graphics) //遍历终端图层思密达
                    {
                        try
                        {
                            int sluIdinmap = Convert.ToInt32(g.Attributes["Bid"]);
                            if (sluIdinmap != sluId) continue;
                            int ctrlIdinmap = Convert.ToInt32(g.Attributes["Lid"]);
                            if (namevalue.ContainsKey(ctrlIdinmap))
                            {
                                if (g.Attributes[namevalue[ctrlIdinmap].Item1] == namevalue[ctrlIdinmap].Item2)
                                    continue;

                                g.Attributes[namevalue[ctrlIdinmap].Item1] =
                                    Convert.ToInt16(namevalue[ctrlIdinmap].Item2);
                            }
                        }
                        catch (Exception ex)
                        {
                            
                        }
                    }
                    Controller.SaveEdits();
               
            

        }

        public void ModifyFeaturesAttribute3(string attributeName) //修改图元属性呀
        {
            try
            {
                if (tmlIcon == null || tmlIcon.Count == 0) return;

                    //if (FeatureDicD.ContainsKey())//苏州园区 将lid地址转换成 唯一识别码
                    //{
                    //    tmpid = FeatureDic[sid];
                    //}
               
                    foreach (Graphic g in Tml.Graphics) //修改终端图层思密达
                    {
                        if (g.Attributes[bsField] == null) continue;
                        var wybh = g.Attributes[bsField].ToString().Trim();
                        
                        if (!FeatureDicD.ContainsKey(wybh)) continue;
                        var lid = FeatureDicD[wybh];
                        if (lid < 100000 || lid > 1100000) continue;
                        if (!tmlIcon.ContainsKey(lid)) continue;
                        if(tmlIcon[lid].Item1 == false  ) continue;//不用更新
                   
                        int os = Convert.ToInt16(g.Attributes[attributeName]);
                        if (os <= 6 || os > 26)
                        {
                            g.Attributes[attributeName] = Convert.ToInt16(tmlIcon[lid].Item2);
                            tmlIcon[lid] = new Tuple<bool, string>(false, tmlIcon[lid].Item2);
                        }
                        else
                        {
                            var a = Convert.ToInt16((os / 10) * 10 + Convert.ToInt16(tmlIcon[lid].Item2));
                            g.Attributes[attributeName] = Convert.ToInt16(a);
                            tmlIcon[lid] = new Tuple<bool, string>(false, tmlIcon[lid].Item2);
                        }

                        //Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogInfo("arcMap:变图标 设备逻辑地址：" + sid + "改变状态为" +
                        //aValue);



                        //Tml.SaveEdits();
                    }
                    Tml.SaveEdits();
                    Tml.Refresh();
                

            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex );
            }
        }
        
        public void FilterSth(string condition)
        {

        } //分析过滤
        string[] files;
        int n = 0;
        List<BitmapImage> bit = new List<BitmapImage> { };
        public void ShowPic(int lid)//显示图片
        {
            TmlImage.Source = null;
            var rtu = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(lid);//.GetEquipmentInfo(lid);
            if (rtu == null)
            {
                showPic.Visibility = Visibility.Hidden;
                //string dirr = Directory.GetCurrentDirectory() + "\\photos";
                //if (!Directory.Exists(dirr)) Directory.CreateDirectory(dirr);
                //string pathh = dirr + "\\addController1.png";
                //BitmapImage bitn = new BitmapImage(new Uri(pathh , UriKind.RelativeOrAbsolute));
                //TmlImage.Source= bitn;
                return;
            }
            var pid = rtu.RtuPhyId;
            string dir = Directory.GetCurrentDirectory() + "\\photos";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            string path = dir + "\\" + pid;
            if ((!Directory.Exists(path))) 
            {
                showPic.Visibility = Visibility.Hidden;
                return; 
            }
            files = Directory.GetFiles(path);// , "*.png",SearchOption.AllDirectories

            if (files == null)
            {
                showPic.Visibility = Visibility.Hidden;
                //string dirr = Directory.GetCurrentDirectory() + "\\photos";
                //if (!Directory.Exists(dirr)) Directory.CreateDirectory(dirr);
                //string pathh = dirr + "\\addController1.png";
                //BitmapImage bitn = new BitmapImage(new Uri(pathh, UriKind.RelativeOrAbsolute));
                //TmlImage.Source = bitn;
                return;
            }
            else
            {
                if (chkShowAttr.IsChecked == true)
                {
                    showPic.Visibility = Visibility.Visible;
                }
                else 
                {
                    showPic.Visibility = Visibility.Hidden;
                    return;
                }
               
                for (int i = 0; i < files.Length; i++)
                {
                    //bit[i].UriSource = new Uri(files[i]);
                    try
                    {
                        BitmapImage bitn = new BitmapImage(new Uri(files[i]));
                        bit.Add(bitn);
                    }
                    catch { }
                    //Debug.WriteLine(files[i]);
                }
                if (bit.Count > 0)
                {
                    TmlImage.Source = bit[1];
                }
            }
        }
        public void lastPic_Click(object sender, RoutedEventArgs e)
        {
            
            if (n + 1 < bit.Count)
            {
                n++;
            }
            else n = 0;
            TmlImage.Source = bit[n];
            
        }

        public void nextPic_Click(object sender, RoutedEventArgs e)
        {
            
            if (n + 1 >1)
            {
                n--;
            }
            else n = bit.Count-1 ;
            TmlImage.Source = bit[n];
            
        }
        public ESRI.ArcGIS.Client.Geometry.Geometry ChangeCoodinateFrom84(double x, double y) //坐标转换
        {
            ESRI.ArcGIS.Client.Geometry.Geometry geo = new ESRI.ArcGIS.Client.Geometry.MapPoint(x, y);
            ESRI.ArcGIS.Client.Geometry.Geometry wgs84Geometry = _mercator.FromGeographic(geo);

            return wgs84Geometry;
        }

        public ESRI.ArcGIS.Client.Geometry.Geometry ChangeCoodinateTo84(double x, double y) //坐标转换
        {
            ESRI.ArcGIS.Client.Geometry.Geometry geo = new ESRI.ArcGIS.Client.Geometry.MapPoint(x, y);
            ESRI.ArcGIS.Client.Geometry.Geometry wgs84Geometry = _mercator.ToGeographic(geo);

            return wgs84Geometry;
        }
        //GeometryService _geometryTask;   //几何任务
        //public void ChangeCoodinate(double x, double y,int wkid)
        //{
        //    LocalGeometryService.GetServiceAsync(lgs =>
        //    {
        //        _geometryTask = new GeometryService();
        //        _geometryTask.Url = lgs.UrlGeometryService;
        //        _geometryTask.ProjectCompleted += geometryService_ProjectCompleted;
        //        _geometryTask.Failed += geometryService_Failed;

        //    });
        //    MapPoint inputMapPoint = new MapPoint(x, y, new SpatialReference(4326));
        //    _geometryTask.ProjectAsync(new List<Graphic>() { new Graphic() { Geometry = inputMapPoint } }, MyMap.SpatialReference, inputMapPoint);
        //}//坐标转换
        //void geometryService_ProjectCompleted(object sender, GraphicsEventArgs e)
        //{
        //    Graphic resultGraphic = e.Results[0];

        //    if (resultGraphic.Geometry.Extent != null)
        //    {
        //        resultGraphic.Symbol = LayoutRoot.Resources["DefaultMarkerSymbol"] as ESRI.ArcGIS.Client.Symbols.Symbol;

        //        MapPoint resultMapPoint = resultGraphic.Geometry as MapPoint;
        //        resultGraphic.Attributes.Add("Output_CoordinateX", resultMapPoint.X);
        //        resultGraphic.Attributes.Add("Output_CoordinateY", resultMapPoint.Y);

        //        MapPoint inputMapPoint = e.UserState as MapPoint;
        //        resultGraphic.Attributes.Add("Input_CoordinateX", inputMapPoint.X);
        //        resultGraphic.Attributes.Add("Input_CoordinateY", inputMapPoint.Y);

        //        MyGraphicsLayer.Graphics.Add(resultGraphic);

        //        MyMap.PanTo(resultGraphic.Geometry);
        //    }
        //    else
        //    {
        //        MessageBox.Show("Invalid input coordinate, unable to project.");
        //    }
        //}
        //void geometryService_Failed(object sender, TaskFailedEventArgs e)
        //{
        //    MessageBox.Show("Geometry Service error: " + e.Error);
        //}

        public class INIClass
        {
            public string inipath;
            [DllImport("kernel32")]
            private static extern long WritePrivateProfileString(
           string section, string key, string val, string filePath);
            [DllImport("kernel32")]
            private static extern int GetPrivateProfileString(
           string section, string key,
           string def, StringBuilder retVal,
           int size, string filePath);
            /// ﹤summary﹥  
            /// 构造方法  
            /// ﹤/summary﹥  
            /// ﹤param name="INIPath"﹥文件路径﹤/param﹥  
            public INIClass(string INIPath)
            {
                inipath = INIPath;
            }
            /// ﹤summary﹥  
            /// 写入INI文件  
            /// ﹤/summary﹥  
            /// ﹤param name="Section"﹥项目名称(如 [TypeName] )﹤/param﹥  
            /// ﹤param name="Key"﹥键﹤/param﹥  
            /// ﹤param name="Value"﹥值﹤/param﹥  
            public void IniWriteValue(string Section, string Key, string Value)
            {
                WritePrivateProfileString(Section, Key, Value, this.inipath);
            }
            /// ﹤summary﹥  
            /// 读出INI文件  
            /// ﹤/summary﹥  
            /// ﹤param name="Section"﹥项目名称(如 [TypeName] )﹤/param﹥  
            /// ﹤param name="Key"﹥键﹤/param﹥  
            public string IniReadValue(string Section, string Key)
            {
                StringBuilder temp = new StringBuilder(500);
                int i = GetPrivateProfileString(Section, Key, "", temp, 500, this.inipath);
                return temp.ToString();
            }
            /// ﹤summary﹥  
            /// 验证文件是否存在  
            /// ﹤/summary﹥  
            /// ﹤returns﹥布尔值﹤/returns﹥  

        }
        #endregion



        private void chkShowAttr_Click(object sender, RoutedEventArgs e)
        {
            if (chkEdit.IsChecked == true)
            {
                MyFeatureDataForm.IsReadOnly = false;
            }
            else
            {
                MyFeatureDataForm.IsReadOnly = true ;
            }
            if (chkShowAttr.IsChecked == true) 
            {
                MyFeatureDataForm.Visibility = Visibility.Visible;

                if (Tml.SelectionCount == 1)
                {
                    int x=Convert.ToInt32( Tml.SelectedGraphics.ToList()[0].Attributes["Lid"]);
                    ShowPic(x); 
                }
                //showPic.Visibility = Visibility.Visible;

            } 
            else 
            {
                MyFeatureDataForm.Visibility = Visibility.Hidden;
                showPic.Visibility = Visibility.Hidden ;
            }
        }



        //private void btnStart_Click(object sender, RoutedEventArgs e)
        //{

        //    System.Timers.Timer t = new System.Timers.Timer(200000);
        //    //实例化Timer类，设置间隔时间为10000毫秒；   
        //    t.Elapsed += new System.Timers.ElapsedEventHandler(theout);
        //    //到达时间的时候执行事件；   
        //    t.AutoReset = true;
        //    //设置是执行一次（false）还是一直执行(true)；   
        //    t.Enabled = true;
        //    //是否执行System.Timers.Timer.Elapsed事件；   
        //}
        //    public void theout(object source,System.Timers.ElapsedEventArgs e)   
        //    {
        //        var rtus = Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary.Keys;
        //        //////////////test  lvffffffffffffffffffffffffffffffffffffffffff

        //        if (i < 4)
        //        {
        //            i++;
        //        }
        //        else { i = 0; }
        //        string t = i.ToString();
        //        foreach (int id in rtus)
        //        {
        //            ModifyFeaturesAttribute(id, "Status", t);
        //        }
        //    }






    }


    public partial class MapGis 
    {


        public void InitEvent()
        {
            EventPublish.AddEventTokener( 
                Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandlers, FundOrderFilters);

            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(OnLoadedRtu, 3, Wlst.Cr.Core.ModuleServices.DelayEventHappen.EventOne);

        }


        public void OnLoadedRtu()
        {
            var rtus = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.Keys;//).EquipmentInfoDictionary.Keys;
            //GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;

            foreach (int id in rtus)
            {
                OnErrChanged(id);

            }
            ModifyFeaturesAttribute3("Status");

            if (isUselocate == 0) return ;
                foreach (
                   var f in Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems.Values)//.EquipmentInfoDictionary.Values)
                {
                    try
                    {

                        //OnErrChanged(f.RtuId);
                        if (f.RtuFid > 0) continue;
                        
                        if (f.RtuMapX.Equals(0) || f.RtuMapY.Equals(0))
                        {
                            //if (isUselocate == 0) continue ;
                            foreach (Graphic g in Tml.Graphics)          //修改终端图层思密达
                                if (Convert.ToInt32(g.Attributes["Lid"]) == f.RtuId)
                                {
                                    var t = g.Geometry as MapPoint;
                                    Graphic gt = new Graphic();
                                    gt.Geometry = ChangeCoodinateTo84(t.X, t.Y);
                                    MapPoint mp = new MapPoint();
                                    mp = gt.Geometry as MapPoint;
                                    double xt = mp.X;
                                    double yt = mp.Y;
                                    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .OnEquipmentMentMapLocationChangeByMap(
                                    TreeSelectId, xt, yt);
                                }
                        }
                        else
                        {
                            if (isUselocate == 0) continue ;
                             foreach (Graphic g in Tml.Graphics)          //修改终端图层思密达
                                 if (Convert.ToInt32(g.Attributes["Lid"]) == f.RtuId)
                                 {
                                    Graphic gt = new Graphic();
                                    gt.Geometry = ChangeCoodinateFrom84(f.RtuMapX, f.RtuMapY);
                                    if (g.Geometry != gt.Geometry)
                                    {
                                        g.Geometry = gt.Geometry;
                                        g.Geometry.SpatialReference = MyMap.SpatialReference;
                                    }   
                                 }
                        }
                    }
                    catch { }
                }
        }   

        #region IEventAggregator Subscription

        public void FundEventHandlers(PublishEventArgs args)
        {
            try
            {
                //if (args.EventType == PublishEventType.SvAv)
                //{
                //    if (MainEquipmentList.Count == 0) this.NavOnLoad();
                //    return;
                //}
                if (args.EventType == PublishEventType.Core)
                {
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipentxyPositonUpdateId)
                    {

                    }
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentAddEventId)
                    {

                        var lst = args.GetParams()[0] as List<Tuple<int, int>>;
                        if (lst == null) return;
                        foreach (var t in lst)
                        {
                            if (t.Item2 == 0)
                            {
                                if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(t.Item1))
                                    continue;
                                var info = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[t.Item1];
                                if(info == null ) continue;
                                if ( FeatureDic.ContainsKey(t.Item1))
                                {
                                    FeatureDic[t.Item1] = info.Idf;
                                }
                                else
                                {
                                    FeatureDic.Add(t.Item1, info.Idf);
                                }

                                //AddMainEquipment(t.Item1); 
                            }
                        }
                        return;
                    }
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentDeleteEventId)
                    {
                        ////////var lst = args.GetParams()[0] as List<Tuple<int, int>>;
                        ////////if (lst == null) return;
                        ////////foreach (var t in lst)
                        ////////{
                        ////////    if (t.Item2 == 0) { }
                        ////////    // DeleteMainEquipment(t.Item1);
                        ////////}
                        ////////return;
                    }

                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentUpdateEventId)
                    {


                        var lst = args.GetParams()[0] as List<Tuple<int, int>>;
                        if (lst == null || lst.Count == 0) return;
                        foreach (var t in lst)
                        {
                            if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(t.Item1))
                                continue;
                            var info =
                                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[t.Item1];
                            if (info == null) continue;
                            if (FeatureDic.ContainsKey(t.Item1))
                            {
                                FeatureDic[t.Item1] = info.Idf;
                            }
                            else
                            {
                                FeatureDic.Add(t.Item1, info.Idf);
                            }

                            if (isUselocate != 0)
                            {
                                foreach (Graphic g in Tml.Graphics) //修改终端图层思密达
                                    if (Convert.ToInt32(g.Attributes["Lid"]) == t.Item1)
                                    {
                                        Graphic gt = new Graphic();
                                        gt.Geometry = ChangeCoodinateFrom84(info.RtuMapX, info.RtuMapY);
                                        if (g.Geometry != gt.Geometry)
                                        {
                                            g.Geometry = gt.Geometry;
                                            g.Geometry.SpatialReference = MyMap.SpatialReference;
                                        }
                                    }
                            }
                        }

                        return;
                    }
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipentxyPositonUpdateId)
                    {
                        if (isUselocate == 0) return ;
                        int rtuid = Convert.ToInt32(args.GetParams()[0]);

                        if (!EquipmentDataInfoHold.InfoItems.ContainsKey( rtuid))//.EquipmentInfoDictionary.ContainsKey(rtuid))
                            return;
                        if (EquipmentDataInfoHold.InfoItems [rtuid].RtuFid > 0) return;

                        var f =
                            Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuid ];//.EquipmentInfoDictionary[rtuid];
                        if (isUselocate == 0) return ;
                        foreach (Graphic g in Tml.Graphics)          //修改终端图层思密达
                            if (Convert.ToInt32(g.Attributes["Lid"]) == rtuid)
                            {
                                g.Geometry = ChangeCoodinateFrom84(f.RtuMapX, f.RtuMapY);
                                g.Geometry.SpatialReference = MyMap.SpatialReference;
                                break;
                            }
                        //   if (ServicesEquipemntInfoHold.EquipmentInfoDictionary[rtuid].AttachRtuId > 0) return;

                        //var f =
                        //    Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary[rtuid];
                        //foreach (var t in MainEquipmentList)
                        //{
                        //    if (t.EquipmentRtuId == rtuid)
                        //    {
                        //        t.EquipmentName = f.RtuName;
                        //        t.EquipmentLocation = new Location(f.Xmap, f.Ymap);
                        //    }
                        //}

                    }
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                    {

                        if (Convert.ToString(args.EventAttachInfo) == "gis") return;
                        int x = Convert.ToInt32(args.GetParams()[0]); //终端、集中器逻辑地址
                        TreeSelectId = x;
                        if (args.GetParams().Count > 1)
                        {
                            int y = Convert.ToInt32(args.GetParams()[1]); //控制器序号
                            x = x*1000 + y;
                            TreeSelectId = x;
                        }
                        if (FeatureDic.ContainsKey(x))
                        {
                            var tmp = FeatureDic[x];
                            if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(x))
                                return;

                            var info = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[x];
                            if (info == null) return;
                            var groupidx =
                                Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuBelongGrp(x);
                            //.Item2;
                            //Wlst.Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuBelongGrp(RtuId);
                            if (groupidx != null)
                            {
                                var infosss =
                                    Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGroupInfomation(
                                        groupidx.Item1, groupidx.Item2);
                                if (infosss != null)
                                {
                                    Model.GroupName = infosss.GroupName;
                                }
                                else
                                {
                                    Model.GroupName = "无分组信息";
                                }

                            }
                            Model.RtuIdf = info.Idf;
                            Model.RtuName = info.RtuName;
                            Model.RtuPhyId = info.RtuPhyId + "";
                            Model.Remark = info.RtuRemark;
                            Model.InstallAddr = info.RtuInstallAddr;
                            Model.DataCreate = new DateTime(info.DateCreate).ToString("yyyy-MM-dd");


                            LocateSth(tmp);
                        }

                        ShowPic(x);
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
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.RunningInfoUpdate1)//todo Sr.EquipemntLightFault.Services.EventIdAssign.RtuErrorStateChanged)
                        try
                        {
                            var lst = args.GetParams()[0] as IEnumerable<int>;
                            if (lst == null) return;
                            //    this.ReUpdateLoadChild();
                            foreach (var t in lst)
                            {
                                OnErrChanged(t);
                            }
                            ModifyFeaturesAttribute3( "Status");
                        }
                        catch (Exception ex)
                        {
                            Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
                        }

                    //if (args.EventId ==Sr.EquipmentInfoHolding.Services.EventIdAssign.RunningInfoUpdate2)// Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuLightHasElectricStatesChanged)
                    //    try
                    //    {
                    //        if (args.GetParams().Count > 1)
                    //        {
                    //            try
                    //            {
                    //                int x1 = Convert.ToInt32(args.GetParams()[0]);
                    //                OnErrChanged(x1);

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
                            var lst = args.GetParams()[0] as List<int>;
                            if (lst == null) return;
                            if (lst.Count == 0)
                            {

                                FilterFeatures();
                                //Tml.Where = "";
                                //Tml.Update();
                                needOnErrChanged = true;
                            
                                //GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;

       
                            }
                            else
                            {
                                //return;
                                lst.Add(0);
                                string idt = "";
                                foreach (var rtuid in lst)
                                {
                                    if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(rtuid))
                                        continue;
                                    var pb = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(rtuid);
                                    if (pb.Idf ==null ) continue;
                                    string tmp = "\'"+ pb.Idf+"\'";
                                    idt = idt + tmp +",";

                                    //////////if (rtuid > 1000001 && rtuid < 1100000)
                                    //////////{
                                    //////////    foreach (Graphic g in Tml.Graphics)
                                    //////////    {
                                    //////////        if (Convert.ToInt32(g.Attributes["Lid"]) == Convert.ToInt32(rtuid))
                                    //////////        {
                                    //////////            Graphic gr = new Graphic
                                    //////////            {
                                    //////////                Geometry = g.Geometry,
                                    //////////                Symbol = LayoutRoot.Resources["DefaultMarkerSymbol"] as ESRI.ArcGIS.Client.Symbols.Symbol, // newSymbol is a SimpleMarkerSymbol (point)
                                    //////////            };
                                    //////////            graphicsLayer.Graphics.Add(gr);
                                    //////////        }
                                    //////////    }
                                    //////////}
                                    //////////////if (rtuid < 1600000 && rtuid > 1500000)
                                    //////////////{
                                    //////////////    foreach (Graphic g in Concentrator .Graphics)
                                    //////////////    {
                                    //////////////        if (Convert.ToInt32(g.Attributes["Lid"]) == Convert.ToInt32(rtuid))
                                    //////////////        {
                                    //////////////            Graphic gr = new Graphic
                                    //////////////            {
                                    //////////////                Geometry = g.Geometry,
                                    //////////////                Symbol = LayoutRoot.Resources["DefaultMarkerSymbol"] as ESRI.ArcGIS.Client.Symbols.Symbol, // newSymbol is a SimpleMarkerSymbol (point)
                                    //////////////            };
                                    //////////////            graphicsLayer.Graphics.Add(gr);
                                    //////////////        }
                                    //////////////    }
                                    //////////////}
                                }

                                idt = idt.Substring(0, idt.Length - 1);
                                string temp = "CODE in (" + idt + ")";
                                //string temp = "CODE in (\'100J\',\'101J\')";
                                Tml.Where = temp;
                                Tml.Update();
                                needOnErrChanged = true;
                            }

                            
                        }

                        catch (Exception ex)
                        {
                        }

                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.MapNeedChangeIcon)
                    {
                        //0-正常，1-光源故障，2-补偿电容故障，3-意外灭灯，4-意外亮灯，5-自熄灯，6-控制器断电告警（苏州）,7-继电器故障
                        int sluId = 0;
                        int os = 0;
                        List<int> ctrls = new List<int>();
                        if (args.GetParams().Count < 2) return;
                        bool haveFeature = false;
                        sluId = (int) args.GetParams()[0];
                        ctrls = args.GetParams()[1] as List<int>;
                        if (ctrls ==null || ctrls.Count < 1) return;
                        if (sluId == 0) return;
                        var sludata = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(sluId);
                        var sludataIcon = sludata.SluCtrlIconStates;
                        Dictionary<int, Tuple<string, string>> namevalue = new Dictionary<int, Tuple<string, string>>();

                        foreach (var ctrl in ctrls)
                        {
                            foreach (Graphic g in Controller.Graphics) //遍历终端图层获取灯杆类型
                            {
                                haveFeature = false;
                                if (Convert.ToInt32(g.Attributes["Bid"]) == sluId &&
                                    Convert.ToInt32(g.Attributes["Lid"]) == ctrl)
                                {
                                    if (!g.Attributes.ContainsKey("TYPE")) return;
                                    var tmpType = g.Attributes["TYPE"];
                                    if (tmpType == null || Convert.ToInt32(tmpType) < 100)
                                    {
                                        os = 100; //100+单挑灯   200+双挑灯
                                    }
                                    else
                                    {
                                        os = Convert.ToInt32(g.Attributes["TYPE"].ToString());
                                        os = os/100*100;
                                    }
                                    haveFeature = true;
                                    break;
                                }
                            }
                            if (haveFeature == false) continue;
                            //////if (IsIconFollowTheRtu)
                            //////{
                                if (sludataIcon.ContainsKey(ctrl))
                                {
                                    string errorindex = "0"; //todo
                                    var ctrldataIcon = sludataIcon[ctrl];

                                    if (ctrldataIcon.IsIconUseRtuStateTo) //跟终端走
                                    {
                                        if (ctrldataIcon.RtuState == 1) //1.全开  2。全关
                                        {
                                            errorindex = (os + 10).ToString(); //10 开灯 
                                        }
                                        if (ctrldataIcon.RtuState == 2)
                                        {
                                            errorindex = (os + 11).ToString(); //11 关灯
                                        }
                                        if (ctrldataIcon.RtuState == 3)
                                        {
                                            errorindex = os.ToString(); //不变
                                        }
                                        if (!namevalue.ContainsKey(ctrl))
                                        {
                                            namevalue.Add(ctrl, new Tuple<string, string>("TYPE", errorindex));
                                        }
                                        else
                                        {
                                            namevalue[ctrl] = new Tuple<string, string>("TYPE", errorindex);
                                        }
                                        //ModifyFeaturesAttribute(lid, "TYPE", errorindex);
                                        continue;
                                    }


                                    if (ctrldataIcon.UnConnected == true)
                                    {
                                        errorindex = os.ToString(); //100 200通讯故障
                                    }
                                    else if (ctrldataIcon.Errors.Count > 0 && ctrldataIcon.Errors[0] != 0)
                                    {
                                        var error = ctrldataIcon.Errors[0];
                                        if (CtrlIcon.ContainsKey(error))
                                        {
                                            if (CtrlIcon[error] == 99)
                                            {
                                                if (ctrldataIcon.states != 3)
                                                    errorindex = (os + 12).ToString(); //12 开灯通用故障
                                                if (ctrldataIcon.states == 3)
                                                    errorindex = (os + 13).ToString(); //13 关灯通用故障

                                            }
                                            else
                                            {
                                                errorindex = (os + CtrlIcon[error]).ToString();
                                                    //-54  55-功率越上限 56功率越下限  57灯具漏电 58光源故障  59补偿电容故障 60意外灭灯  61意外亮灯  62自熄灯 63 控制器断电告警  64 继电器故障
                                                //error1-光源故障 error2-补偿电容故障 error3-意外灭灯 error4-意外亮灯 error5-自熄灯 error6-控制器断电告警 error7-继电器故障
                                            }
                                        }
                                    }
                                    else //正常亮灯 关灯
                                    {
                                        if (ctrldataIcon.states != 3) errorindex = (os + 10).ToString(); //10 正常开灯
                                        if (ctrldataIcon.states == 3) errorindex = (os + 11).ToString(); //11 正常关灯
                                    }
                                    if (!namevalue.ContainsKey(ctrl))
                                    {
                                        namevalue.Add(ctrl, new Tuple<string, string>("TYPE", errorindex));
                                    }
                                    else
                                    {
                                        namevalue[ctrl] = new Tuple<string, string>("TYPE", errorindex);
                                    }
                                    //ModifyFeaturesAttribute(lid, "TYPE", errorindex);
                                }
                            ////}else
                            ////{
                            ////    if (sludata.SluCtrlNewData.ContainsKey(ctrl) == false) continue;
                            ////    var ctrldata = sludata.SluCtrlNewData[ctrl];
                            ////    if (ctrldata == null || ctrldata.Data5 == null) return;
                            ////    var data5 = ctrldata.Data5;

                            ////    var errorUnContected = data5.Info.Status == 3 || data5.Info.DateTimeCtrl == 0;
                            ////    var error = data5.Info.Status != 0;

                            ////    bool isOnLight = false;
                            ////    bool errorLamp = false;
                            ////    bool isSaveEnergy = false;

                            ////    foreach (var fx in data5.Items)
                            ////    {
                            ////        if (!isOnLight && fx.StateWorkingOn != 3) isOnLight = true;
                            ////        if (!isSaveEnergy && (fx.StateWorkingOn == 1 || fx.StateWorkingOn == 2))
                            ////            isSaveEnergy = true;
                            ////        if (!errorLamp && fx.Fault > 0) errorLamp = true;

                            ////    }
                            ////    string errorindex = "0";
                            ////    if (UxTreeSetting.IsRutsNotShowError == false)
                            ////    {
                            ////        if (errorUnContected) errorindex = os.ToString();//"0";                //不在线
                            ////        if (isOnLight && !errorLamp) errorindex = (os + 10).ToString();// "1";          //正常开灯
                            ////        if (isOnLight && errorLamp) errorindex = (os + 12).ToString();//"2";           //开灯有故障
                            ////        //if (isSaveEnergy && !errorLamp) errorindex = (os + 10).ToString();// "3";       //正常节能
                            ////        //if (isSaveEnergy && errorLamp) errorindex = (os + 10).ToString();// "4";        //节能有故障
                            ////        if (!isOnLight && !errorLamp) errorindex = (os + 11).ToString();// "5";         //正常关灯
                            ////        if (!isOnLight && errorLamp) errorindex = (os + 13).ToString();//"6";          //关灯有故障
                            ////    }
                            ////    if (!namevalue.ContainsKey(ctrl))
                            ////    {
                            ////        namevalue.Add(ctrl, new Tuple<string, string>("TYPE", errorindex));
                            ////    }
                            ////    else
                            ////    {
                            ////        namevalue[ctrl] = new Tuple<string, string>("TYPE", errorindex);
                            ////    }
                            ////}
                        }
                        ModifyFeaturesAttribute2(sluId,namevalue);
                    }

                    //单灯最新数据
                    if (args.EventId ==Sr.EquipmentInfoHolding.Services.EventIdAssign.RunningInfoUpdate2)//Wj2090Module.Services.EventIdAssign. && args.EventType == PublishEventType.Core) //单灯最新数据
                    {
                        
                        //if(args .GetParams() .Count <2) return;
                        //var rtus = args.GetParams()[0] as List<int>;
                        //if (rtus ==null || rtus.Count == 0) return;
                        //int sluId = rtus[0];
                        //if (sluId < Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.SluStart) return;
                        //if (sluId > Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.SluEnd ) return;

                        //List<int> ctrls = args.GetParams()[1] as List<int>;
                        //if (ctrls == null||ctrls .Count ==0) return;
  
                        //var sludata = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(sluId);

                        //    if (sludata == null) return;
                        //foreach (var f in ctrls)
                        //{
                        //    int os = new int();
                        //    foreach (Graphic g in Controller.Graphics) //遍历终端图层获取灯杆类型
                        //        if (Convert.ToInt32(g.Attributes["Bid"]) == sluId &&
                        //            Convert.ToInt32(g.Attributes["Lid"]) == f)
                        //        {
                        //            if (string.IsNullOrEmpty(g.Attributes["TYPE"].ToString()))
                        //            {
                        //                os = 0;
                        //            }
                        //            else
                        //            {
                        //                os = Convert.ToInt32(g.Attributes["TYPE"].ToString());
                        //            }

                        //            break;
                        //        }

                        //    if (sludata.SluCtrlNewData.ContainsKey(f) == false) continue;
                        //    var ctrldata = sludata.SluCtrlNewData[f];
                        //    if (ctrldata == null||ctrldata .Data5 ==null) return;
                        //    var data5 = ctrldata.Data5;
                            
                        //    var errorUnContected = data5.Info.Status == 3 || data5.Info.DateTimeCtrl == 0;  
                        //    var error = data5.Info.Status != 0;

                        //    bool isOnLight = false;
                        //    bool errorLamp = false;
                        //    bool isSaveEnergy = false;

                        //    foreach (var fx in data5 .Items )
                        //    {
                        //        if(!isOnLight && fx.StateWorkingOn != 3 ) isOnLight = true;
                        //        if(!isSaveEnergy && (fx.StateWorkingOn == 1 || fx.StateWorkingOn == 2))
                        //            isSaveEnergy = true;
                        //        if (!errorLamp && fx.Fault > 0) errorLamp = true;

                        //    }
                        //    int lid = sluId * 1000 + f;
                        //    string errorindex = "0";
                        //    if (UxTreeSetting.IsRutsNotShowError == false)
                        //    {
                        //        if (errorUnContected ) errorindex = "0";                //不在线
                        //        if (isOnLight && !errorLamp) errorindex = "1";          //正常开灯
                        //        if (isOnLight && errorLamp) errorindex = "2";           //开灯有故障
                        //        if (isSaveEnergy && !errorLamp) errorindex = "3";       //正常节能
                        //        if (isSaveEnergy && errorLamp) errorindex = "4";        //节能有故障
                        //        if (!isOnLight && !errorLamp) errorindex = "5";         //正常关灯
                        //        if (!isOnLight && errorLamp) errorindex = "6";          //关灯有故障
                        //    }
                            
                        //    ModifyFeaturesAttribute1(lid, "TYPE", errorindex);
                        //}
                    }
                    //if (args.EventId == Wlst.Sr .PrivilegesCrl .Services .EventIdAssign .ModflyUserInfomationId||
                    //    args.EventId == Wlst.Sr .PrivilegesCrl .Services .EventIdAssign. ModifyGourpInfoPrivilege||
                    //    args.EventId == Wlst.Sr .PrivilegesCrl .Services .EventIdAssign .ModifyPriGroupInfomation||
                    //    args.EventId == Wlst.Sr .PrivilegesCrl .Services .EventIdAssign.RequestOrUpdateUserPrivilegInfoId)
                    //{
                    //    Model.RaiseEvent();
                    //    if (model.IsEditMapGisEnable == false)
                    //    {
                    //        hideB.Visibility =Visibility.Hidden;
                    //        Storyboard storysh = (Storyboard)LayoutRoot.FindResource("OnClickingSh");
                    //        if (sp.Height > 70)
                    //        {
                    //            storysh.Begin();
                    //            chkEdit.IsChecked = false;
                    //            chkErr.IsChecked = false;
                    //            DelSelectionButton.IsEnabled = false;
                    //            AddMultiController.IsEnabled = false;
                    //            AddConcentrator.IsEnabled = false;
                    //            AddLine.IsEnabled = false;
                    //            AddTml.IsEnabled = false;
                    //            ShowNullButton.IsEnabled = false;
                    //            //DelNullButton.IsEnabled = false;
                    //            BindButton.IsEnabled = false;
                    //            UpdateMapButton.IsEnabled = false;
                    //            //MyFeatureDataForm.Visibility = Visibility.Hidden;
                    //            MyFeatureDataForm.IsReadOnly = true ;
                    //            FindDetailsDataGrid.Visibility = Visibility.Hidden;

                    //        }
                    //    }
                    //    else
                    //    {
                    //        hideB.Visibility = Visibility.Visible;
                    //    }
                    //}
                }
                // int indentfyId = Convert.ToInt32(args.GetParams()[0]);
            }
            catch (Exception xe)
            {
                WriteLog.WriteLogError("MapGis error in FundEventHandlers:ex:" + xe);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool FundOrderFilters(PublishEventArgs args) //接收终端选中变更事件
        {
            try
            {
                // if (args.EventType == PublishEventType.SvAv) return true;
                if (args.EventType == PublishEventType.Core)
                {
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentAddEventId)
                        return true;
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentDeleteEventId)
                        return true;

                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentUpdateEventId)
                        return true;
                  
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                        return true;
                    
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.RunningInfoUpdate1)
                        return true;
                    //if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.RunningInfoUpdate2)   改用 MapNeedChangeIcon
                    //    return true;
                    if (args.EventId == Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuGroupSelectdWantedMapUp)
                        return true;
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.MapNeedChangeIcon)
                        return true;
                    
                }
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
            return false;
        }


        /// <summary>
        /// 苏州园区特殊版  0不用 1停运 2离线 3开灯 4开灯故障 5关灯 6关灯故障   10+景观灯   20+楼宇
        /// </summary>
        /// <param name="EquipmentRtuId"></param>
        public void OnErrChanged(int EquipmentRtuId)
        {
            if (
                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey( //).EquipmentInfoDictionary.ContainsKey(
                    EquipmentRtuId))
            {
                var equ =
                    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems[//.EquipmentInfoDictionary[
                        EquipmentRtuId];
                if (equ.EquipmentType == WjParaBase.EquType.Rtu)// 3005 || equ.RtuModel == 3090)
                {
                    string errorindex = "5";    
                    var s = equ.RtuStateCode ;
                    if (s == 0)
                    {
                        //     EquipmentImageState = 3011; //s
                        //todi  change EquipmentRtuId  picture
                        //ModifyFeaturesAttribute1(EquipmentRtuId, "Status", "0"); //不用
                        errorindex = "0";
                        //Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogInfo("SystemReceive变图标 设备逻辑地址：" + EquipmentRtuId + "改变状态为" + 0);
                        //return;
                    }
                    if (s == 1)
                    {
                        //      EquipmentImageState = 3012;
                        //ModifyFeaturesAttribute1(EquipmentRtuId, "Status", "1");//停用
                        //Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogInfo("SystemReceive变图标 设备逻辑地址：" + EquipmentRtuId + "改变状态为" + 0);
                        //return;
                        errorindex = "1";
                    }
                    var rtuRunInfo = Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(EquipmentRtuId);
                    if (rtuRunInfo != null)
                    {


                        var haserror =
                            rtuRunInfo.ErrorCount > 0;
                        // Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultLstInfoByRtuId(EquipmentRtuId);//.IsRtuHasError(EquipmentRtuId);
                        var lighton =
                            Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(EquipmentRtuId).
                                IsLightHasElectric; //RtuNewDataService.IsRtuHasElectric(EquipmentRtuId);
                        string nname = equ.RtuName;

                        if (haserror && !lighton) errorindex = "6"; //宜兴 绿色 路灯 关灯故障 2
                        if (!haserror && lighton) errorindex = "3"; //宜兴 绿色 路灯 正常亮灯状态 3
                        if (haserror && lighton) errorindex = "4"; //宜兴 绿色 路灯 亮灯故障 4
                    }

                    //if (errorindex =="1")
                    //{
                    //    if (nname.Contains("亮化"))
                    //    {
                    //            errorindex = "5";//宜兴 灰白 亮化 正常关灯状态
                    //    }
                    //}
                    //if (errorindex == "2")
                    //{
                    //    if (nname.Contains("亮化"))
                    //    {
                    //        errorindex = "6";//宜兴 灰白 亮化 关灯故障
                    //    }
                    //}
                    //if (errorindex == "3")
                    //{
                    //    if (nname.Contains("亮化"))
                    //    {
                    //        errorindex = "7";//宜兴 灰白 亮化 正常亮灯状态
                    //    }
                    //}
                    //if (errorindex == "4")
                    //{
                    //    if (nname.Contains("亮化"))
                    //    {
                    //        errorindex = "8";//宜兴 灰白 亮化 亮灯故障
                    //    }
                    //}
                    //Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogInfo("SystemReceive变图标 设备逻辑地址：" + EquipmentRtuId + "改变状态为" + errorindex);

                    if (tmlIcon.ContainsKey(EquipmentRtuId) == false)
                    {
                        tmlIcon.TryAdd(EquipmentRtuId, new Tuple<bool, string>(true ,errorindex));
                    }
                    else
                    {

                        tmlIcon[EquipmentRtuId] = new Tuple<bool, string>(true ,errorindex); 
 
                        
                    }
                    //ModifyFeaturesAttribute1(EquipmentRtuId, "Status", errorindex);
                    //var tmps=  Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetRtuErrorsByRtuId(EquipmentRtuId);


                    //  EquipmentImageState = 3015 + errorindex;


                }

            }
        }

        /// <summary>
        /// 图标变色
        /// rtuid
        /// errorindex
        /// 
        /// </summary>
        private ConcurrentDictionary<int, Tuple<bool, string>> tmlIcon = new ConcurrentDictionary<int, Tuple<bool, string>>(); 

        #endregion


        #region  TmlAttri1


        //public  string RtuIdf;
        //public string RtuName;
        //public string RtuPhyId;
        //public string InstallAddr;
        //public string DataCreate;

        #endregion


    }
}
