using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.CoreInterface;
using System.Collections.ObjectModel;
using Wlst.Ux.Wj2090Module.Wj2090InfoSet.ViewModel;
using System.Windows.Input;

namespace Wlst.Ux.Wj2090Module.Wj2090InfoSet.Services
{
    public interface IIConcentratorParaInformationViewModel : IINavOnLoad, IITab, IIOnHideOrClose
    {
        // void LoadItems();
        //ObservableCollection<TreeItemGrplViewModel> ChildTreeItems { get; }
        //void AddChild(GroupInformation gi);
        //ObservableCollection<TreeItemGrpViewModel> ItemTmls { get; }

        //string HeaderInfo { get; }


        void TreeView_MouseRightButtonDown(object sender, MouseButtonEventArgs e);
        void TreeView_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e);
        string ShowSndInfo { get; set; }
        bool IsEnableCore { get; set; }
    }
}
