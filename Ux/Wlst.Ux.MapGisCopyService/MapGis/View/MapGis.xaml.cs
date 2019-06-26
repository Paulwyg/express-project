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
using Microsoft.Practices.Prism.MefExtensions.Event;
using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using Wlst.Cr.Core.Behavior;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.UtilityFunction;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Sr.Menu.Services;
using Wlst.Ux.MapGisCopyService.MapGis.Services;
using Wlst.Ux.MapGisCopyService.MapGis.ViewModel;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Client.Runtime;
using System.IO;
using System.Timers;
using System.Threading;
using Wlst.client;


namespace Wlst.Ux.MapGisCopyService.MapGis.View
{
    /// <summary>
    /// MapGis.xaml 的交互逻辑
    /// </summary>
    [ViewExport(Ux.MapGisCopyService.Services.ViewIdAssign.MapGisViewId,AttachNow = true, 
       AttachRegion = Ux.MapGisCopyService.Services.ViewIdAssign.MapGisViewAttachRegion)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class MapGis : UserControl
    {
        public static ConcurrentDictionary<int, int> CtrlIcon = new ConcurrentDictionary<int, int>();
        static bool needOnErrChanged=false ;
        private  string SearchService = "";
        private const string XmlConfigName = "MapGis";
        MapGisViewModel model;
        private bool needCopyServices = false;
        private string bsField = null;
        private bool FeatureLayerUpdated =false ;
        private List<string> LocalFeatures = new List<string>();
        private List<string> OnlineFeatures = new List<string>();
        private FeatureLayer Ctrl = new FeatureLayer();
        private FeatureLayer Slu= new FeatureLayer();
        private FeatureLayer TmlSource = new FeatureLayer();
        private FeatureLayer CtrlSource = new FeatureLayer();
        private FeatureLayer SluSource = new FeatureLayer();
        private GraphicsLayer MyGraphicsLayer = new GraphicsLayer();
        private ArcGISLocalFeatureLayer Tml = new ArcGISLocalFeatureLayer();
        private int intervalTime = 60*12*60;
        private double tmlMaxRes;
        public MapGis()
        {
            ArcGISRuntime.InstallPath = @".";
            ArcGISRuntime.SetLicense("runtimestandard,101,rud105202449,none,2KYRMD1AJ48H2T8AG200,25472A32C2C6D077D75848B17A231B715EDFA8A3869919484DE72955AF62B012466BF973A2BF7B4BB6BC4E619BE2F183A42C532CD0A21784E122580892396B4E439755764A218EDFF6F2442299AB05500E823886D5D0AED32AD956D83FFA50AFE1A05C7D441F89E35FC1A9BCD48ACD68316E2D606D996376099B917A274E5719,FID_27b78480_14b65041ba9__6edd");
            InitializeComponent();
            bool haveTml = false;
            bool haveCtrl = false;
            bool haveSlu = false;



            
            var info = Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read(XmlConfigName);
            foreach (var g in info)
            {
                if (g.Key.Contains("CopyFeatureLayers"))
                {
                    needCopyServices = int.Parse(g.Value )==1; 
                }
                break;
            }

            foreach (var f in info)
            {
                if (f.Key == "MapServiceLayer")
                {
                    if (string.IsNullOrEmpty(f.Value)) continue;
                    var TitleMapUrl = info["MapServiceLayer"];
                    ArcGISDynamicMapServiceLayer tLayer = new ArcGISDynamicMapServiceLayer();
                    tLayer.Url = TitleMapUrl;
                    MyMap.Layers.Add(tLayer);
                    MyMap.MouseRightButtonDown += new MouseButtonEventHandler(MyMap_MouseRightButtonDown);

                }
                else if (f.Key == "Interval")
                {
                    intervalTime = int.Parse(info["Interval"]);
                }
                else if(f.Key == "IsShowTools")
                {
                    //model.IsShowTools = int.Parse(info["IsShowTools"]) ==1;//== 1 ? Visibility.Visible : Visibility.Collapsed;
                }
                else if (f.Key == "Tml")
                {
                    if (string.IsNullOrEmpty(f.Value)) continue;
                    TmlSource.Url = f.Value;
                    TmlSource.ID = "TmlSource";
                    TmlSource.DisplayName = "终端";
                    TmlSource.MouseLeftButtonUp += new GraphicsLayer.MouseButtonEventHandler(FeatureLayer_MouseLeftButtonUp);
                    var tmp = new OutFields() { };
                    tmp.Add("*");
                    TmlSource.OutFields = tmp;
                    TmlSource.Mode = FeatureLayer.QueryMode.OnDemand ;
                    TmlSource.DisableClientCaching = true;
                    TmlSource.AutoSave = true;
                    TmlSource.MouseRightButtonDown +=
                        new GraphicsLayer.MouseButtonEventHandler(FeatureLayer_PreviewMouseDown);
                    haveTml = true;
                    //MyMap.Layers.Add(Tml); 
                }
                else if (f.Key == "Ctrl")
                {
                    if (string.IsNullOrEmpty(f.Value)) continue;
                    CtrlSource.Url = f.Value;
                    CtrlSource.ID = "CtrlSource";
                    CtrlSource.DisplayName = "控制器";
                    var tmp = new OutFields() { };
                    tmp.Add("*");
                    CtrlSource.OutFields = tmp;
                    CtrlSource.AutoSave = true;
                    CtrlSource.Mode = FeatureLayer.QueryMode.OnDemand;
                    CtrlSource.DisableClientCaching = true;
                    CtrlSource.MouseLeftButtonUp += new GraphicsLayer.MouseButtonEventHandler(FeatureLayer_MouseLeftButtonUp);
                    CtrlSource.MouseRightButtonDown +=
                        new GraphicsLayer.MouseButtonEventHandler(FeatureLayer_PreviewMouseDown);

                    //MyMap.Layers.Add(Ctrl);
                }
                else if (f.Key == "Slu")
                {
                    if (string.IsNullOrEmpty(f.Value)) continue;
                    SluSource.Url = f.Value;
                    SluSource.ID = "SluSource";
                    SluSource.DisplayName = "集中器";
                    var tmp = new OutFields() { };
                    tmp.Add("*");
                    SluSource.OutFields = tmp;
                    SluSource.AutoSave = true;
                    SluSource.Mode = FeatureLayer.QueryMode.OnDemand;
                    SluSource.DisableClientCaching = true;
                    SluSource.MouseLeftButtonUp += new GraphicsLayer.MouseButtonEventHandler(FeatureLayer_MouseLeftButtonUp);
                    SluSource.MouseRightButtonDown +=
                        new GraphicsLayer.MouseButtonEventHandler(FeatureLayer_PreviewMouseDown);
                    haveSlu = true;

                    //MyMap.Layers.Add(Ctrl);
                }
                else if (f.Key == "SearchService")
                {
                    if (string.IsNullOrEmpty(f.Value)) continue;
                    SearchService = f.Value;
                }
                else if (f.Key.Contains("TmlMaxRes"))
                {
                    if (string.IsNullOrEmpty(f.Value)) continue;
                    tmlMaxRes = double.Parse(info["TmlMaxRes"]);
                    TmlSource.MaximumResolution = tmlMaxRes;
                }
                else if (f.Key.Contains("SluMaxRes"))
                {
                    if (string.IsNullOrEmpty(f.Value)) continue;
                    double sluMaxRes = double.Parse(info["SluMaxRes"]);
                    Slu.MaximumResolution = sluMaxRes;
                }
                else if (f.Key.Contains("CtrlMaxRes"))
                {
                    if (string.IsNullOrEmpty(f.Value)) continue;
                    double ctrlMaxRes = double.Parse(info["CtrlMaxRes"]);
                    Ctrl.MaximumResolution = ctrlMaxRes;
                }
                else if (f.Key.Contains("error1"))
                {
                    if (string.IsNullOrEmpty(f.Value)) continue;
                    CtrlIcon.TryAdd(1, int.Parse(info["error1"]));
                }
                else if (f.Key.Contains("error2"))
                {
                    if (string.IsNullOrEmpty(f.Value)) continue;
                    CtrlIcon.TryAdd(2, int.Parse(info["error2"]));
                }
                else if (f.Key.Contains("error3"))
                {
                    if (string.IsNullOrEmpty(f.Value)) continue;
                    CtrlIcon.TryAdd(3, int.Parse(info["error3"]));
                }
                else if (f.Key.Contains("error4"))
                {
                    if (string.IsNullOrEmpty(f.Value)) continue;
                    CtrlIcon.TryAdd(4, int.Parse(info["error4"]));
                }
                else if (f.Key.Contains("error5"))
                {
                    if (string.IsNullOrEmpty(f.Value)) continue;
                    CtrlIcon.TryAdd(5, int.Parse(info["error5"]));
                }
                else if (f.Key.Contains("error6"))
                {
                    if (string.IsNullOrEmpty(f.Value)) continue;
                    CtrlIcon.TryAdd(6, int.Parse(info["error6"]));
                }
                else if (f.Key.Contains("error7"))
                {
                    if (string.IsNullOrEmpty(f.Value)) continue;
                    CtrlIcon.TryAdd(7, int.Parse(info["error7"]));
                }
                else if (f.Key.Contains("ljdz"))
                {
                    if (string.IsNullOrEmpty(f.Value)) continue;
                    bsField = info["ljdz"];
                }


            }

            //if (haveTml) MyMap.Layers.Add(Tml);
            if (haveCtrl) MyMap.Layers.Add(CtrlSource);
            if (haveSlu) MyMap.Layers.Add(SluSource);
            if (haveTml) MyMap.Layers.Add(TmlSource);
            
            #region load Local Layers

            string path = @".\Map\arcgis.mpk";
            LocalFeatureService lfs = LocalFeatureService.GetService(path);

            Tml.Service = lfs;//.\Map\arcgis.mpk
            Tml.LayerName = "终端";
            Tml.ID = "Tml";
            Tml.DisplayName = "终端";
            Tml.MouseLeftButtonUp += new GraphicsLayer.MouseButtonEventHandler(FeatureLayer_MouseLeftButtonUp);
            Tml.MouseLeftButtonDown += new GraphicsLayer.MouseButtonEventHandler(FeatureLayer_MouseLeftButtonDown);
            Tml.MouseRightButtonUp += new GraphicsLayer.MouseButtonEventHandler(FeatureLayer_MouseRightButtonUp);
            Tml.MouseRightButtonDown +=
                new GraphicsLayer.MouseButtonEventHandler(FeatureLayer_PreviewMouseDown);
            Tml.MouseRightButtonDown +=
    new GraphicsLayer.MouseButtonEventHandler(FeatureLayer_PreviewMouseDown);
            var temp =new OutFields() { };
            temp.Add("*");
            Tml.OutFields = temp;
            Tml.MaximumResolution = tmlMaxRes;
            Tml.Mode = FeatureLayer.QueryMode.Snapshot;
            //Tml.DisableClientCaching = true;
            //Tml.AutoSave = true;
            Tml.Editable = true;
            Tml.MouseRightButtonDown +=
                new GraphicsLayer.MouseButtonEventHandler(FeatureLayer_PreviewMouseDown);
            try
            {
                MyMap.Layers.Add(Tml);
                Tml.UpdateCompleted +=new EventHandler(Tml_UpdateCompleted);
               
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
            MyMap.Layers.Add(MyGraphicsLayer);
            #endregion

              
            //Tml.Where="status>1";
            //this.MyMap.Extent = new Envelope(new MapPoint(13272337.431, 3627494.452), new MapPoint(13392352.671, 3719398.186));
            model = new MapGisViewModel();
            DataContext = model;

            
            MyMap.Layers.LayersInitialized += (o, evt) =>
            {
                model.IsBusy = false;
                ESRI.ArcGIS.Client.Editor myEditor = LayoutRoot.Resources["MyEditor"] as ESRI.ArcGIS.Client.Editor;
                myEditor.Map = MyMap;
                myEditor.LayerIDs = new string[] { "Tml", "Ctrl","Slu" };
                InitEvent();
                Wlst.Cr.Coreb.AsyncTask.Qtz.AddQtz("nuu", 8888, DateTime.Now.Ticks, intervalTime, Funcx);   //todo
                
            };
            TmlSource.UpdateCompleted += new EventHandler(TmlSource_UpdateCompleted);
            
            
          

        }
        void Funcx(object obj)
        {
            Application.Current.Dispatcher.Invoke(new MyInvoke(Funcx1));
        }
         delegate void MyInvoke();

        void Funcx1()
        {
            try
            {
                if (!FeatureLayerUpdated) return;
                if (!needCopyServices) return;
                if(GetLocalFeatures()) CopyFeatureLayers();
               
                //60 s  
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
        }

        void CopyFeatureLayers()
        {
            string lid = "";
            string wybs = "";
            var temp = new List<Graphic>();
            try
            {
                if (bsField == null) bsField = "Lid";  
                foreach (var g in TmlSource.Graphics)
                {
                    if (!g.Attributes.ContainsKey(bsField)) continue ;
                    if (g.Attributes[bsField] == null) continue;
                    if (!string.IsNullOrEmpty(g.Attributes[bsField].ToString()) )
                    {
                        wybs = g.Attributes[bsField].ToString();
                    }else
                    {
                        continue ;
                    }


                    if (LocalFeatures.Contains(wybs)) continue;
                    Graphic gr = new Graphic();
                    gr.Geometry = g.Geometry;
                    gr.Geometry.SpatialReference = MyMap.SpatialReference;
      
                    //todo
                    var rtuLst = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoList(WjParaBase.EquType.Rtu);
                    foreach (var a in rtuLst)
                    {
                        var pb = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(a.RtuId);
                        if (pb == null) continue;
                        if (pb.EquipmentType == WjParaBase.EquType.Rtu && pb.Idf == wybs)
                        {
                            lid = pb.RtuId + "";
                            gr.Attributes.Add("Lid", lid);
                            LocalFeatures.Add(wybs);
                            temp.Add(gr);
                            break;
                        }
                    }

                   
                }
                if (temp.Count == 0) return;
                Tml.Graphics.AddRange(temp);
                System.Threading.Thread.Sleep(28);
                //Tml.SaveEdits();
                Tml.Update();
            }catch (Exception  ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error: " + ex);
            }

        }

        private  bool GetLocalFeatures()
        {

                if (OnlineFeatures.Count != TmlSource.Graphics.Count)
                {
                    OnlineFeatures.Clear();
                    foreach (var g in TmlSource.Graphics)
                    {
                        if (!g.Attributes.ContainsKey(bsField))
                        {
                            var infoss = WlstMessageBox.Show("更新地图出错", "无法在服务中找到唯一标识字段");
                            return false ;
                        }
                        if (g.Attributes[bsField] == null) continue;
                        if (!string.IsNullOrEmpty(g.Attributes[bsField].ToString() ))
                        {
                            OnlineFeatures.Add(g.Attributes[bsField].ToString());
                        }

                    }
                }


                if (LocalFeatures.Count != Tml.Graphics.Count)
                {
                    LocalFeatures.Clear();
                    string wybs = "";
                    foreach (var g in Tml.Graphics)
                    {
                        if (!g.Attributes.ContainsKey("Lid"))
                        {
                            var infoss = WlstMessageBox.Show("更新地图出错", "本地业务图层中无法找到逻辑地址字段");
                            return false ;
                        }
                        if (g.Attributes["Lid"] == null) continue;
                        if (!string.IsNullOrEmpty(g.Attributes["Lid"].ToString()))
                        {
                            int lid = int.Parse(g.Attributes["Lid"].ToString());
                            if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(lid))
                                continue;

                            wybs = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[lid].Idf;
                            LocalFeatures.Add(wybs);
                        }
                    }
                }
         
            return LocalFeatures.Count < OnlineFeatures .Count ;
        }


        void Tml_UpdateCompleted(object sender, EventArgs e)
        {

            if (needOnErrChanged == true)
            {
                //throw new NotImplementedException();
                var rtus = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.Keys;//.EquipmentInfoDictionary.Keys;
                foreach (int id in rtus)
                {
                    OnErrChanged(id);

                }
                needOnErrChanged = false;
            }
        }

        void TmlSource_UpdateCompleted(object sender, EventArgs e)
        {
            FeatureLayerUpdated = true;
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
            FeatureLayer featureLayer = sender as FeatureLayer;
            //chkErr.IsChecked = false;
            //////////////////MyGraphicsLayer.Graphics.Clear();
            //FindDetailsDataGrid.Visibility = Visibility.Hidden;
            if (Keyboard.Modifiers != ModifierKeys.Control)
            {
                for (int i = 0; i < Tml.SelectionCount; i++)
                    Tml.SelectedGraphics.ToList()[i].UnSelect();
                for (int i = 0; i < Ctrl.SelectionCount; i++)
                    Ctrl.SelectedGraphics.ToList()[i].UnSelect();
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
            //ShowPic(Convert.ToInt32(args.Graphic.Attributes["Lid"]));
            MyFeatureDataForm.FeatureLayer = featureLayer;
            MyFeatureDataForm.GraphicSource = args.Graphic;
            FeatureDataFormBorder.Visibility = Visibility.Visible;
            MyGraphicsLayer.Graphics.Clear();
            //GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
            ////////////////////////graphicsLayer.Graphics.Clear();
            MyGraphicsLayer.Graphics.Clear();
            if (featureLayer == Slu)
            {
                //MyGraphicsLayer.Graphics.Clear();
                foreach (Graphic g in Ctrl.Graphics)
                {
                    if (g.Attributes["Flid"] == null || args.Graphic.Attributes["Lid"]==null) continue;
                    if (string.IsNullOrEmpty(g.Attributes["Flid"].ToString() ) || string .IsNullOrEmpty(args.Graphic.Attributes["Lid"].ToString() )) continue;
                    if (Convert.ToInt32(g.Attributes["Flid"]) == Convert.ToInt32(args.Graphic.Attributes["Lid"]))
                    {
                        Graphic gr = new Graphic
                        {
                            Geometry = g.Geometry,
                            Symbol = LayoutRoot.Resources["DefaultMarkerSymbol"] as ESRI.ArcGIS.Client.Symbols.Symbol, // newSymbol is a SimpleMarkerSymbol (point)
                        };
                        MyGraphicsLayer.Graphics.Add(gr);
                    }
                }
            }


            args.Graphic.Select();
            //args.Graphic.Attributes["bh"] = "1111";
            MyFeatureDataForm.GraphicSource = args.Graphic;

            FeatureDataFormBorder.Visibility = Visibility.Visible;

            //发布事件  选中当前节点
            var argss = new PublishEventArgs
            {
                EventType = PublishEventType.Core,
                EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                EventAttachInfo ="gis"
            };
            //if (featureLayer == Controller) 
            //{
            //    argss.AddParams(args.Graphic.Attributes["Flid"]);
            //    argss.AddParams(args.Graphic.Attributes["Lid"]);
                
            //}
            //else
            //{
            //    argss.AddParams(args.Graphic.Attributes["Lid"]);
            //}
            EventPublisher.EventPublish(argss);
        }
        //图层点击
        //给拖动图元做铺垫
        private void FeatureLayer_MouseLeftButtonDown(object sender, GraphicMouseButtonEventArgs e)
        {
            ArcGISLocalFeatureLayer featureLayer = sender as ArcGISLocalFeatureLayer;
         
            model.SetCmUnVisi();
            if (chkEdit.IsChecked == false)
                return;
            e.Handled = true;

        }
        //图层右击，类似左击功能
        private void FeatureLayer_MouseRightButtonUp(object sender, GraphicMouseButtonEventArgs args)
        {
            ArcGISLocalFeatureLayer featureLayer = sender as ArcGISLocalFeatureLayer;

            ////////////////////////MyGraphicsLayer.Graphics.Clear();   
            try
            {
                ESRI.ArcGIS.Client.Editor myEditorC = LayoutRoot.Resources["MyEditorC"] as ESRI.ArcGIS.Client.Editor;
                myEditorC.Map = MyMap;
                myEditorC.LayerIDs = new string[] { "Tml","Ctrl"};
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
            //if (featureLayer == Controller)
            //{
            //    argss.AddParams(args.Graphic.Attributes["Flid"]);
            //    argss.AddParams(args.Graphic.Attributes["Lid"]);
            //}
            //else
            //{
            //    argss.AddParams(args.Graphic.Attributes["Lid"]);
            //}
            EventPublisher.EventPublish(argss);
        }
        //呈现右击菜单
        private void FeatureLayer_PreviewMouseDown(object sender, GraphicMouseButtonEventArgs e)        
        {
            ArcGISLocalFeatureLayer featureLayer = sender as ArcGISLocalFeatureLayer;
            e.Graphic.Select();
            //if (featureLayer == Circuit) return;//线路右击暂时屏蔽
            if (featureLayer == Ctrl)
            {
                if (!e.Graphic.Attributes.ContainsKey("Flid") ) return;
                if(string.IsNullOrEmpty(e.Graphic.Attributes["Flid"].ToString())) return;
                if (!e.Graphic.Attributes.ContainsKey("Lid") ) return;
                if (string.IsNullOrEmpty(e.Graphic.Attributes["Lid"].ToString())) return;
                if (Convert.ToInt32(e.Graphic.Attributes["Flid"]) < 1500000 || Convert.ToInt32(e.Graphic.Attributes["Flid"]) > 1600000)
                    return;
                int a = Convert.ToInt32(e.Graphic.Attributes["Flid"]);  
                int b = Convert.ToInt32(e.Graphic.Attributes["Lid"]);
                int rtuId = (a * 1000) + b;
                model.CurrentRtuId = rtuId;
                model.Cm.IsOpen = true;
            }
            else
            {
                if (!e.Graphic.Attributes.ContainsKey("Lid") ) return;
                if (string.IsNullOrEmpty(e.Graphic.Attributes["Lid"].ToString())) return;
                if (Convert.ToInt32(e.Graphic.Attributes["Lid"]) < 1000000 || Convert.ToInt32(e.Graphic.Attributes["Lid"]) > 2000000)
                    return;
                int rtuId = Convert.ToInt32(e.Graphic.Attributes["Lid"]);
                model.CurrentRtuId = rtuId;
                model.Cm.IsOpen = true;
            }

        }
        //点击搜索按钮

       

        int i = 0;
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (SearchService=="")
            {
                var infos = WlstMessageBox.Show("提示", "没有配置查询服务", WlstMessageBoxType.Close);
                return;
            }

            if (string.IsNullOrEmpty(FindText.Text) || FindText.Text == "") return;
            //GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
            //graphicsLayer.Graphics.Clear();
            MyGraphicsLayer.Graphics.Clear();
            

            FindTask findTask = new FindTask(SearchService);
            findTask.Failed += FindTask_Failed;

            FindParameters findParameters = new FindParameters();
            // Layer ids to search
            findParameters.LayerIds.AddRange(new int[] { 0, 1, 2 });
            // Fields in layers to search
            findParameters.SearchFields.AddRange(new string[] { "Flid","Lid","计控箱名称","编号"});
            // Return features in map's spatial reference
            findParameters.SpatialReference = MyMap.SpatialReference;


            // Bind data grid to find results.  Bind to the LastResult property which returns a list
            // of FindResult instances.  When LastResult is updated, the ItemsSource property on the 
            // will update.  
            Binding resultFeaturesBinding = new Binding("LastResult");
            resultFeaturesBinding.Source = findTask;
            //if (findTask.LastResult == null) return;
            FindDetailsDataGrid.Visibility = Visibility.Visible;
            FindDetailsDataGrid.SetBinding(DataGrid.ItemsSourceProperty, resultFeaturesBinding);

            findParameters.SearchText = FindText.Text;
            findTask.ExecuteAsync(findParameters);
            if (findTask.LastResult == null)
            {
                FindDetailsDataGrid.Visibility = Visibility.Collapsed;
            }

            //if (FindText.Text.Trim() == "")
            //    return;
            //isManuele = true;
            //FindDetailsDataGrid.Visibility = Visibility.Visible;
            //GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
            //graphicsLayer.Graphics.Clear();
            //FindTask findTask = new FindTask();
            ////findTask.Url = _mapService.UrlMapService;
            //findTask.Failed += FindTask_Failed;
            //FindParameters findParameters = new FindParameters();
            //findParameters.LayerIds.AddRange(new int[] { 0,1,2 });
            //findParameters.SearchFields.AddRange(new string[] { "Lid", "Flid", "type", "address","Name","Lid","Flid" });
            //Binding resultFeaturesBinding = new Binding("LastResult");
            //resultFeaturesBinding.Source = findTask;
            //FindDetailsDataGrid.SetBinding(DataGrid.ItemsSourceProperty, resultFeaturesBinding);
            //findParameters.SearchText = FindText.Text;
            //findTask.ExecuteAsync(findParameters);
            //isManuele = false;
        }
        //private void SearchNode(string keyWord)
        //{
        //    GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
        //    graphicsLayer.Graphics.Clear();
        //    FindDetailsDataGrid.Visibility = Visibility.Visible;

        //    FindTask findTask = new FindTask("http://192.168.50.108:64813/ArcGIS/rest/services/jiangbei_yewu/MapServer");
        //    findTask.Failed += FindTask_Failed;

        //    FindParameters findParameters = new FindParameters();
        //    // Layer ids to search
        //    findParameters.LayerIds.AddRange(new int[] { 0, 1, 2 });
        //    // Fields in layers to search
        //    findParameters.SearchFields.AddRange(new string[] { "Flid", "Lid" });
        //    // Return features in map's spatial reference
        //    findParameters.SpatialReference = MyMap.SpatialReference;


        //    // Bind data grid to find results.  Bind to the LastResult property which returns a list
        //    // of FindResult instances.  When LastResult is updated, the ItemsSource property on the 
        //    // will update.  
        //    Binding resultFeaturesBinding = new Binding("LastResult");
        //    resultFeaturesBinding.Source = findTask;
        //    FindDetailsDataGrid.SetBinding(DataGrid.ItemsSourceProperty, resultFeaturesBinding);

        //    findParameters.SearchText = keyWord ;// FindText.Text;
        //    findTask.ExecuteAsync(findParameters);
        
        //}

        private void FindText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (SearchService == "")
                {
                    var infos = WlstMessageBox.Show("提示", "没有配置查询服务", WlstMessageBoxType.Close);
                    return;
                }

                if (string.IsNullOrEmpty(FindText.Text) || FindText.Text == "") return;

                MyGraphicsLayer.Graphics.Clear();


                FindTask findTask = new FindTask(SearchService);
                findTask.Failed += FindTask_Failed;

                FindParameters findParameters = new FindParameters();
                // Layer ids to search
                findParameters.LayerIds.AddRange(new int[] { 0, 1, 2 });
                // Fields in layers to search
                findParameters.SearchFields.AddRange(new string[] { "Flid", "Lid", "计控箱名称", "编号" });
                // Return features in map's spatial reference
                findParameters.SpatialReference = MyMap.SpatialReference;


                // Bind data grid to find results.  Bind to the LastResult property which returns a list
                // of FindResult instances.  When LastResult is updated, the ItemsSource property on the 
                // will update.  
                Binding resultFeaturesBinding = new Binding("LastResult");
                resultFeaturesBinding.Source = findTask;
                //if (findTask.LastResult == null) return;
                FindDetailsDataGrid.Visibility = Visibility.Visible;
                FindDetailsDataGrid.SetBinding(DataGrid.ItemsSourceProperty, resultFeaturesBinding);

                findParameters.SearchText = FindText.Text;
                findTask.ExecuteAsync(findParameters);
            }
        }
        bool isManuele = false;
        //选择搜索结果
        private void FindDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isManuele) return;
            try
            {
                ESRI.ArcGIS.Client.Editor myEditorC = LayoutRoot.Resources["MyEditorC"] as ESRI.ArcGIS.Client.Editor;
                myEditorC.Map = MyMap;
                myEditorC.LayerIDs = new string[] { "Tml", "Ctrl","Slu" };
                myEditorC.ClearSelection.Execute(null);
            }
            catch { }
            FindDetailsDataGrid.Visibility = Visibility.Collapsed;
            DataGrid dataGrid = sender as DataGrid;
            int selectedIndex = dataGrid.SelectedIndex;
            if (selectedIndex > -1)
            {
                FindResult findResult = (FindResult)FindDetailsDataGrid.SelectedItem;
                Graphic graphic = findResult.Feature;
                if (!graphic.Attributes.ContainsKey("逻辑地址")) return;
                if (string.IsNullOrEmpty(graphic.Attributes["逻辑地址"].ToString()) || graphic.Attributes["逻辑地址"].ToString() == "")
                    return;
                var lid = Convert.ToInt32(graphic.Attributes.ContainsKey("逻辑地址"));
                if (graphic.Attributes.ContainsKey("所属设备地址") && !string.IsNullOrEmpty(graphic.Attributes["逻辑地址"].ToString()) && graphic.Attributes["逻辑地址"].ToString() != "")
                {
                    int y = Convert.ToInt32(graphic.Attributes["所属设备地址"]);//控制器序号
                    lid = y * 1000 + lid;
                    
                }
                LocateSth(lid);
                //graphic.Geometry.SpatialReference = MyMap.SpatialReference;
                //switch (graphic.Attributes["SHAPE"].ToString())
                //{
                //    case "Polygon":
                //        graphic.Symbol = LayoutRoot.Resources["DefaultFillSymbol"] as ESRI.ArcGIS.Client.Symbols.Symbol;
                //        break;
                //    case "Polyline":
                //        graphic.Symbol = LayoutRoot.Resources["DefaultLineSymbol"] as ESRI.ArcGIS.Client.Symbols.Symbol;
                //        break;
                //    case "Point":

                //        graphic.Symbol = LayoutRoot.Resources["DefaultMarkerSymbol"] as ESRI.ArcGIS.Client.Symbols.Symbol;
                //        break;
                //}
                ////GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
                ////graphicsLayer.Graphics.Clear();
                ////graphicsLayer.Graphics.Add(graphic);
                //MyGraphicsLayer.Graphics.Clear();
                //MyGraphicsLayer.Graphics.Add(graphic );
                //graphic.Select();
                //MyMap.PanTo(graphic.Geometry);
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
                
                MyFeatureDataForm.IsReadOnly = false ;
                //showPic.Visibility = Visibility.Visible ;
                DelSelectionButton.IsEnabled = true;
                AddMultiController.IsEnabled = true;
                AddTml.IsEnabled = true;
                AddConcentrator.IsEnabled = true;
                //DelNullButton.IsEnabled = true ;
                BindButton.IsEnabled = true;

                try
                {
                    ESRI.ArcGIS.Client.Editor myEditorC = LayoutRoot.Resources["MyEditorC"] as ESRI.ArcGIS.Client.Editor;
                    myEditorC.Map = MyMap;
                    myEditorC.LayerIDs = new string[] { ""};
                    myEditorC.ClearSelection.Execute(null);
                }
                catch { }
            }
        }

        //显示故障设备
        //private void chkErr_Click(object sender, RoutedEventArgs e)
        //{
        //    GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
        //    graphicsLayer.Graphics.Clear();
        //     if (chkErr.IsChecked == true )
        //    {
        //        if (Tml.Visible == true)
        //        {
        //            foreach (Graphic g in Tml.Graphics)
        //            {
        //                if (Convert.ToInt32(g.Attributes["Status"]) > 2)
        //                {
        //                    Graphic gr = new Graphic
        //                    {
        //                        Geometry = g.Geometry,
        //                        Symbol = LayoutRoot.Resources["ErrTml"] as ESRI.ArcGIS.Client.Symbols.Symbol, // newSymbol is a SimpleMarkerSymbol (point)
        //                    };
        //                    graphicsLayer.Graphics.Add(gr);
        //                }
        //            }
        //        }
        //        if (Concentrator.Visible == true)
        //        {
        //            foreach (Graphic g in Concentrator.Graphics)
        //            {
        //                if (Convert.ToInt32(g.Attributes["Status"]) > 3)
        //                {
        //                    Graphic gr = new Graphic
        //                    {
        //                        Geometry = g.Geometry,
        //                        Symbol = LayoutRoot.Resources["ErrConcentrator"] as ESRI.ArcGIS.Client.Symbols.Symbol, // newSymbol is a SimpleMarkerSymbol (point)
        //                    };
        //                    graphicsLayer.Graphics.Add(gr);
        //                }
        //            }
        //        }
        //        if (Controller.Visible == true)
        //        {
        //            foreach (Graphic g in Controller.Graphics)
        //            {
        //                if (Convert.ToInt32(g.Attributes["Status"]) > 3)
        //                {
        //                    Graphic gr = new Graphic
        //                    {
        //                        Geometry = g.Geometry,
        //                        Symbol = LayoutRoot.Resources["ErrController"] as ESRI.ArcGIS.Client.Symbols.Symbol, // newSymbol is a SimpleMarkerSymbol (point)
        //                    };
        //                    graphicsLayer.Graphics.Add(gr);
        //                }
        //            }
        //        }
        //    }
        //}      
        ////隐藏工具栏
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
            {
                foreach (Graphic g in Tml.SelectedGraphics)
                {
                    //遍历终端图层思密达
                    if (!g.Attributes.ContainsKey("Lid")) return ;
                    if (g.Attributes["Lid"] == null) return;
                    if (string.IsNullOrEmpty(g.Attributes["Lid"].ToString())) continue;
                    if ((Convert.ToInt32(g.Attributes["Lid"]) > 1000000) && (Convert.ToInt32(g.Attributes["Lid"]) < 1100000))
                    {
                        listRtu.Add(Convert.ToInt32(g.Attributes["Lid"]));
                        count++;
                    }
                }    
            }
            //if (Concentrator.SelectedGraphics.Count() > 0)
            //{
            //    foreach (Graphic g in Concentrator.SelectedGraphics)          //遍历终端图层思密达
            //        if ((Convert.ToInt32(g.Attributes["Lid"]) > 1500000) && (Convert.ToInt32(g.Attributes["Lid"]) < 1600000))
            //        {
            //            listConcentrator.Add(Convert.ToInt32(g.Attributes["Lid"]));
            //            count++;
            //        }
            //}
            if (Ctrl .SelectedGraphics.Count() > 0)
            {
                foreach (Graphic g in Ctrl.SelectedGraphics)
                {
                    //遍历终端图层思密达
                    if (!g.Attributes.ContainsKey("Lid")) return;
                    if (!g.Attributes.ContainsKey("Flid")) return;
                    if (g.Attributes["Lid"] == null || g.Attributes["Flid"] == null) continue;
                    if (string.IsNullOrEmpty(g.Attributes["Lid"].ToString() )) continue;
                    if (string.IsNullOrEmpty(g.Attributes["Flid"].ToString())) continue;
                    if ((Convert.ToInt32(g.Attributes["Lid"]) > 0) &&
                        (Convert.ToInt32(g.Attributes["Flid"]) > 1500000) &&
                        (Convert.ToInt32(g.Attributes["Flid"]) < 1600000))
                    {
                        int sluid = Convert.ToInt32(g.Attributes["Flid"]);
                        int ctrlid = Convert.ToInt32(g.Attributes["Lid"]);

                        if (!listController.ContainsKey(sluid)) listController.Add(sluid, new List<int>());
                        if (!listController[sluid].Contains(ctrlid)) listController[sluid].Add(ctrlid);
                        count++;
                    }
                }
            }
            if (count <= 1) return;
            model.GetCm(listRtu, listConcentrator, listController);
            model.Cm.IsOpen = true;

        } //批量右击

        private void AddMultiController_Click(object sender, RoutedEventArgs e)
        {
            //lvf need todo
            if (Tml.SelectionCount > 1)
            {
                var infos = WlstMessageBox.Show("提示", "只能选中一个集中器", WlstMessageBoxType.Close);
                return;
            }
            if (Tml.SelectionCount == 0)
            {
                var infos = WlstMessageBox.Show("提示", "请选中一个集中器", WlstMessageBoxType.Close);
                return;
            }
            if (Tml.SelectionCount == 1)
            {
                if (!Tml.SelectedGraphics.ToList()[0].Attributes.ContainsKey("Lid"))
                {
                    var infos = WlstMessageBox.Show("提示", "集中器图层缺少字段", WlstMessageBoxType.Close);
                    return;
                }
                if (Tml.SelectedGraphics.ToList()[0].Attributes["Lid"] == null)
                {
                    var infos = WlstMessageBox.Show("提示", "您选中的集中器没有绑定", WlstMessageBoxType.Close);
                    return;

                }
                else
                {
                    AddMultiController.Style = LayoutRoot.Resources["AddController1"] as Style;
                    AddTml.Style = LayoutRoot.Resources["AddTml0"] as Style;
                    AddConcentrator.Style = LayoutRoot.Resources["AddConcentrator0"] as Style;
                    //AddLine.Style = LayoutRoot.Resources["AddLine0"] as Style;
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
            //GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
            //graphicsLayer.Graphics.Clear();
            MyGraphicsLayer.Graphics.Clear();
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
            AddMultiController.Style = LayoutRoot.Resources["AddController0"] as Style;
            ESRI.ArcGIS.Client.Geometry.Polyline polyline = args.Geometry as ESRI.ArcGIS.Client.Geometry.Polyline;
            polylineT = polyline;
            polyline.SpatialReference = MyMap.SpatialReference;
            Graphic graphic = new Graphic()
            {
                Symbol = LayoutRoot.Resources["DefaultLineSymbol"] as ESRI.ArcGIS.Client.Symbols.Symbol,
                Geometry = polyline
            };

            //GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
            //graphicsLayer.Graphics.Add(graphic);
            MyGraphicsLayer.Graphics.Clear();
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
                int nMax = 0;
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
                    //GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
                    //graphicsLayer.Graphics.Clear();
                    MyGraphicsLayer.Graphics.Clear();
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
                        if (!gr.Attributes.ContainsKey("Type")) return;
                        if (index == 0)
                        {
                            gr.Attributes["Type"] = "10";
                        }
                        else
                        {
                            gr.Attributes["Type"] = "20";
                        }

                        foreach (Graphic g in Ctrl.Graphics)
                        {
                            if (!g.Attributes.ContainsKey("Flid")) return;
                            if (!g.Attributes.ContainsKey("Lid")) return;
                            if (g.Attributes["Flid"] == null || g.Attributes["Lid"] == null) continue;
                            if(!string.IsNullOrEmpty(g.Attributes["Flid"].ToString())|| !string.IsNullOrEmpty(g.Attributes["Lid"].ToString() ))
                            //遍历终端图层思密达
                            if (Convert.ToInt32(g.Attributes["Flid"]) ==
                                Convert.ToInt32(Tml.SelectedGraphics.ToList()[0].Attributes["Lid"]))
                            {
                                if (nMax < Convert.ToInt32(g.Attributes["Lid"]))
                                    nMax = Convert.ToInt32(g.Attributes["Lid"]);
                            }
                        }
                        if (Tml.SelectionCount == 1)
                        {
                            gr.Attributes["Flid"] = Tml.SelectedGraphics.ToList()[0].Attributes["Lid"];
                            string _num = (i + nMax).ToString();
                            gr.Attributes.Add("Lid", _num);
                        }

                        x1 = x1 + xT;
                        y1 = y1 + yT;
                        temp.Add(gr);

                    }
                    Ctrl.Graphics.AddRange(temp);

                }
                //xT = 0;
                //yT = 0;

                Ctrl.Update();
                AddMultiController.Content = "批量添加控制器";
                _drawObject.IsEnabled = false;
                onDraw = false;
            }
        }
        private void AddTml_Click(object sender, RoutedEventArgs e)
        {
            AddTml.Style = LayoutRoot.Resources["AddTml1"] as Style;
            AddConcentrator.Style = LayoutRoot.Resources["AddConcentrator0"] as Style;
            AddMultiController.Style = LayoutRoot.Resources["AddController0"] as Style;
            //AddLine.Style = LayoutRoot.Resources["AddLine0"] as Style;
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
            //AddLine.Style = LayoutRoot.Resources["AddLine0"] as Style;
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
            graphic.Attributes.Add("Lid", "0");

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
            Tml.Graphics.Add(graphic);
            System.Threading.Thread.Sleep(28);
            _drawObject.IsEnabled = false;
            Tml.SaveEdits();
            Tml.Update();
        }
        //清空选中
        private void DelSelectionButton_Click(object sender, RoutedEventArgs e)
        {
            DelSelectionButton.Style = LayoutRoot.Resources["DelSelected1"] as Style;

            AddConcentrator.Style = LayoutRoot.Resources["AddConcentrator0"] as Style;
            AddTml.Style = LayoutRoot.Resources["AddTml0"] as Style;
            AddMultiController.Style = LayoutRoot.Resources["AddController0"] as Style;
            //AddLine.Style = LayoutRoot.Resources["AddLine0"] as Style;
            try
            {
                var infoss = WlstMessageBox.Show("确认删除", "是否删除所选设备？", WlstMessageBoxType.YesNo);
                DelSelectionButton.Style = LayoutRoot.Resources["DelSelected0"] as Style;
                if (infoss != WlstMessageBoxResults.Yes) return;


                ESRI.ArcGIS.Client.Editor myEditor = LayoutRoot.Resources["MyEditor"] as ESRI.ArcGIS.Client.Editor;
                myEditor.Map = MyMap;
                myEditor.LayerIDs = new string[] { "Tml","Ctrl" };
                myEditor.DeleteSelected.Execute(null);
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
        }




        //private void ShowNullButton_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
              
        //        ESRI.ArcGIS.Client.Editor myEditor = LayoutRoot.Resources["MyEditor"] as ESRI.ArcGIS.Client.Editor;
        //        myEditor.Map = MyMap;
        //        myEditor.LayerIDs = new string[] { "终端", "控制器", "集中器", "线路" };
        //        myEditor.ClearSelection.Execute(null);
        //    }
        //    catch { }
        //    ShowNullButton.Style = LayoutRoot.Resources["ShowNull1"] as Style ;
        //    GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
        //    graphicsLayer.Graphics.Clear();
        //    foreach (Graphic g in Controller.Graphics)
        //    {
        //        if (g.Attributes["Lid"] == null)
        //        {
        //            Graphic gr = new Graphic
        //            {
        //                Geometry = g.Geometry,
        //                Symbol = LayoutRoot.Resources["DefaultMarkerSymbol"] as ESRI.ArcGIS.Client.Symbols.Symbol, // newSymbol is a SimpleMarkerSymbol (point)
        //            };
        //            graphicsLayer.Graphics.Add(gr);
        //        }
        //    }
        //    foreach (Graphic g in Tml.Graphics)
        //    {
        //        if (g.Attributes["Lid"] == null)
        //        {
        //            Graphic gr = new Graphic
        //            {
        //                Geometry = g.Geometry,
        //                Symbol = LayoutRoot.Resources["DefaultMarkerSymbol"] as ESRI.ArcGIS.Client.Symbols.Symbol, // newSymbol is a SimpleMarkerSymbol (point)
        //            };
        //            graphicsLayer.Graphics.Add(gr);
        //        }
        //    }
        //    foreach (Graphic g in Concentrator.Graphics)
        //    {
        //        if (g.Attributes["Lid"] == null)
        //        {
        //            Graphic gr = new Graphic
        //            {
        //                Geometry = g.Geometry,
        //                Symbol = LayoutRoot.Resources["DefaultMarkerSymbol"] as ESRI.ArcGIS.Client.Symbols.Symbol, // newSymbol is a SimpleMarkerSymbol (point)
        //            };
        //            graphicsLayer.Graphics.Add(gr);
        //        }
        //    }
        //    ShowNullButton.Style = LayoutRoot.Resources["ShowNull0"] as Style;
        //}  
        //绑定图元，不管先后，只要树和地图都有选中的设备，然后点击绑定按钮，会做些许判断，确认无误后成功绑定
        private void BindButton_Click(object sender, RoutedEventArgs e)
        {
            BindButton.Style = LayoutRoot.Resources["Bind1"] as Style;
            AddConcentrator.Style = LayoutRoot.Resources["AddConcentrator0"] as Style;
            AddTml.Style = LayoutRoot.Resources["AddTml0"] as Style;
            AddMultiController.Style = LayoutRoot.Resources["AddController0"] as Style;
            //AddLine.Style = LayoutRoot.Resources["AddLine0"] as Style;
            FeatureLayer BingFL = new FeatureLayer();
            double snum = 0;
            if (TreeSelectId < 1000000)
            {
                var infoss = WlstMessageBox.Show("绑定出错", "请在右侧列表中选择某一设备");
                BindButton.Style = LayoutRoot.Resources["Bind0"] as Style;
                return;
            }
            if (Tml.SelectionCount > 0)
            {
                snum = snum + Tml.SelectionCount;
                BingFL = Tml;
            }
            if(Ctrl.SelectionCount >0)
            {
                snum = snum + Ctrl.SelectionCount;
                BingFL = Ctrl;
            }
            //if (Controller.SelectionCount > 0)
            //{
            //    snum = snum + Controller.SelectionCount;
            //    BingFL = Controller;
            //}
            //if (Concentrator.SelectionCount > 0)
            //{
            //    snum = snum + Concentrator.SelectionCount;
            //    BingFL = Concentrator;
            //}
            if (snum == 1)
            {
                if (BingFL.SelectedGraphics.ToList()[0].Attributes["Lid"] != null)
                {
                    var infos = WlstMessageBox.Show("绑定提示", "选中图元已绑定，是否覆盖绑定", WlstMessageBoxType.YesNo);
                    if (infos != WlstMessageBoxResults.Yes) return;

                }
                if (TreeSelectId < 1100000 && TreeSelectId > 1000000)//终端
                {
                    if (BingFL == Tml)
                    {
                        
                        var infoss = WlstMessageBox.Show("确认绑定", "是否绑定", WlstMessageBoxType.YesNo);

                        if (infoss != WlstMessageBoxResults.Yes) return;

                        if (Tml.SelectedGraphics.ToList()[0].Attributes.ContainsKey("Lid"))
                        {

                            Tml.SelectedGraphics.ToList()[0].Attributes["Lid"] = TreeSelectId.ToString(); //TreeSelectId.ToString();
                        }
                        else
                        {
                            var infosss = WlstMessageBox.Show("错误", "地图服务需要增加字段");
                        }
                        
                        //Tml.SaveEdits();
                        //Tml.Update();

                        //var f =
                        //Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[TreeSelectId];

                        //if (f.RtuMapX.Equals(0) || f.RtuGisY.Equals(0))  //todo
                        //{
                        //    if (isUselocate == 0) return ;
                        //    var t = BingFL.SelectedGraphics.ToList()[0].Geometry as MapPoint;
                        //    Graphic gt = new Graphic();
                        //    gt.Geometry = ChangeCoodinateTo84(t.X, t.Y);
                        //    MapPoint mp = new MapPoint();
                        //    mp = gt.Geometry as MapPoint;
                        //    double xt = mp.X;
                        //    double yt = mp.Y;
                        //    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.OnEquipmentMentMapLocationChangeByMap(
                        //    TreeSelectId, xt, yt);
                        //}
                        //else
                        //{
                        //    if (isUselocate == 0) return;
                        //    BingFL.SelectedGraphics.ToList()[0].Geometry = ChangeCoodinateFrom84(f.RtuMapX, f.RtuMapY);
                        //    BingFL.SelectedGraphics.ToList()[0].Geometry.SpatialReference = MyMap.SpatialReference;
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

                if (TreeSelectId < 1600000 && TreeSelectId > 1500000 && BingFL == Tml) //集中器
                {
                    if (BingFL == Tml)
                    {
                        if(BingFL.SelectedGraphics.ToList()[0].Attributes.ContainsKey("Lid"))
                        {

                            BingFL.SelectedGraphics.ToList()[0].Attributes["Lid"] = TreeSelectId;
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

                }

                if (TreeSelectId < 1600000000 && TreeSelectId > 1500000000) //控制器
                {
                    if (BingFL == Ctrl)
                    {
                        int ctrId;
                        int concentratorId;
                        ctrId = TreeSelectId % 1000;
                        concentratorId = Convert.ToInt32(TreeSelectId / 1000);
                        var infoss = WlstMessageBox.Show("确认绑定", "是否绑定", WlstMessageBoxType.YesNo);
                        if (infoss != WlstMessageBoxResults.Yes) return;
                        if (Ctrl.SelectedGraphics.ToList()[0].Attributes.ContainsKey("Lid") && Ctrl.SelectedGraphics.ToList()[0].Attributes.ContainsKey( "Flid"))
                        {
                            Ctrl.SelectedGraphics.ToList()[0].Attributes["Lid"] = ctrId;
                            Ctrl.SelectedGraphics.ToList()[0].Attributes["Flid"] = concentratorId; 
                        }else
                        {
                            var infosss = WlstMessageBox.Show("错误", "地图服务需要增加字段");
                        }

                    }
                    else
                    {
                        var infoss = WlstMessageBox.Show("错误", "列表设备与地图设备不匹配"); 
                    }

                }
                BindButton.Style = LayoutRoot.Resources["Bind0"] as Style;
            }
            else
            {
                var infoss = WlstMessageBox.Show("绑定出错", "选中了0个或多个设备，不能执行绑定，请清空地图选择项，并重新选择需要绑定的设备");
                BindButton.Style = LayoutRoot.Resources["Bind0"] as Style;
            }
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
        
        public void LocateSth(int sid)                     //设备定位
        {
                try
                {
                    ESRI.ArcGIS.Client.Editor myEditor = LayoutRoot.Resources["MyEditor"] as ESRI.ArcGIS.Client.Editor;
                    myEditor.Map = MyMap;
                    myEditor.LayerIDs = new string[] { "Tml", "Ctrl" };
                    myEditor.ClearSelection.Execute(null);


                }
                catch { }

            if (sid < 1100000 && sid > 0)
            {

                foreach (Graphic g in Tml.Graphics)
                {
                    //遍历终端图层思密达
                    if (!g.Attributes.ContainsKey("Lid")) return;
                    if (Convert.ToInt32(g.Attributes["Lid"]) == sid)
                    {
                        g.Select();
                        MyMap.PanTo(g.Geometry);
                        MyFeatureDataForm.FeatureLayer = Tml;
                        MyFeatureDataForm.GraphicSource = g;

                    }
                }
            }
            else if (sid < 1600000 && sid > 1500000)
            {
               
                foreach (Graphic g in Tml.Graphics)          //遍历终端图层思密达
                    if (Convert.ToInt32(g.Attributes["Lid"]) == sid)
                    {
                        g.Select();
                        MyMap.PanTo(g.Geometry);
                        MyFeatureDataForm.FeatureLayer = Ctrl;
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
            else if (sid < 1600000000 && sid > 1500000000)
            {
                int ctrId;
                int concentratorId;
                ctrId = sid % 1000;
                concentratorId = Convert.ToInt32(sid / 1000);
                foreach (Graphic g in Ctrl.Graphics)          //遍历终端图层思密达
                {
                    if (!g.Attributes.ContainsKey("Lid")) return;
                    if (!g.Attributes.ContainsKey("Flid")) return;
                    if (Convert.ToInt32(g.Attributes["Flid"]) == concentratorId &&
                        Convert.ToInt32(g.Attributes["Lid"]) == ctrId)
                    {
                        g.Select();
                        MyMap.PanTo(g.Geometry);
                        MyFeatureDataForm.FeatureLayer = Ctrl;
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
        }

        public void ModifyFeaturesAttribute(int sid, string attributeName, string aValue) //修改图元属性呀
        {

            try
            {
                if (sid < 1100000 && sid > 1000000)
                {
                    foreach (Graphic g in Tml.Graphics)
                    {
                        //修改终端图层思密达
                        if (!g.Attributes.ContainsKey("Lid") || g.Attributes["Lid"]==null  ||string.IsNullOrEmpty(g.Attributes["Lid"].ToString()) ) return;
                        if (Convert.ToInt32(g.Attributes["Lid"]) == sid)
                        {
                            //if (attributeName == "Status")
                            //{
                            //    aValue = g.Attributes["Type"] + aValue;
                            //}
                            //if (g.Attributes[attributeName] == aValue) return;
                            if (!g.Attributes.ContainsKey(attributeName)) return;
                            g.Attributes[attributeName] = Convert.ToInt16(aValue);
                            //Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogInfo("arcMap:变图标 设备逻辑地址：" + sid + "改变状态为" + aValue);
                            break;
                        }
                    }

                    //Tml.SaveEdits();

                }
                else if (sid < 160000 && sid > 1500000)
                {
                    foreach (Graphic g in Tml.Graphics)
                    {
                        //修改集中器图层思密达
                        if (!g.Attributes.ContainsKey("Lid") || g.Attributes["Lid"] == null || string.IsNullOrEmpty(g.Attributes["Lid"].ToString())) return;
                        if (Convert.ToInt32(g.Attributes["Lid"]) == sid)
                        {
                            //if (attributeName == "Status")
                            //{
                            //    aValue = g.Attributes["Type"] + aValue;

                            if (g.Attributes[attributeName] == aValue) return;
                            g.Attributes[attributeName] = Convert.ToInt16(aValue);
                        }
                    }
                    //Tml.SaveEdits();
                }
                else if (sid < 1600000000 && sid > 1500000000)
                {
                    int ctrId;
                    int concentratorId;
                    ctrId = sid % 1000;
                    concentratorId = Convert.ToInt32(sid / 1000);
                    foreach (Graphic g in Ctrl.Graphics)
                    {
                        //遍历终端图层思密达
                     
                        if (!g.Attributes.ContainsKey("Flid") || !g.Attributes.ContainsKey("Lid") || g.Attributes["Flid"]==null ||g.Attributes["Lid"]==null  || string.IsNullOrEmpty(g.Attributes["Lid"].ToString()) || string.IsNullOrEmpty(g.Attributes["Flid"].ToString())) return;
                        if (Convert.ToInt32(g.Attributes["Flid"]) == concentratorId &&
                            Convert.ToInt32(g.Attributes["Lid"]) == ctrId)
                        {
                            //if (attributeName == "Status")
                            //{
                            //    aValue = g.Attributes["Type"] + aValue;
                            //}
                            if (g.Attributes[attributeName] == aValue) return;
                            g.Attributes[attributeName] = Convert.ToInt16(aValue);
                        }
                    }
                    //Controller.SaveEdits();
                }
            }
            catch(Exception ex)
            {
                Cr.Coreb.Servers .WriteLog.WriteLogError( "Error: "+ex);
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
                    foreach (Graphic g in Ctrl .Graphics) //遍历终端图层思密达
                    {
                        try
                        {
                            int sluIdinmap = Convert.ToInt32(g.Attributes["Flid"]);
                            if (sluIdinmap != sluId) continue;
                            int ctrlIdinmap = Convert.ToInt32(g.Attributes["Lid"]);
                            if (namevalue.ContainsKey(ctrlIdinmap))
                            {
                                if (g.Attributes[namevalue[ctrlIdinmap].Item1].ToString() == namevalue[ctrlIdinmap].Item2)
                                    continue;

                                g.Attributes[namevalue[ctrlIdinmap].Item1] =
                                    Convert.ToInt16(namevalue[ctrlIdinmap].Item2);
                            }
                        }
                        catch (Exception ex)
                        {
                            
                        }
                    }
                    Ctrl.SaveEdits();
               
            

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
            var rtu = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems[lid];
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
                MyFeatureDataForm.IsReadOnly = true ;
            }
            if (chkShowAttr.IsChecked == true) 
            {
                MyFeatureDataForm.Visibility = Visibility.Visible;

                //if (Tml.SelectionCount == 1)
                //{
                //    int x=Convert.ToInt32( Tml.SelectedGraphics.ToList()[0].Attributes["Lid"]);
                //    ShowPic(x); 
                //}
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
            EventPublisher.AddEventSubScriptionTokener(
                Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandlers, FundOrderFilters);

            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(OnLoadedRtu, 3, Wlst.Cr.Core.ModuleServices.DelayEventHappen.EventOne);

        }

        protected bool IsLoadOnlyOneArea = false;
        public void OnLoadedRtu()
        {
            var rtus = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.Keys;//.EquipmentInfoDictionary.Keys;
            //GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
            var userProperty = UserInfo.UserLoginInfo;
            List<int> areaLst = new List<int>();
            string idtml = "";
            string idctrl = "";
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

            if (userProperty.D != true)
            {

                if (IsLoadOnlyOneArea)
                {
                    int AreaId = areaLst[0];
                    var rtuLst = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(AreaId);
                    foreach (var rtuid in rtuLst)
                    {
                        if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(rtuid))
                            continue;
                        var tmlInfo = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuid];
                        if(tmlInfo.RtuModel== EnumRtuModel.Wj3005)
                        {
                            idtml = idtml + rtuid + ",";
                        }
                        else if (tmlInfo.RtuModel==EnumRtuModel.Wj2090)
                        {
                            idctrl = idctrl + rtuid + ",";
                        }
                        

                    }
                    if (idtml.Length >0)
                    {
                        idtml = idtml.Substring(0, idtml.Length - 1);
                        string tempTml = "Lid in (" + idtml + ")";
                        //Tml.Where = temp;
                        //Tml.Update();
                    }
                   if(idctrl.Length >0)
                   {
                       idctrl = idctrl.Substring(0, idctrl.Length - 1);

                       string tempCtrl = "Lid in (" + idctrl + ")";
                   }
                    
                
                }
                else
                {

                    foreach (var f in areaLst)
                    {
                        var lstInArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(f);

                        foreach (var rtuid in lstInArea)
                        {
                            if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(rtuid))
                                continue;
                            var tmlInfo = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuid];
                            if (tmlInfo.RtuModel == EnumRtuModel.Wj3005)
                            {
                                idtml = idtml + rtuid + ",";
                            }
                            else if (tmlInfo.RtuModel == EnumRtuModel.Wj2090)
                            {
                                idctrl = idctrl + rtuid + ",";
                            }


                        }
                        if (idtml.Length > 0)
                        {
                            idtml = idtml.Substring(0, idtml.Length - 1);
                            string tempTml = "Lid in (" + idtml + ")";
                            //Tml.Where = temp;
                            //Tml.Update();
                        }
                        if (idctrl.Length > 0)
                        {
                            idctrl = idctrl.Substring(0, idctrl.Length - 1);

                            string tempCtrl = "Lid in (" + idctrl + ")";
                        }
                    
                    }
                }
            }
            foreach (int id in rtus)
            {
                OnErrChanged(id);

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
                        //////var lst = args.GetParams()[0] as List<Tuple<int, int>>;
                        //////if (lst == null) return;
                        //////foreach (var t in lst)
                        //////{
                        //////    if (t.Item2 == 0)
                        //////    {
                        //////        //AddMainEquipment(t.Item1); 
                        //////    }
                        //////}
                        //////return;
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
                        ////var lst = args.GetParams()[0] as List<Tuple<int, int>>;
                        ////if (lst == null || lst.Count == 0) return;
                        ////foreach (var t in lst)
                        ////{

                        ////    if (t.Item2 == 0) { }
                        ////    //UpdateMainEquipment(t.Item1);
                        ////    var f =
                        ////                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[t.Item1];//.EquipmentInfoDictionary[t.Item1];
                        ////    foreach (Graphic g in Tml.Graphics)          //修改终端图层思密达
                        ////        if (Convert.ToInt32(g.Attributes["Lid"]) == t.Item1)
                        ////        {
                        ////            Graphic gt = new Graphic();
                        ////            gt.Geometry = ChangeCoodinateFrom84(f.RtuMapX, f.RtuMapY);
                        ////            if (g.Geometry != gt.Geometry)
                        ////            {
                        ////                g.Geometry = gt.Geometry;
                        ////                g.Geometry.SpatialReference = MyMap.SpatialReference;
                        ////            }
                        ////        }
                        ////}

                        ////return;
                    }
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipentxyPositonUpdateId)
                    {
                        //int rtuid = Convert.ToInt32(args.GetParams()[0]);

                        //if (!EquipmentDataInfoHold.InfoItems.ContainsKey(rtuid))//.EquipmentInfoDictionary.ContainsKey(rtuid))
                        //    return;
                        //if (EquipmentDataInfoHold.InfoItems[rtuid].RtuFid > 0) return;

                        //var f =
                        //    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuid];//.EquipmentInfoDictionary[rtuid];
                        //foreach (Graphic g in Tml.Graphics)          //修改终端图层思密达
                        //    if (Convert.ToInt32(g.Attributes["Lid"]) == rtuid)
                        //    {
                        //        g.Geometry = ChangeCoodinateFrom84(f.RtuMapX, f.RtuMapY);
                        //        g.Geometry.SpatialReference = MyMap.SpatialReference;
                        //        break;
                        //    }


                    }
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                    {

                        if (Convert.ToString(args.EventAttachInfo) == "gis") return;
                        int x = Convert.ToInt32(args.GetParams()[0]);//终端、集中器逻辑地址
                        TreeSelectId = x;
                        if (args.GetParams().Count > 1)
                        {
                            int y = Convert.ToInt32(args.GetParams()[1]);//控制器序号
                            x = x * 1000 + y;
                            TreeSelectId = x;
                        }
                        LocateSth(x);
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

                                    idt = idt + rtuid + ",";

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
                                string temp = "Lid in (" + idt + ")";
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
                            foreach (Graphic g in Ctrl.Graphics) //遍历终端图层获取灯杆类型
                            {
                                haveFeature = false ;
                                if (Convert.ToInt32(g.Attributes["Flid"]) == sluId &&
                                    Convert.ToInt32(g.Attributes["Lid"]) == ctrl)
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
                                        os = os/100*100;
                                    }
                                    haveFeature = true;
                                    break;
                                }
                            }
                            if (haveFeature == false) continue;
                           
                            if (sludataIcon.ContainsKey(ctrl))
                            {
                                string errorindex = "0"; //todo
                                var ctrldataIcon = sludataIcon[ctrl];
                                
                                if ( ctrldataIcon.IsIconUseRtuStateTo) //跟终端走
                                {
                                    if(ctrldataIcon.RtuState==1)//1.全开  2。全关
                                    {
                                        errorindex = (os + 10).ToString();    //10 开灯 
                                    }
                                    if (ctrldataIcon.RtuState==2)
                                    {
                                        errorindex = (os + 11).ToString(); //11 关灯
                                    }
                                    if (ctrldataIcon.RtuState == 3)
                                    {
                                        errorindex = os.ToString(); //不变
                                    }
                                    if (!namevalue.ContainsKey(ctrl))
                                    {
                                        namevalue.Add(ctrl,new Tuple<string, string>("TYPE",errorindex));
                                    }
                                    else
                                    {
                                        namevalue[ctrl]=new Tuple<string, string>("TYPE", errorindex);
                                    }
                                    //ModifyFeaturesAttribute(lid, "TYPE", errorindex);
                                    continue;
                                }


                                if (ctrldataIcon.UnConnected == true)
                                {
                                    errorindex = os.ToString();//100 200通讯故障
                                }
                                else if (ctrldataIcon.Errors.Count > 0 && ctrldataIcon.Errors[0] !=0)
                                {
                                    var error = ctrldataIcon.Errors[0];
                                    if(CtrlIcon.ContainsKey(error))
                                    {
                                        if (CtrlIcon[error] == 99)
                                        {
                                            if (ctrldataIcon.states != 3) errorindex = (os + 12).ToString(); //12 开灯通用故障
                                            if (ctrldataIcon.states == 3) errorindex = (os+13).ToString();//13 关灯通用故障
                                            
                                        }else
                                        {
                                            errorindex = (os + CtrlIcon[error]).ToString();  //-54  55-功率越上限 56功率越下限  57灯具漏电 58光源故障  59补偿电容故障 60意外灭灯  61意外亮灯  62自熄灯 63 控制器断电告警  64 继电器故障
                                            //error1-光源故障 error2-补偿电容故障 error3-意外灭灯 error4-意外亮灯 error5-自熄灯 error6-控制器断电告警 error7-继电器故障
                                        }                                      
                                    }   
                                }
                                else    //正常亮灯 关灯
                                {
                                    if (ctrldataIcon.states != 3) errorindex = (os + 10).ToString(); //10 正常开灯
                                    if (ctrldataIcon.states == 3) errorindex = (os + 11).ToString();//11 正常关灯
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
                        }
                        ModifyFeaturesAttribute2(sluId,namevalue);
                    }
                    //单灯最新数据
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.RunningInfoUpdate2)//Wj2090Module.Services.EventIdAssign. && args.EventType == PublishEventType.Core) //单灯最新数据
                   {

                        //////if (args.GetParams().Count < 2) return;
                        //////var rtus = args.GetParams()[0] as List<int>;
                        //////if (rtus == null || rtus.Count == 0) return;
                        //////int sluId = rtus[0];
                        //////if (sluId < Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.SluStart) return;
                        //////if (sluId > Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.SluEnd) return;



                        //////List<int> ctrls = args.GetParams()[1] as List<int>;
                        //////if (ctrls == null || ctrls.Count == 0) return;

                        //////var sludata = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(sluId);
                        //////if (sludata == null) return;
                        //////foreach (var f in ctrls)
                        //////{
                        //////    if (sludata.SluCtrlNewData.ContainsKey(f) == false) continue;
                        //////    var ctrldata = sludata.SluCtrlNewData[f];
                        //////    if (ctrldata == null || ctrldata.Data5 == null) return;
                        //////    var data5 = ctrldata.Data5;

                        //////    var errorUnContected = data5.Info.Status == 3 || data5.Info.DateTimeCtrl == 0;
                        //////    var error = data5.Info.Status != 0;

                        //////    bool isOnLight = false;
                        //////    bool errorLamp = false;
                        //////    bool isSaveEnergy = false;

                        //////    foreach (var fx in data5.Items)
                        //////    {
                        //////        if (!isOnLight && fx.StateWorkingOn != 3) isOnLight = true;
                        //////        if (!isSaveEnergy && (fx.StateWorkingOn == 1 || fx.StateWorkingOn == 2))
                        //////            isSaveEnergy = true;
                        //////        if (!errorLamp && fx.Fault > 0) errorLamp = true;

                        //////    }
                        //////    int lid = sluId * 1000 + f;
                        //////    string errorindex = "0";
                        //////    if (UxTreeSetting.IsRutsNotShowError == false)
                        //////    {
                        //////        if (errorUnContected) errorindex = "0";                //不在线
                        //////        if (isOnLight && !errorLamp) errorindex = "1";          //正常开灯
                        //////        if (isOnLight && errorLamp) errorindex = "2";           //开灯有故障
                        //////        if (isSaveEnergy && !errorLamp) errorindex = "3";       //正常节能
                        //////        if (isSaveEnergy && errorLamp) errorindex = "4";        //节能有故障
                        //////        if (!isOnLight && !errorLamp) errorindex = "5";         //正常关灯
                        //////        if (!isOnLight && errorLamp) errorindex = "6";          //关灯有故障
                        //////    }

                        //////    ModifyFeaturesAttribute(lid, "Status", errorindex);

                            //var tu = new Tuple<int, int>(sluId, f);
                            //if (Wlst.Ux.Wj2090Module.SrInfo.NewDataInfo.MySelf.InfoCtrl.ContainsKey(tu))
                            //{
                            //    var data = Wlst.Ux.Wj2090Module.SrInfo.NewDataInfo.MySelf.InfoCtrl[tu].Data5;
                            //    if (data == null) continue;

                            //    int lid = sluId * 1000 + f;

                            //    bool isLightOn = false;
                            //    bool hasError = false;
                            //    foreach (var g in data.Items)
                            //    {
                            //        if (g.Fault != 0) hasError = true;
                            //        if (g.StateWorkingOn != 3) isLightOn = true;

                            //        string errorindex = "0";
                            //        if (hasError && isLightOn) errorindex = "3";
                            //        if (hasError && !isLightOn) errorindex = "1";
                            //        if (!hasError && isLightOn) errorindex = "2";
                            //        ModifyFeaturesAttribute(lid, "Status", errorindex);

                            //    }
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
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.RunningInfoUpdate2)
                        return true;
                    if (args.EventId == Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuGroupSelectdWantedMapUp)
                        return true;
                    if (args.EventId == Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.MapNeedChangeIcon)
                        return true;

                }
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
            return false;
        }


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
                    var s = equ.RtuStateCode;
                    if (s == 0)
                    {
                        //     EquipmentImageState = 3011; //s
                        //todi  change EquipmentRtuId  picture
                        ModifyFeaturesAttribute(EquipmentRtuId, "Status", "0"); //不用
                        Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogInfo("SystemReceive变图标 设备逻辑地址：" + EquipmentRtuId + "改变状态为" + 0);
                        return;
                    }
                    if (s == 1)
                    {
                        //      EquipmentImageState = 3012;
                        ModifyFeaturesAttribute(EquipmentRtuId, "Status", "0");//停用
                        Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogInfo("SystemReceive变图标 设备逻辑地址：" + EquipmentRtuId + "改变状态为" + 0);
                        return;
                    }
                    bool haserror = false;
                    bool lighton = false;
                    var tmlinfo =
                        Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(EquipmentRtuId);
                    if (tmlinfo!=null)
                    {
                        haserror = tmlinfo.ErrorCount > 0;
                        lighton =tmlinfo.IsLightHasElectric;
                    }
                    // Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultLstInfoByRtuId(EquipmentRtuId);//.IsRtuHasError(EquipmentRtuId);
                    //var lighton = Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(EquipmentRtuId).IsLightHasElectric;//RtuNewDataService.IsRtuHasElectric(EquipmentRtuId);
                    string nname = equ.RtuName;
                    string errorindex = "1";  //宜兴 绿色 路灯 关灯正常状态
                    if (haserror && !lighton) errorindex = "2";  //宜兴 绿色 路灯 关灯故障
                    if (!haserror && lighton) errorindex = "3"; //宜兴 绿色 路灯 正常亮灯状态
                    if (haserror && lighton) errorindex = "4";  //宜兴 绿色 路灯 亮灯故障
                    if (errorindex == "1")
                    {
                        if (nname.Contains("亮化"))
                        {
                            errorindex = "5";//宜兴 灰白 亮化 正常关灯状态
                        }
                    }
                    if (errorindex == "2")
                    {
                        if (nname.Contains("亮化"))
                        {
                            errorindex = "6";//宜兴 灰白 亮化 关灯故障
                        }
                    }
                    if (errorindex == "3")
                    {
                        if (nname.Contains("亮化"))
                        {
                            errorindex = "7";//宜兴 灰白 亮化 正常亮灯状态
                        }
                    }
                    if (errorindex == "4")
                    {
                        if (nname.Contains("亮化"))
                        {
                            errorindex = "8";//宜兴 灰白 亮化 亮灯故障
                        }
                    }
                    Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogInfo("SystemReceive变图标 设备逻辑地址：" + EquipmentRtuId + "改变状态为" + errorindex);
                    ModifyFeaturesAttribute(EquipmentRtuId, "Status", errorindex);
                    //var tmps=  Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetRtuErrorsByRtuId(EquipmentRtuId);


                    //  EquipmentImageState = 3015 + errorindex;


                }

            }
        }

        #endregion





    }
}
