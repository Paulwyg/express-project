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

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultDefineSettingViewModel.RtuAmpSxx
{
    /// <summary>
    /// RtuAmpSxxView.xaml 的交互逻辑
    /// </summary>
    [ViewExport( EquipemntLightFault.Services.ViewIdAssign.RtuAmpSxxViewId)]
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

        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            oneone.MinWidth = 75;
            onetwo.MinWidth = 75;
            onethree.MinWidth = 75;
            twoone.MaxWidth = 75;
            twotwo.MaxWidth = 75;
            twothree.MaxWidth = 75;
            two1.Visibility = Visibility.Visible;
            two2.Visibility = Visibility.Visible;
            two3.Visibility = Visibility.Visible;
            two4.Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            grid1.Visibility = Visibility.Hidden;
            grid2.Visibility = Visibility.Visible;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            grid2.Visibility = Visibility.Hidden;
            grid1.Visibility = Visibility.Visible;
        }

        private void RadGridView_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }





    }
}
