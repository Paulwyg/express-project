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
using Wlst.Ux.Wj6005Module.Jd601ManageSettingViewModel.Services;

namespace Wlst.Ux.Wj6005Module.Jd601ManageSettingViewModel.Views
{
    /// <summary>
    /// Jd601ManageSettingView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(Wj6005Module.Services.ViewIdAssign.Jd601ManageSettingViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Jd601ManageSettingView : UserControl
    {
        public Jd601ManageSettingView()
        {
            InitializeComponent();
        }
        [Import]
        public IIJd601ManageSettingViewModel Model
        {

            get { return DataContext as IIJd601ManageSettingViewModel; }
            set { DataContext = value; }
        }
    }
}
