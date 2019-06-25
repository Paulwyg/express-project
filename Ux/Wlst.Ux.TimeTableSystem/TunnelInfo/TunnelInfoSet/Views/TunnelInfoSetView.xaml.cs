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
using Wlst.Ux.TimeTableSystem.TunnelInfo.TunnelInfoSet.Services;

namespace Wlst.Ux.TimeTableSystem.TunnelInfo.TunnelInfoSet.Views
{
    /// <summary>
    /// TunnelInfoSetView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(TimeTableSystem.Services.ViewIdAssign.TunnelInfoSetViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class TunnelInfoSetView : UserControl
    {
        public TunnelInfoSetView()
        {
            InitializeComponent();
        }
        [Import]
        public IITunnelInfoSet Model
        {
            get { return DataContext as IITunnelInfoSet; }
            set
            {
                DataContext = value;
            }
        }

    }
}
