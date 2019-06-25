using System.Collections.ObjectModel;
using System.Windows.Input;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Ux.Wj2096Module.FieldGroupSet.ViewModel;

namespace Wlst.Ux.Wj2096Module.FieldGroupSet.Services
{
    public interface IIFieldGrpManage : IINavOnLoad, IITab 
    {

        ObservableCollection<TreeItemViewModel> ChildTreeItems { get; }

         ObservableCollection<ItemModel> ItemTmls { get; }

        string HeaderInfo { get; }


        void TreeView_MouseRightButtonDown(object sender, MouseButtonEventArgs e);
        void TreeView_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e);
    }
}
