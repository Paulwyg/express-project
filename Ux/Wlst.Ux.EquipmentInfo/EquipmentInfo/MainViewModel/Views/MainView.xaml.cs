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
using Wlst.Ux.EquipmentInfo.EquipmentInfo.MainViewModel.Services;

namespace Wlst.Ux.EquipmentInfo.EquipmentInfo.MainViewModel.Views
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(Wlst.Ux.EquipmentInfo.Services.ViewIdAssign.MainViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
            terminal.DataContext = new TerminalInfoViewModel.ViewModel.TerminalInfoViewModel();
            singleLamp.DataContext = new SingleLampInfoViewModel.ViewModel.SingleLampInfoViewModel();
            lineDetection.DataContext = new LineDetectionInfoViewModel.ViewModel.LineDetectionInfoViewModel();
            electricMeter.DataContext = new ElectricMeterInfoViewModel.ViewModel.ElectricMeterInfoViewModel();
            Leakage.DataContext = new LeakageInfoViewModel.ViewModel.LeakageInfoViewModel();
        }

        [Import]
        public IIMainViewModel Model
        {
            get { return DataContext as IIMainViewModel; }
            set { DataContext = value; }
        }
    }
}
