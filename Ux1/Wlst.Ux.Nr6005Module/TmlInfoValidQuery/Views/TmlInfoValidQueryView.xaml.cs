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
using Wlst.Cr.Core.CoreServices;
using Wlst.Ux.Nr6005Module.TmlInfoValidQuery.Services;
using Wlst.Ux.Nr6005Module.TmlInfoValidQuery.ViewModel;

namespace Wlst.Ux.Nr6005Module.TmlInfoValidQuery.Views
{
    /// <summary>
    /// TmlInfoValidQueryView.xaml 的交互逻辑
    /// </summary>

    [ViewExport(Nr6005Module.Services.ViewIdAssign.TmlInfoValidQueryView)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class TmlInfoValidQueryView : UserControl
    {
        public TmlInfoValidQueryView()
        {
            InitializeComponent();
        }

        [Import]
        private IITmlInfoValidQuery Model
        {
            get { return DataContext as IITmlInfoValidQuery; }
            set
            {
                DataContext = value;
            }
        }

        private void InvalidTmlInfoGridview_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var listView = sender as Telerik.Windows.Controls.RadGridView;
            if (listView == null) return;
            var ggg = listView.CurrentCellInfo;
            if (ggg == null) return;
            var mvvm = ggg.Item as InvalidTmlModel;
            if (mvvm == null) return;

            RegionManage.ShowViewByIdAttachRegionWithArgu(
                Nr6005Module.Services.ViewIdAssign.Wj3005TmlInfoSetViewId,
                mvvm.RtuId);
        }
    }

}
