using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Framework.Common.Controls
{
    public class PageChangingEventArgs : RoutedEventArgs
    {
        public int OldPageIndex { get; set; }
        public int NewPageIndex { get; set; }
        public bool IsCancel { get; set; }
    }
}
