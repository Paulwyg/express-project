using System.Collections.ObjectModel;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Ux.EquipemntTree.GrpComSingleMuliViewModel;

namespace Wlst.Ux.EquipemntTree.GrpMulitTabShowViewModel.Services 
{
    public interface IIMultiTree : IITab
    {
        ObservableCollection<TreeNodeBaseNode> ChildTreeItems { get; }
    }
}
