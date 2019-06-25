using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.SinglePlan.SinglePlan.Services;

namespace Wlst.Ux.SinglePlan.SinglePlan.View
{
    /// <summary>
    /// SinglePlanView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(Wlst.Ux.SinglePlan.Services.ViewIdAssign.SinglePlanViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class SinglePlanView : UserControl
    {
        public SinglePlanView()
        {
            InitializeComponent();
        }
        [Import]
        public IISinglePlan Model
        {
            get { return DataContext as IISinglePlan; }
            set
            {
                DataContext = value;
            }
        }
    }
}
