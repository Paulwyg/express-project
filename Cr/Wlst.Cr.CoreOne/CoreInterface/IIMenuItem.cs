using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;


namespace Wlst.Cr.CoreOne.CoreInterface
{
    /// <summary>
    /// 菜单底层接口
    /// </summary>
    public interface IIMenuItem 
    {
        /// <summary>
        /// 本菜单下的子菜单 ，不允许操作;系统执行操作
        /// </summary>
        ObservableCollection<IIMenuItem> CmItems { get; set; }

        /// <summary>
        /// 控件作用描述
        /// </summary>
        string Description { get; }

        /// <summary>
        /// 控件地址
        /// </summary>
        int Id { get; }

        /// <summary>
        /// 本菜单父类，不允许操作，系统执行操作
        /// </summary>
        IIMenuItem Parent { get; set; }

        object CommandParameter { get; set; }

        /// <summary>
        /// 菜单分类 鉴别菜单为主菜单 右键菜单 等等
        /// </summary>
        string Classic { get; }

        /// <summary>
        /// 用户界面提示信息
        /// </summary>
        string Tooltips { get; set; }

        /// <summary>
        /// 标签 可有可无 
        /// </summary>
        object Tag { get; set; }

        /// <summary>
        /// 快捷键信息
        /// </summary>
        string ShortCuts { get; set; }

        /// <summary>
        /// 菜单属于分组的名称，默认不属于任何分组，如果属于分组 则在选中时其他默认非选中
        /// </summary>
        string GroupName { get; set; }

        /// <summary>
        /// 是否被点击选中  如果可以Check的情况下
        /// </summary>
        bool IsChecked { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// 显示内容
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// 扩展名称，用于如 开K1-全夜灯 立即运算的
        /// </summary>
        string ExText { get; set; }

        /// <summary>
        /// 临时变量 可以任意修改的
        /// </summary>
        string TextTmp { get; set; }

        /// <summary>
        /// 菜单是否可以点击选中
        /// </summary>
        bool IsCheckable { get; set; }

        /// <summary>
        /// 是否具有分隔栏
        /// </summary>
        bool IsSeparator { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        object  Image { get; set; }

        /// <summary>
        /// 当用户点击菜单后 菜单是否依然显示于用户前
        /// </summary>
        bool StaysOpenOnClick { get; set; }

        /// <summary>
        /// 点击命令
        /// </summary>
        ICommand Command { get; set; }

        /// <summary>
        /// 如果执行Command事件，需要参数则置放于此
        /// </summary>
        object Argu { get; set; }

        /// <summary>
        /// 设置菜单是否可见
        /// </summary>
        Visibility Visibility { get; set; }

        /// <summary>
        /// 本菜单 是否 在生成的时候 动态调用 是否可以显示
        /// </summary>
        bool IsCanBeShowRwx();

        /// <summary>
        /// 当生成菜单的时候需要调用该函数来对Argu参数赋值
        /// </summary>
        void InitDataWhenBeforeUse(object argu);

        ///// <summary>
        ///// 当可视化变化时
        ///// </summary>
        //event EventHandler OnVisibilityChanged;
    };

}
