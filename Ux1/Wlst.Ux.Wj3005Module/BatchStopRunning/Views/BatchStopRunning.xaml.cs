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
using Wlst.Ux.WJ3005Module.BatchStopRunning.Services;

namespace Wlst.Ux.WJ3005Module.BatchStopRunning.Views
{
    /// <summary>
    /// BatchStopRunning.xaml 的交互逻辑
    /// </summary>
    [ViewExport(WJ3005Module.Services.ViewIdAssign.NavToBatchStopView)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class BatchStopRunning : UserControl
    {
        public BatchStopRunning()
        {
            InitializeComponent();
        }


        [Import]
        private IIBatchStopRunning Model
        {
            get { return DataContext as IIBatchStopRunning; }
            set { DataContext = value; }
        }


    }
}
