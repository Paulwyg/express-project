using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Xps;
using HappyPrint.Command;
using HappyPrint.EventArgs;
using HappyPrint.Paginator;

namespace HappyPrint.ViewModels
{
    public class PrintPreviewViewModel : INotifyPropertyChanged
    {
        #region 字段
        private int _pageCount;
        private int _currentPageIndex;
        private DocumentPaginator _paginator;
        private PrintQueue _currentPrintQueue;
        private PageMediaSize _pageSize;
        private PrintConfig _config;
        private int _startIndex;
        private int _endIndex;
        private PrintQueueCollection _printQueues;
        private ReadOnlyCollection<PageMediaSize> _pageMediaSizes;
        private PageOrientation _pageOrientation;
        private Dispatcher _dispatcher;
        private XpsDocumentWriter _xpsDocWrite;
        private int _progressValue;
        private bool _controlStatus;
        private bool _showProgress;
        private bool _isIndeterminate;

        #endregion

        #region 属性
        /// <summary>
        /// 是否显示进度条
        /// </summary>
        public bool ShowProgress
        {
            get
            {
                return _showProgress;
            }
            set
            {
                _showProgress = value;
                OnPropertyChanged("ShowProgress");
            }
        }

        /// <summary>
        /// 控件的禁用状态
        /// </summary>
        public bool ControlStatus
        {
            get
            {
                return _controlStatus;
            }
            set
            {
                _controlStatus = value;
                OnPropertyChanged("ControlStatus");
            }
        }

        public int StartIndex
        {
            get
            {
                return _startIndex;
            }
            set
            {

                if (value <= EndIndex && value >= 0)
                {
                    _startIndex = value;
                    OnPropertyChanged("StartIndex");
                }
            }
        }

        public int EndIndex
        {
            get
            {
                return _endIndex;
            }
            set
            {
                if (value >= StartIndex && value < PageCount)
                {
                    _endIndex = value;
                    OnPropertyChanged("EndIndex");
                }
            }
        }

        public int PageCount
        {
            get
            {
                return _pageCount;
            }
            set
            {
                _pageCount = value;
                OnPropertyChanged("PageCount");
            }
        }

        public int CurrentPageIndex
        {
            get
            {
                return _currentPageIndex;
            }
            set
            {
                if (value >= 0 && value < PageCount && _currentPageIndex != value)
                {
                    _currentPageIndex = value;
                    OnPropertyChanged("CurrentPageIndex");
                }
            }
        }

        public PrintQueue CurrentPrintQueue
        {
            get
            {
                return _currentPrintQueue;
            }
            set
            {
                if (_currentPrintQueue != value)
                {
                    _currentPrintQueue = value;
                    OnPropertyChanged("CurrentPrintQueue");
                }
            }
        }

        public PrintQueueCollection PrintQueues
        {
            get
            {
                return _printQueues;
            }
            set
            {
                _printQueues = value;
                OnPropertyChanged("PrintQueues");
            }
        }

        public PageMediaSize PageSize
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
                    OnPropertyChanged("PageSize");
                    if (_paginator != null)
                    {
                        LoadPaginator();
                    }
                }
            }
        }



        public ReadOnlyCollection<PageMediaSize> PageMediaSizes
        {
            get
            {
                return _pageMediaSizes;
            }
            set
            {
                _pageMediaSizes = value;
                OnPropertyChanged("PageMediaSizes");
            }
        }

        public PageOrientation PageOrientation
        {
            get
            {
                return _pageOrientation;
            }
            set
            {
                _pageOrientation = value;
                OnPropertyChanged("PageOrientation");
                if (_paginator != null)
                {
                    LoadPaginator();
                }
            }
        }
        /// <summary>
        /// 进度值
        /// </summary>
        public int ProgressValue
        {
            get
            {
                return _progressValue;
            }
            set
            {
                _progressValue = value;
                OnPropertyChanged("ProgressValue");
            }
        }

        /// <summary>
        /// 进度条是否是数值不确定的
        /// </summary>
        public bool IsIndeterminate
        {
            get
            {
                return _isIndeterminate;
            }
            set
            {
                _isIndeterminate = value;
                OnPropertyChanged("IsIndeterminate");
            }
        }

        #endregion

        #region 事件
        /// <summary>
        /// 页面改变事件
        /// </summary>
        internal event EventHandler<PageChangedEventArgs> PageChanged;

        /// <summary>
        /// 页面加载事件
        /// </summary>
        internal event EventHandler PageLoading;

        #endregion

        #region 命令
        public ClickCommand LastPageCommand
        {
            get;
            set;
        }

        public ClickCommand NextPageCommand
        {
            get;
            set;
        }

        public ClickCommand PrintRangeCommand
        {
            get;
            set;
        }

        public ClickCommand PrintCurrentCommand
        {
            get;
            set;
        }

        public ClickCommand PrintAllCommand
        {
            get;
            set;
        }

        #endregion

        #region 委托
        public Action Close
        {
            get;
            set;
        }

        #endregion

        #region 构造

        public PrintPreviewViewModel(PrintConfig config)
        {
            _config = config;

            _dispatcher = Dispatcher.CurrentDispatcher;

            LoadPrintQueues();

            LoadPageMediaSize();

            LoadOrientation();

            LoadPaginator();

            InitCommand();
        }
        #endregion

        #region 加载基本数据
        private void InitCommand()
        {
            LastPageCommand = new ClickCommand(LastPage);

            NextPageCommand = new ClickCommand(NextPage);

            PrintRangeCommand = new ClickCommand(PrintRange);

            PrintCurrentCommand = new ClickCommand(PrintCurrentPage);

            PrintAllCommand = new ClickCommand(PrintAll);
        }

        /// <summary>
        /// 加载分页器
        /// </summary>
        private void LoadPaginator()
        {
            _paginator = PaginatorFactory.GetDocumentPaginator(_config);

            OnPageLoading();

            Task.Factory.StartNew(() =>
            {
                if (PageOrientation == PageOrientation.Portrait)
                {
                    _paginator.PageSize = new Size((int)PageSize.Width, (int)PageSize.Height);
                }
                else if (PageOrientation == PageOrientation.Landscape)
                {
                    _paginator.PageSize = new Size((int)PageSize.Height, (int)PageSize.Width);
                }

                PageCount = _paginator.PageCount;

                EndIndex = PageCount - 1;

                CurrentPageIndex = 0;

                _dispatcher.BeginInvoke(new Action(() => OnPageChanged(_paginator.GetPage(CurrentPageIndex))));
            });
        }

        /// <summary>
        /// 加载打印机和默认选中打印机
        /// </summary>
        private void LoadPrintQueues()
        {
            var server = new LocalPrintServer();

            PrintQueues = server.GetPrintQueues();

            CurrentPrintQueue =
                PrintQueues.FirstOrDefault(x => x.FullName == LocalPrintServer.GetDefaultPrintQueue().FullName);
        }



        /// <summary>
        /// 加载打印机支持的纸张类型
        /// </summary>
        private void LoadPageMediaSize()
        {
            PageMediaSizes = CurrentPrintQueue.GetPrintCapabilities().PageMediaSizeCapability;

            PageSize = PageMediaSizes.FirstOrDefault(x => x.PageMediaSizeName == _config.PageSizeName);
        }

        private void LoadOrientation()
        {
            PageOrientation = _config.PageOrientation;
        }

        #endregion

        #region 跳转页面
        /// <summary>
        /// 下一个页面
        /// </summary>
        public void NextPage()
        {
            CurrentPageIndex++;

            var documentPage = _paginator.GetPage(CurrentPageIndex);

            OnPageChanged(documentPage);
        }

        /// <summary>
        /// 上一个页面
        /// </summary>
        public void LastPage()
        {
            CurrentPageIndex--;

            var documentPage = _paginator.GetPage(CurrentPageIndex);

            OnPageChanged(documentPage);
        }

        #endregion

        #region 打印
        /// <summary>
        /// 打印当前页
        /// </summary>
        public void PrintCurrentPage()
        {
            PrintRange(CurrentPageIndex, CurrentPageIndex);
        }

        /// <summary>
        /// 打印范围
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        private void PrintRange(int startIndex, int endIndex)
        {
            if (startIndex >= 0 && endIndex < PageCount && startIndex <= endIndex)
            {
                var queueName = CurrentPrintQueue.FullName;

                //BitmapImage暂时没有办法异步打印
                if (this._paginator is DataTableDocumentPaginator)
                {
                    Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            ProgressValue = 0;

                            if (endIndex != 0)
                            {
                                ShowProgress = true;
                            }

                            ControlStatus = false;
                            IsIndeterminate = false;

                            var p = PaginatorFactory.GetDocumentPaginator(_config);

                            p.PageSize = new Size(_paginator.PageSize.Width, _paginator.PageSize.Height);

                            var server = new LocalPrintServer();

                            var queue = server.GetPrintQueue(queueName);

                            queue.UserPrintTicket.PageMediaSize = PageSize;

                            queue.UserPrintTicket.PageOrientation = PageOrientation;

                            var doc = PrintQueue.CreateXpsDocumentWriter(queue);

                            if (endIndex != 0)
                            {
                                doc.WritingProgressChanged += doc_WritingProgressChanged;
                            }

                            doc.Write(new PageRangeDocumentPaginator(startIndex, endIndex, p));

                            if (endIndex != 0)
                            {
                                doc.WritingProgressChanged -= doc_WritingProgressChanged;
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        finally
                        {
                            ControlStatus = true;
                            ShowProgress = false;

                            _dispatcher.BeginInvoke(new Action(() => Close()));
                        }
                    });
                }
                else
                {
                    ProgressValue = 0;

                    if (endIndex != 0)
                    {
                        ShowProgress = true;
                    }

                    ControlStatus = false;
                    IsIndeterminate = false;

                    var p = PaginatorFactory.GetDocumentPaginator(_config);

                    p.PageSize = new Size(_paginator.PageSize.Width, _paginator.PageSize.Height);

                    var server = new LocalPrintServer();

                    var queue = server.GetPrintQueue(queueName);

                    queue.UserPrintTicket.PageMediaSize = PageSize;

                    queue.UserPrintTicket.PageOrientation = PageOrientation;

                    var doc = PrintQueue.CreateXpsDocumentWriter(queue);

                    if (endIndex != 0)
                    {
                        doc.WritingProgressChanged += doc_WritingProgressChanged;
                    }

                    doc.Write(new PageRangeDocumentPaginator(startIndex, endIndex, p));

                    if (endIndex != 0)
                    {
                        doc.WritingProgressChanged -= doc_WritingProgressChanged;
                    }

                    ControlStatus = true;
                    ShowProgress = false;

                    _dispatcher.BeginInvoke(new Action(() => Close()));
                }
            }

        }

        public void PrintRange()
        {
            PrintRange(StartIndex, EndIndex);
        }

        /// <summary>
        /// 打印全部
        /// </summary>
        public void PrintAll()
        {
            PrintRange(0, PageCount - 1);
        }

        void doc_WritingProgressChanged(object sender, System.Windows.Documents.Serialization.WritingProgressChangedEventArgs e)
        {
            if (ProgressValue != 100)
            {
                int percent = Convert.ToInt32(((double)e.Number / EndIndex) * 100);

                ProgressValue = percent;
            }
            else
            {
                IsIndeterminate = true;
            }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 触发页面改变事件
        /// </summary>
        /// <param name="visual"></param>
        private void OnPageChanged(DocumentPage page)
        {
            if (PageChanged != null)
            {
                PageChanged(this, new PageChangedEventArgs(page));
            }
        }

        /// <summary>
        /// 页面加载事件
        /// </summary>
        private void OnPageLoading()
        {
            if (PageLoading != null)
            {
                PageLoading(this, System.EventArgs.Empty);
            }
        }
        #endregion

        #region 属性更改通知
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
