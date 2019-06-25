using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Ux.EquipmentGroupManage.GrpMulitManageViewModel.Models;
using Wlst.client;

namespace Wlst.Ux.EquipmentGroupManage.GrpMulitManageViewModel.Services
{
    public interface IIGrpMulitManageView : IINavOnLoad,IITab 
    {
       // void LoadItems();
        ObservableCollection<TreeItemViewModel> ChildTreeItems { get; }
        void AddChild(GroupItemsInfo.GroupItem gi);
        //event EventHandler AreaVisiChanged;
        string HeaderInfo { get; }


        void TreeView_MouseRightButtonDown(object sender, MouseButtonEventArgs e);
        void TreeView_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e);
    }
}
