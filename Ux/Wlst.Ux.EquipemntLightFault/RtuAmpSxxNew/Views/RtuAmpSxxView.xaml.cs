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
using Wlst.Ux.EquipemntLightFault.EquipmentFaultDefineSettingViewModel.RtuAmpSxxNew.Services;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultDefineSettingViewModel.RtuAmpSxxNew.Views
{
    /// <summary>
    /// RtuAmpSxxView.xaml 的交互逻辑
    /// </summary>
    [ViewExport( EquipemntLightFault.Services.ViewIdAssign.RtuAmpSxxViewNewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class RtuAmpSxxView : UserControl
    {
        public RtuAmpSxxView()
        {
            InitializeComponent();
        }

        [Import]
        public IIRtuAmpSxx Model
        {
            get { return DataContext as IIRtuAmpSxx; }
            set { DataContext = value; }
        }

        private void ButtonClick1To2(object sender, RoutedEventArgs e)
        {
            grid1.Visibility = Visibility.Hidden;
            grid2.Visibility = Visibility.Visible;
            grid3.Visibility = Visibility.Hidden;
            grid4.Visibility = Visibility.Hidden;
            grid5.Visibility = Visibility.Hidden;
        }

        private void ButtonClick2To3(object sender, RoutedEventArgs e)
        {
            grid1.Visibility = Visibility.Hidden;
            grid2.Visibility = Visibility.Hidden;
            grid3.Visibility = Visibility.Visible;
            grid4.Visibility = Visibility.Hidden;
            grid5.Visibility = Visibility.Hidden;
        }

        private void ButtonClick2To1(object sender, RoutedEventArgs e)
        {
            grid1.Visibility = Visibility.Visible;
            grid2.Visibility = Visibility.Hidden;
            grid3.Visibility = Visibility.Hidden;
            grid4.Visibility = Visibility.Hidden;
            grid5.Visibility = Visibility.Hidden;
        }

        private void ButtonClick3To4(object sender, RoutedEventArgs e)
        {
            grid1.Visibility = Visibility.Hidden;
            grid2.Visibility = Visibility.Hidden;
            grid3.Visibility = Visibility.Hidden;
            grid4.Visibility = Visibility.Visible;
            grid5.Visibility = Visibility.Hidden;
        }

        private void ButtonClick3To2(object sender, RoutedEventArgs e)
        {
            grid1.Visibility = Visibility.Hidden;
            grid2.Visibility = Visibility.Visible;
            grid3.Visibility = Visibility.Hidden;
            grid4.Visibility = Visibility.Hidden;
            grid5.Visibility = Visibility.Hidden;
        }

        private void ButtonClick4To3(object sender, RoutedEventArgs e)
        {
            grid1.Visibility = Visibility.Hidden;
            grid2.Visibility = Visibility.Hidden;
            grid3.Visibility = Visibility.Visible;
            grid4.Visibility = Visibility.Hidden;
            grid5.Visibility = Visibility.Hidden;
        }

        private void ButtonClick4To5(object sender, RoutedEventArgs e)
        {
            grid1.Visibility = Visibility.Hidden;
            grid2.Visibility = Visibility.Hidden;
            grid3.Visibility = Visibility.Hidden;
            grid4.Visibility = Visibility.Hidden;
            grid5.Visibility = Visibility.Visible;
        }

        private void ButtonClick5To4(object sender, RoutedEventArgs e)
        {
            grid1.Visibility = Visibility.Hidden;
            grid2.Visibility = Visibility.Hidden;
            grid3.Visibility = Visibility.Hidden;
            grid4.Visibility = Visibility.Visible;
            grid5.Visibility = Visibility.Hidden;
        }

        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
