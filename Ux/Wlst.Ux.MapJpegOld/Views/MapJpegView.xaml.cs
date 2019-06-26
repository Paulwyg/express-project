using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Telerik.Windows.Controls.DragDrop;
using Telerik.Windows.Controls.Map;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.RadMapJpeg.MapJepg.Services;
using Wlst.Ux.RadMapJpeg.MapJepg.ViewModels;

namespace Wlst.Ux.RadMapJpeg.Views
{
     //<summary>
     //MapJpegView.xaml 的交互逻辑
     //</summary>
    [ViewExport(  RadMapJpeg .Services .ViewIdAssign .MapJpegViewId , RadMapJpeg .Services .ViewIdAssign .MapJpegViewAttachRegion,true  )]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class MapJpegView :UserControl
    {
        public double NorthOfImage { get; set; }

        public double WestOfImage { get; set; }

        public double SouthOfImage { get; set; }

        public double EastOfImage { get; set; }

        public Image Image = null;

        public Uri MapImageSource
        {
            get
            {
                var uri = new Uri(@"Map\osm_{zoom}.bmp");
                return uri;
            }
        }

        public  static double ImageIconWidth = 0;
       // private  double _imageSelectWidth = 0;

        public MapJpegView()
        {
            InitializeComponent();

            try
            {
                dtOnImageSelect = DateTime.Now.AddDays(-1);

                Location geoBoundsNW = new Location(0, 0);
                BitmapImage image = //ImageResource.GetTmlTreeIcon(1);
                    new BitmapImage(new Uri(@"pack://siteoforigin:,,,/Map/osm_16.bmp"));
                Size geoSize = this.RadMap1.GetGeoSize(geoBoundsNW, new Size(image.PixelWidth, image.PixelHeight));
                double height = image.Height;
                double width = image.Width;

                Size iSize = this.RadMap1.GetGeoSize(new Location(0, 0),
                                                     new Size(
                                                         RadMapJpeg.MapJepg.ViewModels.RadMapJpegViewModel.IconWidths,
                                                         RadMapJpeg.MapJepg.ViewModels.RadMapJpegViewModel.IconWidths));
                ImageIconWidth = iSize.Height;
                //  _imageSelectWidth = iSize.Height*1.5;

                this.RadMap1.Center = new Location(geoSize.Height/2, geoSize.Width/2);

                this.RadMap1.GeoBoundsNW = new Location(geoSize.Height, 0);
                this.RadMap1.GeoBoundsSE = new Location(0, geoSize.Width);
                this.ImagePri.GeoBoundsNW = new Location(geoSize.Height, 0);
                this.ImagePri.GeoBoundsSE = new Location(0, geoSize.Width);

                //Uri uri =new Uri( "pack://application:,,,/Maps/osm_16.png");

                this.RadMap1.MouseClickMode = MouseBehavior.None;
                ////this.ImagePri.Uri = new Uri("/RadMapJpeg;component/Maps/osm_{zoom}.png");

                //Location geoBoundsNW = new Location(NorthOfImage, WestOfImage);

                //BitmapImage image = //@"..\..\..\ResourceLibrary\Image\on.bmp";
                //    new BitmapImage(new Uri(@"Images\Maps\osm_12.png", UriKind.RelativeOrAbsolute));

                //Size geoSize = this.RadMap1.GetGeoSize(geoBoundsNW, new Size(image.PixelWidth, image.PixelHeight));

                //Location geoBoundsSE = new Location(geoBoundsNW.Latitude - geoSize.Height,
                //                                    geoBoundsNW.Longitude + geoSize.Width);

                //this.RadMap1.Center = new Location(geoBoundsNW.Latitude - geoSize.Height/2,
                //                                   geoBoundsSE.Longitude - geoSize.Width/2);

                //this.RadMap1.GeoBoundsNW = geoBoundsNW;

                //this.RadMap1.GeoBoundsSE = geoBoundsSE;

                //this.ImagePri.GeoBoundsNW = geoBoundsNW;

                //this.ImagePri.GeoBoundsSE = geoBoundsSE;
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("RaddMapJpeg Init Error :" + ex);
            }
        }

        [Import]
        public IIRadMapJpeg Model
        {
            get { return DataContext as IIRadMapJpeg; }
            set
            {
                DataContext = value;
                Model.OnSelectEquipmentChange = OnSelectEquipmentChange;
            }
        }

        private void OnSelectEquipmentChange(Location location)
        {
            //if(this .RadMap1 .GeoBoundsNW.Latitude  ) todo

            if (DateTime.Now.Ticks - dtOnImageSelect.Ticks < 10000000) return;
            this.RadMap1.Center = location;
        }

        #region  drag drop

        // OnDragQuery event handler
        private void OnDragQuery(object sender, DragDropQueryEventArgs e)
        {
            if (e.Options.Status == DragStatus.DragQuery && Keyboard.Modifiers==ModifierKeys.Control)
            {
                var draggedItem = e.Options.Source;
                e.QueryResult = true;
                e.Handled = true;
                //RadMap1.mous
                // Create Drag and Arrow Cue
                ContentControl dragCue = new ContentControl();
                dragCue.Content = draggedItem.DataContext;
                dragCue.ContentTemplate = this.gridMain.Resources["F1"] as DataTemplate;
                e.Options.DragCue = dragCue;
                e.Options.ArrowCue = RadDragAndDropManager.GenerateArrowCue();
                // Set the payload (this is the item that is currently dragged)
                e.Options.Payload = draggedItem.DataContext;
            }
            if (e.Options.Status == DragStatus.DropSourceQuery)
            {
                e.QueryResult = true;
                e.Handled = true;
            }
        }

        // OnDragInfo event handler
        private void OnDragInfo(object sender, DragDropEventArgs e)
        {
            if (e.Options.Status == DragStatus.DragComplete)
            {
                //var itemsControl = e.Options.Source.FindItemsConrolParent() as ItemsControl;
                //var itemsSource = itemsControl.ItemsSource as IList;
                //itemsSource.Remove(e.Options.Payload);
            }
        }


        // OnDropQuery event handler
        private void OnDropQuery(object sender, DragDropQueryEventArgs e)
        {
            if (e.Options.Status == DragStatus.DropDestinationQuery)
            {
                e.QueryResult = true;
                e.Handled = true;
            }
        }

        // OnDropInfo event handler
        private void OnDropInfo(object sender, DragDropEventArgs e)
        {
            if (e.Options.Status == DragStatus.DropComplete)
            {
                var f = e.Options.Payload as MapNodeViewModel;
                if (f == null) return;
                var fff = Location.GetCoordinates(RadMap1, e.Options.RelativeDragPoint);

                fff.Latitude += ImageIconWidth ;
                fff.Longitude -= ImageIconWidth;
                f.EquipmentLocation = fff;
                f.UpdateEquipmentLocation();
            }
        }

        #endregion

        private DateTime dtOnImageSelect;
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var v = sender as Image;
            if (v == null) return;
            var fff = v.DataContext as MapNodeViewModel;
            dtOnImageSelect = DateTime.Now;
            if (fff != null)
            {
                Model.CurrentSelectNode = fff;
                fff.OnEquipmentSelected();

            }
            // e.Handled = true;
        }


        private DateTime dt = DateTime.Now.AddHours(-1);
        private void RadMap1_ZoomChanged(object sender, EventArgs e)
        {
            dt = DateTime.Now;
           
            //Size iSize = this.RadMap1.GetGeoSize(new Location(0, 0), new Size(12, 12));
            //ImageIconWidth = iSize.Height;
        }

        private void RadMap1_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if(DateTime .Now .Ticks -dt.Ticks <15000000)
            {
                e.Handled = true;
                return;
            }
        }
    }
}