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
using Wlst.Ux.Statistics.RtuElectricityStatistics.Services;

namespace Wlst.Ux.Statistics.RtuElectricityStatistics.Views
{
    /// <summary>
    /// RtuElectricityStatistics.xaml 的交互逻辑
    /// </summary>
    /// 
    [ViewExport(Wlst.Ux.Statistics.Services.ViewIdAssign.UxRtuElectricityStatisticsViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class RtuElectricityStatistics : UserControl
    {
        public RtuElectricityStatistics()
        {
            InitializeComponent();
        }


        [Import]
        public IIRtuElectricityStatisticsViewModel Model
        {
            get { return DataContext as IIRtuElectricityStatisticsViewModel; }
            set { DataContext = value; }

        }
    }
}
