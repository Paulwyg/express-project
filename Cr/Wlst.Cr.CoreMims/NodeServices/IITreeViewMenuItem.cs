using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Wlst.Cr.CoreMims.NodeServices
{
    /// <summary>
    /// 菜单底层接口
    /// </summary>
    public interface IITreeViewMenuItem
    {
        /// <summary>
        /// 子菜单  
        /// </summary>
        ObservableCollection<IITreeViewMenuItem> CmItems { get; set; }

        /// <summary>
        /// 显示内容
        /// </summary>
        string Text { get; set; }


        /// <summary>
        /// 图片
        /// </summary>
        object Image { get; set; }


        /// <summary>
        /// 点击命令
        /// </summary>
        ICommand Command { get; set; }

    };
}
