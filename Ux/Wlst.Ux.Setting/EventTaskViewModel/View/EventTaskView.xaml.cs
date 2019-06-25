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
using Wlst.Ux.Setting.EventTaskViewModel.Services;
using Wlst.Ux.Setting.Services;

namespace Wlst.Ux.Setting.EventTaskViewModel.View
{
    /// <summary>
    /// EventTaskView.xaml 的交互逻辑
    /// </summary>
    [ViewExport( ViewIdAssign.EventTaskViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class EventTaskView : UserControl
    {
        public EventTaskView()
        {
            InitializeComponent();
        }

        [Import]
        public IIEventTaskViewModel Model
        {

            get { return DataContext as IIEventTaskViewModel; }
            set { DataContext = value; }
        }
    }
}
