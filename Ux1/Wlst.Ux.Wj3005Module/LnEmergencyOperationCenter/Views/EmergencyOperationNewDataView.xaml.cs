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
using Wlst.Ux.WJ3005Module.LnEmergencyOperationCenter.Services;

namespace Wlst.Ux.WJ3005Module.LnEmergencyOperationCenter.Views
{
    /// <summary>
    /// EmergencyOperationNewDataView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(WJ3005Module.Services.ViewIdAssign.NavToEmergencyOperationNewDataViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class EmergencyOperationNewDataView : UserControl
    {
        public EmergencyOperationNewDataView()
        {
            InitializeComponent();
        }



        [Import]
        private IIEmergencyOperationNewData Model
        {
            get { return DataContext as IIEmergencyOperationNewData; }
            set
            {
                DataContext = value;
            }
        }


    }
}
