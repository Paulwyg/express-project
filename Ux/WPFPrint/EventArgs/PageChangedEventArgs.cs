using System.Windows.Documents;

namespace HappyPrint.EventArgs
{
    class PageChangedEventArgs : System.EventArgs
    {
        public DocumentPage DocumentPage
        {
            get;
            set;
        }

        public PageChangedEventArgs(DocumentPage page)
        {
            DocumentPage = page;
        }
    }
}
