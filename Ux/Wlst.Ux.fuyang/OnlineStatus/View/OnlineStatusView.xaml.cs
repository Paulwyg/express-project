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
using Wlst.Ux.fuyang.OnlineStatus.Services;

namespace Wlst.Ux.fuyang.OnlineStatus.View
{
    /// <summary>
    /// OnlineStatusView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(fuyang.Services.ViewIdAssign.OnlineStatusViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class OnlineStatusView : UserControl
    {
        public OnlineStatusView()
        {
            InitializeComponent();
        }
        [Import]
        public IIOnlineStatus Model
        {
            get { return DataContext as IIOnlineStatus; }
            set
            {
                DataContext = value;
            }
        }
    }
}
