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
using Wlst.Ux.Wj1080Module.Wj1080ManageSettingViewModel.Services;

namespace Wlst.Ux.Wj1080Module.Wj1080ManageSettingViewModel.Views
{
    /// <summary>
    /// Wj1080ManageSettingView.xaml 的交互逻辑
    /// </summary>
 
    [ViewExport( Wj1080Module.Services.ViewIdAssign.Wj1080ManageSettingViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Wj1080ManageSettingView : UserControl
    {
        public Wj1080ManageSettingView()
        {
            InitializeComponent();
        }
        [Import]
        public IIWj1080ManageSettingViewModel Model
        {

            get { return DataContext as IIWj1080ManageSettingViewModel; }
            set { DataContext = value; }
        }
    }
}
