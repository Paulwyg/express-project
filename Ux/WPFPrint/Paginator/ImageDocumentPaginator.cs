using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HappyPrint.Paginator
{
    /// <summary>
    /// 图片分页类
    /// </summary>
    class ImageDocumentPaginator : DocumentPaginator
    {

        #region 字段

        private BitmapImage _bitmapImage;
        private Thickness _margin;
        private Size _pageSize;
        private int _pageCount;
        private int _xPageCount;//x方向的页数
        private int _yPageCount;//y方向的页数
        private double _width;
        private double _height;
        private List<PageInfo> _pageInfos;
        private double _imageWidth;
        private double _imageHeight;
        private Point _leftTop;
        #endregion

        #region 构造

        public ImageDocumentPaginator(PrintConfig config)
        {
            _margin = new Thickness(11, 11, 11, 11);

            _pageSize = new Size();

            _bitmapImage = config.DataSource as BitmapImage;

            _imageHeight = _bitmapImage.Height;

            _imageWidth = _bitmapImage.Width;

            _leftTop = new Point(_margin.Left, _margin.Top);

            _pageInfos = new List<PageInfo>();
        }
        #endregion

        #region 私有方法

        private void Calculate()
        {
            //1.计算能够打印的区域大小
            _width = _pageSize.Width - _margin.Left * 2;
            _height = _pageSize.Height - _margin.Top * 2;

            //2.计算页数
            _xPageCount = (int)(Math.Ceiling(_imageWidth / _width));
            _yPageCount = (int)(Math.Ceiling((_imageHeight / _height)));

            _pageCount = _xPageCount * _yPageCount;

            _pageInfos.Clear();

            //计算每页信息
            for (int i = 0; i < _xPageCount; i++)
            {
                for (int j = 0; j < _yPageCount; j++)
                {
                    var pageInfo = new PageInfo();

                    pageInfo.LeftTop = new Point(_width * i, _height * j);

                    if (_imageHeight > _height)
                    {
                        if (_imageHeight - (j + 1) * _height > 0)
                        {
                            pageInfo.Height = _height;
                        }
                        else
                        {
                            pageInfo.Height = _height - (_height * (j + 1) - _imageHeight);
                        }
                    }
                    else
                    {
                        pageInfo.Height = _imageHeight;
                    }

                    if (_imageWidth > _width)
                    {
                        if (_imageWidth - (i + 1) * _width > 0)
                        {
                            pageInfo.Width = _width;
                        }
                        else
                        {
                            pageInfo.Width = _width - (_width * (i + 1) - _imageWidth);
                        }
                    }
                    else
                    {
                        pageInfo.Width = _imageWidth;
                    }

                    _pageInfos.Add(pageInfo);
                }
            }
        }

        #endregion

        #region 重写
        public override DocumentPage GetPage(int pageNumber)
        {
            DrawingVisual visual = new DrawingVisual();

            var pageInfo = _pageInfos[pageNumber];

            using (DrawingContext dc = visual.RenderOpen())
            {
                dc.DrawRectangle(Brushes.White, null, new Rect(_pageSize));

                CroppedBitmap clipBitmap = new CroppedBitmap(_bitmapImage, new Int32Rect((int)pageInfo.LeftTop.X, (int)pageInfo.LeftTop.Y, (int)pageInfo.Width, (int)pageInfo.Height));

                dc.DrawImage(clipBitmap, new Rect(_leftTop.X, _leftTop.Y, pageInfo.Width, pageInfo.Height));
            }

            return new DocumentPage(visual, _pageSize, new Rect(_pageSize), new Rect(_pageSize));
        }

        public override bool IsPageCountValid
        {
            get
            {
                return true;
            }
        }

        public override int PageCount
        {
            get
            {
                return _pageCount;
            }
        }

        public override Size PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value;
                Calculate();
            }
        }

        public override IDocumentPaginatorSource Source
        {
            get
            {
                throw null;
            }
        }
        #endregion

        #region 私有类
        /// <summary>
        /// 记录分页信息
        /// </summary>
        private class PageInfo
        {
            /// <summary>
            /// 左上角
            /// </summary>
            public Point LeftTop
            {
                get;
                set;
            }

            /// <summary>
            /// 宽
            /// </summary>
            public double Width
            {
                get;
                set;
            }

            /// <summary>
            /// 高
            /// </summary>
            public double Height
            {
                get;
                set;
            }
        }

        #endregion
    }
}
