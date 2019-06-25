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
using Wlst.Ux.AssetManagementModule.Services;
using Wlst.Ux.AssetManagementModule.SimManage.Services;

namespace Wlst.Ux.AssetManagementModule.SimManage.Views
{
    /// <summary>
    /// SimManageView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(ViewIdAssign.SimManageViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class SimManageView : UserControl
    {
        public SimManageView()
        {
            InitializeComponent();
        }

        [Import]
        public IISimManage Model
        {
            get { return DataContext as IISimManage; }
            set { DataContext = value; }
        }
    }
}
