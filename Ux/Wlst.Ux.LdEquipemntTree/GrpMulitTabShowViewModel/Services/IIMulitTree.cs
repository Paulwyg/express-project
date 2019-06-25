using System.Collections.ObjectModel;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Ux.LdEquipemntTree.GrpComSingleMuliViewModel;

namespace Wlst.Ux.LdEquipemntTree.GrpMulitTabShowViewModel.Services 
{
    public interface IIMultiTree : IITab
    {
        ObservableCollection<TreeNodeBaseNode> ChildTreeItems { get; }
    }
}
