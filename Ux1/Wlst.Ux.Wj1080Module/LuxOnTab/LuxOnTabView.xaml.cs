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

namespace Wlst.Ux.Wj1080Module.LuxOnTab
{
    /// <summary>
    /// LuxOnTabView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(Services.ViewIdAssign.LuxOnTabViewId,
     AttachRegion = Services.ViewIdAssign.LuxOnTabViewAttachRegion 
      )]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class LuxOnTabView : UserControl
    {
        public LuxOnTabView()
        {
            InitializeComponent();
        }
        [Import]
        public IILuxOnTabView Model
        {
            get { return DataContext as IILuxOnTabView; }
            set { DataContext = value; }
        }
    }
}
