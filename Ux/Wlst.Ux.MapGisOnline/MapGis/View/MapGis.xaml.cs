using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Drawing.Imaging;
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
using ESRI.ArcGIS.Client.FeatureService.Symbols;
using ESRI.ArcGIS.Client.Geometry;
using ESRI.ArcGIS.Client.Local;
using ESRI.ArcGIS.Client.Symbols;
using ESRI.ArcGIS.Client.Tasks;
using ESRI.ArcGIS.Client.Toolkit.DataSources;
using Wlst.Cr.Coreb.EventHelper;

using Wlst.Cr.Core.Behavior;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Sr.Menu.Services;
using Wlst.Ux.MapGisOnline.MapGis.Services;
using Wlst.Ux.MapGisOnline.Resources;
using Wlst.Ux.MapGisOnline.MapGis.ViewModel;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Client.Runtime;
using System.IO;
using System.Net;
using System.Timers;
using System.Xml;
using System.Threading;
using EventIdAssign = Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign;
using WriteLog = Wlst.Cr.Core.UtilityFunction.WriteLog;


namespace Wlst.Ux.MapGisOnline.MapGis.View
{
    /// <summary>
    /// MapGis.xaml 的交互逻辑
    /// </summary>
    [ViewExport(Ux.MapGis.Services.ViewIdAssign.MapGisViewId,AttachNow = true, 
       AttachRegion = Ux.MapGis.Services.ViewIdAssign.MapGisViewAttachRegion)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class MapGis : UserControl
    {
        public static ConcurrentDictionary<int, int> CtrlIcon = new ConcurrentDictionary<int, int>();
        static bool needOnErrChanged=false ;
        private  string SearchService = "";
        private const string XmlConfigName = "MapGis";
        private string OnlyField = "wldz";  //终端默认对应字段
        private string CtrlField = "txm";  //单灯默认对应字段
        private string StateField = "status";
        private bool _isChangeXYbyMap = false;
        private double OffsetX = 0;   //图标偏移量
        private double OffsetY = 0;   //图标偏移量
        private string ServiceIP = ""; //在线服务url+port

        MapGisViewModel model;
        private FeatureLayer Tml= new FeatureLayer() ;
        private FeatureLayer Controller = new FeatureLayer();
        private FeatureLayer Concentrator = new FeatureLayer();
        private FeatureLayer Line = new FeatureLayer();

        private GraphicsLayer MyGraphicsLayer = new GraphicsLayer();
        private GraphicsLayer MyGraphicsLayerJuHe = new GraphicsLayer();

        

        private ESRI.ArcGIS.Client.Editor myEditorC;
        public MapGis()
        {
            ArcGISRuntime.InstallPath = @".";
            ArcGISRuntime.SetLicense("runtimestandard,101,rud105202449,none,2KYRMD1AJ48H2T8AG200,25472A32C2C6D077D75848B17A231B715EDFA8A3869919484DE72955AF62B012466BF973A2BF7B4BB6BC4E619BE2F183A42C532CD0A21784E122580892396B4E439755764A218EDFF6F2442299AB05500E823886D5D0AED32AD956D83FFA50AFE1A05C7D441F89E35FC1A9BCD48ACD68316E2D606D996376099B917A274E5719,FID_27b78480_14b65041ba9__6edd");
            ArcGISRuntime.Initialize();
            InitializeComponent();
            bool haveTml = false;
            bool haveCtrl = false;
            bool haveSlu = false;
            bool haveLine = false;
            double xmin = 0;
            double ymin = 0;
            double xmax = 0;
            double ymax = 0;
            WriteLog.WriteLogInfo("初始化地图模块");
            try
            {
                var info = Readxml(XmlConfigName);


                var fp = Directory.GetCurrentDirectory()+ "\\SystemXmlConfig\\MapGis.txt";
                var lst =Wlst.Cr.Coreb.Servers.WriteFile.ReadFiles(fp);
                foreach(var g in lst)
                {
                    var tmp = g.Split('=');
                    if (tmp.Count() > 1 && string.IsNullOrEmpty(tmp[0]) == false && string.IsNullOrEmpty(tmp[1]) == false)
                    {
                        if (info.ContainsKey(tmp[0]) == false)
                        {
                            info.Add(tmp[0], tmp[1]);
                        }
                        else if (string.IsNullOrEmpty(info[tmp[0]]))
                        {
                            info[tmp[0]] = tmp[1];
                        }
                    }




                }
                foreach (var f in info)
                {
                    if (f.Key.Contains("MapServiceLayer"))
                    {
                        WriteLog.WriteLogInfo("加载底图");
                        var TitleMapUrl = info["MapServiceLayer"];
                        ArcGISTiledMapServiceLayer ttlayer = new ArcGISTiledMapServiceLayer();
                        //ArcGISDynamicMapServiceLayer tLayer = new ArcGISDynamicMapServiceLayer();
                        //tLayer.Url = TitleMapUrl;
                        //MyMap.Layers.Add(tLayer);
                        //MyMap.MouseRightButtonDown += new MouseButtonEventHandler(MyMap_MouseRightButtonDown);
                        ttlayer.Url = TitleMapUrl;
                        ttlayer.DisplayName = "底图";
                        MyMap.Layers.Add(ttlayer);
                        MyMap.MouseRightButtonDown += new MouseButtonEventHandler(MyMap_MouseRightButtonDown);
                        WriteLog.WriteLogInfo("加载成功");
                    }

                    if (f.Key == "Tml")
                    {
                        Tml.Url = f.Value;
                        Tml.ID = f.Key;
                        Tml.DisplayName = "终端";
                        Tml.MouseLeftButtonUp += new GraphicsLayer.MouseButtonEventHandler(FeatureLayer_MouseLeftButtonUp);
                        var tmp = new OutFields() { };
                        tmp.Add("*");
                        Tml.OutFields = tmp;
                        //Tml.IgnoreServiceScaleRange = true;
                        //Tml.Where = "wldz <> ''";


                        Tml.Mode = FeatureLayer.QueryMode.Snapshot;
                        Tml.DisableClientCaching = true;
                        Tml.AutoSave = true;
                        Tml.MouseRightButtonDown +=
                            new GraphicsLayer.MouseButtonEventHandler(FeatureLayer_PreviewMouseDown);
                        haveTml = true;


                        //MyMap.Layers.Add(Tml); 
                    }
                    if (f.Key == "Ctrl")
                    {
                        Controller.Url = f.Value;
                        Controller.ID = f.Key;
                        Controller.DisplayName = "控制器";
                        var tmp = new OutFields() { };
                        tmp.Add("*");
                        Controller.OutFields = tmp;
                        Controller.AutoSave = true;
                        //Controller.Where = "txm <> ''";
                        Controller.Mode = FeatureLayer.QueryMode.Snapshot;
                        Controller.DisableClientCaching = true;
                        Controller.MouseLeftButtonUp +=
                            new GraphicsLayer.MouseButtonEventHandler(FeatureLayer_MouseLeftButtonUp);
                        Controller.MouseRightButtonDown +=
                            new GraphicsLayer.MouseButtonEventHandler(FeatureLayer_PreviewMouseDown);
                        haveCtrl = true;
                        //MyMap.Layers.Add(Ctrl);
                    }
                    if (f.Key == "Slu")
                    {
                        Concentrator.Url = f.Value;
                        Concentrator.ID = f.Key;
                        Concentrator.DisplayName = "集中器";
                        var tmp = new OutFields() { };
                        tmp.Add("*");
                        Concentrator.OutFields = tmp;
                        Concentrator.AutoSave = true;
                        Concentrator.Mode = FeatureLayer.QueryMode.OnDemand;
                        Concentrator.DisableClientCaching = true;
                        Concentrator.MouseLeftButtonUp +=
                            new GraphicsLayer.MouseButtonEventHandler(FeatureLayer_MouseLeftButtonUp);
                        Concentrator.MouseRightButtonDown +=
                            new GraphicsLayer.MouseButtonEventHandler(FeatureLayer_PreviewMouseDown);
                        haveSlu = true;
                        //MyMap.Layers.Add(Ctrl);
                    }
                    if (f.Key == "Line")
                    {
                        Line.Url = f.Value;
                        Line.ID = f.Key;
                        Line.DisplayName = "线路";
                        Line.MouseLeftButtonUp += new GraphicsLayer.MouseButtonEventHandler(FeatureLayer_MouseLeftButtonUp);
                        var tmp = new OutFields() { };
                        tmp.Add("*");
                        Line.OutFields = tmp;

                        Line.Mode = FeatureLayer.QueryMode.OnDemand;
                        Line.DisableClientCaching = true;
                        Line.AutoSave = true;
                        Line.MouseRightButtonDown +=
                            new GraphicsLayer.MouseButtonEventHandler(FeatureLayer_PreviewMouseDown);
                        haveLine = true;


                        //MyMap.Layers.Add(Tml); 
                    }

                    if (f.Key == "SearchService")
                    {
                        SearchService = f.Value;
                    }
                    //唯一标识符，若没有则默认为Lid
                    if (f.Key.Contains("ljdz"))
                    {
                        if (string.IsNullOrEmpty(f.Value)) continue;
                        OnlyField = info["ljdz"];
                    }
                    if (f.Key.Contains("ctrldz"))
                    {
                        if (string.IsNullOrEmpty(f.Value)) continue;
                        CtrlField = info["ctrldz"];
                    }
                    if (f.Key.Contains("status"))
                    {
                        if (string.IsNullOrEmpty(f.Value)) continue;
                        StateField = info["status"];
                    }
                    if (f.Key.Contains("IsChangeXYbyMap"))
                    {
                        int intTmp = int.Parse(info["IsChangeXYbyMap"]);
                        _isChangeXYbyMap = intTmp == 1;
                    }
                    if (f.Key.Contains("TmlMaxRes"))
                    {
                        double tmlMaxRes = double.Parse(info["TmlMaxRes"]);
                        if (tmlMaxRes > 1) Tml.MaximumResolution = tmlMaxRes;
                    }
                    if (f.Key.Contains("SluMaxRes"))
                    {
                        double sluMaxRes = double.Parse(info["SluMaxRes"]);
                        if (sluMaxRes > 1) Concentrator.MaximumResolution = sluMaxRes;
                    }
                    if (f.Key.Contains("CtrlMaxRes"))
                    {
                        double ctrlMaxRes = double.Parse(info["CtrlMaxRes"]);
                        if (ctrlMaxRes > 1) Controller.MaximumResolution = ctrlMaxRes;
                    }
                    if (f.Key.Contains("OffsetX"))
                    {
                        OffsetX = double.Parse(info["OffsetX"]);

                    }
                    if (f.Key.Contains("OffsetY"))
                    {
                        OffsetY = double.Parse(info["OffsetY"]);
                    }
                    if (f.Key.Contains("xmin"))
                    {
                        xmin = double.Parse(info["xmin"]);
                    }
                    if (f.Key.Contains("ymin"))
                    {
                        ymin = double.Parse(info["ymin"]);
                    }
                    if (f.Key.Contains("xmax"))
                    {
                        xmax = double.Parse(info["xmax"]);
                    }
                    if (f.Key.Contains("ymax"))
                    {
                        ymax = double.Parse(info["ymax"]);
                    }
                    //服务url+port 懒得用tml url 万一没有终端图层什么的，直接读配置
                    if (f.Key.Contains("ServiceUrl"))
                    {
                        ServiceIP = f.Value;

                    }
                }
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("地图出错，异常为:" + ex);
            }




            if (haveTml) MyMap.Layers.Add(Tml);
            if (haveCtrl) MyMap.Layers.Add(Controller);
            if (haveSlu) MyMap.Layers.Add(Concentrator);
            //添加 线路图层
            if( haveLine) MyMap.Layers.Add(Line);


            MyMap.Layers.Add(MyGraphicsLayer);

            var gl = MyMap.Layers[0];
            MyMap.Layers.RemoveAt(0);
            MyMap.Layers.Add(gl);

            if (xmin > 0 && ymin > 0 && xmax > 0 && ymax > 0)
            {
                this.MyMap.Extent = new Envelope(new MapPoint(xmin, ymin), new MapPoint(xmax, ymax));
            }

            //Tml.Where="status>1";
            //this.MyMap.Extent = new Envelope(new MapPoint(13272337.431, 3627494.452), new MapPoint(13392352.671, 3719398.186));
            model = new MapGisViewModel();
            DataContext = model;


            MyMap.Layers.LayersInitialized += (o, evt) =>
            {
                //model.IsBusy = false;
                //ESRI.ArcGIS.Client.Editor myEditor = LayoutRoot.Resources["MyEditor"] as ESRI.ArcGIS.Client.Editor;
                //myEditor.Map = MyMap;
                //myEditor.LayerIDs = new string[] { "Tml", "Ctrl","Slu" };
                InitEvent();

                myEditorC = new Editor();
                myEditorC.SelectionMode = ESRI.ArcGIS.Client.DrawMode.Rectangle;
                myEditorC.Map = MyMap;
                myEditorC.LayerIDs = new string[] { "Tml", "Ctrl", "Slu","Line" };
                ESRI.ArcGIS.Client.Editor myEditor = LayoutRoot.Resources["MyEditor"] as ESRI.ArcGIS.Client.Editor;
                myEditor.Map = MyMap;
                myEditor.LayerIDs = new string[] { "Tml", "Ctrl", "Slu" ,"Line"};


                //QueryTask queryTask = new QueryTask();
                //queryTask.Url = Tml.Url;
                //queryTask.ExecuteCompleted += queryTask_ExecuteCompleted;
                //Query query = new ESRI.ArcGIS.Client.Tasks.Query();
                //query.OutSpatialReference = MyMap.SpatialReference;
                //query.ReturnGeometry = true;
                //query.Where = "1=1";
                //queryTask.ExecuteAsync(query);

            };
            Tml.UpdateCompleted += new EventHandler(Tml_UpdateCompleted);

            Controller.UpdateCompleted += new EventHandler(Controller_UpdateCompleted);   //事情多  搁置  2018年7月4日08:28:17  待完善

            Line.UpdateCompleted += new EventHandler(Line_UpdateCompleted);  

        }

        public static Dictionary<string, string> Readxml(string xmlFileName, string filePath = null)
        {
            var info = new Dictionary<string, string>();

            if (!xmlFileName.EndsWith(".xml"))
            {
                xmlFileName += ".xml";
            }
            //lvf 2018年3月21日16:45:24  添加可填写路径的配置文件读取
            var fp = "\\SystemXmlConfig";
            if (!string.IsNullOrEmpty(filePath)) fp = filePath.Trim();
            string dir = Directory.GetCurrentDirectory() + fp;
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            string path = dir + "\\" + xmlFileName;
            if (File.Exists(xmlFileName))
                path = xmlFileName;
            if (!File.Exists(path)) return info;
          
            return Read(path);

        }


        public static Dictionary<string, string> Read(string path)
        {
            var info = new Dictionary<string, string>();

            if (!path.EndsWith(".xml"))
            {
                path += ".xml";
            }
            //string dir = Directory.GetCurrentDirectory() + "\\SystemXmlConfig";
            //if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            //string path = dir + "\\" + xmlFileName;
            //if (File.Exists(path))
            //    path = path;

            if (!File.Exists(path)) return info;

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(path);

                XmlNode root = xmlDoc.SelectSingleNode("Root");
                if (root != null)
                {
                    var nodelist = root.ChildNodes;

                    foreach (XmlNode nodeType in nodelist)
                    {
                        XmlElement element = (XmlElement)nodeType;
                        if (element != null)
                        {
                            try
                            {
                                string key = element.GetAttribute("key");
                                string value = element.GetAttribute("value");
                                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value) && !info.ContainsKey(key))
                                {
                                    info.Add(key, value);
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //WriteLog.WriteLogError("Core Config ReadConfig Error: GetConfigFilePaht path: " + configfilepath +
                //                       ", nodeName :" + nodename + "; Ex:" + ex);
                WriteLog.WriteLogError("Read Xml Error: Path :" + path + ",Error:" + ex);
            }
            return info;
        }

        /// <summary>
        /// 控制器图标渲染机制
        /// </summary>
        private  void LoadCtrlSymbols()
        {
            try
            {
                //UniqueValueRenderer myCtrlRenderer = new UniqueValueRenderer();
                ESRI.ArcGIS.Client.Symbols.PictureMarkerSymbol tt = new ESRI.ArcGIS.Client.Symbols.PictureMarkerSymbol();
                //myCtrlRenderer.Field = "TYPE";
                if(ImageResources.GetEquipmentIcon(2090100)== null) return;
                tt.Source = ImageResources.GetEquipmentIcon(2090100);
                tt.OffsetX = OffsetX;
                tt.OffsetY = OffsetY;

                UniqueValueRenderer myUniqueRenderer = new UniqueValueRenderer();
                myUniqueRenderer.Field = StateField; //"TYPE";
                myUniqueRenderer.DefaultSymbol = tt;
                for (int j = 0; j < 2; j++)  //双挑灯 图标遍历
                {
                    for (int i = 0; i < 9; i++)
                    {
                        //int tmp = 2090100 + j * 10 + i;
                        //int vv = 100 + i;
                        //todo
                        int tmp = 2090200 + j*10 + i;
                        int vv = 200 + j*10 + i;
                        var img = ImageResources.GetEquipmentIcon(tmp);
                        if (img == null) continue;
                        ESRI.ArcGIS.Client.Symbols.PictureMarkerSymbol ttt1 =
                            new ESRI.ArcGIS.Client.Symbols.PictureMarkerSymbol();
                        ttt1.Source = img;
                        ttt1.OffsetX = OffsetX;
                        ttt1.OffsetY = OffsetY;
                        ttt1.AngleAlignment= new MarkerSymbol.MarkerAngleAlignment() ;

                        //myUniqueRenderer.Infos.Add(new UniqueValueInfo() { Value = vv, Symbol = ttt1 });
                        //lvf  2019年1月7日10:07:53  原来其他模块不区分灯头，地图先做的，现在大家都支持灯头了，修改为统一状态为 2090 * 1000 + lampnum * 100 + code;
                        myUniqueRenderer.Infos.Add(new UniqueValueInfo() {Value = tmp, Symbol = ttt1});
                    }
                }
                for (int i = 0; i < 15; i++)  //单挑灯图标遍历
                {
                    int tmp = 2090100 + i;
                    int vv = 100 + i;

                    var img = ImageResources.GetEquipmentIcon(tmp);
                    if (img == null) continue;
                    ESRI.ArcGIS.Client.Symbols.PictureMarkerSymbol ttt =
                        new ESRI.ArcGIS.Client.Symbols.PictureMarkerSymbol();
                    ttt.Source = img;
                    ttt.OffsetX = OffsetX;
                    ttt.OffsetY = OffsetY;

                    //myUniqueRenderer.Infos.Add(new UniqueValueInfo() { Value = vv, Symbol = ttt });
                    //lvf  2019年1月7日10:07:53  原来其他模块不区分灯头，地图先做的，现在大家都支持灯头了，修改为统一状态为 2090 * 1000 + lampnum * 100 + code;
                    myUniqueRenderer.Infos.Add(new UniqueValueInfo() { Value = tmp, Symbol = ttt });
                }

                //区别未绑定控制器 
                if (ImageResources.GetEquipmentIcon(2090199) != null &&
                    ImageResources.GetEquipmentIcon(2090299) != null)
                {
                    ESRI.ArcGIS.Client.Symbols.PictureMarkerSymbol noMatching1 = new ESRI.ArcGIS.Client.Symbols.PictureMarkerSymbol();
                    noMatching1.Source = ImageResources.GetEquipmentIcon(2090199);
                    ESRI.ArcGIS.Client.Symbols.PictureMarkerSymbol noMatching2 = new ESRI.ArcGIS.Client.Symbols.PictureMarkerSymbol();
                    noMatching2.Source = ImageResources.GetEquipmentIcon(2090299);
                    noMatching1.OffsetX = OffsetX;
                    noMatching1.OffsetY = OffsetY;
                    noMatching2.OffsetX = OffsetX;
                    noMatching2.OffsetY = OffsetY;

                    myUniqueRenderer.Infos.Add(new UniqueValueInfo() { Value = 2090199, Symbol = noMatching1 });
                    myUniqueRenderer.Infos.Add(new UniqueValueInfo() { Value = 2090299, Symbol = noMatching2 });
                }
              





                Controller.Renderer = myUniqueRenderer;
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }
        /// <summary>
        /// 终端图标渲染机制
        /// </summary>
        private void LoadTmlSymbols()
        {
            try
            {
                //UniqueValueRenderer myCtrlRenderer = new UniqueValueRenderer();
                ESRI.ArcGIS.Client.Symbols.PictureMarkerSymbol tt = new ESRI.ArcGIS.Client.Symbols.PictureMarkerSymbol();
                //myCtrlRenderer.Field = "TYPE";
                if (ImageResources.GetEquipmentIcon("tml1") == null) return ;
                tt.Source = ImageResources.GetEquipmentIcon("tml1");
                //tt.Width = 32;
                //tt.Height = 32;

                UniqueValueRenderer myUniqueRenderer = new UniqueValueRenderer();
                myUniqueRenderer.Field = StateField; //"TYPE";
                myUniqueRenderer.DefaultSymbol = tt;


                for (int i = 1; i < 8; i++)  //终端图标遍历
                {
                    var tmp = "tml" + i;
                    int vv = i;
                    ESRI.ArcGIS.Client.Symbols.PictureMarkerSymbol ttt = new ESRI.ArcGIS.Client.Symbols.PictureMarkerSymbol();
                    var img = ImageResources.GetEquipmentIcon(tmp);
                    if (img == null) continue;
                    ttt.Source = ImageResources.GetEquipmentIcon(tmp);
                    //ttt.Width = 32;
                    //ttt.Height = 32;
                    //ttt.OffsetX = OffsetX;
                    //ttt.OffsetY = OffsetY;
                    myUniqueRenderer.Infos.Add(new UniqueValueInfo() { Value = vv, Symbol = ttt });
                }


                for (int i = 10; i < 18; i++)  //亮化终端图标遍历
                {
                    var tmp = "tml" + i;
                    int vv = i;
                    ESRI.ArcGIS.Client.Symbols.PictureMarkerSymbol ttt = new ESRI.ArcGIS.Client.Symbols.PictureMarkerSymbol();
                    var img = ImageResources.GetEquipmentIcon(tmp);
                    if (img == null) continue;
                    ttt.Source = ImageResources.GetEquipmentIcon(tmp);
                    //ttt.Width = 32;
                    //ttt.Height = 32;
                    //ttt.OffsetX = OffsetX;
                    //ttt.OffsetY = OffsetY;
                    myUniqueRenderer.Infos.Add(new UniqueValueInfo() { Value = vv, Symbol = ttt });
                }

                Tml.Renderer = myUniqueRenderer;
                
            }
            catch(Exception ex )
            {
                throw;
            }
          
        }


        /// <summary>
        /// 终端图标渲染机制
        /// </summary>
        private void LoadLineSymbols()
        {
            try
            {
                //UniqueValueRenderer myCtrlRenderer = new UniqueValueRenderer();
                ESRI.ArcGIS.Client.Symbols.SimpleLineSymbol  closeSymbol = new ESRI.ArcGIS.Client.Symbols.SimpleLineSymbol ();
                closeSymbol.Color = new SolidColorBrush(Color.FromArgb(100, 42, 151, 251));
                closeSymbol.Width = 5;

                UniqueValueRenderer myUniqueRenderer = new UniqueValueRenderer();
                myUniqueRenderer.Field = StateField; //"TYPE";
                myUniqueRenderer.DefaultSymbol = closeSymbol;
                //0：关    1：开
                myUniqueRenderer.Infos.Add(new UniqueValueInfo() { Value = 0, Symbol = closeSymbol });


                ESRI.ArcGIS.Client.Symbols.SimpleLineSymbol openSymbol = new ESRI.ArcGIS.Client.Symbols.SimpleLineSymbol();
                openSymbol.Color = new SolidColorBrush(Color.FromArgb(100, 255, 69, 0));
                openSymbol.Width = 5;
                myUniqueRenderer.Infos.Add(new UniqueValueInfo() { Value = 1, Symbol = openSymbol });

                Line.Renderer = myUniqueRenderer;
                //for (int i = 1; i < 6; i++)  //终端图标遍历
                //{
                //    var tmp = "tml" + i;
                //    int vv = i;
                //    ESRI.ArcGIS.Client.Symbols.PictureMarkerSymbol ttt = new ESRI.ArcGIS.Client.Symbols.PictureMarkerSymbol();
                //    var img = ImageResources.GetEquipmentIcon(tmp);
                //    if (img == null) continue;
                //    ttt.Source = ImageResources.GetEquipmentIcon(tmp);
                //    //ttt.Width = 32;
                //    //ttt.Height = 32;
                //    //ttt.OffsetX = OffsetX;
                //    //ttt.OffsetY = OffsetY;
                //    myUniqueRenderer.Infos.Add(new UniqueValueInfo() { Value = vv, Symbol = ttt });
                //}


                //for (int i = 10; i < 16; i++)  //亮化终端图标遍历
                //{
                //    var tmp = "tml" + i;
                //    int vv = i;
                //    ESRI.ArcGIS.Client.Symbols.PictureMarkerSymbol ttt = new ESRI.ArcGIS.Client.Symbols.PictureMarkerSymbol();
                //    var img = ImageResources.GetEquipmentIcon(tmp);
                //    if (img == null) continue;
                //    ttt.Source = ImageResources.GetEquipmentIcon(tmp);
                //    //ttt.Width = 32;
                //    //ttt.Height = 32;
                //    //ttt.OffsetX = OffsetX;
                //    //ttt.OffsetY = OffsetY;
                //    myUniqueRenderer.Infos.Add(new UniqueValueInfo() { Value = vv, Symbol = ttt });
                //}

                //Tml.Renderer = myUniqueRenderer;

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private void LoadPicSymbols()
        {
            LoadCtrlSymbols();
            LoadTmlSymbols();
            LoadLineSymbols();
        }

        void Controller_UpdateCompleted(object sender, EventArgs e)
        {
            ctrlIndex123 = new ConcurrentDictionary<long , int>();
            ctrlLine = new ConcurrentDictionary<Tuple<int, int>, List<int>>();
            int indexxx = -1;

            try
            {
                //记录 图层中终端的排序 和物理地址对应
                foreach (var g in Controller.Graphics)
                {
                    indexxx++;

                    //if (g.Attributes[CtrlField] == null) continue;
                    //if (g.Attributes[CtrlField].ToString().Trim() == "") continue;


                    if (g.Attributes[CtrlField] == null ||g.Attributes[CtrlField].ToString().Trim() == "")
                    {
                       //todo
                       g.Attributes[StateField] = "2090199";
                        continue;
                    }

                    var barcodetmp = g.Attributes[CtrlField].ToString().Trim();
                    if (barcodetmp.Contains(",")) continue;
                    var barcode =  Convert.ToInt64(g.Attributes[CtrlField].ToString().Trim());

                    var sluid = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetSluIdByLampCode(barcode);
                    long idddd = 0;
                    if (sluid == null)  //nb单灯 以条形码为唯一标识
                    {

                        idddd = barcode;
                        if (ctrlIndex123.ContainsKey(idddd))
                        {
                            if (ctrlIndex123[idddd] == indexxx) continue;
                            ctrlIndex123[idddd] = indexxx;
                        }
                        else
                        {
                            ctrlIndex123.TryAdd(idddd, indexxx);
                        }



                    }
                    else
                    {
                        if (sluid.Item1 == 0 || sluid.Item2 == 0) continue;
                        idddd = sluid.Item1 * 10000L + sluid.Item2;
                        if (ctrlIndex123.ContainsKey(idddd))
                        {
                            if (ctrlIndex123[idddd] == indexxx) continue;
                            ctrlIndex123[idddd] = indexxx;
                        }
                        else
                        {
                            ctrlIndex123.TryAdd(idddd, indexxx);
                        }


                        //记录 单灯与线路的对应关系  lvf 2019年4月24日14:25:38

                        if (g.Attributes["ssxl"] == null || g.Attributes["ssjkx"] == null)continue;
                        if (g.Attributes["ssxl"].ToString().Trim() == "" || g.Attributes["ssjkx"].ToString().Trim() == "") continue;
                        var rtuid =Convert.ToInt32( g.Attributes["ssjkx"].ToString().Trim());
                        var linekx =Convert.ToInt32(g.Attributes["ssxl"].ToString().Trim()) ;
                        var tu = new Tuple<int, int>(rtuid, linekx);
                        if (ctrlLine.ContainsKey(tu))
                        {
                            var lst = ctrlLine[tu];
                            if (lst.Contains(indexxx) == false)
                            {
                                lst.Add(indexxx);
                                ctrlLine[tu] = lst;
                            }
                           
                        }
                        else
                        {
                            var lst = new List<int>();
                            lst.Add(indexxx);
                            ctrlLine.TryAdd(tu, lst);
                        }
                    }

                }
                LoadCtrlSymbols();
                //LoadPicSymbols();

                //var cbr = new ClassBreaksRenderer();
                //cbr.Field = StateField;

                //Symbol tt = LayoutRoot.Resources["DefaultMarkerSymbol"] as ESRI.ArcGIS.Client.Symbols.Symbol;
                //cbr.Classes.Add(new ClassBreakInfo() { MinimumValue = 0, MaximumValue = 100, Symbol = tt });
            }catch(Exception ex)
            {
                
            }


        }

        void Line_UpdateCompleted(object sender, EventArgs e)
        {
            LineIndex = new ConcurrentDictionary<Tuple<int,int>, List<int>>();
            int indexxx = -1;

            try
            {
                //记录 图层中终端的排序 和物理地址对应
                foreach (var g in Line.Graphics)
                {
                    indexxx++;

                    if (g.Attributes["kgl"] == null || g.Attributes["ssjkx"] == null) continue;
                    if (g.Attributes["kgl"].ToString().Trim() == "" || g.Attributes["ssjkx"].ToString().Trim() == "") continue;
                    var switchOutId = Convert.ToInt32( g.Attributes["kgl"].ToString().Trim());
                    var rtuId =Convert.ToInt32( g.Attributes["ssjkx"].ToString().Trim());

                    long idddd = 0;
                    var tu = new Tuple<int,int>(rtuId,switchOutId);
                    if (LineIndex.ContainsKey(tu))
                    {
                        var lst = LineIndex[tu];
                        if ( lst.Contains(indexxx)==false)lst.Add(indexxx);
                        LineIndex[tu] = lst;
                    }
                    else
                    {
                        var lst = new List<int>();
                        lst.Add(indexxx);
                        LineIndex.TryAdd(tu, lst);
                    }

                }

                LoadLineSymbols();
            }
            catch (Exception ex)
            {

            }


        }

        void Tml_UpdateCompleted(object sender, EventArgs e)
        {
            tmlIndex = new ConcurrentDictionary<string , int>();
            int indexxx = -1;
            //记录 图层中终端的排序 和物理地址对应
            foreach (var g in Tml.Graphics)
            {
                indexxx++;

                if (g.Attributes[OnlyField] == null) continue;
                if (g.Attributes[OnlyField].ToString().Trim() == "") continue;
                var phyid = g.Attributes[OnlyField].ToString().Trim();
                if (tmlIndex.ContainsKey(phyid))
                {
                    tmlIndex[phyid] = indexxx;
                }else
                {
                    tmlIndex.TryAdd(phyid, indexxx);
                }

                //不需要从地图更新数据库中坐标信息
                if (_isChangeXYbyMap == false) continue;
                if(g.Attributes.ContainsKey("lat")==false || g.Attributes.ContainsKey("lng")== false ) continue;
                var x = g.Attributes["lat"]+"" ;
                var y = g.Attributes["lng"]+"";
                if (string.IsNullOrEmpty(x) || string.IsNullOrEmpty(y)) continue;
                var xx = Convert.ToDouble(x);
                var yy = Convert.ToDouble(y);
                //将在线服务坐标存入数据库
                //var t = g.Geometry as MapPoint;
                //Graphic gt = new Graphic();
                //gt.Geometry = ChangeCoodinateTo84(t.X, t.Y);
                //MapPoint mp = new MapPoint();
                //mp = gt.Geometry as MapPoint;
                //double xt = mp.X;
                //double yt = mp.Y;
                var lid = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetLidByPhyId(Convert.ToInt16(phyid), WjParaBase.EquType.Rtu);
                Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.OnEquipmentMentMapLocationChangeByMap(
                lid, xx, yy);



            }

            if (needOnErrChanged == true)
            {
                //throw new NotImplementedException();
                var rtus = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.Keys;//.EquipmentInfoDictionary.Keys;
                tmlIcon = new ConcurrentDictionary<int, Tuple<bool, string>>();
                foreach (int id in rtus)
                {
                    OnErrChanged(id);
                }
                ModifyFeaturesAttribute3(StateField);
                needOnErrChanged = false;
            }
            LoadPicSymbols();

            var cbr = new ClassBreaksRenderer();
            cbr.Field = StateField;

            Symbol tt = LayoutRoot.Resources["DefaultMarkerSymbol"] as ESRI.ArcGIS.Client.Symbols.Symbol;
            cbr.Classes.Add(new ClassBreakInfo() { MinimumValue = 0, MaximumValue = 100, Symbol = tt });


        }

        #region Ahri
        /*
         * 地图鼠标处理事件
         * 2014.12  Lvf
         * 嗯？你知道Ahri是谁？灵动起来~有缘人~加油
         */
        //画画，方式很多，目前只用到polyline&point，用于批量添加tml|controller|concentrator
        private Draw _drawObject;
        public static int TreeSelectId = 0;
        public static int TreeSelectCtrlId = 0;
        private static bool onDraw;
        //图层点击
        //先清空各图层的选中图元
        //生成右上数据表格
        //如果选中的是集中器，则渲染其下控制器
        private void FeatureLayer_MouseLeftButtonUp(object sender, GraphicMouseButtonEventArgs args)
        {
            FeatureLayer featureLayer = sender as FeatureLayer;// FeatureLayer;
            //chkErr.IsChecked = false;
            //////////////////MyGraphicsLayer.Graphics.Clear();
            FindDetailsDataGrid.Visibility = Visibility.Hidden;
            if (Keyboard.Modifiers != ModifierKeys.Control)
            {
                try
                {
                    myEditorC.ClearSelection.Execute(null);
                    //ESRI.ArcGIS.Client.Editor myEditorC = LayoutRoot.Resources["MyEditorC"] as ESRI.ArcGIS.Client.Editor;
                    //myEditorC.Map = MyMap;
                    //myEditorC.LayerIDs = new string[] { "终端", "集中器", "控制器","线路" };
                    //myEditorC.ClearSelection.Execute(null);
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
                MyFeatureDataForm.Visibility = Visibility.Visible;
                //showPic.Visibility = Visibility.Visible;
                //ShowPic(Convert.ToInt32(args.Graphic.Attributes["Lid"]));
            }
            else
            {
                MyFeatureDataForm.Visibility = Visibility.Hidden;
                //showPic.Visibility = Visibility.Hidden;
            }
            int x;
            var tmp = args.Graphic.Attributes[OnlyField] + "";

            //todo


            //ShowPic(Convert.ToInt32(args.Graphic.Attributes["Lid"]));
            MyFeatureDataForm.FeatureLayer = featureLayer;
            MyFeatureDataForm.GraphicSource = args.Graphic;
            //MyFeatureDataForm.Height = 500;// args.Graphic.Attributes.Count *35;
            //double ht = args.Graphic.Attributes.Count * 32 + 35;
            //FeatureDataFormBorder.Height =ht>400?400: ht;
            FeatureDataFormBorder.Visibility = Visibility.Visible;

            //GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
            //graphicsLayer.Graphics.Clear();
            MyGraphicsLayer.Graphics.Clear();

            //显示图片  lvf 2019年4月11日16:09:33 
            if (ChkShowPic.IsChecked == true)
            {
                if (args.Graphic.Attributes["photo"] != null)
                {
                    var url = args.Graphic.Attributes["photo"].ToString();
                    if (string.IsNullOrEmpty(url) == false)
                    {
                        ShowPhotos(url);
                    }
                }
            }



            if (featureLayer == Concentrator)
            {
                if (args.Graphic.Attributes[OnlyField] == null || string.IsNullOrEmpty(args.Graphic.Attributes[OnlyField].ToString().Trim()) ) return;
                var rtuId = Convert.ToInt32(args.Graphic.Attributes[OnlyField]);

                if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(rtuId)== false )
                    return;
                var sluInfo = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuId] as Wj2090Slu;
                if (sluInfo == null) return; 
                var ctrlst = sluInfo.WjSluCtrls;
                var barcodelst = (from t in ctrlst select t.Value.BarCodeId).ToList();
                //graphicsLayer.Graphics.Clear();
                foreach (Graphic g in Controller.Graphics)
                {
                    if (g.Attributes[OnlyField] == null) continue;

                    if (barcodelst.Contains(Convert.ToInt32(g.Attributes[OnlyField]))==false ) continue;
                    Graphic gr = new Graphic
                    {
                        Geometry = g.Geometry,
                        Symbol = LayoutRoot.Resources["DefaultMarkerSymbol"] as ESRI.ArcGIS.Client.Symbols.Symbol, // newSymbol is a SimpleMarkerSymbol (point)
                        //Symbol = ImageResources.SluIcon as ESRI.ArcGIS.Client.Symbols.PictureMarkerSymbol
                    };
                    MyGraphicsLayer.Graphics.Add(gr);
          
                }
            }
            args.Graphic.Select();

            try
            {
                //发布事件  选中当前节点
                var argss = new PublishEventArgs
                {
                    EventType = PublishEventType.Core,
                    EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                    EventAttachInfo = "gis"
                };

                if (featureLayer == Controller)
                {
                    if (args.Graphic.Attributes[CtrlField] == null || string.IsNullOrEmpty(args.Graphic.Attributes[CtrlField].ToString().Trim())) return;
                    var ctrlbarcode = Convert.ToInt64(args.Graphic.Attributes[CtrlField]);
                    var sluid = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetSluIdByLampCode(ctrlbarcode);

                    //nb 单灯
                    if (sluid == null)
                    {


                        var ctrlid = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetCtrlIdByBarcode(ctrlbarcode);
                        if (ctrlid == 0) return;

                        var para = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetCtrlField(ctrlid);

                        var argsnb = new PublishEventArgs
                        {
                            EventType = PublishEventType.Core,
                            EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                            EventAttachInfo = "gis"
                        };

                        //var para = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetCtrlField(NodeId);

                        argsnb.AddParams(para);
                        argsnb.AddParams(ctrlid);

                        EventPublish.PublishEvent(argsnb);
                        return;
                    }
                    if (ctrlbarcode == 0 ||sluid ==null|| sluid.Item1==0 ||sluid.Item2==0) return;
                    argss.AddParams(sluid.Item1);
                    argss.AddParams(sluid.Item2);
                }
                else
                {
                    if (args.Graphic.Attributes[OnlyField] == null || string.IsNullOrEmpty(args.Graphic.Attributes[OnlyField].ToString().Trim())) return;
                    var pyid = Convert.ToInt32(args.Graphic.Attributes[OnlyField]);

                    var rtuid =
                        Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetLidByPhyId(pyid, WjParaBase.EquType.Rtu);
                    if (rtuid == 0) return;
                    argss.AddParams(rtuid);

                    //lvf  2018年5月22日14:40:48  记录当前点击终端
                    Wlst.Sr.EquipmentInfoHolding.Services.Others.CurrentSelectRtuId = rtuid;
                }
                EventPublish.PublishEvent(argss);



            }
            catch (Exception)
            {
                
                throw;
            }
           
        }
        //图层点击
        //给拖动图元做铺垫
        private void FeatureLayer_MouseLeftButtonDown(object sender, GraphicMouseButtonEventArgs e)
        {
            FeatureLayer featureLayer = sender as FeatureLayer;
            //if (featureLayer == Circuit) return;

            model.SetCmUnVisi();
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
            FeatureLayer featureLayer = sender as FeatureLayer;
            //chkErr.IsChecked = false;
            GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
            graphicsLayer.Graphics.Clear();
            ////////////////////////MyGraphicsLayer.Graphics.Clear();   
            try
            {
                myEditorC.ClearSelection.Execute(null);
                //ESRI.ArcGIS.Client.Editor myEditorC = LayoutRoot.Resources["MyEditorC"] as ESRI.ArcGIS.Client.Editor;
                //myEditorC.Map = MyMap;
                //myEditorC.LayerIDs = new string[] { "终端", "集中器", "控制器", "线路" };
                //myEditorC.ClearSelection.Execute(null);
            }
            catch { }
            args.Graphic.Select();
            //发布事件  选中当前节点
            var argss = new PublishEventArgs
            {
                EventType = PublishEventType.Core,
                EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                EventAttachInfo = "gis"
            };
            if (featureLayer == Controller)
            {
                var ctrlbarcode = Convert.ToInt64(args.Graphic.Attributes[OnlyField]);
                var sluid = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetSluIdByLampCode(ctrlbarcode);

                //nb 单灯
                if (sluid == null)
                {


                    var ctrlid = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetCtrlIdByBarcode(ctrlbarcode);
                    if (ctrlid == 0) return;
                    var para = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetCtrlField(ctrlid);

                    var argsnb = new PublishEventArgs
                    {
                        EventType = PublishEventType.Core,
                        EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                    };

                    //var para = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetCtrlField(NodeId);

                    argsnb.AddParams(para);
                    argsnb.AddParams(ctrlid);

                    EventPublish.PublishEvent(argsnb);
                    return;
                }



                argss.AddParams(sluid);
                argss.AddParams(ctrlbarcode);
            }
            else
            {
                if (args.Graphic.Attributes[OnlyField] == null|| string.IsNullOrEmpty(args.Graphic.Attributes[OnlyField].ToString().Trim())) return;
                var pyid = Convert.ToInt32(args.Graphic.Attributes[OnlyField]);

                var rtuid =
                    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetLidByPhyId(pyid, WjParaBase.EquType.Rtu);
                argss.AddParams(rtuid);
                //Wlst.Sr.EquipmentInfoHolding.Services.Others.CurrentSelectRtuId = rtuid;
            }
            EventPublish.PublishEvent(argss);
        }
        //呈现右击菜单
        private void FeatureLayer_PreviewMouseDown(object sender, GraphicMouseButtonEventArgs e)
        {
            FeatureLayer featureLayer = sender as FeatureLayer;
            try
            {
                myEditorC.ClearSelection.Execute(null);
            }
            catch { }
            e.Graphic.Select();


            //if (featureLayer == Circuit) return;//线路右击暂时屏蔽
            if (featureLayer == Controller)
            {

                //int a = Convert.ToInt32(e.Graphic.Attributes["Bid"]);
                //int b = Convert.ToInt32(e.Graphic.Attributes["Lid"]);
                if (e.Graphic.Attributes[CtrlField] == null || string.IsNullOrEmpty(e.Graphic.Attributes[CtrlField].ToString())) return;
                var ctrlbarcode = Convert.ToInt64(e.Graphic.Attributes[CtrlField]);
                var sluid = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetSluIdByLampCode(ctrlbarcode);

                //nb单灯
                if (sluid == null)
                {
                    var ctrlid = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetCtrlIdByBarcode(ctrlbarcode);
                    var para = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetCtrlField(ctrlid);
                    sluid = new Tuple<int, int>(para,ctrlid);
                }
                //int rtuId = (a * 1000) + b;
                model.CurrentRtuId = sluid.Item1;// rtuId;
                model.CurrentCtrlId = sluid.Item2;
                model.Cm.IsOpen = true;


            }
            else
            {
                //if (Convert.ToInt32(e.Graphic.Attributes["Lid"]) < 1000000 || Convert.ToInt32(e.Graphic.Attributes["Lid"]) > 2000000)
                //    return;

                
                int phyid = Convert.ToInt32(e.Graphic.Attributes[OnlyField]);
                int rtuId = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetLidByPhyId(phyid,
                                                                                                      GetFeatureType(
                                                                                                          featureLayer));

                Wlst.Sr.EquipmentInfoHolding.Services.Others.CurrentSelectRtuId = rtuId;

                model.CurrentRtuId = rtuId;
                model.Cm.IsOpen = true;
            }

            var argss = new PublishEventArgs
            {
                EventType = PublishEventType.Core,
                EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                EventAttachInfo = "gis"
            };
            if (featureLayer == Controller)
            {

                if (e.Graphic.Attributes[CtrlField] == null || string.IsNullOrEmpty(e.Graphic.Attributes[CtrlField].ToString())) return;
                var ctrlbarcode = Convert.ToInt64(e.Graphic.Attributes[CtrlField]);
                var sluid = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetSluIdByLampCode(ctrlbarcode);
                if (ctrlbarcode == 0 || sluid.Item1 == 0 || sluid.Item2 == 0) return;
                argss.AddParams(sluid.Item1);
                argss.AddParams(sluid.Item2);
            }
            else
            {

                if (e.Graphic.Attributes[OnlyField] == null || string.IsNullOrEmpty(e.Graphic.Attributes[OnlyField].ToString())) return;
                var pyid = Convert.ToInt32(e.Graphic.Attributes[OnlyField]);

                var rtuid =
                    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetLidByPhyId(pyid, WjParaBase.EquType.Rtu);
                if (rtuid == 0) return;
                argss.AddParams(rtuid);
            }
            EventPublish.PublishEvent(argss);
            //int rtuId = Convert.ToInt32(e.Graphic.Attributes["Lid"]);
            //model.CurrentRtuId = rtuId;
            //model.Cm.IsOpen = true;
        }
        //点击搜索按钮

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            //////////ModifyFeaturesAttribute(1000001, StateField, "10");
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
            //////    ModifyFeaturesAttribute(id, StateField, t);
            //////}
            ////////////////232313213123213test
            if (FindText.Text.Trim() == "")
                return;
            isManuele = true;
            FindDetailsDataGrid.Visibility = Visibility.Visible;
            //GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
            //graphicsLayer.Graphics.Clear();
            MyGraphicsLayer.Graphics.Clear();
            FindTask findTask = new FindTask();
            findTask.Url = SearchService;// _mapService.UrlMapService;
            findTask.Failed += FindTask_Failed;
            FindParameters findParameters = new FindParameters();
            findParameters.LayerIds.AddRange(new int[] { 0, 1, 2 });
            findParameters.SearchFields.AddRange(new string[] { OnlyField, StateField,  "Name" });
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
                //GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
                //graphicsLayer.Graphics.Clear();
                MyGraphicsLayer.Graphics.Clear();
                FindTask findTask = new FindTask();
                findTask.Url = SearchService;//_mapService.UrlMapService;
                findTask.Failed += FindTask_Failed;
                FindParameters findParameters = new FindParameters();
                findParameters.LayerIds.AddRange(new int[] { 0, 1, 2 });
                findParameters.SearchFields.AddRange(new string[] { OnlyField, StateField, "Name" });
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
                FindResult findResult = (FindResult) FindDetailsDataGrid.SelectedItem;
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

                        graphic.Symbol =
                            LayoutRoot.Resources["DefaultMarkerSymbol"] as ESRI.ArcGIS.Client.Symbols.Symbol;
                        break;
                        }
                        //GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
                        //graphicsLayer.Graphics.Clear();
                        MyGraphicsLayer.Graphics.Clear();
                        MyGraphicsLayer.Graphics.Add(graphic);
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

                //DelNullButton.IsEnabled = false;
                BindButton.IsEnabled = false;
                AddCtrllerNum.Text = "";
                //SelectButton.IsEnabled = true ;
                AddMultiController.Style = LayoutRoot.Resources["AddController0"] as Style;
                AddTml.Style = LayoutRoot.Resources["AddTml0"] as Style;
                AddConcentrator.Style = LayoutRoot.Resources["AddConcentrator0"] as Style;

            }
            else
            {

                MyFeatureDataForm.IsReadOnly = false;
                //showPic.Visibility = Visibility.Visible ;
                DelSelectionButton.IsEnabled = true;
                AddMultiController.IsEnabled = true;
                AddTml.IsEnabled = true;
                AddConcentrator.IsEnabled = true;
      
                BindButton.IsEnabled = true;
                //SelectButton.IsEnabled = false ;
                try
                {
                    myEditorC.ClearSelection.Execute(null);
                    //ESRI.ArcGIS.Client.Editor myEditorC = LayoutRoot.Resources["MyEditorC"] as ESRI.ArcGIS.Client.Editor;
                    //myEditorC.Map = MyMap;
                    //myEditorC.LayerIDs = new string[] { "终端", "集中器", "控制器", "线路" };
                    //myEditorC.ClearSelection.Execute(null);
                }
                catch { }
            }
        }

        /// <summary>
        /// 通过图层获取设备类型
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        private  WjParaBase.EquType GetFeatureType(FeatureLayer layer)
        {
            if (layer == Tml) return WjParaBase.EquType.Rtu;
            if (layer == Concentrator) return WjParaBase.EquType.Slu;
            return WjParaBase.EquType.UnKnown;
        }

        ////显示故障设备
        //private void chkErr_Click(object sender, RoutedEventArgs e)
        //{
        //    GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
        //    graphicsLayer.Graphics.Clear();
        //    if (chkErr.IsChecked == true)
        //    {
        //        if (Tml.Visible == true)
        //        {
        //            int err = chkShowON.IsChecked == true ? 4 : 2;
        //            foreach (Graphic g in Tml.Graphics)
        //            {

        //                int x;
        //                var tmp = g.Attributes[StateField] + "";
        //                if (Int32.TryParse(tmp, out x))
        //                {
        //                    if (x == err) //Convert.ToInt32(g.Attributes[StateField]) > 2
        //                    {
        //                        Graphic gr = new Graphic
        //                        {
        //                            Geometry = g.Geometry,
        //                            Symbol =
        //                                LayoutRoot.Resources["ErrTml"] as
        //                                ESRI.ArcGIS.Client.Symbols.Symbol,
        //                            // newSymbol is a SimpleMarkerSymbol (point)
        //                        };
        //                        graphicsLayer.Graphics.Add(gr);


        //                        //目前没有做，因为没有资产信息可以打印。
        //                        //g.Select();   


        //                        //model.SelectedFeaturesData.Add(new FeatureData()
        //                        //{
        //                        //    LampId = 
        //                        //});



        //                    }
        //                }
        //            }
        //        }
        //        //if (Concentrator.Visible == true)
        //        //{
        //        //    foreach (Graphic g in Concentrator.Graphics)
        //        //    {
        //        //        if (Convert.ToInt32(g.Attributes[StateField]) > 3)
        //        //        {
        //        //            Graphic gr = new Graphic
        //        //            {
        //        //                Geometry = g.Geometry,
        //        //                Symbol = LayoutRoot.Resources["ErrConcentrator"] as ESRI.ArcGIS.Client.Symbols.Symbol, // newSymbol is a SimpleMarkerSymbol (point)
        //        //            };
        //        //            graphicsLayer.Graphics.Add(gr);
        //        //        }
        //        //    }
        //        //}
        //        if (Controller.Visible == true)
        //        {
        //            List<string> err = chkShowON.IsChecked == true
        //                                   ? new List<string>() { "4", "6", "8" }
        //                                   : new List<string>() { "2", "4", "6", "8" };
        //            foreach (Graphic g in Controller.Graphics)
        //            {
        //                int x;

        //                var tmp = g.Attributes["TYPE"] + "";
        //                //if (Int32.TryParse(tmp, out x))
        //                //{
        //                var cl = tmp.Substring(tmp.Length - 1);

        //                if (err.Contains(cl))
        //                {
        //                    Graphic gr = new Graphic
        //                    {
        //                        Geometry = g.Geometry,
        //                        Symbol =
        //                            LayoutRoot.Resources["ErrTml"] as
        //                            ESRI.ArcGIS.Client.Symbols.Symbol,
        //                        // newSymbol is a SimpleMarkerSymbol (point)
        //                    };
        //                    graphicsLayer.Graphics.Add(gr);
        //                }

        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (chkShowON.IsChecked == false) return;
        //        if (Tml.Visible == true)
        //        {

        //            int err = 3;//开灯无故障
        //            foreach (Graphic g in Tml.Graphics)
        //            {

        //                int x;
        //                var tmp = g.Attributes[StateField] + "";
        //                if (Int32.TryParse(tmp, out x))
        //                {
        //                    if (x == err) //Convert.ToInt32(g.Attributes[StateField]) > 2
        //                    {
        //                        Graphic gr = new Graphic
        //                        {
        //                            Geometry = g.Geometry,
        //                            Symbol =
        //                                LayoutRoot.Resources["ErrTml"] as
        //                                ESRI.ArcGIS.Client.Symbols.Symbol,
        //                            // newSymbol is a SimpleMarkerSymbol (point)
        //                        };
        //                        graphicsLayer.Graphics.Add(gr);


        //                        //目前没有做，因为没有资产信息可以打印。
        //                        //g.Select();   


        //                        //model.SelectedFeaturesData.Add(new FeatureData()
        //                        //{
        //                        //    LampId = 
        //                        //});



        //                    }
        //                }
        //            }
        //        }
        //        //if (Concentrator.Visible == true)
        //        //{
        //        //    foreach (Graphic g in Concentrator.Graphics)
        //        //    {
        //        //        if (Convert.ToInt32(g.Attributes[StateField]) > 3)
        //        //        {
        //        //            Graphic gr = new Graphic
        //        //            {
        //        //                Geometry = g.Geometry,
        //        //                Symbol = LayoutRoot.Resources["ErrConcentrator"] as ESRI.ArcGIS.Client.Symbols.Symbol, // newSymbol is a SimpleMarkerSymbol (point)
        //        //            };
        //        //            graphicsLayer.Graphics.Add(gr);
        //        //        }
        //        //    }
        //        //}
        //        if (Controller.Visible == true)
        //        {
        //            List<string> err = new List<string>() { "03", "05", "07", "04", "06", "08", "13", "14" };//开灯无故障
        //            foreach (Graphic g in Controller.Graphics)
        //            {
        //                int x;

        //                var tmp = g.Attributes["TYPE"] + "";
        //                //if (Int32.TryParse(tmp, out x))
        //                //{
        //                var cl = tmp.Substring(tmp.Length - 2);

        //                if (err.Contains(cl))
        //                {
        //                    Graphic gr = new Graphic
        //                    {
        //                        Geometry = g.Geometry,
        //                        Symbol =
        //                            LayoutRoot.Resources["ErrTml"] as
        //                            ESRI.ArcGIS.Client.Symbols.Symbol,
        //                        // newSymbol is a SimpleMarkerSymbol (point)
        //                    };
        //                    graphicsLayer.Graphics.Add(gr);
        //                }

        //            }
        //        }
        //    }
        //}

        /// <summary>
        ///// 筛选灯具故障
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void chkLampErr_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        ESRI.ArcGIS.Client.Editor myEditorC = LayoutRoot.Resources["MyEditorC"] as ESRI.ArcGIS.Client.Editor;
        //        myEditorC.Map = MyMap;
        //        myEditorC.LayerIDs = new string[] { "终端", "集中器", "控制器", "线路" };
        //        myEditorC.ClearSelection.Execute(null);
        //    }
        //    catch { }

        //    if(chkLampErr.IsChecked==true  )
        //    {
        //        foreach (var g in Controller.Graphics)
        //        {
        //            if (!g.Attributes.ContainsKey("TYPE")) return;
        //            if (string.IsNullOrEmpty(g.Attributes["TYPE"].ToString())) continue;
        //            int os = Convert.ToInt32( g.Attributes["TYPE"]);
        //            int oss = os%100;
        //            if (oss ==0 || oss >11 )
        //            {
        //                g.Select();
        //            }
        //        }
        //    }


        //}

        ///// <summary>
        ///// 筛选灯具开关灯状态
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void chkLampOn_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        ESRI.ArcGIS.Client.Editor myEditorC = LayoutRoot.Resources["MyEditorC"] as ESRI.ArcGIS.Client.Editor;
        //        myEditorC.Map = MyMap;
        //        myEditorC.LayerIDs = new string[] { "终端", "集中器", "控制器", "线路" };
        //        myEditorC.ClearSelection.Execute(null);
        //    }
        //    catch { }

        //    if (chkLampErr.IsChecked == true)
        //    {
        //        foreach (var g in Controller.Graphics)
        //        {
        //            if (!g.Attributes.ContainsKey("TYPE")) return;
        //            if (string.IsNullOrEmpty(g.Attributes["TYPE"].ToString())) continue;
        //            int os = Convert.ToInt32(g.Attributes["TYPE"]);
        //            int oss = os % 100;
        //            if (oss == 10 || oss ==12)
        //            {
        //                g.Select();
        //            }
        //        }
        //    }else
        //    {
        //        foreach (var g in Controller.Graphics)
        //        {
        //            if (!g.Attributes.ContainsKey("TYPE")) return;
        //            if (string.IsNullOrEmpty(g.Attributes["TYPE"].ToString())) continue;
        //            int os = Convert.ToInt32(g.Attributes["TYPE"]);
        //            int oss = os % 100;
        //            if (oss == 11 || oss == 13)
        //            {
        //                g.Select();
        //            }
        //        }
        //    }

        //}


        //private void ClearButton_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        ESRI.ArcGIS.Client.Editor myEditorC = LayoutRoot.Resources["MyEditorC"] as ESRI.ArcGIS.Client.Editor;
        //        myEditorC.Map = MyMap;
        //        myEditorC.LayerIDs = new string[] { "终端", "集中器", "控制器", "线路" };
        //        myEditorC.ClearSelection.Execute(null);
        //    }
        //    catch { }
        //}
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
            if (sp.Height < 70) //70
            {
                storyop.Begin();
            }
            else
            {
                storysh.Begin();
                chkEdit.IsChecked = false;
                
                DelSelectionButton.IsEnabled = false;
                AddMultiController.IsEnabled = false;
                AddConcentrator.IsEnabled = false;
              
                AddTml.IsEnabled = false;
              
                //DelNullButton.IsEnabled = false;
                BindButton.IsEnabled = false;
          
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
                selectedPointGraphic.Selected = true;
                selectedPointGraphic = null;
            }
            //INIClass iniC = new INIClass(@".\MapGis.ini");
            //try
            //{
            //    iniC.IniWriteValue("MapGis", "Resolution", MyMap.Resolution.ToString());
            //}
            //catch { }
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
                    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.OnEquipmentMentMapLocationChangeByMap(
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
        Dictionary<int, List<int>> listController = new Dictionary<int, List<int>>();
        private void MyMap_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            int count = 0;

            listRtu.Clear();
            listConcentrator.Clear();
            listController.Clear();
            if (Tml.SelectedGraphics.Count() > 0)
            {
                foreach (Graphic g in Tml.SelectedGraphics)
                {
                    //遍历终端图层思密达
                    //if ((Convert.ToInt32(g.Attributes["Lid"]) > 1000000) && (Convert.ToInt32(g.Attributes["Lid"]) < 1100000))
                    if (g.Attributes[OnlyField] == null) continue;
                    if (Convert.ToInt32(g.Attributes[OnlyField]) > 0)
                    {
                        var phyid = Convert.ToInt32(g.Attributes[OnlyField]);
                        listRtu.Add(Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetLidByPhyId(phyid,
                                                                                                              GetFeatureType
                                                                                                                  (Tml)));
                        count++;
                    }
                }
            }
            if (Concentrator.SelectedGraphics.Count() > 0)
            {
                foreach (Graphic g in Concentrator.SelectedGraphics) //遍历终端图层思密达
                {
                    if (g.Attributes[OnlyField] == null) continue;
                    if (Convert.ToInt32(g.Attributes[OnlyField]) > 0)
                    {
                        var phyid = Convert.ToInt32(g.Attributes[OnlyField]);
                        listConcentrator.Add(
                            Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetLidByPhyId(phyid,
                                                                                                      GetFeatureType(
                                                                                                          Concentrator)));
                        count++;
                    }
                }


                //if ((Convert.ToInt32(g.Attributes["Lid"]) > 1500000) && (Convert.ToInt32(g.Attributes["Lid"]) < 1600000))
                //{
                //    listConcentrator.Add(Convert.ToInt32(g.Attributes["Lid"]));
                //    count++;
                //}
            }
            if (Controller.SelectedGraphics.Count() > 0)
            {
                foreach (Graphic g in Controller.SelectedGraphics) //遍历终端图层思密达
                {
                    if (g.Attributes[OnlyField] == null) continue;
                    if (Convert.ToInt32(g.Attributes[OnlyField]) > 0)
                    {
                        var phyid = Convert.ToInt32(g.Attributes[OnlyField]);
                        var ctrlbarcode = Convert.ToInt64(g.Attributes[OnlyField]);
                        var sluctrl =
                            Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetSluIdByLampCode(ctrlbarcode);

                        if (!listController.ContainsKey(sluctrl.Item1))
                            listController.Add(sluctrl.Item1, new List<int>());
                        if (!listController[sluctrl.Item1].Contains(sluctrl.Item2))
                            listController[sluctrl.Item1].Add(sluctrl.Item2);
                        count++;
                    }
                }
            }


            if (count <= 1) return;
            model.GetCm(listRtu, listConcentrator, listController);
            model.Cm.IsOpen = true;

        } //批量右击
        //批量增加控制器 先画线  然后 输入增加的数量
        private void AddMultiController_Click(object sender, RoutedEventArgs e)
        {
                //暂时只实现 读取服务，不可添加，添加控制器通过录入系统，牵涉到灯杆类型，灯头数量  lvf 2018年6月8日09:48:10
            return;
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
                if (Concentrator.SelectedGraphics.ToList()[0].Attributes[OnlyField] == null)
                {
                    var infos = WlstMessageBox.Show("提示", "您选中的集中器没有绑定", WlstMessageBoxType.Close);
                    return;

                }
                else
                {
                    AddMultiController.Style = LayoutRoot.Resources["AddController1"] as Style;
                    AddTml.Style = LayoutRoot.Resources["AddTml0"] as Style;
                    AddConcentrator.Style = LayoutRoot.Resources["AddConcentrator0"] as Style;
               
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
            return;
            GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
            graphicsLayer.Graphics.Clear();
        }
        double x1;
        double x2;
        double y1;
        double y2;
        int num;
        double xT;
        double yT;
        ESRI.ArcGIS.Client.Geometry.Polyline polylineT;
        private void DrawObject_DrawComplete(object sender, DrawEventArgs args)
        {
            //暂时只实现 读取服务，不可添加，添加控制器通过录入系统，牵涉到灯杆类型，灯头数量  lvf 2018年6月8日09:48:10
            return;
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
            //暂时只实现 读取服务，不可添加，添加控制器通过录入系统，牵涉到灯杆类型，灯头数量  lvf 2018年6月8日09:48:10
            return;
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

            //暂时只实现 读取服务，不可添加，添加控制器通过录入系统，牵涉到灯杆类型，灯头数量  lvf 2018年6月8日09:48:10
            return;
            if (onDraw == false) return;
            if (AddCtrllerNum.Text == null) return;
            if (e.Key == Key.Enter)
            {

                int index = this.AddCtrllerType.SelectedIndex;

                int errorIndex = 0;
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
                    if (!Concentrator.SelectedGraphics.ToList()[0].Attributes.ContainsKey(OnlyField)) return;
                    var phyId = (int) Concentrator.SelectedGraphics.ToList()[0].Attributes[OnlyField];
                    int sluid = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetLidByPhyId(phyId,
                                                                                                          GetFeatureType
                                                                                                              (
                                                                                                                  Concentrator));
                    //int sluid = (int)Concentrator.SelectedGraphics.ToList()[0].Attributes[];
                    if (sluid == 0)
                    {
                        var infos = WlstMessageBox.Show("提示", "您选中的集中器没有绑定", WlstMessageBoxType.Close);
                        return;
                    }
                    int nMax = 0;
                    int ctrlid = 0;
                    string name = "";
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
                    xT = (x2 - x1)/num;
                    yT = (y2 - y1)/num;
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

                            //gr.Attributes["TYPE"] = "100";
                            errorIndex = 100;

                        }
                        else
                        {

                            //gr.Attributes["TYPE"] = "200";
                            errorIndex = 200;
                        }
                        //gr.Attributes["Type"] = indexx;
                        
                        //获取地图中该集中器下最大控制器，用于自动绑定，改用条形码识别之后 屏蔽该功能
                        foreach (Graphic g in Controller.Graphics) //遍历终端图层思密达
                        {
                            if (Convert.ToInt32(g.Attributes["Bid"]) ==
                                Convert.ToInt32(Concentrator.SelectedGraphics.ToList()[0].Attributes["Lid"]))
                            {
                                if (nMax < Convert.ToInt32(g.Attributes["Lid"]))
                                    nMax = Convert.ToInt32(g.Attributes["Lid"]);
                            }
                        }
                        if (sluid < 1600000 && sluid > 1500000)
                        {
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
                                    name = g.Value.LampCode;
                                    break;
                                }
                            }
                        }
                        else if (sluid > 1700000 && sluid < 1800000)
                        {
                            // nb单灯
                            //var sluinfo = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(sluid);//2096
                            //if (sluinfo == null) return ;
                            //if (nMax == 0) nMax = 8000000;
                            //foreach (var g in sluinfo.CtrlLst)
                            //{
                            //   if(g.CtrlId == nMax +i)
                            //   {
                            //       ctrlid = g.CtrlId;
                            //       name = g.CtrlName;
                            //       break;
                            //   }
                            //}

                        }
                        if (Concentrator.SelectionCount == 1)
                        {
                            gr.Attributes["Bid"] = sluid;
                            //Concentrator.SelectedGraphics.ToList()[0].Attributes["Lid"];
                            gr.Attributes.Add("Lid", ctrlid);
                            gr.Attributes.Add("NAME", name);
                            var errorIndexss = errorIndex +
                                               Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetCtrlErrorCode(
                                                   sluid, ctrlid);
                            gr.Attributes["TYPE"] = errorIndexss + "";
                            //gr.Attributes.Add("Lid", i+nMax );
                        }

                        x1 = x1 + xT;
                        y1 = y1 + yT;
                        temp.Add(gr);
                    }
                    Controller.Graphics.AddRange(temp);
                    System.Threading.Thread.Sleep(28);

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
            //暂时只实现 读取服务，不可添加，添加控制器通过录入系统，牵涉到灯杆类型，灯头数量  lvf 2018年6月8日09:48:10
            return;
            AddTml.Style = LayoutRoot.Resources["AddTml1"] as Style;
            AddConcentrator.Style = LayoutRoot.Resources["AddConcentrator0"] as Style;
            AddMultiController.Style = LayoutRoot.Resources["AddController0"] as Style;
        
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
            //暂时只实现 读取服务，不可添加，添加控制器通过录入系统，牵涉到灯杆类型，灯头数量  lvf 2018年6月8日09:48:10
            return;
            AddConcentrator.Style = LayoutRoot.Resources["AddConcentrator1"] as Style;
            AddTml.Style = LayoutRoot.Resources["AddTml0"] as Style;
            AddMultiController.Style = LayoutRoot.Resources["AddController0"] as Style;
        
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
            //暂时只实现 读取服务，不可添加，添加控制器通过录入系统，牵涉到灯杆类型，灯头数量  lvf 2018年6月8日09:48:10
            return;
            AddTml.Style = LayoutRoot.Resources["AddTml0"] as Style;
            ESRI.ArcGIS.Client.Geometry.MapPoint point = args.Geometry as ESRI.ArcGIS.Client.Geometry.MapPoint;
            point.SpatialReference = MyMap.SpatialReference;

            Graphic graphic = new Graphic()
            {
                Symbol = LayoutRoot.Resources["DefaultMarkerSymbol"] as ESRI.ArcGIS.Client.Symbols.Symbol,
                Geometry = point,

            };
            graphic.Attributes.Add("Lid", null);

            graphic.Geometry.SpatialReference = MyMap.SpatialReference;
            Tml.Graphics.Add(graphic);
            //System.Threading.Thread.Sleep(28);
            _drawObject.IsEnabled = false;
            //Tml.Update();
            Tml.SaveEdits();
            Tml.Update();

        }
        private void DrawObjectConcentrator_DrawComplete(object sender, DrawEventArgs args)
        {
            //暂时只实现 读取服务，不可添加，添加控制器通过录入系统，牵涉到灯杆类型，灯头数量  lvf 2018年6月8日09:48:10
            return;
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

            try
            {
                var infoss = WlstMessageBox.Show("确认删除", "是否删除所选设备？", WlstMessageBoxType.YesNo);
                DelSelectionButton.Style = LayoutRoot.Resources["DelSelected0"] as Style;
                if (infoss != WlstMessageBoxResults.Yes) return;


                ESRI.ArcGIS.Client.Editor myEditor = LayoutRoot.Resources["MyEditor"] as ESRI.ArcGIS.Client.Editor;
                myEditor.Map = MyMap;
                myEditor.LayerIDs = new string[] { "终端", "控制器", "集中器", "线路" };
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
                myEditorC.ClearSelection.Execute(null);
                //ESRI.ArcGIS.Client.Editor myEditor = LayoutRoot.Resources["MyEditor"] as ESRI.ArcGIS.Client.Editor;
                //myEditor.Map = MyMap;
                //myEditor.LayerIDs = new string[] { "终端", "控制器", "集中器","线路" };
                //myEditor.ClearSelection.Execute(null);
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
                myEditor.LayerIDs = new string[] { "终端", "控制器", "集中器", "线路" };
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
                myEditorC.ClearSelection.Execute(null);
                //ESRI.ArcGIS.Client.Editor myEditor = LayoutRoot.Resources["MyEditor"] as ESRI.ArcGIS.Client.Editor;
                //myEditor.Map = MyMap;
                //myEditor.LayerIDs = new string[] { "终端", "控制器", "集中器", "线路" };
                //myEditor.ClearSelection.Execute(null);
            }
            catch { }
          
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
           
        }
        //绑定图元，不管先后，只要树和地图都有选中的设备，然后点击绑定按钮，会做些许判断，确认无误后成功绑定
        private void BindButton_Click(object sender, RoutedEventArgs e)
        {
            BindButton.Style = LayoutRoot.Resources["Bind1"] as Style;
            AddConcentrator.Style = LayoutRoot.Resources["AddConcentrator0"] as Style;
            AddTml.Style = LayoutRoot.Resources["AddTml0"] as Style;
            AddMultiController.Style = LayoutRoot.Resources["AddController0"] as Style;
          
            FeatureLayer BingFL = new FeatureLayer();
            double snum = 0;
            if (TreeSelectId < 1000000) return;


            //if (Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(TreeSelectId) == false )
            //    return;
            //var rtuInfo = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[TreeSelectId].RtuName;

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
            if (snum == 1)
            {
                if (BingFL.SelectedGraphics.ToList()[0].Attributes[OnlyField] != null)
                {
                    var infos = WlstMessageBox.Show("绑定提示", "选中图元已绑定，是否覆盖绑定", WlstMessageBoxType.YesNo);
                    if (infos != WlstMessageBoxResults.Yes) return;

                }
                if (TreeSelectId < 1100000 && TreeSelectId > 1000000)//终端
                {
                    if (BingFL == Tml)
                    {
                        var phyId =
                            Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(TreeSelectId).
                                RtuPhyId;
                        BingFL.SelectedGraphics.ToList()[0].Attributes[OnlyField] = phyId+"";// TreeSelectId.ToString();
                        //BingFL.SelectedGraphics.ToList()[0].Attributes["Name"] = name;
                        var infoss = WlstMessageBox.Show("确认绑定", "是否绑定", WlstMessageBoxType.YesNo);
                        if (infoss != WlstMessageBoxResults.Yes) return;
                        BingFL.SaveEdits();
                        BingFL.Update();

                        //更改坐标，读取服务版本 不需要
                        //var f =
                        //Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[TreeSelectId];//.EquipmentInfoDictionary[TreeSelectId];
                        //if (isUselocate != 0)
                        //{
                        //    if (f.RtuMapX.Equals(0) || f.RtuMapY.Equals(0))
                        //    {

                        //        var t = BingFL.SelectedGraphics.ToList()[0].Geometry as MapPoint;
                        //        Graphic gt = new Graphic();
                        //        gt.Geometry = ChangeCoodinateTo84(t.X, t.Y);
                        //        MapPoint mp = new MapPoint();
                        //        mp = gt.Geometry as MapPoint;
                        //        double xt = mp.X;
                        //        double yt = mp.Y;
                        //        Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.
                        //            OnEquipmentMentMapLocationChangeByMap(
                        //                TreeSelectId, xt, yt);
                        //    }
                        //    else
                        //    {

                        //        BingFL.SelectedGraphics.ToList()[0].Geometry = ChangeCoodinateFrom84(f.RtuMapX,
                        //                                                                             f.RtuMapY);
                        //        BingFL.SelectedGraphics.ToList()[0].Geometry.SpatialReference =
                        //            MyMap.SpatialReference;
                        //    }
                        //}
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
                        //BingFL.SelectedGraphics.ToList()[0].Attributes["Lid"] = TreeSelectId;
                        var phyId =
                           Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(TreeSelectId).
                               RtuPhyId;
                        BingFL.SelectedGraphics.ToList()[0].Attributes[OnlyField] = phyId + "";
                        //BingFL.SelectedGraphics.ToList()[0].Attributes["Name"] = name;
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
                if (TreeSelectId < 1800000 && TreeSelectId > 1700000 && BingFL == Concentrator) //nb集中器
                {
                    if (BingFL == Concentrator)
                    {
                        //BingFL.SelectedGraphics.ToList()[0].Attributes["Lid"] = TreeSelectId;
                        var phyId =
                           Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(TreeSelectId).
                               RtuPhyId;
                        BingFL.SelectedGraphics.ToList()[0].Attributes[OnlyField] = phyId + "";
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
                if (TreeSelectCtrlId > -1) //控制器     TreeSelectId < 1600000000 && TreeSelectId > 1500000000 
                {
                    if (BingFL == Controller)
                    {
                        //int ctrId;
                        //int concentratorId;
                        //ctrId = TreeSelectId % 1000;
                        //concentratorId = Convert.ToInt32(TreeSelectId / 1000);
                        //BingFL.SelectedGraphics.ToList()[0].Attributes["Lid"] = TreeSelectCtrlId;// ctrId;
                        //BingFL.SelectedGraphics.ToList()[0].Attributes["Bid"] = TreeSelectId;//concentratorId;

                        if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(TreeSelectId) == true)
                        {
                            var sluctrls =
                                Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[TreeSelectId] as
                                Wj2090Slu;
                            if (sluctrls == null) return;
                            if (sluctrls.WjSluCtrls.ContainsKey(TreeSelectCtrlId) == false) return;
                            var barCode = sluctrls.WjSluCtrls[TreeSelectCtrlId].BarCodeId;
                            BingFL.SelectedGraphics.ToList()[0].Attributes[CtrlField] = barCode + ""; // ctrId;
                            var infoss = WlstMessageBox.Show("确认绑定", "是否绑定", WlstMessageBoxType.YesNo);
                            if (infoss != WlstMessageBoxResults.Yes) return;
                            BingFL.SaveEdits();
                            BingFL.Update();
                        }
                        else
                        {
                            var ctrlinfo = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.Get(TreeSelectCtrlId);
                            if (ctrlinfo == null)return;
                            var barcode = ctrlinfo.BarCodeId;
                            BingFL.SelectedGraphics.ToList()[0].Attributes[CtrlField] = barcode + ""; // ctrId;
                            var infoss = WlstMessageBox.Show("确认绑定", "是否绑定", WlstMessageBoxType.YesNo);
                            if (infoss != WlstMessageBoxResults.Yes) return;
                            BingFL.SaveEdits();
                            BingFL.Update();
                        }

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
         
            AddConcentrator.Style = LayoutRoot.Resources["AddConcentrator0"] as Style;
            AddTml.Style = LayoutRoot.Resources["AddTml0"] as Style;
            AddMultiController.Style = LayoutRoot.Resources["AddController0"] as Style;
      
            foreach (var f in Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems)//.EquipmentInfoDictionary)//终端
            {
                Boolean needU;
                var bs = f.Value as Sr.EquipmentInfoHolding.Model.WjParaBase;// Wlst.Cr.WjEquipmentBaseModels.Models.EquipmentInfomation;
                if (bs == null) continue;
                if (bs.RtuGisX == 0 || bs.RtuGisY == 0) continue;
                needU = true;
                if (f.Key > 1000001 && f.Key < 1100000)//终端  自己筛选自己需要的设备 
                {
                    foreach (Graphic g in Tml.Graphics)          //遍历终端图层思密达
                        if (Convert.ToInt32(g.Attributes["Lid"]) == f.Key)
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
                        graphic.Attributes["Lid"] = f.Key;
                        graphic.Geometry.SpatialReference = MyMap.SpatialReference;
                        Tml.Graphics.Add(graphic);
                        System.Threading.Thread.Sleep(28);
                        Tml.Update();
                    }
                }
            }
            foreach (var f in Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems)//.EquipmentInfoDictionary)
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
                            if (Convert.ToInt32(g.Attributes["Bid"]) == f.Key && Convert.ToInt32(g.Attributes["Lid"]) == ctrl.Value.CtrlId)
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
           
        }
        private void AddLine_Click(object sender, RoutedEventArgs e)
        {
        
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
            //在线服务 屏蔽画线   lvf  2018年6月12日09:03:59
            return;
            ESRI.ArcGIS.Client.Geometry.Polyline polyline = args.Geometry as ESRI.ArcGIS.Client.Geometry.Polyline;
            polylineT = polyline;
            polyline.SpatialReference = MyMap.SpatialReference;
            Graphic graphic = new Graphic()
            {
                Symbol = LayoutRoot.Resources["DefaultLineSymbol"] as ESRI.ArcGIS.Client.Symbols.Symbol,
                Geometry = polyline
            };

            GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
       
            System.Threading.Thread.Sleep(28);
            _drawObject.IsEnabled = false;
            

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

        public void LocateSth(int sid, int ctrlid)                     //设备定位
        {
            try
            {
                myEditorC.ClearSelection.Execute(null);
                //ESRI.ArcGIS.Client.Editor myEditor = LayoutRoot.Resources["MyEditor"] as ESRI.ArcGIS.Client.Editor;
                //myEditor.Map = MyMap;
                //myEditor.LayerIDs = new string[] { "终端", "控制器", "集中器", "线路" };
                //myEditor.ClearSelection.Execute(null);
                //INIClass iniC = new INIClass(@".\MapGis.ini");
                //iniC.IniWriteValue("MapGis", "lastTml", sid.ToString());
                //iniC.IniWriteValue("MapGis", "Resolution", MyMap.Resolution.ToString());

            }
            catch { }
            if (ctrlid == -1)
            {
                if (sid < 1100000 && sid > 0)
                {
                    var phyId = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(sid).RtuPhyId + "";
                    //if (tmlIndex.ContainsKey(phyId) == false)
                    //{
                    //    int indexxx = -1;
                    //    foreach (Graphic g in Tml.Graphics)
                    //    {
                    //        indexxx++;
                    //        if (g.Attributes[OnlyField] == null) continue;
                    //        if (g.Attributes[OnlyField].ToString().Trim() == "") continue;
                    //        if (g.Attributes[OnlyField].ToString().Trim() == phyId)
                    //        {
                    //            //g.Symbol = ImageResources.SluIcon;
                    //            g.Select();
                    //            MyMap.PanTo(g.Geometry);
                    //            MyFeatureDataForm.FeatureLayer = Tml;
                    //            MyFeatureDataForm.GraphicSource = g;
                    //            tmlIndex.TryAdd(phyId, indexxx);
                    //        }
                    //    }

                    //    return;
                    //}
                    if (tmlIndex.ContainsKey(phyId) == false) return;
                    var index = tmlIndex[phyId];
                    if (Tml.Graphics.Count < index) return;
                    Tml.Graphics[index].Select();
                    MyMap.PanTo(Tml.Graphics[index].Geometry);
                    MyFeatureDataForm.FeatureLayer = Tml;
                    MyFeatureDataForm.GraphicSource = Tml.Graphics[index];


                    //显示图片  lvf 2019年4月11日16:09:33 
                    if (ChkShowPic.IsChecked == true)
                    {
                        if (Tml.Graphics[index].Attributes["photo"] == null) return;
                        var url = Tml.Graphics[index].Attributes["photo"].ToString();
                        if (string.IsNullOrEmpty(url)) return;
                        ShowPhotos(url);
                    }


                    //foreach (Graphic g in Tml.Graphics)
                    //{
                    //    if (g.Attributes[OnlyField] == null) continue;
                    //    if (g.Attributes[OnlyField].ToString().Trim() == "") continue;
                    //    if (g.Attributes[OnlyField].ToString().Trim() == phyId)
                    //    {
                    //        //g.Symbol = ImageResources.SluIcon;
                    //        g.Select();
                    //        MyMap.PanTo(g.Geometry);
                    //        MyFeatureDataForm.FeatureLayer = Tml;
                    //        MyFeatureDataForm.GraphicSource = g;

                    //    }
                    //}

                }
                else if (sid < 1600000 && sid > 1500000)
                {
                    return;
                    var phyId = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(sid).RtuPhyId + "";

                    foreach (Graphic g in Concentrator.Graphics)
                    {         //遍历终端图层思密达
                        if (g.Attributes[OnlyField] == null) continue;
                        if (g.Attributes[OnlyField].ToString().Trim() == "") continue;
                        if (g.Attributes[OnlyField].ToString().Trim() == phyId)
                        {
                            g.Select();
                            MyMap.PanTo(g.Geometry);
                            MyFeatureDataForm.FeatureLayer = Concentrator;
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
                }
                else if (sid < 1500000 && sid > 1400000)
                {

                    var phyId = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(sid).RtuPhyId + "";
                    foreach (Graphic g in Concentrator.Graphics)
                    {         //遍历终端图层思密达
                        if (g.Attributes[OnlyField] == null) continue;
                        if (g.Attributes[OnlyField].ToString().Trim() == "") continue;
                        if (g.Attributes[OnlyField].ToString().Trim() == phyId)
                        {
                            g.Select();
                            MyMap.PanTo(g.Geometry);
                            MyFeatureDataForm.FeatureLayer = Concentrator;
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
                }
                else if (sid < 1800000 && sid > 1700000)    //nb 单灯
                {
                    return;
                    var phyId = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(sid).RtuPhyId + "";

                    foreach (Graphic g in Concentrator.Graphics)
                    {
                        if (g.Attributes[OnlyField] == null) continue;
                        if (g.Attributes[OnlyField].ToString().Trim() == "") continue;
                        //遍历终端图层思密达
                        if (g.Attributes[OnlyField].ToString().Trim() == phyId)
                        {
                            g.Select();
                            MyMap.PanTo(g.Geometry);
                            MyFeatureDataForm.FeatureLayer = Concentrator;
                            MyFeatureDataForm.GraphicSource = g;

                        }
                    }
                }

            }
            else  //控制器定位
            {
                //return;//todo
                //int ctrId;
                //int concentratorId;
                //ctrId = ctrlid;// sid % 1000;
                //concentratorId = Convert.ToInt32(sid / 1000);

                if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(sid))
                {
                    long iddx = sid*10000L + ctrlid;

                    if (ctrlIndex123.ContainsKey(iddx) == false) return;
                    var index = ctrlIndex123[iddx];
                    if (Controller.Graphics.Count < index) return;

                    Controller.Graphics[index].Select();
                    MyMap.PanTo(Controller.Graphics[index].Geometry);
                    MyFeatureDataForm.FeatureLayer = Controller;
                    MyFeatureDataForm.GraphicSource = Controller.Graphics[index];


                    //显示图片  lvf 2019年4月11日16:09:33 
                    if (ChkShowPic.IsChecked == true)
                    {
                        if (Controller.Graphics[index].Attributes["photo"] == null) return;
                        var url = Controller.Graphics[index].Attributes["photo"].ToString();
                        if (string.IsNullOrEmpty(url)) return;
                        ShowPhotos(url);
                    }

                }
                else   //nb单灯
                {
                    var ctrlinfo = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.Get(TreeSelectCtrlId);
                    if (ctrlinfo == null) return;
                    var barcode = ctrlinfo.BarCodeId;



                    if (ctrlIndex123.ContainsKey(barcode) == false) return;
                    var index = ctrlIndex123[barcode];
                    if (Controller.Graphics.Count < index) return;

                    Controller.Graphics[index].Select();
                    MyMap.PanTo(Controller.Graphics[index].Geometry);
                    MyFeatureDataForm.FeatureLayer = Controller;
                    MyFeatureDataForm.GraphicSource = Controller.Graphics[index];

                    //显示图片  lvf 2019年4月11日16:09:33 
                    if (ChkShowPic.IsChecked == true)
                    {
                        if (Controller.Graphics[index].Attributes["photo"] == null) return;
                        var url = Controller.Graphics[index].Attributes["photo"].ToString();
                        if (string.IsNullOrEmpty(url)) return;
                        ShowPhotos(url);
                    }
                }
                return;
                //foreach (Graphic g in Controller.Graphics)
                //{
                //    if (g.Attributes[OnlyField] == null) continue;
                //    //遍历终端图层思密达
                //    if (g.Attributes[OnlyField].ToString().Trim() == barCode)
                //    {
                //        g.Select();
                //        MyMap.PanTo(g.Geometry);
                //        MyFeatureDataForm.FeatureLayer = Controller;
                //        MyFeatureDataForm.GraphicSource = g;

                //    }

                //    //else if (sid < 1600000000 && sid > 1500000000)
                //    //    {

                //    //            }
                //}
            }




        }
        public void AddFeatures(int sid)                     //添加设备
        {
            if (sid < 1100000 && sid > 1000000)
            {
                Graphic graphic = new Graphic();
                //GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
                //graphicsLayer.Graphics.Clear();
                MyGraphicsLayer.Graphics.Clear();
                graphic.Attributes.Add("NAME", "123");
                graphic.Attributes.Add("CODE", "123");
                //double Longitude = Convert.ToDouble(MyMap.Extent.GetCenter());
                //double Latitude = Convert.ToDouble(MyMap.Extent.GetCenter ());
                graphic.Geometry = Tml.FullExtent.GetCenter();//new MapPoint(Longitude, Latitude);
                graphic.Geometry.SpatialReference = MyMap.SpatialReference;

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
                return;
                if (sid < 1100000 && sid > 1000000)
                {
                    foreach (Graphic g in Tml.Graphics)          //修改终端图层思密达
                        if (Convert.ToInt32(g.Attributes["Lid"]) == sid)
                        {
                            //if (attributeName == StateField)
                            //{
                            //    aValue = g.Attributes["Type"] + aValue;
                            //}
                            //if (g.Attributes[attributeName] == aValue) return;
                            g.Attributes[attributeName] = Convert.ToInt16(aValue);
                            Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogInfo("arcMap:变图标 设备逻辑地址：" + sid + "改变状态为" + aValue);
                            break;
                        }
                        else
                        {

                        }
                    //Tml.SaveEdits();

                }
                else if (sid < 160000 && sid > 1500000)
                {
                    foreach (Graphic g in Concentrator.Graphics)          //修改集中器图层思密达
                        if (Convert.ToInt32(g.Attributes["LID"]) == sid)
                        {
                            //if (attributeName == StateField)
                            //{
                            //    aValue = g.Attributes["Type"] + aValue;

                            if (g.Attributes[attributeName] == aValue) return;
                            g.Attributes[attributeName] = Convert.ToInt16(aValue);
                        }
                    Concentrator.SaveEdits();
                }
            }
            catch { }

        }

        /// <summary>
        /// sluid    , 2、ctrlid  name-value
        /// </summary>
        /// <param name="sluId"></param>
        /// <param name="namevalue"></param>
        public void ModifyFeaturesAttribute2(object obj) //修改图元属性呀   int sluId,Dictionary< int ,Tuple< bool ,int >>  namevalue
        {

            var tu = obj as Tuple<int, Dictionary<int, Tuple<bool, int>>>;
            var sluId = tu.Item1;
            var namevalue = tu.Item2;

            if (namevalue == null || namevalue.Count == 0) return;
            //int ctrId;
            //int concentratorId;
            //ctrId = sid%1000;
            //concentratorId = Convert.ToInt32(sid/1000);
            foreach (Graphic g in Controller.Graphics) //遍历终端图层思密达
            {
                try
                {


                    int os = 100;

                    int sluIdinmap = Convert.ToInt32(g.Attributes["Bid"]);
                    if (sluIdinmap != sluId) continue;
                    int ctrlIdinmap = Convert.ToInt32(g.Attributes["Lid"]);
                    if (namevalue.ContainsKey(ctrlIdinmap))
                    {
                        if (!g.Attributes.ContainsKey("TYPE")) return;
                        var tmpType = g.Attributes["TYPE"];
                        if (tmpType == null || Convert.ToInt32(tmpType) < 100)
                        {
                            os = 100;   //100+单挑灯   200+双挑灯
                        }
                        else
                        {
                            os = Convert.ToInt32(g.Attributes["TYPE"].ToString());
                            os = os / 100 * 100;
                        }


                        //if (g.Attributes[namevalue[ctrlIdinmap].Item1] == namevalue[ctrlIdinmap].Item2.ToString())
                        //    continue;
                        var a = Convert.ToInt16(os) +
                            Convert.ToInt16(namevalue[ctrlIdinmap].Item2);
                        g.Attributes["TYPE"] = Convert.ToInt16(a);
                        namevalue[ctrlIdinmap] = new Tuple<bool, int>(false, namevalue[ctrlIdinmap].Item2);
                    }
                }
                catch (Exception ex)
                {

                }
            }
            Controller.SaveEdits();



        }

        /// <summary>
        /// sluid    , 2、 name-value : ctrlid errorindex
        /// </summary>
        /// <param name="sluId"></param>
        /// <param name="namevalue"></param>
        public void ModifyFeaturesAttributeCtrl(object obj) //修改图元属性呀  int sluId, Dictionary<int, int> namevalue
        {
            var tu = obj as Tuple<int, Dictionary<long, int>>;

            var namevalue = tu.Item2;
            //var sluId = tu.Item1;
            if (namevalue == null || namevalue.Count == 0) return;

            foreach (var g in namevalue)
            {
                try
                {
                    if (ctrlIndex123.ContainsKey(g.Key ) == false) continue;
                    var index = ctrlIndex123[g.Key ];
                    if (Controller.Graphics.Count < index) continue;
                    //var tmpType = Controller.Graphics[index].Attributes[StateField];
                    //int os = 100;
                    //if (tmpType == null || Convert.ToInt32(tmpType) < 100)
                    //{
                    //    os = 100;   //100+单挑灯   200+双挑灯
                    //}
                    //else
                    //{
                    //    os = Convert.ToInt32(tmpType);
                    //    os = os / 100 * 100;
                    //    os = os%1000;
                    //}
                    //var a = Convert.ToInt32(os) +
                    //      Convert.ToInt32(g.Value%10);
                    //Controller.Graphics[index].Attributes[StateField] = a+"";
                    var StatusNow = Controller.Graphics[index].Attributes[StateField]+"";
                    if (StatusNow == g.Value + "") continue;

                    //lvf  2019年1月7日10:07:53  原来其他模块不区分灯头，地图先做的，现在大家都支持灯头了，修改为统一状态为 2090 * 1000 + lampnum * 100 + code;
                    Controller.Graphics[index].Attributes[StateField] = g.Value+"";
                }
                catch (Exception)
                {
                    
                    throw;
                }
               
            }




            return;
            //foreach (Graphic g in Controller.Graphics) //遍历终端图层思密达
            //{
            //    try
            //    {


            //        int os = 100;

            //        int sluIdinmap = Convert.ToInt32(g.Attributes["Bid"]);
            //        if (sluIdinmap != sluId) continue;
            //        int ctrlIdinmap = Convert.ToInt32(g.Attributes["Lid"]);
            //        if (namevalue.ContainsKey(ctrlIdinmap))
            //        {
            //            if (!g.Attributes.ContainsKey("TYPE")) return;
            //            var tmpType = g.Attributes["TYPE"];
            //            if (tmpType == null || Convert.ToInt32(tmpType) < 100)
            //            {
            //                os = 100;   //100+单挑灯   200+双挑灯
            //            }
            //            else
            //            {
            //                os = Convert.ToInt32(g.Attributes["TYPE"].ToString());
            //                os = os / 100 * 100;
            //            }


            //            //if (g.Attributes[namevalue[ctrlIdinmap].Item1] == namevalue[ctrlIdinmap].Item2.ToString())
            //            //    continue;
            //            var a = Convert.ToInt16(os) +
            //                Convert.ToInt16(namevalue[ctrlIdinmap]);
            //            g.Attributes["TYPE"] = Convert.ToInt16(a);
            //        }
            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //}
            //Controller.SaveEdits();



        }


        public void ModifyFeaturesAttribute3(object obj) //修改图元属性呀
        {

            try
            {

                foreach (var g in tmlIcon)
                {
                    //判断是否需要更改图标
                    if (g.Value.Item1 == false) continue;
                    var para = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(g.Key);
                    if (para == null || para.EquipmentType != WjParaBase.EquType.Rtu) continue;
                    var phyid=  para.RtuPhyId+"";
                    if(tmlIndex.ContainsKey(phyid)==false ) continue;
                    var index = tmlIndex[phyid];
                    Tml.Graphics[index].Attributes[StateField] = g.Value.Item2 + "";
                    
                    tmlIcon[g.Key] = new Tuple<bool, string>(false, tmlIcon[g.Key].Item2);

                    
                }



                //变化线路图标 todo lvf 2019年4月18日16:52:40
                //EquipmentRtuId * 1000L + index
                foreach (var k in LineIcon)
                {
                    if (k.Value.Item1 == false) continue;
                    var para = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(k.Key.Item1);
                    if (para == null || para.EquipmentType != WjParaBase.EquType.Rtu) continue;
                    var phyid = para.RtuPhyId ;

                    var tu = new Tuple<int,int>(phyid,k.Key.Item2);

                    if (LineIndex.ContainsKey(tu))
                    {
                        //线路变色
                        foreach (var l in LineIndex[tu])
                        {
                            //var indexx = LineIndex[tu];
                            var status = k.Value.Item2 ? "1" : "0";

                            Line.Graphics[l].Attributes[StateField] = status;

                            LineIcon[k.Key] = new Tuple<bool, bool>(false, LineIcon[k.Key].Item2);

                        }
                        //灯杆变色
                        if (ctrlLine.ContainsKey(tu))
                        {
                            foreach (var g in ctrlLine[tu])
                            {

                                var statuss = k.Value.Item2 ? "1" : "0";
                                if (Controller.Graphics[g].Attributes[StateField] == null || string.IsNullOrEmpty(Controller.Graphics[g].Attributes[StateField].ToString().Trim())) continue;
                                var stt = Controller.Graphics[g].Attributes[StateField].ToString().Trim();
                                if (string.IsNullOrEmpty(stt) == false)
                                {
                                    if (stt.Length > 3)
                                    {
                                        var statusTmp = stt.Substring(stt.Length - 3, 3);
                                        var intStatusTmp = Convert.ToInt32(statusTmp);
                                        if (intStatusTmp > 200)
                                        {
                                            statuss = k.Value.Item2 ? "2090207" : "2090201";
                                        }
                                        else
                                        {
                                            statuss = k.Value.Item2 ? "2090103" : "2090101";
                                        }
                                    }
                                    else
                                    {
                                        statuss = k.Value.Item2 ? "2090103" : "2090101";
                                    }

                                    Controller.Graphics[g].Attributes[StateField] = statuss;
                                }

                            }

                        }
                        

                    }

                }




                //foreach (Graphic g in Tml.Graphics)          //修改终端图层思密达
                //{
                //    if (tmlIcon.Count == 0) return;
                //    if (g.Attributes[OnlyField] == null) continue;
                //    var lidttt = g.Attributes[OnlyField].ToString().Trim();
                //    if (lidttt == "") continue;
                //    var phyId = Convert.ToInt32(lidttt);
                //    var lid = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetLidByPhyId(phyId,
                //                                                                                        WjParaBase.
                //                                                                                            EquType.Rtu);
                //    if (lid == 0) continue;
                //    if (!tmlIcon.ContainsKey(lid)) continue;
                //    if (tmlIcon[lid].Item1 == false) continue; //不用更新

                //    if (g.Attributes.ContainsKey(StateField) == false) continue;

                //    g.Attributes[StateField] = tmlIcon[lid].Item2 + "";
             
                //}

                //Tml.SaveEdits();

                //}
                //else if (lid < 160000 && lid > 1500000)
                //{
                //    foreach (Graphic g in Concentrator.Graphics)          //修改集中器图层思密达
                //    {
                //        if (!tmlIcon.ContainsKey(lid)) continue;
                //        if (tmlIcon[lid].Item1 == false) continue;//不用更新

                //        int os = Convert.ToInt16(g.Attributes[attributeName]);
                //        if (os <= 6 || os > 26)
                //        {
                //            g.Attributes[attributeName] = Convert.ToInt16(tmlIcon[lid].Item2);
                //            tmlIcon[lid] = new Tuple<bool, string>(false, tmlIcon[lid].Item2);
                //        }
                //        else
                //        {
                //            var a = Convert.ToInt16((os / 10) * 10 + Convert.ToInt16(tmlIcon[lid].Item2));
                //            g.Attributes[attributeName] = Convert.ToInt16(a);
                //            tmlIcon[lid] = new Tuple<bool, string>(false, tmlIcon[lid].Item2);
                //        }
                //    }
                //}



            }
            catch(Exception ex)
            {
                WriteLog.WriteLogError(ex + "");
            }

        }


        public void FilterSth(string condition)
        {

        } //分析过滤
        string[] files;
        int n = 0;
        List<BitmapImage> bit = new List<BitmapImage> { };

       /// <summary>
       /// 显示图片
       /// </summary>
       /// <param name="url"></param>
        public void ShowPhotos(string url)
       {
           if (ChkShowPic.IsChecked == false) return;
            TmlImage.Source = null;
            var urltmp = url.Replace("\\", "/");
            var picPath = ServiceIP +"/"+ urltmp;

            //picPath = "http://pic15.nipic.com/20110628/1369025_192645024000_2.jpg";
            BitmapImage bi = new BitmapImage();
            // BitmapImage.UriSource must be in a BeginInit/EndInit block.
            bi.BeginInit();
            bi.UriSource = new Uri(@picPath, UriKind.RelativeOrAbsolute);
            bi.EndInit();
            TmlImage.Source = bi;


       }


        public void ShowPic(int lid)//显示图片
        {
            return;
            TmlImage.Source = null;
            var rtu = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(lid);//.GetEquipmentInfo(lid);
            if (rtu == null)
            {
                ShowPicSp.Visibility = Visibility.Hidden;
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
                ShowPicSp.Visibility = Visibility.Hidden;
                return;
            }
            files = Directory.GetFiles(path);// , "*.png",SearchOption.AllDirectories

            if (files == null)
            {
                ShowPicSp.Visibility = Visibility.Hidden;
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
                    ShowPicSp.Visibility = Visibility.Visible;
                }
                else
                {
                    ShowPicSp.Visibility = Visibility.Hidden;
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
                    TmlImage.Source = bit[0];
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

            if (n + 1 > 1)
            {
                n--;
            }
            else n = bit.Count - 1;
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


        }
        #endregion

        [Import]
        public IIMapGis Model
        {
            get { return DataContext as IIMapGis; }
            set
            {
                DataContext = value;
            }
        }

        private void chkShowAttr_Click(object sender, RoutedEventArgs e)
        {
            if (chkEdit.IsChecked == true)
            {
                MyFeatureDataForm.IsReadOnly = false;
            }
            else
            {
                MyFeatureDataForm.IsReadOnly = true;
            }
            if (chkShowAttr.IsChecked == true)
            {
                MyFeatureDataForm.Visibility = Visibility.Visible;

                //if (Tml.SelectionCount == 1)
                //{
                //    int x;
                //    var tmp = Tml.SelectedGraphics.ToList()[0].Attributes["Lid"] + "";
                //    if (Int32.TryParse(tmp, out x))
                //    {
                //        ShowPic(x);
                //    }
                //}
                //showPic.Visibility = Visibility.Visible;
                BindButton.IsEnabled = true;

            }
            else
            {
                MyFeatureDataForm.Visibility = Visibility.Hidden;
                //showPic.Visibility = Visibility.Hidden;
                BindButton.IsEnabled = false;
            }
        }


        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            foreach (var g in MyMap.Layers)
            {
                g.Initialize();
            }
            Tml.Refresh();
            Concentrator.Refresh();
            Controller.Refresh();
        }

        private void MyMap_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //UseAcceleratedDisplay="True" 为false 才能用聚合
            return;
            if (e.PropertyName == "SpatialReference")
            {
                LoadGraphics();
                MyMap.PropertyChanged -= MyMap_PropertyChanged;
            }
        }
        private void LoadGraphics()
        {
            QueryTask queryTask =
                new QueryTask(Tml.Url);
            queryTask.ExecuteCompleted += queryTask_ExecuteCompleted;

            Query query = new ESRI.ArcGIS.Client.Tasks.Query();
            query.OutSpatialReference = MyMap.SpatialReference;
            query.ReturnGeometry = true;
            query.Where = "1=1";
            queryTask.ExecuteAsync(query);
        }


        private void queryTask_ExecuteCompleted(object sender, QueryEventArgs args)
        {
            FeatureSet featureSet = args.FeatureSet;

            if (featureSet == null || featureSet.Features.Count < 1)
            {
                MessageBox.Show("No features retured from query");
                return;
            }

            //GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer1"] as GraphicsLayer;
            glayer.ClearGraphics();
            foreach (Graphic graphic in featureSet.Features)
            {
                graphic.Symbol =
                    LayoutRoot.Resources["MediumMarkerSymbol"] as ESRI.ArcGIS.Client.Symbols.SimpleMarkerSymbol;
                glayer.Graphics.Add(graphic);
            }
        }

        //显示照片控件
        private void chkShowPic_Click(object sender, RoutedEventArgs e)
        {
            if (ChkShowPic.IsChecked == true)
            {
                ShowPicSp.Visibility = Visibility.Visible;
            }
            else
            {
                ShowPicSp.Visibility = Visibility.Collapsed;
            }

        }

        //private void chkShowON_Click(object sender, RoutedEventArgs e)
        //{
        //    GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
        //    graphicsLayer.Graphics.Clear();
        //    if (chkShowON.IsChecked == true)
        //    {
        //        if (Tml.Visible == true)
        //        {
        //            int err = chkErr.IsChecked == true ? 4 : 3;
        //            foreach (Graphic g in Tml.Graphics)
        //            {

        //                int x;
        //                var tmp = g.Attributes[StateField] + "";
        //                if (Int32.TryParse(tmp, out x))
        //                {
        //                    if (x == err) //Convert.ToInt32(g.Attributes[StateField]) > 2
        //                    {
        //                        Graphic gr = new Graphic
        //                        {
        //                            Geometry = g.Geometry,
        //                            Symbol =
        //                                LayoutRoot.Resources["ErrTml"] as
        //                                ESRI.ArcGIS.Client.Symbols.Symbol,
        //                            // newSymbol is a SimpleMarkerSymbol (point)
        //                        };
        //                        graphicsLayer.Graphics.Add(gr);
        //                    }
        //                }
        //            }
        //        }
        //        //if (Concentrator.Visible == true)
        //        //{
        //        //    foreach (Graphic g in Concentrator.Graphics)
        //        //    {
        //        //        if (Convert.ToInt32(g.Attributes[StateField]) > 3)
        //        //        {
        //        //            Graphic gr = new Graphic
        //        //            {
        //        //                Geometry = g.Geometry,
        //        //                Symbol = LayoutRoot.Resources["ErrConcentrator"] as ESRI.ArcGIS.Client.Symbols.Symbol, // newSymbol is a SimpleMarkerSymbol (point)
        //        //            };
        //        //            graphicsLayer.Graphics.Add(gr);
        //        //        }
        //        //    }
        //        //}
        //        if (Controller.Visible == true)
        //        {
        //            List<string> err = chkErr.IsChecked == true
        //                                   ? new List<string>() { "04", "06", "08", "14" }
        //                                   : new List<string>() { "03", "05", "07", "04", "06", "08", "13", "14" };
        //            foreach (Graphic g in Controller.Graphics)
        //            {
        //                int x;
        //                var tmp = g.Attributes["TYPE"] + "";
        //                //if (Int32.TryParse(tmp, out x))
        //                //{
        //                var cl = tmp.Substring(tmp.Length - 2);
        //                if (err.Contains(cl))
        //                {
        //                    Graphic gr = new Graphic
        //                    {
        //                        Geometry = g.Geometry,
        //                        Symbol =
        //                            LayoutRoot.Resources["ErrTml"] as
        //                            ESRI.ArcGIS.Client.Symbols.Symbol,
        //                        // newSymbol is a SimpleMarkerSymbol (point)
        //                    };
        //                    graphicsLayer.Graphics.Add(gr);
        //                }
        //                //}
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (chkErr.IsChecked == false) return;
        //        if (Tml.Visible == true)
        //        {
        //            int err = 2;
        //            foreach (Graphic g in Tml.Graphics)
        //            {

        //                int x;
        //                var tmp = g.Attributes[StateField] + "";
        //                if (Int32.TryParse(tmp, out x))
        //                {
        //                    if (x == err) //Convert.ToInt32(g.Attributes[StateField]) > 2
        //                    {
        //                        Graphic gr = new Graphic
        //                        {
        //                            Geometry = g.Geometry,
        //                            Symbol =
        //                                LayoutRoot.Resources["ErrTml"] as
        //                                ESRI.ArcGIS.Client.Symbols.Symbol,
        //                            // newSymbol is a SimpleMarkerSymbol (point)
        //                        };
        //                        graphicsLayer.Graphics.Add(gr);
        //                    }
        //                }
        //            }
        //        }
        //        //if (Concentrator.Visible == true)
        //        //{
        //        //    foreach (Graphic g in Concentrator.Graphics)
        //        //    {
        //        //        if (Convert.ToInt32(g.Attributes[StateField]) > 3)
        //        //        {
        //        //            Graphic gr = new Graphic
        //        //            {
        //        //                Geometry = g.Geometry,
        //        //                Symbol = LayoutRoot.Resources["ErrConcentrator"] as ESRI.ArcGIS.Client.Symbols.Symbol, // newSymbol is a SimpleMarkerSymbol (point)
        //        //            };
        //        //            graphicsLayer.Graphics.Add(gr);
        //        //        }
        //        //    }
        //        //}
        //        if (Controller.Visible == true)
        //        {
        //            var err = new List<string>() { "2", "4", "6", "8" }; ;
        //            foreach (Graphic g in Controller.Graphics)
        //            {
        //                int x;
        //                var tmp = g.Attributes["TYPE"] + "";
        //                //if (Int32.TryParse(tmp, out x))
        //                //{
        //                var cl = tmp.Substring(tmp.Length - 1);
        //                if (err.Contains(cl))
        //                {
        //                    Graphic gr = new Graphic
        //                    {
        //                        Geometry = g.Geometry,
        //                        Symbol =
        //                            LayoutRoot.Resources["ErrTml"] as
        //                            ESRI.ArcGIS.Client.Symbols.Symbol,
        //                        // newSymbol is a SimpleMarkerSymbol (point)
        //                    };
        //                    graphicsLayer.Graphics.Add(gr);
        //                }
        //                //}
        //            }
        //        }
        //    }
        //}

        //private void CmdExport_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        var lsttitle = new List<Object>();
        //        lsttitle.Add("序号");
        //        lsttitle.Add("终端地址");
        //        lsttitle.Add("终端名称");
        //        lsttitle.Add("故障回路");
        //        lsttitle.Add("故障名称");
        //        lsttitle.Add("发生时间");
        //        lsttitle.Add("备注");
        //        //if (IsOldFaultQuery) 
        //        lsttitle.Add("电流上下限备注");


        //        var lstobj = new List<List<object>>();

        //        //foreach (var g in Records)
        //        //{
        //        //    var tmp = new List<object>();
        //        //    tmp.Add(g.Index);
        //        //    tmp.Add(g.PhyId);
        //        //    tmp.Add(g.RtuName);
        //        //    tmp.Add(g.RtuLoopName);
        //        //    tmp.Add(g.FaultName);
        //        //    tmp.Add(g.DtCreateTime);
        //        //    if (IsOldFaultQuery) tmp.Add(g.DtRemoceTime);
        //        //    tmp.Add(g.Remark);
        //        //    //if (IsOldFaultQuery)
        //        //    //{

        //        //    //}

        //        //    lstobj.Add(tmp);
        //        //}
        //        //Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(lsttitle, lstobj);
        //        //lstobj = null;
        //        //lsttitle = null;
        //    }
        //    catch (Exception ex)
        //    {
        //        Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("导出报表时报错:" + ex);
        //    }
        //}









    }


    public partial class MapGis
    {
        public void InitEvent()
        {
            EventPublish.AddEventTokener(
                Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandlers, FundOrderFilters);

            EventPublish.AddEventTokener(
    Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandlers1, FundOrderFilters1, false);

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
            ModifyFeaturesAttribute3(StateField);






        }

        #region IEventAggregator Subscription

        public void FundEventHandlers(Wlst.Cr.Coreb.EventHelper.PublishEventArgs args)
        {
            try
            {
                if (args.EventType == PublishEventType.Core)
                {
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipentxyPositonUpdateId)
                    {

                    }
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentAddEventId)
                    {

                    }
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentDeleteEventId)
                    {

                    }

                    if (args.EventId == EventIdAssign.EquipmentUpdateEventId)
                    {
                        var lst = args.GetParams()[0] as List<Tuple<int, int>>;
                        if (lst == null || lst.Count == 0) return;
                        foreach (var t in lst)
                        {
                            if (t.Item2 == 0)
                                OnDataChanged(t.Item1);
                        }
                        Wlst.Cr.Core.CoreServices.RegionManage.DispatcherInvoke(ModifyFeaturesAttribute3, StateField);
                    }
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                    {

                        if (Convert.ToString(args.EventAttachInfo) == "gis") return;
                        int x = Convert.ToInt32(args.GetParams()[0]);//终端、集中器逻辑地址
                        TreeSelectId = x;
                        TreeSelectCtrlId = -1;
                        if (args.GetParams().Count > 1)
                        {


                            TreeSelectCtrlId = Convert.ToInt32(args.GetParams()[1]);//控制器序号


                            //x=x*1000+y;
                            //TreeSelectId = x;
                        }
                        LocateSth(x, TreeSelectCtrlId);
                        ShowPic(x);

                    }

                    if (args.EventId == Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuGroupSelectdWantedMapUp)
                        try
                        {
                            var lst = args.GetParams()[0] as List<int>;
                            if (lst == null) return;
                            if (lst.Count == 0)
                            {

                                Tml.Where = "";
                                Tml.Update();
                                needOnErrChanged = true;

                                //GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;


                            }
                            else
                            {
                                lst.Add(0);
                                string idt = "";
                                foreach (var rtuid in lst)
                                {
                                    var info =
                                        Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(rtuid);

                                    if (info == null) continue;
                                    var phyid =info.RtuPhyId;
                                    idt = idt + phyid + ",";

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
                                string temp = OnlyField+" in (" + idt + ")";
                                Tml.Where = temp;
                                Tml.Update();
                                needOnErrChanged = true;
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
                WriteLog.WriteLogError("MapGis error in FundEventHandlers:ex:" + xe);
            }
        }

        private void FundEventHandlers1(PublishEventArgs args)
        {
            try
            {


                if (args.EventType == PublishEventType.Core)
                {

                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.RunningInfoUpdate1)
                    {
                        //todo Sr.EquipemntLightFault.Services.EventIdAssign.RtuErrorStateChanged)
                        try
                        {
                            var lst = args.GetParams()[0] as IEnumerable<int>;
                            if (lst == null) return;
                            //    this.ReUpdateLoadChild();

                            foreach (var t in lst)
                            {
                                OnDataChanged(t);
                            }
                            Wlst.Cr.Core.CoreServices.RegionManage.DispatcherInvoke(ModifyFeaturesAttribute3, StateField);
                            //ModifyFeaturesAttribute3(StateField);
                        }
                        catch (Exception ex)
                        {
                            Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
                        }
                    }
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.RunningInfoUpdate2)//Wj2090Module.Services.EventIdAssign. && args.EventType == PublishEventType.Core) //单灯最新数据
                    {
                        
                        if (args.GetParams().Count < 2)
                        {
                            try
                            {
                                var lst = args.GetParams()[0] as IEnumerable<int>;
                                if (lst == null) return;
                                //    this.ReUpdateLoadChild();
                                foreach (var t in lst)
                                {
                                    OnDataChanged(t);
                                }
                                Wlst.Cr.Core.CoreServices.RegionManage.DispatcherInvoke(ModifyFeaturesAttribute3, StateField);
                                //ModifyFeaturesAttribute3(StateField);
                            }
                            catch (Exception ex)
                            {
                                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
                            }
                        }
                        else //单灯
                        {
                            //return;
                            var lst = args.GetParams()[0] as List<int>;
                            var ctrlList = args.GetParams()[1] as List<int>;
                            if (lst == null) return;
                            if (ctrlList == null) return;

                            var sluid = lst[0];

                            Ac(sluid, ctrlList);
                        }


                    }
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.RunningInfoUpdate3)//Wj2090Module.Services.EventIdAssign. && args.EventType == PublishEventType.Core) //单灯最新数据
                    {


                        var lst = args.GetParams()[0] as List<long>;
                        if (lst == null) return;
                        Ac1(lst);



                    }


                }
            }
            catch (Exception ex)
            {
            }
        }
        void Ac1(List<long> ctrlList)
        {
            var dic = new Dictionary<long, int>();
            foreach (var f in ctrlList )
            {
                int imagecode = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetCtrlImageCode(f);
                if (dic.ContainsKey(f) == false) dic.Add(f, imagecode);
            }
            //var namevv = Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetCtrlIcon(sluid, ctrlList);
            Wlst.Cr.Core.CoreServices.RegionManage.DispatcherInvoke(ModifyFeaturesAttributeCtrl,
                                                                    new Tuple<int, Dictionary<long, int>>(0,
                                                                                                         dic));
        }

        void Ac(int sluid, List<int> ctrlList)
        {
            return;
            var dic = new Dictionary<long, int>();
            if (ctrlList.Count == 0)
            {
                var tmpp = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[sluid] as Wlst.Sr.EquipmentInfoHolding.Model.Wj2090Slu;
                foreach (var g in tmpp.WjSluCtrls.Values)
                {
                    ctrlList.Add(g.CtrlId);
                }
            }
            foreach (var f in ctrlList)
            {
                long idddd = 0;
                idddd = sluid * 10000L + f;
                int imagecode = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetCtrlImageCode(idddd);
                if (dic.ContainsKey(f) == false) dic.Add(idddd, imagecode);
            }
            //var namevv = Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetCtrlIcon(sluid, ctrlList);
            Wlst.Cr.Core.CoreServices.RegionManage.DispatcherInvoke(ModifyFeaturesAttributeCtrl,
                                                                    new Tuple<int, Dictionary<long, int>>(0,
                                                                                                    dic));


       


            return;
            if (ctrlList.Count == 0)
            {
                var tmpp = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[sluid] as Wlst.Sr.EquipmentInfoHolding.Model.Wj2090Slu;
                foreach (var g in tmpp.WjSluCtrls.Values)
                {
                    ctrlList.Add(g.CtrlId);
                }
            }


            if (true)//IsNewCtrlIconSystem
            {
                var namevv = Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetCtrlIcon(sluid, ctrlList);
                Wlst.Cr.Core.CoreServices.RegionManage.DispatcherInvoke(ModifyFeaturesAttributeCtrl, new Tuple<int, Dictionary<int, int>>(sluid, namevv));
                //ModifyFeaturesAttributeCtrl(sluid, namevv);
            }
            else
            {
                var data = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(sluid);
                if (data == null) return;

                var errors =
                    Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultLstInfoByRtuId(
                        sluid);

                Dictionary<int, Tuple<bool, int>> namevalue =
                    new Dictionary<int, Tuple<bool, int>>();

                foreach (var ctrlid in ctrlList)
                {
                    int errorindex = 0;
                    if (data.SluCtrlNewData.ContainsKey(ctrlid) == false ||
                        data.SluCtrlNewData[ctrlid].Data5 == null ||
                        data.SluCtrlNewData[ctrlid].Data5.Info == null ||
                        data.SluCtrlNewData[ctrlid].Data5.Info.Status == 3 ||
                        data.SluCtrlNewData[ctrlid].Data5.Info.DateTimeCtrl < 10)
                    {

                        if (!namevalue.ContainsKey(ctrlid))
                        {
                            namevalue.Add(ctrlid, new Tuple<bool, int>(true, errorindex));
                        }
                        else
                        {
                            if (namevalue[ctrlid].Item2 == errorindex)
                            {
                                namevalue[ctrlid] = new Tuple<bool, int>(false, errorindex);
                            }
                            else
                            {
                                namevalue[ctrlid] = new Tuple<bool, int>(true, errorindex);
                            }

                        }
                        continue;
                    }

                    bool hasError = false;
                    if (UxTreeSetting.IsRutsNotShowError == false)
                    {
                        hasError = (from t in errors where t.LoopId == ctrlid select t).ToList().Count > 0;
                    }

                    var ctrldata = data.SluCtrlNewData[ctrlid].Data5;
                    bool isLight = false;

                    if (ctrldata.Items != null)
                        foreach (var l in ctrldata.Items)
                        {
                            if (l.Current > 0.01) isLight = true;
                        }

                    if (hasError && isLight) errorindex = 12; //开灯有故障
                    if (hasError && isLight == false) errorindex = 13; //关灯有故障
                    if (hasError == false && isLight) errorindex = 10; //正常开灯
                    if (hasError == false && isLight == false) errorindex = 11; //正常关灯

                    if (!namevalue.ContainsKey(ctrlid))
                    {
                        namevalue.Add(ctrlid, new Tuple<bool, int>(true, errorindex));
                    }
                    else
                    {
                        if (namevalue[ctrlid].Item2 == errorindex)
                        {
                            namevalue[ctrlid] = new Tuple<bool, int>(false, errorindex);
                        }
                        else
                        {
                            namevalue[ctrlid] = new Tuple<bool, int>(true, errorindex);
                        }

                    }

                }
                Wlst.Cr.Core.CoreServices.RegionManage.DispatcherInvoke(ModifyFeaturesAttribute2, new Tuple<int, Dictionary<int, Tuple<bool, int>>>(sluid, namevalue));
                //ModifyFeaturesAttribute2(sluid, namevalue);
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

                    //if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.RunningInfoUpdate1)
                    //{
                    //    return true;
                    //}
                    //if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.RunningInfoUpdate2)
                    //{
                    //    return true;
                    //}


                    if (args.EventId == Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuGroupSelectdWantedMapUp)
                        return true;
                    //if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.MapNeedChangeIcon)  //取消 单灯数据反馈改进，单灯状态跟随自己数据（RunningInfoUpdate2）
                    //    return true;

                }
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
            return false;
        }

        public bool FundOrderFilters1(PublishEventArgs args) //接收终端选中变更事件
        {
            try
            {

                if (args.EventType == PublishEventType.Core)
                {
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.RunningInfoUpdate1)
                        return true;
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.RunningInfoUpdate2)
                        return true;
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.RunningInfoUpdate3)
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
        /// 图标变色
        /// rtuid
        /// errorindex
        /// 
        /// </summary>
        private ConcurrentDictionary<int, Tuple<bool, string>> tmlIcon = new ConcurrentDictionary<int, Tuple<bool, string>>();
        private ConcurrentDictionary<string, int> tmlIndex = new ConcurrentDictionary<string , int>();
        private ConcurrentDictionary<long, int> ctrlIndex123 = new ConcurrentDictionary<long, int>();
        private ConcurrentDictionary<Tuple<int, int>, List<int>> LineIndex = new ConcurrentDictionary<Tuple<int, int>, List<int>>();
        private ConcurrentDictionary<string, int> sluIndex = new ConcurrentDictionary<string, int>();
        private ConcurrentDictionary<Tuple<int,int>, Tuple<bool, bool>> LineIcon = new ConcurrentDictionary<Tuple<int,int>, Tuple<bool, bool>>();
        private ConcurrentDictionary<Tuple<int, int>, List<int>> ctrlLine = new ConcurrentDictionary<Tuple<int, int>, List<int>>();

        public void OnErrChanged(int EquipmentRtuId)
        {
            if (
                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey( //).EquipmentInfoDictionary.ContainsKey(
                    EquipmentRtuId))
            {
                var equ =
                    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[//.EquipmentInfoDictionary[
                        EquipmentRtuId];
                if (equ.EquipmentType == WjParaBase.EquType.Rtu)// 3005 || equ.RtuModel == 3090)
                {
                    int errorindex = 1; //宜兴 绿色 路灯 关灯正常状态


                    var s = equ.RtuStateCode;

                    //lvf 2018年11月19日14:10:04 区别亮化
                    var tpye = equ as Wj3005Rtu;
                    if (tpye == null) return;
                    var rtuUseType = tpye.WjVoltage.RtuUsedType;
                    int rtuType = 0;
                    if (rtuUseType == 1) rtuType = 0;
                    if (rtuUseType == 2) rtuType = 10; //如果  是亮化 基数+10


                    if (s == 0)
                    {
                        //     EquipmentImageState = 3011; //s
                        //todi  change EquipmentRtuId  picture
                        //ModifyFeaturesAttribute1(EquipmentRtuId, StateField, "0"); //不用
                        //Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogInfo("SystemReceive变图标 设备逻辑地址：" + EquipmentRtuId +
                        //                                                   "改变状态为" + 0);
                        //return;
                        errorindex = 5;

                    }
                    if (s == 1)
                    {
                        //      EquipmentImageState = 3012;
                        //ModifyFeaturesAttribute1(EquipmentRtuId, StateField, "0"); //停用
                        //Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogInfo("SystemReceive变图标 设备逻辑地址：" + EquipmentRtuId +
                        //                                                   "改变状态为" + 0);
                        //return;

                        errorindex = 6;
                    }
                    var rtuRunInfo = Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(EquipmentRtuId);
                    if (rtuRunInfo != null)
                    {
                        if (rtuRunInfo.IsOnLine == false)
                        {
                            errorindex = 5;
                        }
                        else
                        {
                            var haserror =
                                rtuRunInfo.ErrorCount > 0;
                            // Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultLstInfoByRtuId(EquipmentRtuId);//.IsRtuHasError(EquipmentRtuId);
                            var lighton =
                                Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(EquipmentRtuId).
                                    IsLightHasElectric; //RtuNewDataService.IsRtuHasElectric(EquipmentRtuId);
                            string nname = equ.RtuName;

                            if (haserror && !lighton) errorindex = 2; //宜兴 绿色 路灯 关灯故障
                            if (!haserror && lighton) errorindex = 3; //宜兴 绿色 路灯 正常亮灯状态
                            if (haserror && lighton) errorindex = 4; //宜兴 绿色 路灯 亮灯故障

                        }

                        //////////////////if (errorindex == "1")
                        //////////////////{
                        //////////////////    if (nname.Contains("亮化"))
                        //////////////////    {
                        //////////////////        errorindex = "5"; //宜兴 灰白 亮化 正常关灯状态
                        //////////////////    }
                        //////////////////}
                        //////////////////if (errorindex == "2")
                        //////////////////{
                        //////////////////    if (nname.Contains("亮化"))
                        //////////////////    {
                        //////////////////        errorindex = "6"; //宜兴 灰白 亮化 关灯故障
                        //////////////////    }
                        //////////////////}
                        //////////////////if (errorindex == "3")
                        //////////////////{
                        //////////////////    if (nname.Contains("亮化"))
                        //////////////////    {
                        //////////////////        errorindex = "7"; //宜兴 灰白 亮化 正常亮灯状态
                        //////////////////    }
                        //////////////////}
                        //////////////////if (errorindex == "4")
                        //////////////////{
                        //////////////////    if (nname.Contains("亮化"))
                        //////////////////    {
                        //////////////////        errorindex = "8"; //宜兴 灰白 亮化 亮灯故障
                        //////////////////    }
                        //////////////////}
                    }


                    if (tmlIcon.ContainsKey(EquipmentRtuId) == false)
                    {
                        tmlIcon.TryAdd(EquipmentRtuId, new Tuple<bool, string>(true, rtuType+errorindex+""));
                    }
                    else
                    {

                        if (tmlIcon[EquipmentRtuId].Item2 == rtuType + errorindex + "")
                        {
                            tmlIcon[EquipmentRtuId] = new Tuple<bool, string>(false, rtuType + errorindex + "");
                        }
                        else
                        {
                            tmlIcon[EquipmentRtuId] = new Tuple<bool, string>(true, rtuType + errorindex + "");
                        }

                    }
                    //Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogInfo("SystemReceive变图标 设备逻辑地址：" + EquipmentRtuId + "改变状态为" + errorindex);
                    //ModifyFeaturesAttribute1(EquipmentRtuId, StateField, errorindex);
                    //var tmps=  Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetRtuErrorsByRtuId(EquipmentRtuId);
                    //  EquipmentImageState = 3015 + errorindex;


                }
                else if (equ.EquipmentType == WjParaBase.EquType.Slu)
                {

                    Ac(EquipmentRtuId, new List<int>());
          


                }

            }
        }

        public void OnDataChanged(int EquipmentRtuId)
        {

          
            if (
                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey( //).EquipmentInfoDictionary.ContainsKey(
                    EquipmentRtuId))
            {
                var equ =
                    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[ //.EquipmentInfoDictionary[
                        EquipmentRtuId];
                if (equ.EquipmentType == WjParaBase.EquType.Rtu) // 3005 || equ.RtuModel == 3090)
                {
                    int errorindex =1; //宜兴 绿色 路灯 关灯正常状态
                    var s = equ.RtuStateCode;

                    //lvf 2018年11月19日14:10:04 区别亮化
                    var tpye = equ as Wj3005Rtu;
                    if (tpye == null) return;
                    var rtuUseType = tpye.WjVoltage.RtuUsedType;
                    int rtuType = 0;
                    if (rtuUseType == 1) rtuType = 0;
                    if (rtuUseType == 2) rtuType = 10; //如果  是亮化 基数+10


                    if (s == 0 )  //不用
                    {
                        errorindex =7;

                        if (tmlIcon.ContainsKey(EquipmentRtuId) == false)
                        {
                            tmlIcon.TryAdd(EquipmentRtuId, new Tuple<bool, string>(true, errorindex + rtuType + ""));
                        }
                        else
                        {
                            tmlIcon[EquipmentRtuId] = new Tuple<bool, string>(true, errorindex + rtuType + "");
                        }
                        return;

                    }
                    if(s==1)//停运
                    {
                        errorindex = 6;

                        if (tmlIcon.ContainsKey(EquipmentRtuId) == false)
                        {
                            tmlIcon.TryAdd(EquipmentRtuId, new Tuple<bool, string>(true, errorindex + rtuType + ""));
                        }
                        else
                        {
                            tmlIcon[EquipmentRtuId] = new Tuple<bool, string>(true, errorindex + rtuType + "");
                        }
                        return;
                    }
                    //if (s == 1)
                    //{
                    //   errorindex = "1";
                    //   if (tmlIcon.ContainsKey(EquipmentRtuId) == false)
                    //   {
                    //       tmlIcon.TryAdd(EquipmentRtuId, new Tuple<bool, string>(true, errorindex));
                    //   }
                    //   else
                    //   {
                    //       tmlIcon[EquipmentRtuId] = new Tuple<bool, string>(true, errorindex);
                    //   }
                    //   return;
                    //}
                    var rtuRunInfo = Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(EquipmentRtuId);
                    if (rtuRunInfo != null)
                    {
                        





                        if ( rtuRunInfo.IsOnLine ==false )
                        {
                            errorindex = 5;
                        }
                        else
                        {
                            var haserror =
                                rtuRunInfo.ErrorCount > 0;
                            // Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultLstInfoByRtuId(EquipmentRtuId);//.IsRtuHasError(EquipmentRtuId);
                            var lighton =
                                Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(EquipmentRtuId).
                                    IsLightHasElectric; //RtuNewDataService.IsRtuHasElectric(EquipmentRtuId);
                            string nname = equ.RtuName;

                            if (haserror && !lighton) errorindex = 2; //宜兴 绿色 路灯 关灯故障
                            if (!haserror && lighton) errorindex = 3; //宜兴 绿色 路灯 正常亮灯状态
                            if (haserror && lighton) errorindex = 4; //宜兴 绿色 路灯 亮灯故障
                        }


                    }


                    if (tmlIcon.ContainsKey(EquipmentRtuId) == false)
                    {
                        tmlIcon.TryAdd(EquipmentRtuId, new Tuple<bool, string>(true, errorindex + rtuType + ""));
                    }
                    else
                    {
                        tmlIcon[EquipmentRtuId] = new Tuple<bool, string>(true, errorindex + rtuType + "");
                    }
                    //Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogInfo("SystemReceive变图标 设备逻辑地址：" + EquipmentRtuId + "改变状态为" + errorindex);
                    //ModifyFeaturesAttribute1(EquipmentRtuId, StateField, errorindex);
                    //var tmps=  Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetRtuErrorsByRtuId(EquipmentRtuId);
                    //  EquipmentImageState = 3015 + errorindex;



                    //记录线缆状态  lvf 2019年4月18日14:14:29
                    if (rtuRunInfo.RtuNewData == null) return;
                    var index = 0;
                    foreach (var g in rtuRunInfo.RtuNewData.IsSwitchOutAttraction)
                    {
                        index++;
                      
                        var tukey= new Tuple<int,int>(EquipmentRtuId,index);
                        if (LineIcon.ContainsKey(tukey) == false)
                        {
                            LineIcon.TryAdd(tukey, new Tuple<bool, bool>(true,g) );
                        }
                        else
                        {
                            LineIcon[tukey] = new Tuple<bool, bool>(true, g);
                        }
                    }



                }
                else if (equ.EquipmentType == WjParaBase.EquType.Slu)
                {

                    Ac(EquipmentRtuId, new List<int>());


                }

            }
        }

        //private ConcurrentDictionary<int, string> tmpdata = new ConcurrentDictionary<int, string>(); 
        #endregion



    }





}
