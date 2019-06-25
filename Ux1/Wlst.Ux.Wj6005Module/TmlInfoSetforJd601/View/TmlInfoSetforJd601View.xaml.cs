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
using Wlst.Ux.Wj6005Module.TmlInfoSetforJd601.Services;

namespace Wlst.Ux.Wj6005Module.TmlInfoSetforJd601.View
{
    /// <summary>
    /// TmlInfoSetforJd601View.xaml 的交互逻辑
    /// </summary>
    [ViewExport(
        AttachNow = false,
        AttachRegion = Ux.Wj6005Module.Services.ViewIdAssign.Jd601TmlInfoSetViewIdAttachRegion,
        ID = Ux.Wj6005Module.Services.ViewIdAssign.Jd601TmlInfoSetViewIdViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class TmlInfoSetforJd601View : UserControl
    {
        public TmlInfoSetforJd601View()
        {
            InitializeComponent();
        }


        [Import]
        public IIJd601TmlInfoSet Model
        {
            get { return DataContext as IIJd601TmlInfoSet; }
            set { DataContext = value; }
        }
    }
}
