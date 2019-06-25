using System.ComponentModel.Composition;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.PrivilegesManage.ModflyOtherUserInfoViewModel.Services;
using Wlst.Ux.PrivilegesManage.Services;

namespace Wlst.Ux.PrivilegesManage.ModflyOtherUserInfoViewModel.View
{
    /// <summary>
    /// ModflyOtherUserInfoView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(
        AttachNow = false,
        AttachRegion = ViewIdAssign.ModflyOtherUserInfoViewAttachRegion,
        ID = ViewIdAssign.ModflyOtherUserInfoViewId)]
    public partial class ModflyOtherUserInfoView
    {
        public ModflyOtherUserInfoView()
        {
            InitializeComponent();
        }

        [Import]
        public IIModflyOtherUserInfoViewModel Model
        {
            get { return DataContext as IIModflyOtherUserInfoViewModel; }
            set { DataContext = value; }
        }
    }
}
