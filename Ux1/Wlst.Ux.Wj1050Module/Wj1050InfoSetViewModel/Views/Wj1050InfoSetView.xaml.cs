using System.ComponentModel.Composition;
using System.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.Wj1050Module.Wj1050InfoSetViewModel.Services;

namespace Wlst.Ux.Wj1050Module.Wj1050InfoSetViewModel.Views
{
    /// <summary>
    /// Wj1050InfoSetView.xaml 的交互逻辑 Wj1050ModuleWj1050InfoSetView
    /// </summary>
    [ViewExport( Wj1050Module .Services .ViewIdAssign .Wj1050InfoSetViewId )]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Wj1050InfoSetView : UserControl
    {
        public Wj1050InfoSetView()
        {
            InitializeComponent();
        }

        [Import]
        public IITmlInformationViewModel Model
        {
            get { return DataContext as IITmlInformationViewModel; }
            set { DataContext = value; }
        }
    }
}
