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
using Wlst.Ux.Nr6005Module.LnEmergencyOperationCenter.Services;

namespace Wlst.Ux.Nr6005Module.LnEmergencyOperationCenter.Views
{
    /// <summary>
    /// LnEmergencyOperationCenterView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(Nr6005Module.Services.ViewIdAssign.NavToLnEmergencyOperationCenterViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class LnEmergencyOperationCenterView : UserControl
    {
        public LnEmergencyOperationCenterView()
        {
            InitializeComponent();
        }


        [Import]
        private IILnEmergencyOperationCenter Model
        {
            get { return DataContext as IILnEmergencyOperationCenter; }
            set
            {
                DataContext = value;
            }
        }


        void value_OnNavOnLoadSelectdRtus(object sender, EventArgs e)
        {
            //rtl.AutoExpandItems = true;
            //rtl.ExpandAllGroups();
            if (sender == null)
            {
                rtl.ExpandAllHierarchyItems();
            }
            else
            {
                rtl.CollapseAllHierarchyItems();
            }
            //throw new NotImplementedException();
        }
    }
}
