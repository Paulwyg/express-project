using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
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
using Wlst.Ux.EquipmentInfo.DailyStatistics.MainViewModel.Services;

namespace Wlst.Ux.EquipmentInfo.DailyStatistics.MainViewModel.Views
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(Wlst.Ux.EquipmentInfo.Services.ViewIdAssign.MainStatisticsViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
            terminal.DataContext = new TerminalViewModel.ViewModel.TerminalViewModel(tv1.RadChart2);
            SingleLamp.DataContext = new SingleLampViewModel.ViewModel.SingleLampViewModel(tv2.Fault);
            Leakage.DataContext = new LeakageViewModel.ViewModel.LeakageViewModel(tv3.leakage);

        }

        [Import]
        public IIMainViewModel Model
        {
            get { return DataContext as IIMainViewModel; }
            set { DataContext = value; }
        }

    }
}
