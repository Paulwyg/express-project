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
using Wlst.Ux.Wj1090Module.Wj1090ParaInfoSet.Services;

namespace Wlst.Ux.Wj1090Module.Wj1090ParaInfoSet.Views
{
    /// <summary>
    /// Wj1090paraInfoSet.xaml 的交互逻辑
    /// </summary>
    [ViewExport(
  AttachNow = false,
  AttachRegion = Wj1090Module.Services.ViewIdAssign.Wj1090ParaInfoSetViewAttachRegion,
  ID = Wj1090Module.Services.ViewIdAssign.Wj1090ParaInfoSetViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Wj1090paraInfoSet : UserControl
    {
        public Wj1090paraInfoSet()
        {
            InitializeComponent();
        }
        [Import]
        public IIWj1090ParaInfoSet Model
        {
            get { return DataContext as IIWj1090ParaInfoSet; }
            set { DataContext = value; }
        }

    }
}
