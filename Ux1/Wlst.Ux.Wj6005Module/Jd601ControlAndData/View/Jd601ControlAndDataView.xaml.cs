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
using Wlst.Ux.Wj6005Module.Jd601ControlAndData.Services;

namespace Wlst.Ux.Wj6005Module.Jd601ControlAndData.View
{
    /// <summary>
    /// Jd601ControlAndDataView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(
        AttachNow = false,
        AttachRegion = Ux.Wj6005Module.Services.ViewIdAssign.Jd601ControlAndDataViewAttachRegion,
        ID = Ux.Wj6005Module.Services.ViewIdAssign.Jd601ControlAndDataViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Jd601ControlAndDataView : UserControl
    {
        public Jd601ControlAndDataView()
        {
            InitializeComponent();
        }

        [Import]
        public IIJd601ControlAndData Model
        {
            get { return DataContext as IIJd601ControlAndData; }
            set { DataContext = value; }
        }
    }
}
