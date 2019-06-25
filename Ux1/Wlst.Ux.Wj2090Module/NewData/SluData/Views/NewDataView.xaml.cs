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
using Wlst.Ux.Wj2090Module.NewData.Services;

namespace Wlst.Ux.Wj2090Module.NewData.Views
{
    /// <summary>
    /// NewDataView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(
    Wj2090Module.Services.ViewIdAssign.NewDataViewId,
     Wj2090Module.Services.ViewIdAssign.NewDataViewAttachRegion )]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class NewDataView : UserControl
    {
        public NewDataView()
        {
            InitializeComponent();
        }
        [Import]
        public IINewData Model
        {
            get { return DataContext as IINewData; }
            set
            {
                DataContext = value;
            }
        }
    }
}
