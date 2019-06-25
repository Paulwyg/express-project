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
using Wlst.Ux.Wj1050Module.Wj1050ManageSettingViewModel.Services;

namespace Wlst.Ux.Wj1050Module.Wj1050ManageSettingViewModel.Views
{
    /// <summary>
    /// Wj1050ManageSettingView.xaml 的交互逻辑
    /// </summary>


    [ViewExport( Wj1050Module.Services.ViewIdAssign.Wj1050ManageSettingViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Wj1050ManageSettingView : UserControl
    {
        public Wj1050ManageSettingView()
        {
            InitializeComponent();
        }
        [Import]
        public IIWj1050ManageSettingViewModel Model
        {

            get { return DataContext as IIWj1050ManageSettingViewModel; }
            set { DataContext = value; }
        }
    }
}
