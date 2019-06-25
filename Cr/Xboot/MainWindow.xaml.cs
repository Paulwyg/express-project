using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
 
using Telerik.Windows.Controls;
using WindowForWlst;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.CoreOne.Services;
 

namespace Xboot
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    [Export]
    public partial class MainWindow : CustomChromeWindow
    {

        //protected override void OnSourceInitialized(EventArgs e)
        //{
        //    base.OnSourceInitialized(e);

        //    bool ForceSoftwareRendering = true;

        //    if (ForceSoftwareRendering)
        //    {

        //        HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;

        //        HwndTarget hwndTarget = hwndSource.CompositionTarget;

        //        hwndTarget.RenderMode = RenderMode.SoftwareOnly;

        //    }


        //}

        //WindowForWlst:CustomChromeWindow
        //private MenuResourcesModule.Services.RegisterShortCuts rsCuts;
        //private HitTestOnMouseMove hitTest;
        public MainWindow()
        {
            InitializeComponent();
            //  Elysium.Manager.Apply(Application.Current, Elysium.Theme.Light);
            //    System.Windows.Application.Current.MainWindow.Visibility = Visibility.Hidden;
            // fff = new Class1();

            //  I36N.Services.I36N.Myself.ChangeCulture("en-US",false );

            // Elysium.Manager.Apply(Application.Current, Elysium.Theme.Light);
            //hitTest = new HitTestOnMouseMove();

            //  this .ResizeMode=ResizeMode.NoResize;

         //   Wlst.Cr.Coreb.Servers.WriteLog.WriteError("");

            double x = SystemParameters.WorkArea.Width; //得到屏幕工作区域宽度
            double y = SystemParameters.WorkArea.Height; //得到屏幕工作区域高度
            double x1 = SystemParameters.PrimaryScreenWidth; //得到屏幕整体宽度
            double y1 = SystemParameters.PrimaryScreenHeight; //得到屏幕整体高度
            this.Width = x1 - 4; //设置窗体宽度
            this.Height = y1 - 4; //设置窗体高度

            this.Title = this.Ttle;
            //    this .ResizeMode =ResizeMode.NoResize;
            //   TalkIsVisi = Visibility.Visible ;
            TalkIsVisi = Visibility.Collapsed;
            MapIsVisi = Visibility.Collapsed;
            MsgIsVisi = Visibility.Collapsed;
            try
            {
                // The rendering tier corresponds to the high-order word of the Tier property.
                //int renderingTier = (RenderCapability.Tier >> 16);



                //var  hwndSource = PresentationSource.FromVisual(Application .Current.MainWindow);
                //var ggg=hwndSource as HwndSource;
                //if (ggg != null)
                //{
                //    HwndTarget hwndTarget = ggg .CompositionTarget;
                //    if (hwndTarget != null) hwndTarget.RenderMode = RenderMode.Default;
                //}
            }
            catch (Exception ex)
            {

            }
         

            this.DataContext = this;
            this.OnInit();

            if (MySelf == null) MySelf = this;
            UpdateAreaSet();

            Thread.CurrentThread.CurrentCulture = (CultureInfo) Thread.CurrentThread.CurrentCulture.Clone();

            Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongDatePattern = "yyyy-MM-dd HH:mm:ss";
            Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            this.OnInitWindows();
            //   Initializzze();

            InitMainWindowInfo();

            panegrop .MouseUp+=new System.Windows.Input.MouseButtonEventHandler(panegrop_MouseUp);

          //  Elysium.Manager.Apply(Application.Current, Elysium.Theme.Light);


            //this.GCTimer.Interval = TimeSpan.FromMinutes(10); //垃圾释放定时器 我定为每十分钟释放一次，大家可根据需要修改
            //this.GCTimer.Tick += new EventHandler(OnGarbageCollection);
            //this.GCTimer.Start() ;

        }

        void panegrop_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            foreach (var f in panegrop.RadPaneGroupOverride.EnumeratePanes())
            {
                if (f.IsActive)
                {
                    var str = f.Name;
                    str += "fsd";
                }
            }
        }
        DispatcherTimer GCTimer = new DispatcherTimer();
 

        //public void EventsRegistion()
        //{
        //    this.GCTimer.Tick += new EventHandler(OnGarbageCollection);
        //}
        //public void EventDeregistration()
        //{
        //    this.GCTimer.Tick -= new EventHandler(OnGarbageCollection);
        //}
        void OnGarbageCollection(object sender, EventArgs e)
        {
            try
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
            catch (Exception ex)
            {
                //
            }
        }



        //public void Initializzze()
        //{
        //    //new LocalProtocol().RegistProtocol();
        //    //  new LoginView().Show();
        //    //new Test.UserControl1().Show();
        //    return;

        //    Thread newWindowThread = new Thread(new ThreadStart(ThreadStartingPoint));
        //    newWindowThread.SetApartmentState(ApartmentState.STA);
        //    newWindowThread.IsBackground = true;
        //    newWindowThread.Start();
        //}
        //private void ThreadStartingPoint()
        //{
        //    Login.Login.LoginView tempWindow = new Login.Login.LoginView();
        //    tempWindow.Show();
        //    System.Windows.Threading.Dispatcher.Run();
        //}



        public static MainWindow MySelf;

        //class fff
        //{
        //    public  int x;
        //}

        private string title;

        public string Ttle
        {
            set
            {
                if (title == value) return;
                title = value;
                this.OnPropertyChanged("Ttle");
            }
            get
            {
                if (string.IsNullOrEmpty(title))
                {
                    title = GetTitle();
                }
                return title;
            }
        }

        
        private const string TitleSetPath = "Cetc50_System_Title";
        private const string TitleFilePath = "SystemXmlConfig";
        private string GetTitle()
        {
            var info = Elysium.ThemesSet.Common.ReadSave.Read(TitleSetPath,TitleFilePath);
            string res = "";
            if (info.ContainsKey("SystemTitle"))
            {
                res = info["SystemTitle"];
                return res;
            }
            else
            {
                res = "城市数字照明综合监控管理系统";
                try
                {
                    var temp = new Dictionary<string, string>();
                    temp.Add("SystemTitle", res);
                    Elysium.ThemesSet.Common.ReadSave.Save(temp, TitleSetPath);
                }
                catch (Exception)
                {

                }
                return res;

            }

        }

        protected override void OnClosed(EventArgs e)
        {
            //Application.Current.MainWindow.Visibility =Visibility.Hidden;
            base.OnClosed(e);
            Environment.Exit(11);
        }

        //private void ellipse_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
        //    AreaSet.AreaSeta info = new AreaSeta();
        //    //  info.Show();
        //}

        protected override void OnClosing(CancelEventArgs e)
        {
            var info = WlstMessageBox.Show("是否确定退出该应用程序？", WlstMessageBoxType.YesNo);
            if (info == WlstMessageBoxResults.No)
            {
                e.Cancel = true;
                return;
            }

            base.OnClosing(e);
        }

    }




    public partial class MainWindow
    {
        private void OnInitWindows()
        {
            EventPublish.AddEventTokener( 
                Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandler,
                FundOrderFilter);
        }

        #region IEventAggregator Subscription

        public void FundEventHandler(PublishEventArgs args)
        {
            if (args.EventType == "MainWindow.Measure.show")
            {
                DataIsVisi = Visibility.Visible;
            }
            if (args.EventType == "MainWindow.title.change")
            {
                try
                {
                    Ttle = args.GetParams()[0].ToString();
                }
                catch (Exception ex)
                {

                }
            }
            if (args.EventType == "MainWindow.update.windowsset")
            {
                try
                {
                    this .UpdateAreaSet();
                    //var sq = args.GetParams()[0] as Tuple<string, string, string, string, string, string,string >;
                    //var sqq = args.GetParams()[1] as Tuple<int, int, int, int, int, int>;
                    //if (sq == null) return;
                    //if (sqq == null) return;

                    //AreaSet.AreaSets.MySelf.Color1 = sq.Item1;
                    //AreaSet.AreaSets.MySelf.Color2 = sq.Item2;
                    //AreaSet.AreaSets.MySelf.Color3 = sq.Item3;
                    //AreaSet.AreaSets.MySelf.Color4 = sq.Item4;
                    //AreaSet.AreaSets.MySelf.Color5 = sq.Item5;
                    //AreaSet.AreaSets.MySelf.ColorBottom = sq.Item6;
                    //AreaSet.AreaSets.MySelf.ColorMenu = sq.Item7;
                    //AreaSet.AreaSets.MySelf.DataArea = sqq.Item1;
                    //AreaSet.AreaSets.MySelf.MsgArea = sqq.Item2;
                    //AreaSet.AreaSets.MySelf.Area1Wide = sqq.Item3;
                    //AreaSet.AreaSets.MySelf.Area35Wide = sqq.Item4;
                    //AreaSet.AreaSets.MySelf.Area45Height = sqq.Item5;
                    //AreaSet.AreaSets.MySelf.MainArea = sqq.Item6;
                    //AreaSet.AreaSets.MySelf.UpdateCurrentSet();
                }
                catch (Exception ex)
                {

                }
            }
            if (args.EventType == "MainWindow.update.msgisvisi")
            {
              //  MsgIsVisi = Visibility.Collapsed;
            }
            if (args.EventType == "MainWindow.chat.hiding")
            {
                TalkIsVisi = Visibility.Collapsed;
            }
            if (args.EventType == "MainWindow.chat.newmsg")
            {
                if (TalkIsVisi == Visibility.Visible) return;
                TalkHasNewMsg = true;
                //   TalkIsVisi = Visibility.Visible;
            }
        }

        /// <summary>
        /// 事件过滤
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool FundOrderFilter(PublishEventArgs args) //接收终端选中变更事件
        {
            if (args.EventType == "MainWindow.Measure.show")
            {
                return true;
            }
            if (args.EventType == "MainWindow.title.change")
            {
                return true;
            }
            if (args.EventType == "MainWindow.update.windowsset")
            {
                return true;
            }
            if (args.EventType == "MainWindow.update.msgisvisi")
            {
                return true;
            }
            if (args.EventType == "MainWindow.chat.hiding")
            {
                return true;
            }
            if (args.EventType == "MainWindow.chat.newmsg")
            {
                return true;
            }
            return false;
        }

        #endregion

    }

    public partial class MainWindow
    {

        private void OnInit()
        {
            Wlst.Cr.Core.CoreServices.RegionManage.RegionManagerInstances.Regions.CollectionChanged +=
                new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Regions_CollectionChanged);

        }

        private bool _addEvent = false;

        private void Regions_CollectionChanged(object sender,
                                               System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

            //throw new NotImplementedException();  
            if (!_addEvent)
            {
                if (
                    Wlst.Cr.Core.CoreServices.RegionManage.RegionManagerInstances.Regions.ContainsRegionWithName(
                        RegionNames.DocumentRegion))
                {
                    Wlst.Cr.Core.CoreServices.RegionManage.RegionManagerInstances.Regions[
                        RegionNames.DocumentRegion].ActiveViews.CollectionChanged +=
                        new System.Collections.Specialized.NotifyCollectionChangedEventHandler(
                            ActiveViews_CollectionChanged);

                    _addEvent = true;

                }
            }
        }


        private void ActiveViews_CollectionChanged(object sender,
                                                   System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //throw new NotImplementedException();

            if (panegrop.Items.Count > 0)
            {
                
                var ggg = panegrop.Items[0] as RadPaneGroup;
                if (ggg != null)
                {
                    if (asfsdf == false)
                    {
                        asfsdf = true;
                        ggg.OnItemsChangeded += new EventHandler(ggg_OnItemsChangeded);
                        // ggg.OnItemsChangeded(null, EventArgs.Empty);
                        ggg_OnItemsChangeded(ggg, EventArgs.Empty);
                    }
                }
            }
        }


        private bool asfsdf = false;

        private void ggg_OnItemsChangeded(object sender, EventArgs e)
        {
            // throw new NotImplementedException();
            var ggg = sender as RadPaneGroup;
            if (ggg != null)
            {
                if (ggg.Items.Count == 0)
                {
                    //  docking.Visibility = Visibility.Collapsed;
                    Gaussian = 0;

                    MapIsVisi = Visibility.Collapsed;
                }
                else
                {
                    MapIsVisi = Visibility.Visible;
                    Gaussian = 12;
                }
            }
        }

        //private void ggg_OnItemsChangeded_first(object sender, EventArgs e)
        //{
        //    // throw new NotImplementedException();
        //    var ggg = sender as RadPaneGroup;
        //    if (ggg != null)
        //    {
        //        if (ggg.Items.Count == 0)
        //        {
        //            docking.Visibility = Visibility.Collapsed;
        //            Gaussian = 0;
        //        }
        //        else
        //        {
        //            docking.Visibility = Visibility.Visible;
        //            Gaussian = 12;
        //        }
        //    }
        //}




    }

    public partial class MainWindow : INotifyPropertyChanged
    {


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        private static bool _first = true;
        public void UpdateAreaSet()
        {
            this.Color1 = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOption(3301, 1, Colors.Transparent.ToString(), "\\SystemColorAndFont"); //AreaSet.AreaSets.MySelf.Color1;
            this.Color2 = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOption(3301, 2, Colors.Transparent.ToString(), "\\SystemColorAndFont"); //AreaSet.AreaSets.MySelf.Color2;
            this.Color3 = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOption(3301, 3, Colors.Transparent.ToString(), "\\SystemColorAndFont"); //AreaSet.AreaSets.MySelf.Color3;
            this.Color4 = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOption(3301, 4, Colors.Transparent.ToString(), "\\SystemColorAndFont"); //AreaSet.AreaSets.MySelf.Color4;
            this.Color5 = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOption(3301, 5, Colors.Transparent.ToString(), "\\SystemColorAndFont"); //AreaSet.AreaSets.MySelf.Color5;
            ColorBottom = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOption(3301, 6, Colors.Transparent.ToString(), "\\SystemColorAndFont");// AreaSet.AreaSets.MySelf.ColorBottom;
            this.ColorMenu = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOption(3301, 7, Colors.Transparent.ToString(), "\\SystemColorAndFont");//AreaSet.AreaSets.MySelf.ColorMenu;

            Wlst.Cr.Core.CoreServices.RegionManage.IsNewViewInDocumentRegionPopup =
                Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(3301, 16, "\\SystemColorAndFont") == true;
            Wlst.Cr.Core.CoreServices.RegionManage.IsNewViewInDocumentRegionPopupUseDefaultWin =
                Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(3301, 17, "\\SystemColorAndFont") == true;

            if (_first)
            {
                _first = false;
                this.TreeIsVisi = Visibility.Visible;
                this.DataIsVisi = Visibility.Visible;
                this.MsgIsVisi = Visibility.Collapsed;
            }

            int dataarea = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(3301, 8, 4, "\\SystemColorAndFont");
            if (dataarea == 5)
            {
                DataColum = 1;
                DataColumSpan = 1;
            }
            else if (dataarea == 45)
            {
                DataColum = 0;
                DataColumSpan = 2;
            }
            else
            {
                DataColum = 0;
                DataColumSpan = 1;
            }

            int msgarea = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(3301, 9, 4, "\\SystemColorAndFont");
            switch (msgarea)
            {
                case 3:
                    MsgColum = 1;
                    MsgColumSpan = 1;
                    MsgRow = 0;
                    MsgRowSpan = 1;
                    MsgMaxHeight = 1000;
                    MsgPlacement = Dock.Top;
                    break;
                case 4:
                    MsgColum = 0;
                    MsgColumSpan = 1;
                    MsgRow = 1;
                    MsgRowSpan = 1;
                    MsgMaxHeight = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(3301, 12, 4, "\\SystemColorAndFont");
                    MsgPlacement = Dock.Bottom;
                    break;
                case 5:
                    MsgColum = 1;
                    MsgColumSpan = 1;
                    MsgRow = 1;
                    MsgRowSpan = 1;
                    MsgMaxHeight = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(3301, 12, 4, "\\SystemColorAndFont");
                    MsgPlacement = Dock.Bottom;
                    break;
                case 35:
                    MsgColum = 0;
                    MsgColumSpan = 2;
                    MsgRow = 0;
                    MsgRowSpan = 1;
                    MsgMaxHeight = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(3301, 12, 4, "\\SystemColorAndFont");
                    MsgPlacement = Dock.Top;

                    //MsgColum = 1;
                    //MsgColumSpan = 1;
                    //MsgRow = 0;
                    //MsgRowSpan = 2;
                    //MsgMaxHeight = 1000;
                    //MsgPlacement = Dock.Top;
                    break;
                case 45:
                    MsgColum = 0;
                    MsgColumSpan = 2;
                    MsgRow = 1;
                    MsgRowSpan = 1;
                    MsgMaxHeight = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(3301, 12, 4, "\\SystemColorAndFont");
                    MsgPlacement = Dock.Bottom;
                    break;
                default:
                    MsgColum = 1;
                    MsgColumSpan = 1;
                    MsgRow = 1;
                    MsgRowSpan = 1;
                    MsgMaxHeight = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(3301, 12, 4, "\\SystemColorAndFont");
                    MsgPlacement = Dock.Bottom;
                    break;
            }


            this.Area1Wide = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(3301, 11, 200, "\\SystemColorAndFont");
            this.Area35Wide = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(3301, 13, 5, "\\SystemColorAndFont");
            this.Area45Height = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(3301, 12, 4, "\\SystemColorAndFont");

            if (Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(3301, 10, 23, "\\SystemColorAndFont") == 2)
            {
                MainColumSpan = 1;
                MainRowsSpan = 1;
            }
            else if (Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(3301, 10, 23, "\\SystemColorAndFont") == 23)
            {
                MainRowsSpan = 2;
                MainColumSpan = 1;
            }
            else
            {
                MainRowsSpan = 2;
                MainColumSpan = 2;
            }

            Area3Wide = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(3301, 14, 23, "\\SystemColorAndFont");
            Area0Wide = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(3301, 15, 60, "\\SystemColorAndFont");
        }

        //   private const string XmlSetPath = "CETC50_DemoAreaSet";


        //private void SetDockMargin()
        //{
        //    return;
        //    if (ActualHeight < 850)
        //    {
        //        this.docking.Margin = new Thickness(5);
        //        return;
        //    }

        //    if (ActualWidth < 1400)
        //    {
        //        this.docking.Margin = new Thickness(5);
        //        return;
        //    }

        //    double dwith = 950; // Width * 0.7;
        //    double dheight = 700; // Height * 0.7;
        //    //if (dwith < 900) dwith = 900;
        //    //if (dheight < 650) dheight = 650;
        //    if (ActualWidth > 1900)
        //    {
        //        dwith = 1200;
        //        dheight = 750;
        //    }


        //    double topx = (this.ActualHeight - dheight)*4/11;
        //    double rigtx = (this.ActualWidth - dwith - Area1Wide)*7/11;
        //    double downx = (this.ActualHeight - dheight)*4/11;
        //    double leftx = (this.ActualWidth - dwith - Area1Wide)*7/11;
        //    if (DataIsVisi == Visibility.Visible)
        //    {
        //        downx = 10;
        //        topx = 10;
        //    }
        //    if (MsgIsVisi == Visibility.Visible && TreeIsVisi == Visibility.Visible)
        //    {
        //        leftx = 10;
        //        rigtx = 10;
        //    }
        //    else
        //    {
        //        if (TreeIsVisi == Visibility.Visible)
        //        {
        //            rigtx = (this.ActualWidth - dwith - Area1Wide)*4/11;
        //            leftx = (this.ActualWidth - dwith - Area1Wide)*7/11;

        //            if (DataIsVisi == Visibility.Visible)
        //            {
        //                rigtx = (this.ActualWidth - dwith - Area1Wide)*10/11;
        //                leftx = (this.ActualWidth - dwith - Area1Wide)*1/11;
        //            }
        //        }
        //        if (MsgIsVisi == Visibility.Visible)
        //        {
        //            rigtx = (this.ActualWidth - dwith - Area35Wide)*4/11;
        //            leftx = (this.ActualWidth - dwith - Area35Wide)*7/11;

        //            if (DataIsVisi == Visibility.Visible)
        //            {
        //                topx = 10;
        //                leftx = 0;
        //            }
        //        }
        //        if (MsgIsVisi != Visibility.Visible && TreeIsVisi != Visibility.Visible)
        //        {
        //            rigtx = (this.ActualWidth - dwith)/2;
        //            leftx = (this.ActualWidth - dwith)/2;
        //        }
        //    }
        //    if (leftx < 10) leftx = 5;
        //    if (rigtx < 10) rigtx = 5;
        //    if (topx < 10) topx = 5;
        //    if (downx < 10) downx = 5;

        //    this.docking.Margin = new Thickness(leftx, topx, rigtx, downx);
        //}
    }

    public partial class MainWindow
    {
        #region TalkIsVisi




        private Visibility _talkIsHide;

        public Visibility TalkIsVisi
        {
            get { return _talkIsHide; }
            set
            {
                if (_talkIsHide != value)
                {
                    _talkIsHide = value;
                    this.OnPropertyChanged("TalkIsVisi");
                    if (value == Visibility.Visible) TalkHasNewMsg = false;
                }
            }
        }


        private bool _talkIsnewmsg;

        public bool TalkHasNewMsg
        {
            get { return _talkIsnewmsg; }
            set
            {
                if (_talkIsnewmsg != value)
                {
                    _talkIsnewmsg = value;
                    this.OnPropertyChanged("TalkHasNewMsg");
                }
            }
        }

        #endregion


        #region 高斯模糊

        private int _Gaussian;

        public int Gaussian
        {
            get { return _Gaussian; }
            set
            {
                if (_Gaussian != value)
                {
                    _Gaussian = value;
                    this.OnPropertyChanged("Gaussian");
                }
            }
        }

        #endregion


        #region Color1

        private string _Color1;

        public string Color1
        {
            get { return _Color1; }
            set
            {
                if (_Color1 != value)
                {
                    _Color1 = value;
                    this.OnPropertyChanged("Color1");
                }
            }
        }


        #endregion

        #region Color2

        private string _Color2;

        public string Color2
        {
            get { return _Color2; }
            set
            {
                if (_Color2 != value)
                {
                    _Color2 = value;
                    this.OnPropertyChanged("Color2");
                }
            }
        }


        #endregion

        #region Color3

        private string _Color3;

        public string Color3
        {
            get { return _Color3; }
            set
            {
                if (_Color3 != value)
                {
                    _Color3 = value;
                    this.OnPropertyChanged("Color3");
                }
            }
        }


        #endregion

        #region Color4

        private string _Color4;

        public string Color4
        {
            get { return _Color4; }
            set
            {
                if (_Color4 != value)
                {
                    _Color4 = value;
                    this.OnPropertyChanged("Color4");
                }
            }
        }


        #endregion

        #region Color5

        private string _Color5;

        public string Color5
        {
            get { return _Color5; }
            set
            {
                if (_Color5 != value)
                {
                    _Color5 = value;
                    this.OnPropertyChanged("Color5");
                }
            }
        }


        #endregion

        #region ColorBottom

        private string _colorBottom;

        public string ColorBottom
        {
            get { return _colorBottom; }
            set
            {
                if (_colorBottom != value)
                {
                    _colorBottom = value;
                    OnPropertyChanged("ColorBottom");
                }
            }
        }

        #endregion


        #region ColorColorMenuMenu

        private string ColorColorMenuMenu;

        public string ColorMenu
        {
            get { return ColorColorMenuMenu; }
            set
            {
                if (ColorColorMenuMenu != value)
                {
                    ColorColorMenuMenu = value;
                    OnPropertyChanged("ColorMenu");
                }
            }
        }

        #endregion

        #region TreeIsVisi

        private Visibility _TreeIsHide;

        public Visibility TreeIsVisi
        {
            get { return _TreeIsHide; }
            set
            {
                if (_TreeIsHide != value)
                {
                    _TreeIsHide = value;
                    this.OnPropertyChanged("TreeIsVisi");
                    Xinfosizechanged();
                }
            }
        }


        #endregion


        #region MapIsVisi

        private Visibility _MapIsVisi;

        public Visibility MapIsVisi
        {
            get { return _MapIsVisi; }
            set
            {
                if (_MapIsVisi != value)
                {
                    if (value == Visibility.Visible)
                    {
                        if (panegrop != null && panegrop.Items.Count > 0)
                        {
                            var ggg = panegrop.Items[0] as RadPaneGroup;
                            if (ggg != null)
                            {
                                if (ggg.Items.Count == 0)
                                {
                                    value = Visibility.Collapsed;
                                }
                            }
                        }

                    }
                    _MapIsVisi = value;
                    this.OnPropertyChanged("MapIsVisi");
                    Xinfosizechanged();

                }
            }
        }


        #endregion


        #region DataIsVisi

        private Visibility _DataIsHide;

        public Visibility DataIsVisi
        {
            get { return _DataIsHide; }
            set
            {
                if (_DataIsHide != value)
                {
                    _DataIsHide = value;
                    this.OnPropertyChanged("DataIsVisi");
                    Xinfosizechanged();
                    //DataIsVisiChange();
                }
            }
        }

        //public void DataIsVisiChange()
        //{
        //    var args = new PublishEventArgs
        //    {
        //        EventType = "newdateheightshow",
        //        EventId = 1,
        //    };
        //    bool isshow = false;
        //    if (DataIsVisi == Visibility.Visible) isshow = true;
        //    args.AddParams(isshow);
        //    EventPublish.PublishEvent(args);
        //}
        #endregion

        #region MsgIsVisi

        private Visibility _MsgIsHide;

        public Visibility MsgIsVisi
        {
            get { return _MsgIsHide; }
            set
            {
                if (_MsgIsHide != value)
                {
                    _MsgIsHide = value;
                    this.OnPropertyChanged("MsgIsVisi");
                    Xinfosizechanged();
                }
            }
        }


        #endregion

        #region DataArea 4 5 45

        private int _DataArea;

        public int DataColum
        {
            get { return _DataArea; }
            set
            {
                if (_DataArea != value)
                {
                    _DataArea = value;
                    this.OnPropertyChanged("DataColum");
                }
            }
        }

        private int _DataColumSpan;

        public int DataColumSpan
        {
            get { return _DataColumSpan; }
            set
            {
                if (_DataColumSpan != value)
                {
                    _DataColumSpan = value;
                    this.OnPropertyChanged("DataColumSpan");
                }
            }
        }

        #endregion

        #region MsgArea 3 4 5 35 45

        private int _MsgArea;

        public int MsgColum
        {
            get { return _MsgArea; }
            set
            {
                if (_MsgArea != value)
                {
                    _MsgArea = value;
                    this.OnPropertyChanged("MsgColum");
                }
            }
        }

        private int _MsgColumSpan;

        public int MsgColumSpan
        {
            get { return _MsgColumSpan; }
            set
            {
                if (_MsgColumSpan != value)
                {
                    _MsgColumSpan = value;
                    this.OnPropertyChanged("MsgColumSpan");
                }
            }
        }


        private int _MsgArea1;

        public int MsgRow
        {
            get { return _MsgArea1; }
            set
            {
                if (_MsgArea1 != value)
                {
                    _MsgArea1 = value;
                    this.OnPropertyChanged("MsgRow");
                }
            }
        }

        private int _MsgRowSpan;

        public int MsgRowSpan
        {
            get { return _MsgRowSpan; }
            set
            {
                if (_MsgRowSpan != value)
                {
                    _MsgRowSpan = value;
                    this.OnPropertyChanged("MsgRowSpan");
                }
            }
        }


        private int _MsgMaxHeight;

        public int MsgMaxHeight
        {
            get { return _MsgMaxHeight; }
            set
            {
                if (_MsgMaxHeight != value)
                {
                    _MsgMaxHeight = value;
                    this.OnPropertyChanged("MsgMaxHeight");
                }
            }
        }



        private Dock _MsgPlacement;

        public Dock MsgPlacement
        {
            get { return _MsgPlacement; }
            set
            {
                if (_MsgPlacement != value)
                {
                    _MsgPlacement = value;
                    this.OnPropertyChanged("MsgPlacement");
                }
            }
        }

        #endregion

        #region MainArea 2 23 2345

        private int _mainArea;

        public int MainColumSpan
        {
            get { return _mainArea; }
            set
            {
                if (_mainArea != value)
                {
                    _mainArea = value;
                    this.OnPropertyChanged("MainColumSpan");
                }
            }
        }

        private int _mainArea11111;

        public int MainRowsSpan
        {
            get { return _mainArea11111; }
            set
            {
                if (_mainArea11111 != value)
                {
                    _mainArea11111 = value;
                    this.OnPropertyChanged("MainRowsSpan");
                }
            }
        }

        #endregion

        #region Area1Wide

        private int _Area1Wide;

        public int Area1Wide
        {
            get { return _Area1Wide; }
            set
            {
                if (_Area1Wide != value)
                {
                    if (value < 0) value = 0;
                    if (value > 500) value = 500;
                    _Area1Wide = value;
                    this.OnPropertyChanged("Area1Wide");
                }
            }
        }


        #endregion

        #region Area45Height

        private int _Area45Height;

        public int Area45Height
        {
            get { return _Area45Height; }
            set
            {
                if (_Area45Height != value)
                {
                    if (value < 100) value = 100;
                    if (value > 950) value = 950;
                    _Area45Height = value;
                    this.OnPropertyChanged("Area45Height");
                }
            }
        }


        #endregion

        #region Area35Wide

        private int _Area35Wide;

        public int Area35Wide
        {
            get { return _Area35Wide; }
            set
            {
                if (_Area35Wide != value)
                {
                    if (value < 100) value = 100;
                    if (value > 950) value = 950;
                    _Area35Wide = value;
                    this.OnPropertyChanged("Area35Wide");
                }
            }
        }


        #endregion


        #region Area3Wide 左侧信息

        private int _Area3Wide;

        public int Area3Wide
        {
            get { return _Area3Wide; }
            set
            {
                if (_Area3Wide != value)
                {
                    if (value < 100) value = 100;
                    if (value > 950) value = 950;
                    _Area3Wide = value;
                    this.OnPropertyChanged("Area3Wide");
                }
            }
        }


        #endregion

 

        #region _Area0Wide 左侧扼要信息宽度

        private int _Area0Wide;

        public int Area0Wide
        {
            get { return _Area0Wide; }
            set
            {
                if (_Area0Wide != value)
                {
                    // if (value < 40) value = 100;
                    if (value > 250) value = 250;
                    _Area0Wide = value;
                    this.OnPropertyChanged("Area0Wide");
                }
            }
        }


        #endregion
    }

    /// <summary>
    /// 最左侧的状态信息区域 数据提供
    /// </summary>
    public partial class MainWindow
    {
        public void InitMainWindowInfo()
        {
            var logx = Wlst.Cr.CoreOne.Services.ImageSourceHelper.MySelf.GetImageSourceById(10000103);
            if (logx != null) log.Source = logx;


            xinfo.SizeChanged += new SizeChangedEventHandler(xinfo_SizeChanged);
            xdata.SizeChanged += new SizeChangedEventHandler(xinfo_SizeChanged);
           //xtree .SizeChanged +=new SizeChangedEventHandler(xinfo_SizeChanged);
        }

        //定义委托
        public delegate void DoTask();

        void Xinfosizechanged()
        {
            xinfo_SizeChanged(null, null);
            return;
            try
            {
                Thread th = new Thread(Runx);
                th.Start();
            }
            catch (Exception ex)
            {
                
            }

        }

        void Runx()
        {
            Thread .Sleep(100);
            Application.Current.Dispatcher.Invoke(
            System.Windows.Threading.DispatcherPriority.Normal, new DoTask(Runrfx));
        }

        void Runrfx()
        {
            try
            {
                xinfo_SizeChanged(null, null);
            }
            catch (Exception ex)
            {

            }
        }

        void xinfo_SizeChanged(object sender, SizeChangedEventArgs e)
        {
         
            //if (ActualHeight < 850 || ActualWidth < 1400)
            {
                this.docking.Margin = new Thickness(5,5,5,5);
                return;
            }

            var xheigt = xgird.ActualHeight - xdata.ActualHeight;
            var xwidthx = xgird.ActualWidth - xinfo.ActualWidth;


            double dwith = 950;
            double dheight = 700;

            if (xheigt < dheight)
            {
                this.docking.Margin = new Thickness(5,5,5,5);
                return;
            }

            double topx = (xheigt - dheight)*5/11;
            double downx = (xheigt - dheight) * 6 / 11;

            var leftx = (xwidthx - dwith)*5/11;
            if (leftx < 5) leftx = 5;
            if (leftx > 200) leftx = 200;
            if (topx > 100) topx = 100;

            this.docking.Margin   = new Thickness(leftx, topx,0 , 0);
        }



    }
}