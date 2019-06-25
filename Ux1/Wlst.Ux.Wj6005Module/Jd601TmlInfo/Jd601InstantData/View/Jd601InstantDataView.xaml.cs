using System.ComponentModel.Composition;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.Wj6005Module.Jd601TmlInfo.Jd601InstantData.Service;

namespace Wlst.Ux.Wj6005Module.Jd601TmlInfo.Jd601InstantData.View
{
    /// <summary>
    /// Jd601InstantDataView.xaml 的交互逻辑
    /// </summary>
    [ViewExport( Services.ViewIdAssign.Jd601InstantDataViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Jd601InstantDataView
    {
        public Jd601InstantDataView()
        {
            InitializeComponent();
        }
        [Import]
        public IIJd601InstantData Model
        {
            get { return DataContext as IIJd601InstantData; }
            set { DataContext = value; }
        }
    }
}
