using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Printing;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace HappyPrint.Paginator
{
    /// <summary>
    /// DataTable分页器
    /// </summary>
    class DataTableDocumentPaginator : DocumentPaginator
    {
        #region 字段
        private int _pageCount;
        private PrintConfig _config;
        private DataTable _dataTable;
        private Thickness _headmargin;
        private Thickness _margin;
        private int _rowsCount;//每页行数
        private int _columnsCount;//每页列数
        private int _singleColumnPageCount;//单列子页数
        private int _singleRowPageCount;//单行所需页数
        private Pen _pen;
        private Size _pageSize;
        private double _width;//绘图区域的宽
        private double _height;//绘图区域的高
        private double _rowHeight;//行高
        private double _columnWidth;//列宽
        private Point _leftTop;
        private PageMediaSizeName? _pageSizeName;
        private List<int> _columnWidthList;//列表宽集合
        private List<PageInfo> _pageInfos;
        private List<List<int>> perPageColumns = new List<List<int>>();
        private string _timeString = string.Empty;
        #endregion

        #region 构造函数
        public DataTableDocumentPaginator(PrintConfig config)
        {
            _config = config;

            _margin = new Thickness(11, 11, 11, 11);

            _headmargin = new Thickness(10, 0, 10, 0);

            _leftTop = new Point(_margin.Left, _margin.Top);

            _pen = new Pen(Brushes.Black, 1);

            _pageInfos = new List<PageInfo>();

            _columnWidthList = new List<int>();

            _pageSize = new Size();

            if (config.DataSource == null)
            {
                throw new NullReferenceException("PrintConfig中的DataTable没有赋值");
            }

            _dataTable = config.DataSource as DataTable;

        }
        #endregion

        #region 私有方法

        private void CalculateRowCount()
        {
            _rowHeight = GetMaxRowHeight();

            var text = GetFormattedText("A");
            var title = GetTitleText("A");

            _rowsCount = (int)(Math.Floor((_height - text.Height - title.Height * 2) / Math.Ceiling(text.Height)));
            //表头也算一行
            _rowsCount -= 1;
        }

        /// <summary>
        /// 返回列宽集合
        /// </summary>
        private void GetColumnWidthList()
        {
            _columnWidthList.Clear();

            //求出每列的最大宽度作为本列的宽度
            foreach (DataColumn column in _dataTable.Columns)
            {
                var columnText = GetHeaderText(column.ColumnName);
                var maxWidth = (int)columnText.Width;

                foreach (DataRow row in _dataTable.Rows)
                {
                    var data = GetFormattedText(row[column].ToString());

                    if (data.Width > maxWidth)
                    {
                        maxWidth = (int)data.Width;
                    }
                }

                _columnWidthList.Add(Convert.ToInt32((maxWidth + _headmargin.Left * 2)));
            }
        }

        /// <summary>
        /// 计算纵向页数和横向页数，页码地图
        /// </summary>
        private void CalculateRowColumnPage()
        {
            //1.计算纵向页数，总行数/每页行数
            _singleColumnPageCount = (int)(Math.Ceiling((double)_dataTable.Rows.Count / _rowsCount));

            //2.计算横向页数

            //获取列宽集合
            GetColumnWidthList();

            //横向页数，每页能够容纳的列数
            //每页所包含集合序列的集合
            perPageColumns = new List<List<int>>();

            var tempColumnWidthList = _columnWidthList.GetRange(0, _columnWidthList.Count);

            var columnsIndex = new List<int>();

            var sum = 0;

            int index = 0;

            while (tempColumnWidthList.Count != 0)
            {
                if (sum + tempColumnWidthList[0] > _width)
                {
                    perPageColumns.Add(columnsIndex);

                    columnsIndex = new List<int>();

                    sum = 0;
                }
                else
                {
                    columnsIndex.Add(index++);

                    sum += tempColumnWidthList[0];

                    tempColumnWidthList.RemoveAt(0);
                }

            }

            //没有加够一页的
            perPageColumns.Add(columnsIndex);

            _singleRowPageCount = perPageColumns.Count;

            //总页数
            _pageCount = _singleColumnPageCount * _singleRowPageCount;

            //当没有值时，总是能够打印一个空白页
            if (_pageCount == 0)
            {
                _pageCount = 1;
            }
        }

        /// <summary>
        /// 计算每页的配置参数
        /// </summary>
        private void CalculatePerPageInfo()
        {
            _pageInfos.Clear();

            for (int i = 0; i < _singleRowPageCount; i++)
            {
                for (int j = 0; j < _singleColumnPageCount; j++)
                {
                    var pageInfo = new PageInfo();

                    pageInfo.RowStartIndex = j * _rowsCount;

                    pageInfo.ColumnsIndex = perPageColumns[i];

                    _pageInfos.Add(pageInfo);
                }
            }
        }

        //重新计算界面的一切参数
        private void Calculate()
        {
            _width = PageSize.Width - _margin.Left * 2;
            _height = PageSize.Height - _margin.Top * 2;

            //2.确定每页的行数，行高
            CalculateRowCount();

            //3.计算横向页数和纵向页数
            CalculateRowColumnPage();

            //4.计算每页的配置参数
            CalculatePerPageInfo();
        }

        /// <summary>
        /// 获取DrawText文本
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private FormattedText GetFormattedText(string text)
        {
            return new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, _config.Typeface, _config.FontSize, Brushes.Black);
        }

        private FormattedText GetHeaderText(string text)
        {
            return new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, _config.HeaderTypeface, _config.HeaderFontSize, Brushes.Black);
        }

        private FormattedText GetTitleText(string text)
        {
            return new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, _config.TitleTypeface, _config.TitleFontSize, Brushes.Black);
        }




        /// <summary>
        /// 获取行高
        /// </summary>
        /// <returns></returns>
        private int GetMaxRowHeight()
        {
            var text = GetFormattedText("A");
            return (int)(Math.Ceiling(text.Height));
        }

        #region 绘制标题

        private void DrawTitle(DrawingContext dc, string title, Point leftTop)
        {
            var text = GetTitleText(title);

            leftTop.X = leftTop.X + _width / 2 - text.Width / 2;

            dc.DrawText(text, leftTop);
        }

        #endregion

        #region 绘制表格
        private void DrawGrid(DrawingContext dc, Point leftTop, int pageNumber)
        {
            leftTop.Y += _rowHeight * 2;

            var pageInfo = _pageInfos[pageNumber];

            var height = _rowHeight * _rowsCount;
            var width = pageInfo.ColumnsIndex.Sum(index => _columnWidthList[index]);

            leftTop.X += (_pageSize.Width - _margin.Left * 2 - width) / 2;

            var sum = 0;

            //画纵线
            for (int i = 0; i < pageInfo.ColumnsIndex.Count; i++)
            {
                var starPoint = new Point(leftTop.X + sum, leftTop.Y);
                var endPoint = new Point(leftTop.X + sum, leftTop.Y + height);

                dc.DrawLine(_pen, starPoint, endPoint);

                sum += _columnWidthList[pageInfo.ColumnsIndex[i]];
            }

            //补齐最后一根线
            dc.DrawLine(_pen, new Point(leftTop.X + sum, leftTop.Y), new Point(leftTop.X + sum, leftTop.Y + height));


            //画横线
            for (int i = 0; i <= _rowsCount; i++)
            {
                var starPoint = new Point(leftTop.X, leftTop.Y + i * _rowHeight);
                var endPoint = new Point(leftTop.X + width, leftTop.Y + i * _rowHeight);

                dc.DrawLine(_pen, starPoint, endPoint);
            }

            sum = 0;

            //绘制表头
            for (int i = 0; i < pageInfo.ColumnsIndex.Count; i++)
            {
                var startPoint = new Point(leftTop.X + sum + _headmargin.Left, leftTop.Y);
                FormattedText headerText = GetHeaderText(_dataTable.Columns[pageInfo.ColumnsIndex[i]].ColumnName);
                dc.DrawText(headerText, startPoint);

                sum += _columnWidthList[pageInfo.ColumnsIndex[i]];
            }
        }
        #endregion

        #region 绘制数据
        private void DrawData(DrawingContext dc, Point leftTop, int pageNumber)
        {
            leftTop.X += _headmargin.Left;

            var pageInfo = _pageInfos[pageNumber];

            var width = pageInfo.ColumnsIndex.Sum(index => _columnWidthList[index]);

            leftTop.X += (_pageSize.Width - _margin.Left * 2 - width) / 2;

            var startPoint = new Point(leftTop.X, leftTop.Y + _rowHeight * 3);

            for (int i = 0; i < _rowsCount - 1; i++)
            {
                for (int j = 0; j < pageInfo.ColumnsIndex.Count; j++)
                {
                    if (i + pageInfo.RowStartIndex < _dataTable.Rows.Count)
                    {
                        var data = _dataTable.Rows[i + pageInfo.RowStartIndex][_dataTable.Columns[pageInfo.ColumnsIndex[j]].ColumnName].ToString();

                        if (!string.IsNullOrEmpty(data))
                        {
                            dc.DrawText(GetFormattedText(data), startPoint);
                        }

                        startPoint.X += _columnWidthList[pageInfo.ColumnsIndex[j]];
                    }
                }

                startPoint.Y += _rowHeight;
                startPoint.X = leftTop.X;
            }
        }
        #endregion

        #region 绘制脚注
        private void DrawFoot(DrawingContext dc, Point leftTop, int pageNumber)
        {
            var footnote = string.Format("第{0}页 共{1}页", pageNumber + 1, _pageCount);

            var text = GetFormattedText(footnote);

            leftTop.X = leftTop.X + _width / 2 - text.Width / 2;
            leftTop.Y = _height;

            dc.DrawText(text, leftTop);
        }
        #endregion

        #region 绘制时间戳

        private void DrawTimeStamp(DrawingContext dc, string timeString)
        {
            var text = GetFormattedText(timeString);

            var leftTop = new Point(_leftTop.X, _leftTop.Y)
            {
                X = _width - text.Width - _margin.Right,
                Y = _height
            };

            dc.DrawText(text, leftTop);
        }

        #endregion

        #endregion

        #region 重写
        public override DocumentPage GetPage(int pageNumber)
        {
            if (pageNumber == 0)
            {
                _timeString = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            }

            //绘制打印页
            DrawingVisual visual = new DrawingVisual();

            using (DrawingContext dc = visual.RenderOpen())
            {
                //背景区域
                dc.DrawRectangle(Brushes.White, null, new Rect(_pageSize));

                if (_dataTable.Rows.Count != 0)
                {
                    DrawTitle(dc, _dataTable.TableName, _leftTop);

                    DrawGrid(dc, _leftTop, pageNumber);

                    //绘制数据
                    DrawData(dc, _leftTop, pageNumber);


                    DrawFoot(dc, _leftTop, pageNumber);

                    DrawTimeStamp(dc, _timeString);
                }
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
                if (_pageSize != value)
                {
                    _pageSize = value;
                    Calculate();
                }
            }
        }

        public override IDocumentPaginatorSource Source
        {
            get
            {
                return null;
            }
        }
        #endregion

        #region 私有类
        private class PageInfo
        {
            /// <summary>
            /// 行起始位置
            /// </summary>
            public int RowStartIndex
            {
                get;
                set;
            }

            /// <summary>
            /// 本页所包含列集合序号
            /// </summary>
            public List<int> ColumnsIndex
            {
                get;
                set;
            }
        }
        #endregion
    }
}
