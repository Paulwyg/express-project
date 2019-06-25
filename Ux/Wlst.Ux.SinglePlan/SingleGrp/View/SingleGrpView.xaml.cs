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
using Wlst.Ux.SinglePlan.SingleGrp.Services;
using Wlst.Ux.SinglePlan.SingleGrp.ViewModel;

namespace Wlst.Ux.SinglePlan.SingleGrp.View
{
    /// <summary>
    /// SingleGrpView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(Wlst.Ux.SinglePlan.Services.ViewIdAssign.SingleGrpViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class SingleGrpView : UserControl
    {
        private SingleGrpViewModel Model = null;
        public SingleGrpView()
        {
            InitializeComponent();
            Model = new SingleGrpViewModel();
            this.DataContext = Model;
            tree1.DataContext = Model.Tvc;
        }

        //[Import]
        //public SingleGrpViewModel Model
        //{
        //    get { return DataContext as SingleGrpViewModel; }
        //    set
        //    {
        //        DataContext = value;
        //    }
        //}
    }
}
