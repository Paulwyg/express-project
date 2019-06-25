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
using Elysium.ThemesSet.RadGridViewSet;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.Setting.SystemInformationViewModel.Services;

namespace Wlst.Ux.Setting.SystemInformationViewModel.View
{
    /// <summary>
    /// SystemInformationView.xaml 的交互逻辑
    /// </summary>
    [ViewExport( Setting.Services.ViewIdAssign.EventSystemInformationViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class SystemInformationView : UserControl
    {
        public SystemInformationView()
        {
            InitializeComponent();
        }

        [Import]
        public IISystemInformationViewModel Model 
        {
            get { return DataContext as IISystemInformationViewModel; }
            set { DataContext = value; }
        }


    }
}
