using System.Collections.ObjectModel;
using System.Windows.Input;
using DragDropExtend.DragAndDrop;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Ux.MenuManage.MenuInstanceRelationViewModel.ViewModel;

namespace Wlst.Ux.MenuManage.MenuInstanceRelationViewModel.Services
{
    public interface IIInstancesRelationManagement : IINavOnLoad, IITab ,IIOnHideOrClose 
    {
        /// <summary>
        /// 菜单母版
        /// </summary>
        ObservableCollection<NameValueInt> ClassicItems { get; }

        /// <summary>
        /// 当前选中的菜单母版
        /// </summary>
        NameValueInt CurrenSelectClassicItem { get; set; }


        /// <summary>
        /// 设备类型列表
        /// </summary>
        ObservableCollection<MenuInstancesViewModel> InstancesItems { get; }

        /// <summary>
        /// 当前选中的设备类型
        /// </summary>
        MenuInstancesViewModel CurrentSelectInstancesItem { get; set; }


        ObservableCollection<MenuItemBase> MenuItems { get; }


        MenuItemBase CurrentSelectMenuItem { get; set; }



        ObservableCollection<MenuTreeItemViewModel> ChildTreeItems { get; }

        ICommand CmdAdd { get; }

        ICommand CmdUpdage { get; }

        ICommand CmdDelete { get; }

        ICommand CmdSave { get; }

        /// <summary>
        /// 获取菜单树当前选择的菜单
        /// </summary>
        /// <returns></returns>
        MenuTreeItemViewModel GetSelectMvvm();


        IDragSource DragSourceList { get; }


        IDragSource DragSourceTree { get; }


        IDropTarget DropTargetTree { get; }
    }
}
