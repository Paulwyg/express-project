using System.ComponentModel.Composition;
using System.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.Wj6005Module.Jd601TmlInfo.InfoSet.Services;

namespace Wlst.Ux.Wj6005Module.Jd601TmlInfo.InfoSet.Views
{
    /// <summary>
    /// Jd601TmlInfoView.xaml 的交互逻辑
    /// </summary>
    [ViewExport( Ux.Wj6005Module.Services.ViewIdAssign.Jd601TmlInfoViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Jd601TmlInfoView : UserControl
    {
        public Jd601TmlInfoView()
        {
            InitializeComponent();
        }


        [Import]
        public IIJd601TmlInfoView Model
        {
            get { return DataContext as IIJd601TmlInfoView; }
            set { DataContext = value; }
        }
    }
}
