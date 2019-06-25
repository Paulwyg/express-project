using System;
using System.Data;
using System.IO;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using HappyPrint.Enum;
using HappyPrint.ViewModels;
using HappyPrint.Views;

namespace HappyPrint
{
    public class PrintHelper
    {
        public static void PrintDataTable(DataTable dt)
        {
            var config = new PrintConfig()
            {
                DataSource = dt,
                DataSourceType = DataSourceTypeDefine.DataTable
            };

            var viewModel = new PrintPreviewViewModel(config);

            var view = new PrintPreviewView(viewModel);

            view.ShowDialog();
        }

        public static void PrintPicture(BitmapImage bitmapImage)
        {
            var config = new PrintConfig()
            {
                DataSource = bitmapImage,
                DataSourceType = DataSourceTypeDefine.Image
            };

            var viewModel = new PrintPreviewViewModel(config);

            var view = new PrintPreviewView(viewModel);

            view.ShowDialog();
        }

        public static void PrintControl(FrameworkElement element)
        {
            //将控件转化为图片...
            var rt = new RenderTargetBitmap((int)element.ActualWidth, (int)element.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            rt.Render(element);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rt));
            var bitmapImage = new BitmapImage();

            using (Stream stream = new MemoryStream())
            {
                encoder.Save(stream);

                stream.Seek(0, SeekOrigin.Begin);
                stream.Position = 0;
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();

            }

            var config = new PrintConfig()
            {
                DataSource = bitmapImage,
                DataSourceType = DataSourceTypeDefine.Image
            };

            var viewModel = new PrintPreviewViewModel(config);

            var view = new PrintPreviewView(viewModel);

            view.ShowDialog();
        }
    }
}
