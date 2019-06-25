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
using Wlst.Ux.Wj1090Module.LduTreeSettingViewModel.Services;

namespace Wlst.Ux.Wj1090Module.LduTreeSettingViewModel.Views
{
    /// <summary>
    /// LduTreeSettingView.xaml 的交互逻辑
    /// </summary>


    [ViewExport(
  AttachNow = false,
  AttachRegion = Wj1090Module.Services.ViewIdAssign.LduTreeSettingViewAttachRegion,
  ID = Wj1090Module.Services.ViewIdAssign.LduTreeSettingViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class LduTreeSettingView : UserControl
    {
        public LduTreeSettingView()
        {
            InitializeComponent();
        }
        [Import]
        public IILduTreeSettingViewModel Model
        {

            get { return DataContext as IILduTreeSettingViewModel; }
            set { DataContext = value; }
        }
    }
}
