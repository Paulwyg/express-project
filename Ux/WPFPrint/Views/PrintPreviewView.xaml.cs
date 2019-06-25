using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HappyPrint.EventArgs;
using HappyPrint.ViewModels;

namespace HappyPrint.Views
{
    /// <summary>
    /// Description for PrintPreview.
    /// </summary>
    partial class PrintPreviewView : Window
    {
        #region 字段
        private PrintPreviewViewModel _viewModel;
        #endregion

        #region 构造
        public PrintPreviewView(PrintPreviewViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;

            _viewModel.Close = this.Close;

            this.DataContext = viewModel;

            this.Loaded += PrintPreviewWindow_Loaded;
            this.Unloaded += PrintPreviewWindow_Unloaded;
        }

        #endregion

        #region PageChanged事件
        private void _printPreview_PageChanged(object sender, PageChangedEventArgs e)
        {
            drawingCanvas.Width = e.DocumentPage.Size.Width;
            drawingCanvas.Height = e.DocumentPage.Size.Height;

            drawingCanvas.RemoveAll();
            drawingCanvas.AddVisual(e.DocumentPage.Visual);

            Grid.IsEnabled = true;
        }

        void _viewModel_PageLoading(object sender, System.EventArgs e)
        {
            PageLoading();
        }
        #endregion

        #region 窗口加载/卸载事件
        void PrintPreviewWindow_Loaded(object sender, RoutedEventArgs e)
        {
            PageLoading();

            _viewModel.PageChanged += _printPreview_PageChanged;
            _viewModel.PageLoading += _viewModel_PageLoading;
        }

        void PrintPreviewWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            _viewModel.PageChanged -= _printPreview_PageChanged;
            _viewModel.PageLoading -= _viewModel_PageLoading;
        }

        private void PageLoading()
        {
            drawingCanvas.RemoveAll();

            drawingCanvas.Width = 200;
            drawingCanvas.Height = 200;

            DrawingVisual visual = new DrawingVisual();

            using (DrawingContext dc = visual.RenderOpen())
            {
                FormattedText text = new FormattedText("正在加载，请稍等......", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("微软雅黑"), 18, Brushes.Black);

                dc.DrawText(text, new Point(0, drawingCanvas.Height / 2));
            }

            drawingCanvas.AddVisual(visual);

            Grid.IsEnabled = false;
        }

        #endregion
    }
}