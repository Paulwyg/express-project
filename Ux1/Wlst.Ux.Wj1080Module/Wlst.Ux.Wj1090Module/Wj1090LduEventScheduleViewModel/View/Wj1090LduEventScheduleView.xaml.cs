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
using Wlst.Ux.Wj1090Module.Wj1090LduEventScheduleViewModel.Services;


namespace Wlst.Ux.Wj1090Module.Wj1090LduEventScheduleViewModel.View
{
    /// <summary>
    /// Wj1090LduEventScheduleView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(
      AttachNow = false,
      AttachRegion = Wj1090Module.Services.ViewIdAssign.Wj1090LduEventScheduleViewAttachRegion,
      ID = Wj1090Module.Services.ViewIdAssign.Wj1090LduEventScheduleViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Wj1090LduEventScheduleView : UserControl
    {
        public Wj1090LduEventScheduleView()
        {
            InitializeComponent();
        }


        [Import]
        public IIWj1090LduEventScheduleViewModel Model
        {
            get { return DataContext as IIWj1090LduEventScheduleViewModel; }
            set { DataContext = value; }
        }
    }
}
