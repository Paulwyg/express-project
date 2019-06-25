using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
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

namespace Wlst.Ux.Wj2090Module.ZcInfo.SluTime
{
    /// <summary>
    /// ZcSluTimeView.xaml 的交互逻辑
    /// </summary>
    [ViewExport( Wj2090Module.Services.ViewIdAssign.ZcSluTimeViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class ZcSluTimeView : UserControl
    {
        public ZcSluTimeView()
        {
            InitializeComponent();
        }

        [Import]
        public IIZcSluTime Model
        {
            get { return DataContext as IIZcSluTime; }
            set { DataContext = value; }
        }
    }
}
