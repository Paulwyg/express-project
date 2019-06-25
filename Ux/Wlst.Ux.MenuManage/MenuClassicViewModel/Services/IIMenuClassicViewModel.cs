using System.Collections.ObjectModel;
using System.Windows.Input;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Ux.MenuManage.MenuClassicViewModel.ViewModel;

namespace Wlst.Ux.MenuManage.MenuClassicViewModel.Services
{
    public interface IIMenuClassicViewModel : IINavOnLoad, IITab ,IIOnHideOrClose
    {
        /// <summary>
        /// 基础本件列表
        /// </summary>
        ObservableCollection<MenuItemforClassis> MenusItems { get; }

        /// <summary>
        /// 菜单大类列表
        /// </summary>
        ObservableCollection<NameValueInt > MenuClassicItems { get; }

        /// <summary>
        /// 当前选中的菜单母版
        /// </summary>
        NameValueInt CurrentClassicItem { get; set; }

        ICommand CmdAdd { get; }

        ICommand CmdUpdage { get; }

        ICommand CmdDelete { get; }
    }
}
