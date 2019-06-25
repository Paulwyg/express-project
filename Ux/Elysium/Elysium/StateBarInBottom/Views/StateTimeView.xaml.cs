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
using Wlst.Ux.StateBarModule.Services;
using Wlst.Ux.StateBarModule.StateBarInBottom.Services;

namespace Wlst.Ux.StateBarModule.StateBarInBottom.Views
{
    /// <summary>
    /// StateTimeView.xaml 的交互逻辑
    /// </summary>
    //[ViewExport(AttachNow = true, ID = ViewIdAssign.StateBarTimeViewId,
    //AttachRegion = ViewIdAssign.StateBarTimeViewAttachRegion)]
    //[PartCreationPolicy(CreationPolicy.Shared)]
    public partial class StateTimeView : UserControl
    {
        public StateTimeView()
        {
            InitializeComponent();
        }

        [Import]
        public IIStateTimeViewModule Model
        {
            get { return DataContext as IIStateTimeViewModule; }
            set { DataContext = value; }
        }
    }
}
