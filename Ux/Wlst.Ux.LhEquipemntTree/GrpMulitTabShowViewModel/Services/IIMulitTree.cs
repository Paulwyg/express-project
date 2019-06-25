using System.Collections.ObjectModel;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Ux.LhEquipemntTree.GrpComSingleMuliViewModel;

namespace Wlst.Ux.LhEquipemntTree.GrpMulitTabShowViewModel.Services 
{
    public interface IIMultiTree : IITab
    {
        ObservableCollection<TreeNodeBaseNode> ChildTreeItems { get; }
    }
}
