using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wlst.Cr.Core.Behavior;
using System.ComponentModel.Composition;
using Wlst.Ux.Wj2096Module.ZcInfo.ZcConLocalArgs.Services;

namespace Wlst.Ux.Wj2096Module.ZcInfo.ZcConLocalArgs.Views
{
    /// <summary>
    /// XcConnLocalArgs.xaml 的交互逻辑
    /// </summary>
    [ViewExport(Wj2096Module.Services.ViewIdAssign.ZcConnLocalArgsViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class XcConnLocalArgs : UserControl
    {
        public XcConnLocalArgs()
        {
            InitializeComponent();
        }
        [Import]
        public IIXcConnLocalArgs Model
        {
            get { return DataContext as IIXcConnLocalArgs; }
            set { DataContext = value; }
        }
    }
}
