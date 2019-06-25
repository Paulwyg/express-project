using System.ComponentModel.Composition;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.Wj1090Module.LduInfoSet.Services;

namespace Wlst.Ux.Wj1090Module.LduInfoSet.View
{
    /// <summary>
    /// LduInfoSetView.xaml 的交互逻辑
    /// </summary>
    //[ViewExport(
    //    AttachNow = false,
    //    AttachRegion = Wj1090Module.Services.ViewIdAssign.LduInfoSetViewAttachRegion,
    //    ID = Wj1090Module.Services.ViewIdAssign.LduInfoSetViewId)]
    //[PartCreationPolicy(CreationPolicy.Shared)]
    public partial class LduInfoSetView
    {
        public LduInfoSetView()
        {
            InitializeComponent();
        }

        [Import]
        public IILduInfoSetView Model
        {
            get { return DataContext as IILduInfoSetView; }
            set { DataContext = value; }
        }
    }
}
