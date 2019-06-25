using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using Telerik.Windows.Controls.DragDrop;
using Telerik.Windows.Controls.Map;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.RadMapJpeg.MapJepg.Services;
using Wlst.Ux.RadMapJpeg.MapJepg.ViewModels;
using MercatorProjection = GMap.NET.Projections.MercatorProjection;

namespace Wlst.Ux.RadMapJpeg.Views
{


 

    //<summary>
    //MapJpegView.xaml 的交互逻辑
    //</summary>
    [ViewExport( RadMapJpeg.Services.ViewIdAssign.MapJpegViewId,AttachNow = true, 
        AttachRegion = RadMapJpeg.Services.ViewIdAssign.MapJpegViewAttachRegion)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class MapJpegView : UserControl
    {

        public MapJpegView()
        {
            InitializeComponent();


            if (MapJepg.ViewModels.RadMapJpegViewModelSet.MySelf.IsUserNetMap == false)
                this.MainMap.Manager.Mode = AccessMode.CacheOnly;
            // else this.MainMap.Manager.Mode = AccessMode.ServerOnly;

            if (MapJepg.ViewModels.RadMapJpegViewModelSet.MySelf.usebaidu == 1)
                this.MainMap.MapProvider = BaiduMapProvider.Instance;
            //GMapProviders.BaiduMap; //BaiduMapProvider.Instance; // GMapProviders.BaiduMap;
            else if (MapJepg.ViewModels.RadMapJpegViewModelSet.MySelf.usebaidu == 0)
                this.MainMap.MapProvider = GMapProviders.AMap;
            else if (MapJepg.ViewModels.RadMapJpegViewModelSet.MySelf.usebaidu == 2)
                this.MainMap.MapProvider = GMapProviders.GoogleChinaMap;
            else this.MainMap.MapProvider = GMapProviders.AMap;


   
            MainMap.DragButton = MouseButton.Left;
            MainMap.ShowTileGridLines = false;



            MainMap.MouseWheelZoomType = MouseWheelZoomType.MousePositionWithoutCenter; //.MousePositionWithoutCenter;
            MainMap.ShowCenter = false;

            MainMap.MouseLeftButtonDown += new MouseButtonEventHandler(MainMap_MouseLeftButtonDown);
            MainMap.MouseLeftButtonUp += new MouseButtonEventHandler(MainMap_MouseLeftButtonUp);

            this.MainMap.Zoom =4;
            this.MainMap.MinZoom = 4;
            this.MainMap.MaxZoom = 18;

            MainMap.Position = new PointLatLng(30, 120);
            MainMap.Position = new PointLatLng(19.26, 104.53);

            MainMap.CacheLocation = Environment.CurrentDirectory + "\\Map\\GMapCache";

            MainMap.IgnoreMarkerOnMouseWheel = true;
            MainMap.Loaded += new RoutedEventHandler(MainMap_Loaded);
            MainMap.OnMapZoomChanged += new MapZoomChanged(MainMap_OnMapZoomChanged);

        }

        void MainMap_OnMapZoomChanged()
        {
        //    MainMap.DragButton = MouseButton.Left;
            MainMap.CanDragMap = true ;
            //throw new NotImplementedException();
        }

        void MainMap_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (startload == false)
            {
                stratload1 = false;
                return;
            }
            if (startload && stratload1==false )
            {
                stratload1 = true;
                return;
            }
            stratload1 = false;
            startload = false;


            //throw new NotImplementedException();
            MainMap.CanDragMap = true ;
           

            RectLatLng area = MainMap.SelectedArea;
            if (!area.IsEmpty)
            {
                bool allload = false;
                MessageBoxResult resg =
                    MessageBox.Show("下载缩放比例  " + (int) MainMap.Zoom + " 到 " + MainMap.MaxZoom + " 的所有切片地图?", "WLST",
                                    MessageBoxButton.YesNo);
                {
                    allload = (resg == MessageBoxResult.Yes);
                }

                for (int i = (int) MainMap.Zoom; i <= MainMap.MaxZoom; i++)
                {

                    if (allload)
                    {
                        TilePrefetcher obj = new TilePrefetcher();
                        obj.Owner = Application.Current.MainWindow;
                        obj.ShowCompleteMessage = false;
                        obj.Start(area, i, MainMap.MapProvider, 100);
                        continue;
                    }


                    MessageBoxResult res = MessageBox.Show("即将下载缩放比例为 " + i + " 的切片地图?", "WLST",
                                                           MessageBoxButton.YesNoCancel);

                    if (res == MessageBoxResult.Yes)
                    {
                        TilePrefetcher obj = new TilePrefetcher();
                        obj.Owner = Application.Current.MainWindow;
                        obj.ShowCompleteMessage = true;
                        obj.Start(area, i, MainMap.MapProvider, 100);
                    }
                    else if (res == MessageBoxResult.No)
                    {
                        continue;
                    }
                    else if (res == MessageBoxResult.Cancel)
                    {
                        break;
                    }
                }

            }
            else
            {
                //    MessageBox.Show("", "GMap.NET", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private bool startload = false;
        private bool stratload1 = false;
        void MainMap_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (MainMap.Manager.Mode == AccessMode.CacheOnly) return;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Keyboard.Modifiers & ModifierKeys.Alt) > 0 && (Keyboard.Modifiers & ModifierKeys.Control) > 0)
            {

                startload = true;
               
                MainMap.CanDragMap = false;
            }
       
        }

        void MainMap_Loaded(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
          
            //MapManagerLoader.Instance.Load( Environment.CurrentDirectory + "\\Map\\GMapCache\\TileDBv5\\en\\Data.gmdb");
            if (this.Model != null) Model.SetGmap(this.MainMap);

           
        }

        [Import]
        public IIRadMapJpeg Model
        {
            get { return DataContext as IIRadMapJpeg; }
            set
            {
                DataContext = value;
                //  value.SetGmap(this.MainMap);
            }
        }

    }

    public abstract class BaiduMapProviderBase : GMapProvider
    {
        private string ClientKey = "1308e84a0e8a1fc2115263a4b3cf87f1";
        public BaiduMapProviderBase()
        {
            MaxZoom = null;
            RefererUrl = "http://map.baidu.com";
            //Copyright = string.Format("©{0} Baidu Corporation, ©{0} NAVTEQ, ©{0} Image courtesy of NASA", DateTime.Today.Year);
            Copyright = string.Format("©{0} CETC50 Corporation, CODE BY LP & XY , DIRECT BY SQY & YHJ & XK", DateTime.Today.Year);

        }

        public override PureProjection Projection
        {
            get { return MercatorProjection.Instance; }
        }

        GMapProvider[] overlays;
        public override GMapProvider[] Overlays
        {
            get
            {
                if (overlays == null)
                {
                    overlays = new GMapProvider[] { this };
                }
                return overlays;
            }
        }
    }

    public class BaiduMapProvider : BaiduMapProviderBase
    {
        public static readonly BaiduMapProvider Instance;

        readonly Guid id = new Guid("608748FC-5FDD-4d3a-9027-356F24A755E5");
        public override Guid Id
        {
            get { return id; }
        }

        readonly string name = "BaiduMap";
        public override string Name
        {
            get
            {
                return name;
            }
        }

        static BaiduMapProvider()
        {
            Instance = new BaiduMapProvider();
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            string url = MakeTileImageUrl(pos, zoom, LanguageStr);

            return GetTileImageUsingHttp(url);
        }

        string MakeTileImageUrl(GPoint pos, int zoom, string language)
        {
            zoom = zoom - 1;
            var offsetX = Math.Pow(2, zoom);
            var offsetY = offsetX - 1;

            var numX = pos.X - offsetX;
            var numY = -pos.Y + offsetY;

            zoom = zoom + 1;
            var num = (pos.X + pos.Y) % 8 + 1;
            var x = numX.ToString().Replace("-", "M");
            var y = numY.ToString().Replace("-", "M");

            //原来：http://q3.baidu.com/it/u=x=721;y=209;z=12;v=014;type=web&fm=44
            //更新：http://online1.map.bdimg.com/tile/?qt=tile&x=23144&y=6686&z=17&styles=pl
            string url = string.Format(UrlFormat, x, y, zoom);
            Console.WriteLine("url:" + url);
            return url;
        }


        static readonly string UrlFormat = "http://online1.map.bdimg.com/tile/?qt=tile&x={0}&y={1}&z={2}&styles=pl";

    }


}