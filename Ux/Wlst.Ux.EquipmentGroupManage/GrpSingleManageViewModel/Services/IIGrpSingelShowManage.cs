using System.Collections.ObjectModel;
using System.Windows.Input;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Ux.EquipmentGroupManage.GrpSingleManageViewModel.ViewModels;

namespace Wlst.Ux.EquipmentGroupManage.GrpSingleManageViewModel.Services
{
    public interface IIGrpSingelShowManage : IINavOnLoad,IITab 
    {

        ObservableCollection<TreeItemViewModel> ChildTreeItems { get; }

         ObservableCollection<ItemModel> ItemTmls { get; }

        string HeaderInfo { get; }


        void TreeView_MouseRightButtonDown(object sender, MouseButtonEventArgs e);
        void TreeView_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e);
    }
}
