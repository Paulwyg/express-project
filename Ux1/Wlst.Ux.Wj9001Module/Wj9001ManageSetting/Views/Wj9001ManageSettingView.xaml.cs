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
using Wlst.Ux.Wj9001Module.Wj9001ManageSetting.Services;

namespace Wlst.Ux.Wj9001Module.Wj9001ManageSetting.Views
{
    /// <summary>
    /// Wj9001ManageSettingView.xaml 的交互逻辑
    /// </summary>

    [ViewExport(Wj9001Module.Services.ViewIdAssign.Wj9001ManageSettingViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Wj9001ManageSettingView : UserControl
    {
        public Wj9001ManageSettingView()
        {
            InitializeComponent();
        }
        [Import]
        public IIWj90011ManageSettingViewModel Model
        {

            get { return DataContext as IIWj90011ManageSettingViewModel; }
            set { DataContext = value; }
        }
    }
}
