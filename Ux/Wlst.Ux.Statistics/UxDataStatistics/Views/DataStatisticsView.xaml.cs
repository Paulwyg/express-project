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
using Wlst.Ux.Statistics.UxDataStatistics.ViewModel;
using Wlst.Ux.Statistics.UxDataStatistics.Services;

namespace Wlst.Ux.Statistics.UxDataStatistics.Views
{
    /// <summary>
    /// DataStatisticsView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(Wlst.Ux.Statistics.Services.ViewIdAssign.UxDataStatisticsViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class DataStatisticsView : UserControl
    {
        public DataStatisticsView()
        {
            InitializeComponent();
        }


        [Import]
        public IIUxDataStatisticsModule Model
        {
            get
            {
                return DataContext as IIUxDataStatisticsModule;
            }
            set
            {
                DataContext = value;
            }
        }
    }
}
