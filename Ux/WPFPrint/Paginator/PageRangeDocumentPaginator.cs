using System.Windows;
using System.Windows.Documents;

namespace HappyPrint.Paginator
{
    /// <summary>
    /// 这是个分页的。。。
    /// </summary>
    class PageRangeDocumentPaginator : DocumentPaginator
    {
        private int _startIndex;
        private int _endIndex;
        private DocumentPaginator _paginator;

        public PageRangeDocumentPaginator(int startIndex, int endIndex, DocumentPaginator paginator)
        {
            _startIndex = startIndex;
            _endIndex = endIndex;
            _paginator = paginator;
        }

        public override DocumentPage GetPage(int pageNumber)
        {
            return _paginator.GetPage(pageNumber + _startIndex);
        }

        public override bool IsPageCountValid
        {
            get
            {
                return _paginator.IsPageCountValid;
            }
        }

        public override int PageCount
        {
            get
            {
                return _endIndex - _startIndex + 1;
            }
        }

        public override Size PageSize
        {
            get
            {
                return _paginator.PageSize;
            }
            set
            {
                _paginator.PageSize = value;
            }
        }

        public override IDocumentPaginatorSource Source
        {
            get
            {
                return null;
            }
        }
    }
}
