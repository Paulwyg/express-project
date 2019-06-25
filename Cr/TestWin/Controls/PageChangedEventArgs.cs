using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Framework.Common.Controls
{
    public class PageChangedEventArgs : RoutedEventArgs
    {
        public int CurrentPageIndex { get; set; }
    }
}
