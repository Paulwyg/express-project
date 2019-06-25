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
using Wlst.Ux.StateBarModule.DataAreaController.Services;

namespace Wlst.Ux.StateBarModule.DataAreaController.View
{
    /// <summary>
    /// DataAreaController.xaml 的交互逻辑
    /// </summary>
    [ViewExport( StateBarModule.Services.ViewIdAssign.DataAreaControllerViewId,
    AttachNow = true,
    AttachRegion = StateBarModule.Services.ViewIdAssign.DataAreaControllerAttachRegion 
    )]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class DataAreaController : UserControl
    {
        public DataAreaController()
        {
            InitializeComponent();
        }

        [Import]
        public IIDataAreaController Model
        {
            get { return DataContext as IIDataAreaController; }
            set { DataContext = value; }
        }
    }
}
