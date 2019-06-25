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
using Wlst.Ux.Wj1090Module.LduInfoSet.Services;

namespace Wlst.Ux.Wj1090Module.LduInfoSet.View
{
    /// <summary>
    /// LinesInfo.xaml 的交互逻辑
    /// </summary>



    [ViewExport(
      AttachNow = false,
      AttachRegion = Wj1090Module.Services.ViewIdAssign.LduInfoSetViewAttachRegion,
      ID = Wj1090Module.Services.ViewIdAssign.LduInfoSetViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class LinesInfo
    {
        public LinesInfo()
        {
            InitializeComponent();
        }

        [Import]
        public IILduInfoSetView Model
        {
            get { return DataContext as IILduInfoSetView; }
            set { DataContext = value; }
        }
    }
}
